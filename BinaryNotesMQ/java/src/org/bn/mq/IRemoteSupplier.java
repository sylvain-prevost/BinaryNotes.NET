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
