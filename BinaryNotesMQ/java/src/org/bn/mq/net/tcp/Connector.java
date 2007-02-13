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

import java.io.IOException;
import java.net.InetSocketAddress;
import java.nio.channels.SocketChannel;

public class Connector implements Runnable {
    private ConnectorStorage storage;
    private boolean finish = false;
    
    public Connector(ConnectorStorage storage) {
        this.storage = storage;
    }

    public void run() {
        do {
            ConnectorStorage.ConnectorStorageEvent event =  storage.getAwaitingEvent();
            if(event!=null) {
                ConnectorTransport transport = event.getTransportToConnect();
                if(transport!=null) {
                    System.out.println("Connecting started for "+transport.getAddr());
                    boolean connected = false;
                    try {
                        connected = transport.connect();
                        if(connected) {
                            transport.onConnected();
                        }                
                    }
                    catch (IOException ex) {
                        connected = false;
                    }
                    if(!connected) {
                        System.out.println("Unable to connect for "+transport.getAddr());
                        transport.setSocket(null);
                        transport.onNotConnected();
                    }
                }
                else
                if(event.getDisconnectedTransport()!=null) {
                    transport = event.getDisconnectedTransport();
                    transport.onDisconnect();
                    System.out.println("!!!!!");
                }
            }
        }
        while(!finish);
    }
    
    public void stop() {
        finish = true;
    }
}
