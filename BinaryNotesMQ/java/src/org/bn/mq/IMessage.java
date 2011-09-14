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

import java.util.Date;

/**
 * Interface represents message.
 * Implementation of interface can be created with queue.createMessage() method 
 */
public interface IMessage<T> {
    /**
     * Get message Id
     * @return Message Id
     */
    String getId();
    
    /**
     * Get sender Id
     * @todo For future version
     * @return Sender Id
     */
    String getSenderId();
    
    /**
     * Get message queue path
     * @return Message queue path
     */
    String getQueuePath();
    
    /**
     * Get the priority of message. 
     * @note Not all implementation of queue handling this parameter
     * @return priority of message
     */
    int getPriority();
    
    /**
     * Set the priority of message
     * @note Not all implementation of queue handling this parameter
     * @param prio priority of message
     */
    void setPriority(int prio);
    
    /**
     * If message mandatory for delivery, this flag has true value.
     * Mandatory messages maybe stored in MQ for delivering to persistence consumers
     * @return Mandatory flag
     */
    boolean isMandatory();
    
    /**
     * Set message mandatory flag. 
     * Messages with mandatory flag maybe stored in MQ for delivering to persistence consumers
     * @param flag
     */
    void setMandatory(boolean flag);
    
    /**
     * Get the life time of message
     * @todo For future version
     * @return the life time of message
     */
    Date getLifeTime();
    
    /**
     * Set the life time of message
     * @todo For future version
     * @param lifeTime the life time of message
     */
    void setLifeTime(Date lifeTime);
    
    /**
     * Get the user body content
     * @return the user body content
     */
    T getBody();
    
    /**
     * Set the user body content
     * @param body the user body content
     */
    void setBody(T body);
}
