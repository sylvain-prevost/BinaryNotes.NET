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

package org.bn.mq.impl;

import java.util.Date;
import java.util.LinkedList;
import java.util.List;
import java.util.concurrent.atomic.AtomicInteger;
import org.bn.mq.*;
import org.bn.mq.net.*;
import org.bn.mq.protocol.*;

public class PTPSession<T> implements IPTPSession<T>, ITransportReader {
    private ITransport transport;
    private String sessionName;
    private String pointName;
    private AtomicInteger callCurId = new AtomicInteger(0);
    protected Class<T> messageClass;
    protected static AtomicInteger messageIdGenerator = new AtomicInteger(1);
    protected List < IPTPSessionListener<T> > listeners = new LinkedList < IPTPSessionListener<T> > ();

    public PTPSession(String pointName, String sessionName, ITransport transport, Class<T> messageClass) {
        this.transport = transport;
        this.sessionName = sessionName;
        this.pointName = pointName;
        this.transport.addReader(this);
        this.messageClass = messageClass;
    }

    public void sendMessage(IMessage<T> message) throws Exception {        
        sendMessage(message,this.transport);
    }

    public void sendMessage(IMessage<T> message, ITransport forTransport) throws Exception {        
        if(message.getBody()==null)
            throw new Exception("Incorrect empty message body is specified to send!");
        Message<T> msgImpl = new Message<T>(message, this.messageClass);
        MessageEnvelope envelope = msgImpl.createEnvelope();
        envelope.getBody().getMessageUserBody().setConsumerId(this.pointName);
        forTransport.sendAsync(envelope);
    }
    
    public T call(T args, ITransport forTransport, int timeout) throws Exception {
        Message<T> envelope =  new Message<T>(this.messageClass);
        envelope.setId(this.sessionName +"/call-"+callCurId.getAndIncrement());
        envelope.setBody(args);
        MessageEnvelope argsEnv = envelope.createEnvelope();        
        argsEnv.getBody().getMessageUserBody().setQueuePath(this.sessionName);
        argsEnv.getBody().getMessageUserBody().setConsumerId(this.pointName);
        
        MessageEnvelope result = forTransport.call(argsEnv,timeout);
        envelope.fillFromEnvelope(result);
        return envelope.getBody();
    }    

    public T call(T args, ITransport forTransport) throws Exception {
        return call(args, forTransport, 120);
    }
    
    public T call(T args) throws Exception {
        return call(args,transport);
    }

    public T call(T args, int timeout) throws Exception {
        return call(args,transport, timeout);
    }
    
    
    public void callAsync(T args, ITransport forTransport, ICallAsyncListener<T> listener, int timeout) throws Exception {
        Message<T> envelope =  new Message<T>(this.messageClass);
        envelope.setId(this.sessionName +"/call-"+callCurId.getAndIncrement());
        envelope.setBody(args);
        MessageEnvelope argsEnv = envelope.createEnvelope();        
        argsEnv.getBody().getMessageUserBody().setQueuePath(this.sessionName);
        argsEnv.getBody().getMessageUserBody().setConsumerId(this.pointName);
        forTransport.callAsync(argsEnv, new ProxyCallAsyncListener<T>(listener,messageClass),timeout );
    }    

    public void callAsync(T args, ITransport forTransport, ICallAsyncListener<T> listener) throws Exception {
        callAsync(args, forTransport, listener,120);
    }
    
    public void callAsync(T args, ICallAsyncListener<T> listener) throws Exception {
        callAsync(args, listener,120);
    }

    public void callAsync(T args, ICallAsyncListener<T> listener, int timeout) throws Exception {
        callAsync(args, transport, listener,timeout);
    }    

    public void addListener(IPTPSessionListener<T> listener) {
        synchronized(listeners) {
            listeners.add(listener);
        }
    }

    public void delListener(IPTPSessionListener<T> listener) {
        synchronized(listeners) {
            listeners.remove(listener);
        }    
    }

    
    public boolean onReceive(MessageEnvelope message, ITransport transport) {
        if(message.getBody().isMessageUserBodySelected() && message.getBody().getMessageUserBody().getQueuePath().equalsIgnoreCase(this.sessionName) ) {
            fireReceiveMessage(message,transport);
            return true;
        }
        return false;
    }
    
    public IMessage<T> createMessage(T body) {
        Message<T> result = new Message<T>(this.messageClass);
        result.setBody(body);
        result.setSenderId(this.pointName);
        result.setId(this.sessionName +"/#"+new Date().getTime()+"/"+messageIdGenerator.getAndIncrement());
        result.setQueuePath(this.sessionName);
        return result;
    }
    
    public IMessage<T> createMessage() {
        return createMessage(null);
    }    
    
    public void close() {
        this.transport.delReader(this);
    }

    private void fireReceiveMessage(MessageEnvelope messageEnv, ITransport forTransport) {
        Message<T> message = new Message<T>(messageClass);
        try {
            message.fillFromEnvelope(messageEnv);
            synchronized(listeners) {
                for(IPTPSessionListener<T> listener: listeners) {
                    T result = listener.onMessage(this,forTransport,message);
                    if(result!=null) {                    
                        Message<T> resultMsg = new Message<T>(this.messageClass);
                        resultMsg.setId(message.getId());
                        resultMsg.setBody(result);
                        resultMsg.setQueuePath(message.getQueuePath());                        
                        MessageEnvelope resultMsgEnv;
                        try {
                            resultMsgEnv = resultMsg.createEnvelope();
                            resultMsgEnv.getBody().getMessageUserBody().setConsumerId(this.pointName);
                            forTransport.sendAsync(resultMsgEnv);
                        }
                        catch (Exception e) {
                            e.printStackTrace();
                        }
                    }
                }
            }
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }
}
