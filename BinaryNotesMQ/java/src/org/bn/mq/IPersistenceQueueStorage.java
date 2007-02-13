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
package org.bn.mq;

import java.io.Serializable;

import java.util.List;

import org.bn.mq.impl.InMemoryQueueStorage;

/**
 * Interface specification of persistence storage implementations for queue 
 */
public interface IPersistenceQueueStorage<T> {
    /**
     * Get messages to send for consumer (awaiting list)
     * @param consumer consumer instance
     * @return awaiting list
     */
    List< IMessage <T> > getMessagesToSend(IConsumer<T> consumer);
    
    /**
     * Persistence subscribe consumer
     * @param consumer consumer instance
     */
    void persistenceSubscribe(IConsumer<T> consumer) throws Exception;
    
    /**
     * Remove persistence subscription
     * @param consumer consumer instance
     */
    void persistenceUnsubscribe(IConsumer<T> consumer) throws Exception;
    
    /**
     * Register persistence message for all consumers subscriptions
     * @param message message instance
     */
    void registerPersistenceMessage(IMessage<T> message) throws Exception;
    
    /**
     * Remove registered message for specified consumer
     * @param consumer consumer instance
     * @param message message instance
     */
    void removeDeliveredMessage(String consumerId, String messageId) throws Exception ;
    
    /**
     * Close & finalize storage
     */
    void close();
}
