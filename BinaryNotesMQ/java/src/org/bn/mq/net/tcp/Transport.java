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

import java.nio.ByteBuffer;
import java.nio.channels.SocketChannel;

import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Timer;
import java.util.TimerTask;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.TimeoutException;
import java.util.concurrent.atomic.AtomicReference;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReadWriteLock;
import java.util.concurrent.locks.ReentrantLock;
import java.util.concurrent.locks.ReentrantReadWriteLock;
import org.bn.mq.IMessage;

import org.bn.mq.net.ITransport;
import org.bn.mq.net.ITransportCallListener;
import org.bn.mq.net.ITransportConnectionListener;
import org.bn.mq.net.ITransportMessageCoder;
import org.bn.mq.net.ITransportReader;
import org.bn.mq.protocol.MessageEnvelope;

public abstract class Transport implements ITransport {
    private URI addr;
    private SocketChannel socket = null;
    protected ReadWriteLock socketLock =  new ReentrantReadWriteLock();
    protected SocketFactory socketFactory;
    
    protected List<ITransportConnectionListener> listeners = new LinkedList<ITransportConnectionListener>();
    protected ReadWriteLock listenersLock =  new ReentrantReadWriteLock();

    protected List<ITransportReader> readers = new LinkedList<ITransportReader>();
    protected ReadWriteLock readersLock =  new ReentrantReadWriteLock();
    protected ITransportReader unhanledReader = null;  
        
    protected ByteBuffer tempReceiveBuffer = ByteBuffer.allocate(64*1024);
    protected ITransportMessageCoder messageCoder;
    
    protected final Lock callLock = new ReentrantLock();
    protected final Condition callLockEvent  = callLock.newCondition();        
    protected AtomicReference<String> currentCallMessageId = new AtomicReference<String>("-none-");
    protected MessageEnvelope currentCallMessage = null;
        
    public Transport(URI addr, SocketFactory factory) {
        setAddr(addr);
        this.socketFactory = factory ;
    }
       
    protected void onTransportClosed() {
        close();
    }    
    
    public void close() {
        socketLock.writeLock().lock();
        if(isAvailable()) {
            try {
                getSocket().close();
                setSocket(null);
            }
            catch (IOException e) {
                // TODO
            }
        }
        socketLock.writeLock().unlock();
    }

    
    public ByteBuffer receiveAsync() throws IOException {        
        ByteBuffer result = null;
        if(isAvailable()) {
            //result = ByteBuffer.allocate(4096);                
            int readedBytes = -1;
            //do {
                socketLock.readLock().lock();
                try {
                    tempReceiveBuffer.clear();
                    SocketChannel channel = getSocket();
                    if(channel!=null)
                        readedBytes = channel.read(tempReceiveBuffer);
                    /*if(readedBytes>0) {
                        if(result.remaining()<readedBytes) {
                           byte[] data = result.array();
                           result = ByteBuffer.allocate(data.length+4096 + readedBytes);
                           result.put(data);
                        }
                        result.put(tempReceiveBuffer.array(),result.position(),tempReceiveBuffer.position());
                    }*/
                }
                finally {
                    socketLock.readLock().unlock();
                }
                if(readedBytes==-1) {
                    onTransportClosed();
                }                    
            //}                
            //while(readedBytes>0);
            //result.flip();
            tempReceiveBuffer.flip();
            result = tempReceiveBuffer;
        }
        else {
            throw new IOException("Not connected");
        }
        return result;        
    }
    
    public void sendAsync(ByteBuffer buffer) throws IOException {
        if(socketFactory.getWriterStorage()!=null) {
            if(isAvailable()) {
                socketFactory.getWriterStorage().pushPacket(this, buffer);
            }
            else
                throw new IOException("Transport is not connected!");
        }
        else
            throw new IOException("Unable to write readonly transport!");
    }
    
    public void sendAsync(byte[] buffer) throws IOException {
        sendAsync(ByteBuffer.wrap(buffer));
    }
    
    public void send(MessageEnvelope message) throws Exception {
        if(isAvailable()) {    
            ByteBuffer buffer = messageCoder.encode(message);
            send(buffer);
        }
        else
            throw new IOException("Transport is not connected!");
    }
        
    public void sendAsync(MessageEnvelope message) throws Exception {
        if(isAvailable()) {        
            ByteBuffer buffer = messageCoder.encode(message);
            sendAsync(buffer);
        }
        else
            throw new IOException("Transport is not connected!");
    }     
    
    public void send(ByteBuffer buffer) throws IOException {
        try {
            socketLock.readLock().lock();        
            try {                        
                if(isAvailable()) {
                    SocketChannel channel = getSocket();
                    if(channel!=null)
                        channel.write(buffer);
                }
                else {
                    throw new IOException("Not connected");
                }
            }
            finally {
                socketLock.readLock().unlock();
            }    
        }
        catch(Exception ex) {
            this.onTransportClosed();            
        }
    }

    public void send(byte[] buffer) throws IOException {
        send(ByteBuffer.wrap(buffer));
    }
    
    public void addConnectionListener(ITransportConnectionListener listener) {
        //synchronized(listeners) {
        listenersLock.writeLock().lock();
        listeners.add(listener);
        listenersLock.writeLock().unlock();
    }

    public void delConnectionListener(ITransportConnectionListener listener) {
        listenersLock.writeLock().lock();
        listeners.remove(listener);
        listenersLock.writeLock().unlock();
    }
    
