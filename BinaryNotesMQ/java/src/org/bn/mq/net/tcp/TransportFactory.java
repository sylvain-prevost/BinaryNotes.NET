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
import org.bn.mq.net.*;

public class TransportFactory implements ITransportFactory {
    private final String scheme = "bnmq";

    protected WriterStorage writerStorage = new WriterStorage();
    protected ReaderStorage readerStorage = new ReaderStorage();
    
    protected ConnectorFactory conFactory = new ConnectorFactory( writerStorage, readerStorage, this);
    protected AcceptorFactory acpFactory = new AcceptorFactory(writerStorage, readerStorage, this);

    protected Thread writerThread;
    protected Writer writerThreadBody;
    
    protected Thread readerThread;
    protected Reader readerThreadBody;
    protected ITransportMessageCoderFactory messageCoderFactory;
    protected AsyncCallManager asyncCallMgr = new AsyncCallManager();
    
    public TransportFactory () {
        startAsyncDispatchers();
    }        
    
    public void setTransportMessageCoderFactory(ITransportMessageCoderFactory coderFactory) {
        this.messageCoderFactory = coderFactory;
        if(this.messageCoderFactory!=null) {
            writerStorage.setMessageCoder(this.messageCoderFactory.newCoder(null));
        }
    }
    
    public ITransportMessageCoderFactory getTransportMessageCoderFactory() {
        return messageCoderFactory;
    }
    
    public AsyncCallManager getAsyncCallManager() {
        return this.asyncCallMgr;
    }
    
    public ITransport getClientTransport(URI addr) throws IOException {        
        return conFactory.getTransport(addr);
    }

    public ITransport getServerTransport(URI addr) throws IOException {        
        return acpFactory.getTransport(addr);
    }

    public boolean checkURISupport(URI addr) {
        return addr.getScheme().equalsIgnoreCase(scheme);
    }
    
    public ConnectorFactory getConnectorFactory() {
        return this.conFactory;
    }

    public AcceptorFactory getAcceptorFactory() {
        return this.acpFactory;
    }
    
    protected void startAsyncDispatchers() {
        writerThreadBody = new Writer(writerStorage);
        writerThread = new Thread(writerThreadBody);
        writerThread.setName("BNMQ-TCPWriter");
        writerThread.start();
        readerThreadBody = new Reader(readerStorage);
        readerThread = new Thread(readerThreadBody);
        readerThread.setName("BNMQ-TCPReader");
        readerThread.start();
    }
    
    public void close() {
        writerThreadBody.stop();
        readerThreadBody.stop();
        writerStorage.close();  
        if(writerThread.isAlive()) {
            try {
                writerThread.join();
            }
            catch(Exception ex) {
                ex = null;
            }
        }
        if(readerThread.isAlive()) {
            try {
                readerThread.join();    
            }
            catch(Exception ex) {
                ex = null;
            }            
        }
        
        readerStorage.close();
        conFactory.close();
        acpFactory.close();
        asyncCallMgr.stop();
    }
    
}
