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

import org.bn.mq.net.ITransport;

/**
 * Transport connection listener
 */
public interface IMQConnectionListener {
    /**
     * Event invokes when connection is disconnected
     * If IMQConnection is server, onDisconnect can occur more than one time for all connected clients
     * @param connection MQConnection instance
     * @param networkTransport network transport instance. 
     */
    void onDisconnected(IMQConnection connection, ITransport networkTransport);
    
    /**
     * Event invokes when connection is connected
     * If IMQConnection is server, onConnected can occur more than one time for all connecting clients
     * @param connection MQConnection instance
     * @param networkTransport Network transport instance
     */    
    void onConnected(IMQConnection connection, ITransport networkTransport);
}
