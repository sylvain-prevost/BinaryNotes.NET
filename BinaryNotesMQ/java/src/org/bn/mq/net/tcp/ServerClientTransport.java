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

package org.bn.mq.net.tcp;

import java.net.URI;

import java.nio.ByteBuffer;

import java.util.List;

import org.bn.mq.net.*;
import org.bn.mq.protocol.MessageEnvelope;

public class ServerClientTransport extends Transport {
    protected AcceptorFactory factory;
    protected ServerTransport serverTransport;
    
    public ServerClientTransport(URI addr, ServerTransport server, AcceptorFactory factory) {
        super(addr, factory);
        this.factory = factory;
        this.serverTransport = server;
    }
    
    public void start() {
        // do nothing for client socket
    }
    
    protected void fireReceivedData(ByteBuffer packet) throws Exception {
        try {
            List<MessageEnvelope> messages =  messageCoder.decode(packet);
            if(messages!=null) {
                for(MessageEnvelope message: messages) {
                    serverTransport.fireReceivedData(message,this);                    
                }
            }
        }
        catch(Exception ex) {
            System.err.println("Pkt: "+packet);
            System.err.println("Pkt len"+packet.limit());
            throw ex;
        }
    }

    protected void onTransportClosed() {
        serverTransport.removeClient(this);
        //fireDisconnectedEvent();
        super.onTransportClosed();
    }
}
