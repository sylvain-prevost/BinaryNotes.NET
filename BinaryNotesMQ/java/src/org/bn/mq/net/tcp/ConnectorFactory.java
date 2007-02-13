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

import java.util.LinkedList;
import java.util.List;
import java.util.SortedMap;
import java.util.TreeMap;
import java.util.concurrent.Executor;
import java.util.concurrent.Executors;
import org.bn.mq.net.*;

public class ConnectorFactory extends SocketFactory {
    
    protected ConnectorStorage connectorStorage = new ConnectorStorage();    
    protected Connector connector = new Connector(connectorStorage);
    protected List<ConnectorTransport> createdTransports = new LinkedList<ConnectorTransport>();
    
    //protected final int connectorsPoolSize = 2;
    //protected Executor connectorsExecutor = Executors.newFixedThreadPool(connectorsPoolSize);
    protected Thread connectorThread = new Thread(connector);
    
    
    public ConnectorFactory( WriterStorage writerStorage, ReaderStorage readerStorage, TransportFactory factory) {
        super(writerStorage , readerStorage, factory);
        connectorThread.setName("BNMQ-Connector");
        connectorThread.start();
    }    
    
    /*protected ConnectorTransport getCreatedTransport(URI addr) {
        ConnectorTransport result = null;
        result = createdTransports.get(addr);
        return result;
    }*/
    
    protected ConnectorTransport createTransport(URI addr) {
        ConnectorTransport transport = new ConnectorTransport(addr, this);
        createdTransports.add(transport);
        return transport;    
    }
        
    public ITransport getTransport(URI addr) {
        ConnectorTransport transport = null;
        boolean created = false;
        synchronized (createdTransports) {
            //transport = getCreatedTransport(addr);
            //if(transport == null) {
                transport = createTransport(addr);
                created = true;
            //}
        }
/*        if(created) {
            connectorStorage.addAwaitingTransport(transport);
            //connectorsExecutor.execute(connector);
            transport.finishConnect();
        }*/
        return transport;
    }
    
    public void reconnect(ConnectorTransport transport) {
        connectorStorage.addAwaitingTransport(transport);
        //connectorsExecutor.execute(connector);
    }
    
    public ConnectorStorage getConnectorStorage() {
        return this.connectorStorage;        
    }
    
    public void close() {        
        List<ConnectorTransport> tempCreatedTransports = new LinkedList<ConnectorTransport>();
        synchronized (createdTransports) {            
            tempCreatedTransports.addAll(createdTransports);
            createdTransports.clear();           
        }
        for(ConnectorTransport item: tempCreatedTransports) {
            item.close();
        }        
        connectorStorage.close();
        connector.stop();
        try {
            if(connectorThread.isAlive())
                connectorThread.join();
        }
        catch (InterruptedException e) {
            // TODO
        }
    }

    protected void removeTransport(ConnectorTransport transport) {
        synchronized (createdTransports) {
            createdTransports.remove(transport);
        }
    }
}
