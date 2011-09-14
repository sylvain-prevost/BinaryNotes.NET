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

import java.util.HashMap;
import java.util.Map;

import org.bn.mq.IMessageQueue;
import org.bn.mq.IPersistenceQueueStorage;
import org.bn.mq.IQueue;
import org.bn.mq.IRemoteMessageQueue;
import org.bn.mq.ISupplier;
import org.bn.mq.net.ITransport;
import org.bn.mq.net.ITransportConnectionListener;
import org.bn.mq.net.ITransportReader;
import org.bn.mq.protocol.MessageBody;
import org.bn.mq.protocol.MessageEnvelope;
import org.bn.mq.protocol.SubscribeResult;
import org.bn.mq.protocol.SubscribeResultCode;
import org.bn.mq.protocol.UnsubscribeResult;
import org.bn.mq.protocol.UnsubscribeResultCode;

public class Supplier implements ISupplier, ITransportReader {
    private ITransport transport;
    private String supplierId;
    private Map<String,MessageQueue > queues = new HashMap<String,MessageQueue >();
    
    public Supplier(String supplierId, ITransport transport) {
        this.transport = transport;
        this.supplierId = supplierId;
        this.transport.setUnhandledMessagesReader(this);
    }

    public <T> IRemoteMessageQueue<T> lookupQueue(String queuePath, Class<T> messageClass) {
        IRemoteMessageQueue<T> queue = null;
        synchronized(queues) {
            queue = (IRemoteMessageQueue<T>)queues.get(queuePath);
        }
        return queue;
    }
    
    public <T> IMessageQueue<T> createQueue(String queuePath, Class<T> messageClass, IQueue<T> queueImpl, IPersistenceQueueStorage<T> storage) {
        MessageQueue<T> queue = new MessageQueue<T>(queuePath,transport, messageClass);
        queue.setQueue(queueImpl);
        queue.setPersistenseStorage(storage);
        synchronized(queues) {
            queues.put(queuePath,queue);
        }
        return queue;
    }

    public <T> IMessageQueue<T> createQueue(String queuePath, Class<T> messageClass, IQueue<T> queueImpl) {
        NullStorage<T> nullStorage =  new NullStorage<T>(null);
        return createQueue(queuePath, messageClass, queueImpl, nullStorage.createQueueStorage(queuePath) );
    }

    public <T> IMessageQueue<T> createQueue(String queuePath, Class<T> messageClass) {
        NullStorage<T> nullStorage =  new NullStorage<T>(null);
        return createQueue(queuePath,messageClass,new Queue<T>(), nullStorage.createQueueStorage(queuePath) );
    }
    
    public <T> IMessageQueue<T> createQueue(String queuePath, Class<T> messageClass, IPersistenceQueueStorage<T> storage) {
        return createQueue(queuePath,messageClass,new Queue<T>(), storage);
    }

    public <T> void removeQueue(IMessageQueue<T> queue) {
        synchronized(queues) {
            queues.remove(queue.getQueuePath());
        }    
    }

    public String getId() {
        return supplierId;
    }

    public boolean onReceive(MessageEnvelope message, ITransport transport) {
        if(message.getBody().isSubscribeRequestSelected()) {
            MessageEnvelope resultMsg = new MessageEnvelope();
            MessageBody body = new MessageBody();
            SubscribeResult subscribeResult = new SubscribeResult();
            SubscribeResultCode subscribeResultCode = new SubscribeResultCode();
            body.selectSubscribeResult(subscribeResult);
            resultMsg.setBody(body);
            resultMsg.setId(message.getId());
            subscribeResultCode.setValue(SubscribeResultCode.EnumType.unknownQueue);
            subscribeResult.setCode(subscribeResultCode);
            try {
                transport.sendAsync(resultMsg);
            }
            catch (Exception e) {
                e.printStackTrace();
            }
        }
        else
        if(message.getBody().isUnsubscribeRequestSelected()) {
            MessageEnvelope resultMsg = new MessageEnvelope();
            MessageBody body = new MessageBody();
            UnsubscribeResult unsubscribeResult = new UnsubscribeResult();
            UnsubscribeResultCode unsubscribeResultCode = new UnsubscribeResultCode();
            body.selectUnsubscribeResult(unsubscribeResult);
            resultMsg.setBody(body);
            resultMsg.setId(message.getId());
            unsubscribeResultCode.setValue(UnsubscribeResultCode.EnumType.unknownQueue);
            unsubscribeResult.setCode(unsubscribeResultCode);
            try {
                transport.sendAsync(resultMsg);
            }
            catch (Exception e) {
                e.printStackTrace();
            }
        }
        
        return true;
    }
}
