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


public class AcceptorFactory extends SocketFactory {
    
    protected AcceptorStorage acceptorStorage = new AcceptorStorage();
    protected Thread acceptorThread;
    protected Acceptor acceptorThreadBody;

    //protected final int acceptorsPoolSize = 2;
    //protected Executor acceptorsExecutor = Executors.newFixedThreadPool(acceptorsPoolSize);
    
    public AcceptorFactory( WriterStorage writerStorage, ReaderStorage readerStorage, TransportFactory factory) {
        super (writerStorage, readerStorage, factory);        
        initAcceptors();
    }
       
    protected ServerTransport getCreatedTransport(URI addr) {
        ServerTransport result = null;
        result = acceptorStorage.getServerTransportByURI(addr);
        return result;
    }
    
    protected ServerTransport createTransport(URI addr) throws IOException {        
        ServerTransport transport = new ServerTransport (addr, this);
        //transport.startListener();
        //acceptorStorage.addTransport(transport);
        return transport;
    }
    
    public AcceptorStorage getAcceptorStorage() {
        return this.acceptorStorage;
    }
           
    public ITransport getTransport(URI addr) throws IOException {
        ServerTransport transport = null;
        synchronized (acceptorStorage) {
            //transport = getCreatedTransport(addr);
            //if(transport == null) {
                transport = createTransport(addr);
            //}
        }
        return transport;
    }

    private void initAcceptors() {
      //  for(int i=0;i<acceptorsPoolSize;i++)
      //      acceptorsExecutor.execute(new Acceptor(acceptorStorage));
       acceptorThreadBody = new Acceptor(acceptorStorage);
       acceptorThread = new Thread(acceptorThreadBody);
       acceptorThread.setName("BNMQ-Acceptor");
       acceptorThread.start();
    }
    
    public void close()  {
        acceptorThreadBody.stop();
        try {
            if(acceptorThread.isAlive()) {
                acceptorThread.join();
            }
        }
        catch(Exception ex) {
            ex = null;
        }
        acceptorStorage.close();
    }
}
