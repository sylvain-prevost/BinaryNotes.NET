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

import java.nio.channels.ClosedChannelException;
import java.nio.channels.SelectionKey;
import java.nio.channels.Selector;

import java.nio.channels.SocketChannel;
import java.util.HashMap;
import java.util.Iterator;
import java.util.LinkedList;
import java.util.List;
import java.util.Set;

public class ReaderStorage {
    protected Selector selector = null;
    protected HashMap<Transport,SelectionKey> readerKeys = new HashMap<Transport,SelectionKey>();
    
    protected ReaderStorage() {
        try {
            selector = Selector.open();
        }
        catch (IOException e) {
             System.err.println(e);
        }
    }
    
    private int currentIndex = 0;
        
    public void addTransport(Transport transport)  {
        SocketChannel socket = transport.getSocket();
        if(socket!=null) {
            SelectionKey clientKey;
            try {
                clientKey = socket.register(selector,SelectionKey.OP_READ);
                synchronized(readerKeys) {
                    clientKey.attach(transport);
                    readerKeys.put(transport,clientKey);                
                }
            }
            catch (ClosedChannelException e) {
                System.err.println(e);
            }
            
        }
    }
    
    public void removeTransport(Transport transport) {
        synchronized(readerKeys) {
            SelectionKey key = readerKeys.get(transport);
            if(key!=null) {
                key.cancel();
                readerKeys.remove(transport);
            }
        }
    }
    
    public void close() {
        try {
            selector.close();
        }
        catch (IOException e) {
            e = null;
        }
    }

    public synchronized List<Transport> waitTransports() throws IOException {
        List<Transport> result = new LinkedList<Transport>();
        selector.select(100);
        synchronized(readerKeys) {
            Set keys = selector.selectedKeys();
            for (Iterator i = keys.iterator(); i.hasNext();) {
                SelectionKey key = (SelectionKey) i.next();
                i.remove();
                if(!key.isValid())
                    continue;
                if((!key.isReadable()))
                    continue;
                result.add((Transport)key.attachment());
            }
        }
        return result;        
    }
}
