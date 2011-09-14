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

import java.io.IOException;

import java.net.URI;

/**
 * Messaging bus and transport layer abstraction
 */
public interface IMessagingBus {
    /**
     * Create client connection
     * @param addr remote address
     * @return MQConnection instance
     */
    IMQConnection connect(URI addr) throws IOException;
    
    /**
     * Create server listener
     * @param addr bind address & port
     * @return MQConnection instance
     */
    IMQConnection create(URI addr) throws IOException;
    
    /**
     * Close bus & destroy all listeners & connectors
     */
    void close();
}
