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

package org.bn.mq.impl;

import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.UUID;
import java.util.concurrent.atomic.AtomicBoolean;
import java.util.concurrent.atomic.AtomicInteger;

import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;

import org.bn.mq.ICallAsyncListener;
import org.bn.mq.IConsumer;
import org.bn.mq.IPersistenceQueueStorage;
import org.bn.mq.IRemoteConsumer;
import org.bn.mq.IMessage;
import org.bn.mq.IMessageQueue;
import org.bn.mq.IQueue;
import org.bn.mq.net.ITransport;
import org.bn.mq.net.ITransportConnectionListener;
import org.bn.mq.net.ITransportReader;
import org.bn.mq.net.tcp.TransportPacket;
import org.bn.mq.protocol.DeliveredStatus;
import org.bn.mq.protocol.MessageBody;
import org.bn.mq.protocol.MessageEnvelope;
import org.bn.mq.protocol.SubscribeRequest;
import org.bn.mq.protocol.SubscribeResult;
import org.bn.mq.protocol.SubscribeResultCode;
import org.bn.mq.protocol.UnsubscribeResult;
import org.bn.mq.protocol.UnsubscribeResultCode;

public class MessageQueue<T> implements IMessageQueue<T>, Runnable, ITransportConnectionListener, ITransportReader {
    private ITransport transport;
    private String queuePath;
    private IQueue<T> queue = new Queue<T>();
    private AtomicInteger callCurId = new AtomicInteger(0);
    private Thread senderThread = new Thread(this);
    private AtomicBoolean stop = new AtomicBoolean(false);
    protected final Lock awaitMessageLock = new ReentrantLock();
    protected final Condition awaitMessageEvent  = awaitMessageLock.newCondition();
    protected Map<String,IConsumer<T> > consumers = new HashMap<String,IConsumer<T> >();
    protected Class<T> messageClass;
    protected IPersistenceQueueStorage<T> persistStorage;
    protected static AtomicInteger messageIdGenerator = new AtomicInteger(1);
        
    public MessageQueue(String queuePath, ITransport transport, Class<T> messageClass) {
        this.transport = transport;
        this.queuePath = queuePath;
        this.transport.addConnectionListener(this);
        this.transport.addReader(this);
        this.messageClass = messageClass;
        senderThread.setName("BNMessageQueue-"+queuePath);
        start();
    }
    
    public IQueue<T> getQueue() {
        return queue;
    }

    public void setQueue(IQueue<T> queue) {
        synchronized(queue) {
            this.queue = queue;
        }
    }

    public void sendMessage(IMessage<T> message) throws Exception {        
        if(message.getBody()==null)
            throw new Exception("Incorrect empty message body is specified to send!");
        awaitMessageLock.lock();
        synchronized(queue) {
            if(message.isMandatory())
                persistStorage.registerPersistenceMessage(message);
            queue.push(message);
        }        
        awaitMessageEvent.signal();
        awaitMessageLock.unlock();
    }
    
    public T call(T args, String consumerId, int timeout) throws Exception {
        Message<T> envelope =  new Message<T>(this.messageClass);
        envelope.setId(queuePath +"/call-"+callCurId.getAndIncrement());
        envelope.setBody(args);
        MessageEnvelope argsEnv = envelope.createEnvelope();
        MessageEnvelope result = null;
        argsEnv.getBody().getMessageUserBody().setQueuePath(this.getQueuePath());
        argsEnv.getBody().getMessageUserBody().setConsumerId(consumerId);
        IConsumer<T> consumer = null;
        synchronized(consumers) {
            if(!consumers.containsKey(consumerId)) {
                throw new Exception("Consumer with id:"+consumerId+" not found!");
            }
            else {
                 consumer = consumers.get(consumerId);
            }
        }
        if(consumer instanceof IRemoteConsumer) {
            ITransport consTransport = ((IRemoteConsumer<T>)consumer).getNetworkTransport();
            result = consTransport.call(argsEnv,timeout);
            envelope.fillFromEnvelope(result);
            return envelope.getBody();
        }
        else
            throw new Exception("Call enabled only for remote consumer !");
    }    

    public T call(T args, String consumerId) throws Exception {
        return call(args, consumerId, 120);
    }
    
