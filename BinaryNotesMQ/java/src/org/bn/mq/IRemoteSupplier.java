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

import java.net.URI;

import org.bn.mq.net.ITransport;

/**
 * Interface to remote supplier 
 */
public interface IRemoteSupplier {
    /**
     * Get id to remote supplier
     * @return
     */
    String getId();
    
    /**
     * Lookup remote queue by specified path & message body type
     * @param queuePath queue path
     * @param messageClass user message body type
     * @return proxy to remote queue instance
     */
    <T> IRemoteMessageQueue<T> lookupQueue(String queuePath, Class<T> messageClass);
}