    public void fireConnectedEvent() {
        listenersLock.readLock().lock();
        try {
            //synchronized(listeners) {
            for(ITransportConnectionListener listener: listeners) {                    
                listener.onConnected(this);
            }            
        }
        finally {
            listenersLock.readLock().unlock();
        }
    }

    public void fireDisconnectedEvent() {
        listenersLock.readLock().lock();
        try {
            for(ITransportConnectionListener listener: listeners) {                    
                listener.onDisconnected(this);
            }            
        }
        finally {
            listenersLock.readLock().unlock();
        }
    }
    
    protected boolean processReceivedCallMessage(MessageEnvelope message) {
        boolean result = false;
        callLock.lock();
        if(message!=null) {            
            if(currentCallMessageId.get().equals(message.getId())) {
                currentCallMessage = message;
                callLockEvent.signal();
                result = true;
            }            
        }
        callLock.unlock();
        
        if(!result) {
            AsyncCallManager mgr = socketFactory.getTransportFactory().getAsyncCallManager();
            AsyncCallItem callAsyncResult =  mgr.getAsyncCall(message);
            if(callAsyncResult!=null) {                    
                result = true;
                if(callAsyncResult.getListener()!=null) {
                    callAsyncResult.getListener().onCallResult(callAsyncResult.getRequest(),message);
                }
            }
        }
        
        return result;
    }
    
    protected void doProcessReceivedData(MessageEnvelope message, Transport forTransport) throws Exception {        
        if(message.getBody().isAliveRequestSelected())
            return;
        boolean doProcessListeners = !forTransport.processReceivedCallMessage(message);

        readersLock.readLock().lock();
        try {
            if(doProcessListeners) {
                boolean handled = false;
                for(ITransportReader reader: readers) {                    
                    handled = reader.onReceive(message,forTransport);
                }
                if(!handled && this.unhanledReader!=null)
                    this.unhanledReader.onReceive(message,forTransport);
            }
        }        
        finally {
            readersLock.readLock().unlock();
        }
        
    }
    
    protected void doProcessReceivedData(ByteBuffer packet, Transport forTransport) throws Exception {
        List<MessageEnvelope> messages =  messageCoder.decode(packet);
        if(messages!=null) {
            for(MessageEnvelope message: messages) {
                doProcessReceivedData(message,forTransport);
            }
        }
    }
    
    protected void fireReceivedData(ByteBuffer packet) throws Exception {
        doProcessReceivedData(packet,this);
    }

    public synchronized MessageEnvelope call(MessageEnvelope message, int timeout) throws Exception {
        MessageEnvelope result = null;
        callLock.lock();
        currentCallMessage = null;
        currentCallMessageId.set(message.getId());
        try {
            sendAsync(message);
            callLockEvent.await(timeout,TimeUnit.SECONDS);
            result = currentCallMessage;
            currentCallMessageId.set(" -none- ");
        }
        finally {
            callLock.unlock();
        }
        if(result == null)
            throw new TimeoutException("Call by RPC-style message timeout in "+this+"!");
        return result;    
    }
    
    public synchronized MessageEnvelope call(MessageEnvelope message) throws Exception {
        return this.call(message,120); // By default timeout is 2 min
    }
       
    public void callAsync(MessageEnvelope message, ITransportCallListener listener, int timeout) throws Exception {
        AsyncCallManager mgr = socketFactory.getTransportFactory().getAsyncCallManager();
        try {
            mgr.storeRequest(message,listener,timeout);
            sendAsync(message);
        }
        catch(Exception ex) {
            mgr.getAsyncCall(message.getId());
            throw ex;
        }        
    }
    
    public void callAsync(MessageEnvelope message, ITransportCallListener listener) throws Exception {
        this.callAsync(message,listener,120); // By default timeout is 2 min
    }

    public URI getAddr() {
        return addr;
    }

    protected void setAddr(URI addr) {
        this.addr = addr;
    }

    public SocketChannel getSocket() {
        return socket;
    }

    public void setSocket(SocketChannel socket) {
        socketLock.writeLock().lock();
        this.socket = socket;
        if(this.socketFactory!=null) {
            if(this.socket!=null) {
                this.socketFactory.getReaderStorage().addTransport(this);
                this.socketFactory.getWriterStorage().addAliveReqInspection(this);
                messageCoder = socketFactory.getTransportFactory().getTransportMessageCoderFactory().newCoder(this);
            }
            else {
                this.socketFactory.getReaderStorage().removeTransport(this);
                this.socketFactory.getWriterStorage().delAliveReqInspection(this);
            }
        }
        socketLock.writeLock().unlock();
    }

    public boolean isAvailable() {
        socketLock.readLock().lock();
        boolean result = getSocket() != null && getSocket().isOpen() && getSocket().isConnected() ;
        socketLock.readLock().unlock();
        return result;
    }
    
    public void finalize() {
        close();
    }
    
    public void addReader(ITransportReader reader) {
        readersLock.writeLock().lock();
        readers.add(reader);
        readersLock.writeLock().unlock();
    }

    public void delReader(ITransportReader reader) {
        readersLock.writeLock().lock();
        readers.remove(reader);
        readersLock.writeLock().unlock();
    }
    
    public void setUnhandledMessagesReader(ITransportReader reader) {
        readersLock.writeLock().lock();
        this.unhanledReader = reader;
        readersLock.writeLock().unlock();
    }
    
}