    public void callAsync(T args, String consumerId, ICallAsyncListener<T> listener, int timeout) throws Exception {
        Message<T> envelope =  new Message<T>(this.messageClass);
        envelope.setId(queuePath +"/call-"+callCurId.getAndIncrement());
        envelope.setBody(args);
        MessageEnvelope argsEnv = envelope.createEnvelope();
        argsEnv.getBody().getMessageUserBody().setQueuePath(this.getQueuePath());
        argsEnv.getBody().getMessageUserBody().setConsumerId(consumerId);
        IConsumer<T> consumer = null;
        synchronized(consumers) {
            if(!consumers.containsKey(consumerId)) {
                throw new Exception("Consumer with id:"+consumerId+" not found!");
            }
            else {
                 consumer = consumers.get(consumerId);
            }
        }
        if(consumer instanceof IRemoteConsumer) {
            ITransport consTransport = ((IRemoteConsumer<T>)consumer).getNetworkTransport();
            consTransport.callAsync(argsEnv, new ProxyCallAsyncListener<T>(listener,messageClass),timeout );
        }
        else
            throw new Exception("Call enabled only for remote consumer !");
    }    

    public void callAsync(T args, String consumerId, ICallAsyncListener<T> listener) throws Exception {
        callAsync(args, consumerId, listener,120);
    }

    public void setPersistenseStorage(IPersistenceQueueStorage<T> storage) {
        this.persistStorage = storage;
    }

    public IPersistenceQueueStorage<T> getPersistenceStorage() {
        return this.persistStorage;
    }

    public void addConsumer(IConsumer<T> consumer) throws Exception {
        addConsumer(consumer, false);
    }

    public void addConsumer(IConsumer<T> consumer, Boolean persistence) throws Exception {
        addConsumer(consumer, persistence, null);
    }

    public void addConsumer(IConsumer<T> consumer, Boolean persistence, String filter) throws Exception {
        synchronized(consumers) {
            if(consumers.containsKey(consumer.getId())) {
                throw new Exception("Consumer with id:"+consumer.getId()+" is already subscription!");
            }
            else {
                consumers.put(consumer.getId(),consumer);
                if(persistence)
                    persistStorage.persistenceSubscribe(consumer);
            }
        }
    }

    public void delConsumer(IConsumer<T> consumer) throws Exception {
        synchronized(consumers) {
            if(!consumers.containsKey(consumer.getId())) {
                throw new Exception("Consumer with id:"+consumer.getId()+" doesn't have any subscription!");
            }
            else {
                consumers.remove(consumer.getId());
                persistStorage.persistenceUnsubscribe(consumer);                
            }        
        }
    }

    public String getQueuePath() {
        return queuePath;
    }

    public void run() {        
        IMessage<T> message = null;
        do {
            awaitMessageLock.lock();
            synchronized(queue) {
                message = queue.getNext();
            }
            if(message==null) {
                
                try {
                    awaitMessageEvent.await();
                }
                catch(Exception ex) {ex =null; }                
            }            
            awaitMessageLock.unlock();
            if(message!=null) {
                synchronized(consumers) {
                    for(Map.Entry<String, IConsumer<T> > entry: consumers.entrySet()) {
                        entry.getValue().onMessage(message);
                    }
                }
            }
            
        }
        while(!stop.get());
    }

    public synchronized void stop() {
        stop.set(true);        
        try {
            if(senderThread.isAlive()) {
                awaitMessageLock.lock();
                awaitMessageEvent.signal();
                awaitMessageLock.unlock();
                this.transport.delConnectionListener(this);
                this.transport.delReader(this);
                senderThread.join();
            }
        }
        catch (InterruptedException e) {
             e.printStackTrace();
        }
    }

    public synchronized void start() {
        stop.set(false);
        if(!senderThread.isAlive()) {
            senderThread.start();
        }
        
    }
    
    protected void dispose() {        
        stop();
    }
    
    private void onReceiveSubscribeRequest(MessageEnvelope message, ITransport transport) {
        RemoteConsumer<T> remoteConsumer = new RemoteConsumer<T>(message.getBody().getSubscribeRequest().getConsumerId(), transport, this.messageClass);
        
        MessageEnvelope resultMsg = new MessageEnvelope();
        MessageBody body = new MessageBody();
        SubscribeResult subscribeResult = new SubscribeResult();
        SubscribeResultCode subscribeResultCode = new SubscribeResultCode();
        body.selectSubscribeResult(subscribeResult);
        resultMsg.setBody(body);
        resultMsg.setId(message.getId());            
        try {                
            addConsumer(remoteConsumer,message.getBody().getSubscribeRequest().getPersistence(),message.getBody().getSubscribeRequest().getFilter());
            subscribeResultCode.setValue(SubscribeResultCode.EnumType.success);
        }
        catch (Exception e) {
            subscribeResultCode.setValue(SubscribeResultCode.EnumType.alreadySubscription);
            subscribeResult.setDetails(e.toString());
        }
        subscribeResult.setCode(subscribeResultCode);
        try {
            transport.sendAsync(resultMsg);
            if(subscribeResultCode.getValue() == SubscribeResultCode.EnumType.success 
            && message.getBody().getSubscribeRequest().getPersistence()) {
                List<IMessage<T>> messages =  persistStorage.getMessagesToSend(remoteConsumer);
                awaitMessageLock.lock();
                synchronized(queue) {
                    queue.push(messages);
                }        
                awaitMessageEvent.signal();
                awaitMessageLock.unlock();
            }
            
        }
        catch (Exception e) {
            e.printStackTrace();
        }    
    }

