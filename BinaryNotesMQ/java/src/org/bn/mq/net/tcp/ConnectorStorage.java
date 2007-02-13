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

import java.util.LinkedList;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;

public class ConnectorStorage {
    public class ConnectorStorageEvent {
        private ConnectorTransport transportConnect = null;
        private ConnectorTransport disconnectedTransport = null;
        
        public void setTransportToConnect(ConnectorTransport transport) {
            this.transportConnect = transport;
        }
        
        public ConnectorTransport getTransportToConnect() {
            return this.transportConnect;
        }
                
        public ConnectorTransport getDisconnectedTransport() {
            return this.disconnectedTransport;
        }

        public void setDisconnectedTransport(ConnectorTransport transport) {
            this.disconnectedTransport = transport;
        }
        
    }
    
    private LinkedList<ConnectorStorageEvent> awaitingEvents = new LinkedList<ConnectorStorageEvent> ();
    protected final Lock awaitLock = new ReentrantLock();
    protected final Condition awaitEvent  = awaitLock.newCondition(); 
    private boolean finishThread = false;
        
    public ConnectorStorage() {
    }
    
    public void addAwaitingTransport(ConnectorTransport transport) {
        awaitLock.lock();    
        synchronized (awaitingEvents) {
            ConnectorStorageEvent event = new ConnectorStorageEvent();
            event.setTransportToConnect(transport);
            awaitingEvents.add(event);
        }
        awaitEvent.signal();
        awaitLock.unlock();                
    }
    
    public void addDisconnectedTransport(ConnectorTransport transport) {
        awaitLock.lock();    
        synchronized (awaitingEvents) {
            ConnectorStorageEvent event = new ConnectorStorageEvent();
            event.setDisconnectedTransport(transport);
            awaitingEvents.add(event);
        }
        awaitEvent.signal();
        awaitLock.unlock();                
    }    
    
    
    public void removeAwaitingTransport(ConnectorTransport transport) {
        synchronized (awaitingEvents) {
            for(ConnectorStorageEvent event: awaitingEvents) {
                if(event.getTransportToConnect()!=null && event.getTransportToConnect().equals(transport)) {
                    awaitingEvents.remove(event);
                    break;
                }
            }
            
        }
    }
    
    public ConnectorStorageEvent getAwaitingEvent() {        
        ConnectorStorageEvent result = null;
        if(finishThread)
            return result;
        
        do {
            awaitLock.lock();
            synchronized (awaitingEvents) {
                if(!awaitingEvents.isEmpty()) {
                    result = awaitingEvents.getFirst();
                    awaitingEvents.removeFirst();
                }
            }
                        
            if(result==null) {
                try {
                    awaitEvent.await();
                }
                catch(Exception ex) {ex =null; }
            }            
            awaitLock.unlock();
        }
        while(result==null && !finishThread);
                    
        return result;
    }

    public void clear() {
        synchronized (awaitingEvents) {
            awaitingEvents.clear();
        }
    }
    
    public void close() {
        synchronized(awaitingEvents) {
            finishThread = true;        
            awaitingEvents.clear();
        }
        awaitLock.lock();        
        awaitEvent.signal();
        awaitLock.unlock();
    }
}
