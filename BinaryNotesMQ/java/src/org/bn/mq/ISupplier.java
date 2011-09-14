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

import org.bn.mq.impl.Supplier;

/**
 * Interface to supplier
 */
public interface ISupplier extends IRemoteSupplier {
    /**
     * Create new queue for specified path & user message body type
     * @param queuePath queue path
     * @param messageClass user message body type
     * @return queue instance
     */
    public <T> IMessageQueue<T> createQueue(String queuePath, Class<T> messageClass);
    
    /**
     * Create new queue for specified path & user message body type
     * @param queuePath queue path
     * @param messageClass user message body type
     * @param queueImpl queue algorithm
     * @return queue instance
     */
    public <T> IMessageQueue<T> createQueue(String queuePath, Class<T> messageClass, IQueue<T> queueImpl);
    
    /**
     * Create new queue for specified path & user message body type
     * @param queuePath queue path
     * @param messageClass user message body type
     * @param queueImpl queue algorithm
     * @param storage persistence storage for queue
     * @return queue instance
     */    
    public <T> IMessageQueue<T> createQueue(String queuePath, Class<T> messageClass, IQueue<T> queueImpl, IPersistenceQueueStorage<T> storage);
    
    /**
     * Create new queue for specified path & user message body type
     * @param queuePath queue path
     * @param messageClass user message body type
     * @param storage persistence storage for queue
     * @return queue instance
     */        
    public <T> IMessageQueue<T> createQueue(String queuePath, Class<T> messageClass, IPersistenceQueueStorage<T> storage);
    
    /**
     * Remove supplier queue
     * @param queue queue instance
     */
    public <T> void removeQueue(IMessageQueue<T> queue);
}
