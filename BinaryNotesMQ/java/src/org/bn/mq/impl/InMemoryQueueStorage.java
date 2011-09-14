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
import java.util.Iterator;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

import org.bn.mq.IConsumer;
import org.bn.mq.IMessage;
import org.bn.mq.IMessageQueue;
import org.bn.mq.IPersistenceQueueStorage;
import org.bn.mq.IRemoteConsumer;

public class InMemoryQueueStorage<T> implements IPersistenceQueueStorage<T> {
    private String queueStorageName;
    private Map < String, List < IMessage<T> > > storage = new HashMap < String, List < IMessage<T> > > ();
    private final List < IMessage<T> > nullList = new LinkedList < IMessage<T> >();
    
    public InMemoryQueueStorage(String queueStorageName) {
        this.queueStorageName = queueStorageName;        
    }
    
    public List< IMessage<T> > getMessagesToSend(IConsumer<T> consumer) {
        List< IMessage<T> > result;
        synchronized(storage) {
            result = storage.get(consumer.getId());
            if(result==null)
                result=nullList;
        }    
        return result;
    }
    
    public void removeDeliveredMessage(String consumerId, String messageId) {
        synchronized(storage) {
            List< IMessage<T> > messages = storage.get(consumerId);
            if(messages!=null) {
                Iterator<IMessage<T>> iterator =  messages.iterator();
                for(IMessage<T> item: messages) {
                    if(item.getId().equals(messageId)) {
                        messages.remove(item);
                        break;
                    }
                }
            }
        }        
    }

    public void persistenceSubscribe(IConsumer<T> consumer) {
        synchronized(storage) {
            if(!storage.containsKey(consumer.getId())) {
                List < IMessage<T> > list = new LinkedList< IMessage<T> >();
                storage.put(consumer.getId(),list);
            }
        }
    }

    public void persistenceUnsubscribe(IConsumer<T> consumer) {
        synchronized(storage) {
            storage.remove(consumer.getId());
        }    
    }

    public void registerPersistenceMessage(IMessage<T> message) {
        synchronized(storage) {
            for( Map.Entry< String, List < IMessage<T> > > item : storage.entrySet() ) {
                item.getValue().add(message);
            }
        }    
    }

    public void close() {
    }
}