    private void onReceiveUnsubscribeRequest(MessageEnvelope message, ITransport transport) {
        IConsumer<T> consumer = null;
        synchronized(consumers) {
            consumer = consumers.get(message.getBody().getUnsubscribeRequest().getConsumerId());
        }
        MessageEnvelope resultMsg = new MessageEnvelope();
        MessageBody body = new MessageBody();
        UnsubscribeResult unsubscribeResult = new UnsubscribeResult();
        UnsubscribeResultCode unsubscribeResultCode = new UnsubscribeResultCode();
        body.selectUnsubscribeResult(unsubscribeResult);
        resultMsg.setBody(body);
        resultMsg.setId(message.getId());            
        
        if(consumer!=null) {            
            try {
                delConsumer(consumer);
            }
            catch (Exception e) {
                // TODO
            }
            unsubscribeResultCode.setValue(UnsubscribeResultCode.EnumType.success);
        }
        else {
            unsubscribeResultCode.setValue(UnsubscribeResultCode.EnumType.subscriptionNotExists);
        }
        unsubscribeResult.setCode(unsubscribeResultCode);
        try {
            transport.sendAsync(resultMsg);
        }
        catch (Exception e) {
            e.printStackTrace();
        }    
    }
    
    private void onReceivedDeliveryReport(MessageEnvelope message, ITransport transport) {
        try {
            if(message.getBody().getDeliveryReport().getStatus().getValue() == DeliveredStatus.EnumType.delivered ) {
                persistStorage.removeDeliveredMessage(
                    message.getBody().getDeliveryReport().getConsumerId(),
                    message.getBody().getDeliveryReport().getMessageId()
                );
            }            
        }
        catch (Exception e) {
            e.printStackTrace();
        }        
    }
    

    public boolean onReceive(MessageEnvelope message, ITransport transport) {
        if(message.getBody().isSubscribeRequestSelected() && message.getBody().getSubscribeRequest().getQueuePath().equalsIgnoreCase(queuePath) ) {
            onReceiveSubscribeRequest(message,transport);
            return true;
        }
        else
        if(message.getBody().isUnsubscribeRequestSelected() && message.getBody().getUnsubscribeRequest().getQueuePath().equalsIgnoreCase(queuePath) ) {
            onReceiveUnsubscribeRequest(message,transport);
            return true;
        }
        else
        if(message.getBody().isDeliveryReportSelected() && message.getBody().getDeliveryReport().getQueuePath().equalsIgnoreCase(queuePath) ) {
            onReceivedDeliveryReport(message,transport);
        }
        return false;
    }

    public void onConnected(ITransport transport) {
    }

    public void onDisconnected(ITransport transport) {
        synchronized(consumers) {
            Map<String,IConsumer<T> > newConsumers = new HashMap<String,IConsumer<T> >();
            for(Map.Entry<String,IConsumer<T>  > entry : consumers.entrySet()) {
                if(entry.getValue() instanceof IRemoteConsumer ) {
                    IRemoteConsumer<T> consumer = (IRemoteConsumer<T>) entry.getValue();
                    if(!consumer.getNetworkTransport().equals(transport)) {
                        newConsumers.put(entry.getValue().getId(),entry.getValue());
                    }                
                }
            }
            consumers.clear();
            consumers.putAll(newConsumers);
        }
    }
    
    public IMessage<T> createMessage(T body) {
        Message<T> result = new Message<T>(this.messageClass);
        result.setBody(body);
        result.setId(this.getQueuePath()+"/#"+new Date().getTime()+"/"+messageIdGenerator.getAndIncrement());
        result.setQueuePath(getQueuePath());
        return result;
    }
    
    public IMessage<T> createMessage() {
        return createMessage(null);
    }    
}
