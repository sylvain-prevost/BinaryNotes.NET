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

package org.bn.mq.impl;

import java.io.IOException;

import java.net.URI;
import org.bn.mq.*;
import org.bn.mq.net.*;

public class MessagingBus implements IMessagingBus {
    protected ITransportFactory factory = new org.bn.mq.net.tcp.TransportFactory();
    
    public MessagingBus() {
        factory.setTransportMessageCoderFactory( new ASN1TransportMessageCoderFactory());
    }
    
    public IMQConnection connect(URI addr) throws IOException {
        return new MQConnection(factory.getClientTransport(addr));
    }

    public IMQConnection create(URI addr) throws IOException {
        return new MQServerConnection(factory.getServerTransport(addr));
    }
    
    public void close() {
        factory.close();
    }
}
