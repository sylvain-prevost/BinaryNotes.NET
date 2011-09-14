/*
 Copyright 2006-2011 Abdulla Abdurakhmanov (abdulla@latestbit.com)
 Original sources are available at www.latestbit.com

 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at

 http://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.
 */
using System;
using System.Collections.Generic;
using org.bn.mq.protocol;
using org.bn.mq.net;

namespace org.bn.mq.impl
{
    public class PTPSession<T> : IPTPSession<T>, ITransportReader
    {
        private ITransport transport;
        private string sessionName;
        private string pointName;
        private int callCurId = 0;
        protected static int messageIdGenerator = 1;
        protected IList<IPTPSessionListener<T>> listeners = new List<IPTPSessionListener<T>>();

        public PTPSession(string pointName, string sessionName, ITransport transport)
        {
            this.transport = transport;
            this.sessionName = sessionName;
            this.pointName = pointName;
            this.transport.addReader(this);
        }


        public IMessage<T> createMessage(T body)
        {
            Message<T> result = new Message<T>();
            result.Body = (body);
            result.SenderId = (this.pointName);
            result.Id = this.sessionName + "/#" + (DateTime.Now.Ticks / 10000).ToString() + "/" + messageIdGenerator++;
            result.QueuePath = (this.sessionName);
            return result;
        }

        public IMessage<T> createMessage()
        {
            return createMessage(default(T));
        }

        public void sendMessage(IMessage<T> message)
        {
            sendMessage(message, this.transport);
        }

        public void sendMessage(IMessage<T> message, ITransport forTransport) {        
            if(message.Body==null)
                throw new Exception("Incorrect empty message body is specified to send!");
            Message<T> msgImpl = new Message<T>(message);
            MessageEnvelope envelope = msgImpl.createEnvelope();
            envelope.Body.MessageUserBody.ConsumerId = (this.pointName);
            forTransport.sendAsync(envelope);
        }


        public T call(T args, ITransport forTransport, int timeout)
        {
            Message<T> envelope = new Message<T>();
            envelope.Id = (this.sessionName + "/call-" + callCurId++);
            envelope.Body = (args);
            MessageEnvelope argsEnv = envelope.createEnvelope();
            argsEnv.Body.MessageUserBody.QueuePath = (this.sessionName);
            argsEnv.Body.MessageUserBody.ConsumerId = (this.pointName);

            MessageEnvelope result = forTransport.call(argsEnv, timeout);
            envelope.fillFromEnvelope(result);
            return envelope.Body;
        }

        public T call(T args, ITransport forTransport)
        {
            return call(args, forTransport, 120);
        }


        public T call(T args)
        {
            return call(args, this.transport);
        }

        public T call(T args, int timeout)
        {
            return call(args, this.transport, timeout);
        }

        public void callAsync(T args, ITransport forTransport, ICallAsyncListener<T> listener, int timeout)
        {
            Message<T> envelope = new Message<T>();
            envelope.Id = (this.sessionName + "/call-" + callCurId++);
            envelope.Body = (args);
            MessageEnvelope argsEnv = envelope.createEnvelope();
            argsEnv.Body.MessageUserBody.QueuePath = (this.sessionName);
            argsEnv.Body.MessageUserBody.ConsumerId = (this.pointName);

            forTransport.callAsync(argsEnv, new ProxyCallAsyncListener<T>(listener), timeout);

        }

        public void callAsync(T args, ITransport forTransport, ICallAsyncListener<T> listener)
        {
            callAsync(args, forTransport, listener, 120);
        }

        public void callAsync(T args, ICallAsyncListener<T> listener)
        {
            callAsync(args, this.transport, listener);
        }

        public void callAsync(T args, ICallAsyncListener<T> listener, int timeout)
        {
            callAsync(args, this.transport, listener, timeout);
        }

        public void callAsync(T args, ITransport forTransport, CallAsyncDelegateResult<T> resultDelegate, CallAsyncDelegateTimeout<T> timeoutDelegate, int timeout)
        {
            Message<T> envelope = new Message<T>();
            envelope.Id = (this.sessionName + "/call-" + callCurId++);
            envelope.Body = (args);
            MessageEnvelope argsEnv = envelope.createEnvelope();
            argsEnv.Body.MessageUserBody.QueuePath = (this.sessionName);
            argsEnv.Body.MessageUserBody.ConsumerId = (this.pointName);

            forTransport.callAsync(argsEnv, new ProxyCallAsyncListener<T>(resultDelegate, timeoutDelegate), timeout);
        }

        public void callAsync(T args, ITransport forTransport, CallAsyncDelegateResult<T> resultDelegate, CallAsyncDelegateTimeout<T> timeoutDelegate)
        {
            callAsync(args, forTransport, resultDelegate, timeoutDelegate,120);
        }

        public void callAsync(T args, CallAsyncDelegateResult<T> resultDelegate, CallAsyncDelegateTimeout<T> timeoutDelegate)
        {
            callAsync(args, this.transport, resultDelegate, timeoutDelegate);
        }

        public void callAsync(T args, CallAsyncDelegateResult<T> resultDelegate, CallAsyncDelegateTimeout<T> timeoutDelegate, int timeout)
        {
            callAsync(args, this.transport, resultDelegate, timeoutDelegate, timeout);
        }

        public void addListener(IPTPSessionListener<T> listener)
        {
            lock(listeners) {
                listeners.Add(listener);
            }
        }

        public void delListener(IPTPSessionListener<T> listener)
        {
            lock(listeners) {
                listeners.Remove(listener);
            }
        }

        public void close()
        {
        }

        public bool onReceive(MessageEnvelope message, ITransport transport)
        {
            if (message.Body.isMessageUserBodySelected() && message.Body.MessageUserBody.QueuePath.ToLower().Equals(this.sessionName.ToLower()))
            {
                fireReceiveMessage(message, transport);
                return true;
            }
            return false;
        }

        private void fireReceiveMessage(MessageEnvelope messageEnv, ITransport forTransport) {
            Message<T> message = new Message<T>();
            try {
                message.fillFromEnvelope(messageEnv);
                lock(listeners) {
                    foreach(IPTPSessionListener<T> listener in listeners) {
                        T result = listener.onMessage(this,forTransport,message);
                        if (result != null)
                        {
                            Message<T> resultMsg = new Message<T>();
                            resultMsg.Id = (message.Id);
                            resultMsg.Body = (result);
                            resultMsg.QueuePath = (message.QueuePath);
                            MessageEnvelope resultMsgEnv;
                            try
                            {
                                resultMsgEnv = resultMsg.createEnvelope();
                                resultMsgEnv.Body.MessageUserBody.ConsumerId = (this.pointName);
                                forTransport.sendAsync(resultMsgEnv);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }


    }
}
