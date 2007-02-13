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

import java.net.URI;

import java.nio.channels.ClosedChannelException;
import java.nio.channels.SelectionKey;
import java.nio.channels.Selector;
import java.nio.channels.ServerSocketChannel;
import java.util.HashMap;
import java.util.Iterator;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import java.util.Set;
import org.bn.mq.net.*;

public class AcceptorStorage {
    protected Selector selector = null;
    protected HashMap<ServerTransport,SelectionKey> acceptKeys = new HashMap<ServerTransport,SelectionKey>();
    
    protected AcceptorStorage() {
        try {
            selector = Selector.open();
        }
        catch (IOException e) {
             System.err.println(e);
        }
    }
    
    private int currentIndex = 0;
        
    public void addTransport(ServerTransport transport)  {
        ServerSocketChannel socket = transport.getServerSocket();
        if(socket!=null) {
            SelectionKey clientKey;
            try {
                clientKey = socket.register(selector,SelectionKey.OP_ACCEPT);
                synchronized(acceptKeys) {
                    clientKey.attach(transport);
                    acceptKeys.put(transport,clientKey);                
                }
            }
            catch (ClosedChannelException e) {
                System.err.println(e);
            }
            
        }
    }
    
    public void removeTransport(ServerTransport transport) {
        synchronized(acceptKeys) {
            SelectionKey key = acceptKeys.get(transport);
            if(key!=null) {
                key.cancel();
                acceptKeys.remove(transport);
            }
        }
    }
    
    public ServerTransport getServerTransportByURI(URI addr) {
        synchronized(acceptKeys) {
            for(Map.Entry<ServerTransport,SelectionKey> entry: acceptKeys.entrySet()) {
                if(entry.getKey().getAddr().equals(addr)) {
                    return entry.getKey();
                }
            }
        }
        return null;
    }
    
    
    public void close() {
        try {
            selector.close();
        }
        catch (IOException e) {
            e = null;
        }
    
        synchronized(acceptKeys) {
            for(Map.Entry<ServerTransport,SelectionKey> entry: acceptKeys.entrySet()) {
                entry.getKey().close();
                entry.getValue().cancel();
            }
            acceptKeys.clear();
        }    
    }

    public List<ServerTransport> waitTransports() throws IOException {
        List<ServerTransport> result = new LinkedList<ServerTransport>();
        synchronized(selector) {
            selector.select(100);            
            Set keys = selector.selectedKeys();
            for (Iterator i = keys.iterator(); i.hasNext();) {
                SelectionKey key = (SelectionKey) i.next();
                i.remove();
                result.add((ServerTransport)((SelectionKey)key).attachment());
            }
        }            
        return result;        
    }
}
