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

package org.bn.mq;

import java.io.Serializable;

/**
 * Defintion of supplier MessageQueue
 */
public interface IMessageQueue<T> extends IRemoteMessageQueue<T> {
    /**
     * Get queue implementation (algorithm)
     * @return queue implementation (algorithm)
     */
    IQueue<T> getQueue();

    /**
     * Set queue implementation (algorithm)
     * @return queue implementation (algorithm)
     */    
    void setQueue(IQueue<T> queue);
    
    /**
     * Creating message for delivery over this queue
     * @param body user content body
     * @return created message
     */
    IMessage<T> createMessage(T body);
    
    /**
     * Creating message for delivery over this queue     
     * @return created message
     */
    IMessage<T> createMessage();
    
    /**
     * Send message to queue consumers
     * @param message message to send
     */
    void sendMessage(IMessage<T> message) throws Exception;    
    
    /**
     * Synchronized RPC-style call 
     * @param args call arguments
     * @param consumerId destination consumerId
     * @return call result
     */
    T call(T args, String consumerId) throws Exception;
    
    /**
     * Synchronized RPC-style call 
     * @param args call arguments
     * @param consumerId destination consumerId
     * @param timeout call timeout
     * @return call result
     */
    T call(T args, String consumerId, int timeout) throws Exception;
    
    /**
     * RPC-style async call 
     * @param args call arguments
     * @param consumerId destination consumerId
     * @param listener Listener can handles result/timeout
     */    
    void callAsync(T args, String consumerId, ICallAsyncListener<T> listener) throws Exception;
    /**
     * RPC-style async call 
     * @param args call arguments
     * @param consumerId destination consumerId
     * @param listener Listener can handles result/timeout
     * @param timeout call timeout     
     */        
    void callAsync(T args, String consumerId, ICallAsyncListener<T> listener, int timeout) throws Exception;
    
    /**
     * Set persistence storage implementation
     * @param storage persistence storage implementation
     * @see MQFactory.createPersistenceStorage()
     */
    void setPersistenseStorage(IPersistenceQueueStorage<T> storage);
    
    /**
     * Get persistence storage implementation
     * @return persistence storage implementation
     * @see MQFactory.createPersistenceStorage()
     */
    IPersistenceQueueStorage<T> getPersistenceStorage();
}
