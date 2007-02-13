/*
 * Copyright 2006 Abdulla G. Abdurakhmanov (abdulla.abdurakhmanov@gmail.com).
 * 
 * Licensed under the LGPL, Version 2 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.gnu.org/copyleft/lgpl.html
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * With any your questions welcome to my e-mail 
 * or blog at http://abdulla-a.blogspot.com.
 */
using System;
using System.Threading;
using csUnit;

using org.bn.mq.net;
using org.bn.mq.net.tcp;
using org.bn.mq.protocol;
using org.bn.mq;

namespace test.org.bn.mq.net 
{
    [TestFixture]
    public class MessageDecoderThreadTest
    {        
        public MessageEnvelope createMessage(String vl) {
            MessageEnvelope message = new MessageEnvelope();
            message.Id = "MsgId-"+vl;
            MessageBody msgBody = new MessageBody();
            MessageUserBody userBody = new MessageUserBody();
            userBody.UserBody = new byte[] { (byte)0xFF, (byte)0xFE };
            userBody.ConsumerId = this.ToString();
            userBody.QueuePath = "testQueuePath/Temp";
            msgBody.selectMessageUserBody(userBody);
            message.Body = msgBody;
            return message;
        }

        public void testTakeMessage() {
            const String connectionString = "bnmq://localhost:3333";
            TransportFactory conFactory = new TransportFactory();
            try {
                conFactory.TransportMessageCoderFactory = new ASN1TransportMessageCoderFactory();
                
                ITransport server = conFactory.getServerTransport(new Uri(connectionString));
                Assert.NotNull(server);
                MessageListener ml = new MessageListener(this);
                server.addConnectionListener(ml);
                server.addReader(ml);
                server.start();
                
                ITransport client = conFactory.getClientTransport(new Uri(connectionString));
                ml = new MessageListener(this);
                client.addConnectionListener(ml);
                client.addReader(ml);
                Assert.NotNull(client);
                client.start();
                    
                client.send(createMessage("AAAaasasasasassas"));
                client.sendAsync(createMessage("Two"));
                Thread.Sleep(1500);
                client.close();
                server.close();
                
            }
            finally {
                conFactory.close();
            }
            Console.WriteLine("Finished: testTakeMessage");
        }
        
        public void testCall() {
            const String connectionString = "bnmq://localhost:3333";
            TransportFactory conFactory = new TransportFactory();
            try {
                conFactory.TransportMessageCoderFactory = (new ASN1TransportMessageCoderFactory());
                
                ITransport server = conFactory.getServerTransport(new Uri(connectionString));
                Assert.NotNull(server);
                CallMessageListener cl = new CallMessageListener(this);
                server.addConnectionListener(cl);
                server.addReader(cl);
                Thread.Sleep(500);
                server.start();
                
                ITransport client = conFactory.getClientTransport(new Uri(connectionString));
                Assert.NotNull(client);
                client.start();
                MessageEnvelope result = client.call(createMessage("Call"), 10);
                Console.WriteLine("Result call received with Id:"+result.Id+" has been received successfully");
                client.close();
                server.close();
                
            }
            finally {
                conFactory.close();
            }
            Console.WriteLine("Finished: testCall");
        }    

        public void testAsyncCall() {
            const String connectionString = "bnmq://localhost:3333";
            TransportFactory conFactory = new TransportFactory();
            try {
                conFactory.TransportMessageCoderFactory = (new ASN1TransportMessageCoderFactory());
                
                ITransport server = conFactory.getServerTransport(new Uri(connectionString));
                Assert.NotNull(server);
                CallMessageListener cl = new CallMessageListener(this);
                server.addConnectionListener(cl);
                server.addReader(cl);
                server.start();
                
                ITransport client = conFactory.getClientTransport(new Uri(connectionString));
                Assert.NotNull(client);
                client.start();
                client.callAsync(createMessage("CallAsync"), new AsyncCallMessageListener());
                Thread.Sleep(500);

                client.close();
                server.close();
                
            }
            finally {
                conFactory.close();
            }
            Console.WriteLine("Finished: testCall");
        }    

        private class MessageListener : ITransportConnectionListener, ITransportReader {         
            private MessageDecoderThreadTest parent;
            private int counter = 0;
            public MessageListener(MessageDecoderThreadTest parent) {
                this.parent = parent;
            }
            public bool onReceive(MessageEnvelope message, ITransport transport) {
                Console.WriteLine("Message from " + transport + " with Id:" + message.Id + " has been received successfully");
                try {
                    if(counter<10) {
                        transport.send(parent.createMessage("Three" + counter++));
                        transport.sendAsync(parent.createMessage("Four" + counter++));
                    }
                    
                }
                catch (Exception e) {
                    Console.WriteLine(e.ToString());
                    throw e;
                }
                return true;
            }

            public void onConnected(ITransport transport) {
                Console.WriteLine("Connected from " + transport + ". Addr:" + transport.getAddr());
            }

            public void onDisconnected(ITransport transport) {
                Console.WriteLine("Disconnected from " + transport + ". Addr:" + transport.getAddr());
            }
        }
        
        private class CallMessageListener : ITransportConnectionListener, ITransportReader {         
            private MessageDecoderThreadTest parent;
            private int counter = 0;
            public CallMessageListener(MessageDecoderThreadTest parent) {
                this.parent = parent;
            }
            public bool onReceive(MessageEnvelope message, ITransport transport) {
                Console.WriteLine("Call from "+transport+" with Id:"+message.Id+" has been received successfully");
                try {
                    MessageEnvelope result = parent.createMessage("result");
                    result.Id = (message.Id);
                    transport.sendAsync(result);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
                return true;
            }

            public void onConnected(ITransport transport) {
                Console.WriteLine("Connected from " + transport + ". Addr:" + transport.getAddr());
            }

            public void onDisconnected(ITransport transport) {
                Console.WriteLine("Disconnected from " + transport + ". Addr:" + transport.getAddr());
            }
        }    
        
        private class AsyncCallMessageListener : ITransportCallListener {

            public void onCallResult(MessageEnvelope request, 
                                     MessageEnvelope result) {
                Console.WriteLine("Call result received: " + result.ToString());
            }

            public void onCallTimeout(MessageEnvelope request) {
                Console.WriteLine("!! Call result timeout !!. Request: " + request.ToString());
            }
        }
    }
}
