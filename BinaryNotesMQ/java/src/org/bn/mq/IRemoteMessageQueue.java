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

import org.bn.mq.impl.RemoteMessageQueue;

/**
 * Remote message queue interface
 */
public interface IRemoteMessageQueue<T> {
    /**
     * Add new subscription for specified consumer
     * @param consumer consumer instance
     */
    void addConsumer(IConsumer<T> consumer)  throws Exception;
    
    /**
     * Add new subscription for specified consumer
     * @param consumer consumer instance
     * @param persistence if true then persistence subscription will performed
     */    
    void addConsumer(IConsumer<T> consumer, Boolean persistence)  throws Exception ;

    /**
     * Add new subscription for specified consumer
     * @param consumer consumer instance
     * @param persistence if true then persistence subscription will performed
     * @param filter filter for subscription (not supported for version 1.0)
     */        
    void addConsumer(IConsumer<T> consumer, Boolean persistence, String filter)  throws Exception ;
    
    /**
     * Remove subscription to queue
     * @param consumer consumer instance
     */
    void delConsumer(IConsumer<T> consumer) throws Exception;
    
    /**
     * Get queue path
     * @return queue path
     */
    String getQueuePath();

    /**
     * Start queue processing
     * @note when queue created is already started
     */
    void start();
    
    /**
     * Stop queue processing
     */
    void stop();
}
