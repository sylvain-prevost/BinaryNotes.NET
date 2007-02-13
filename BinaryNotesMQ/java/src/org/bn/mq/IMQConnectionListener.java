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
