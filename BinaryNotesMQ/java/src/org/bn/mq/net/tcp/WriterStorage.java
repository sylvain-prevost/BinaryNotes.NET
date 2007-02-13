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

import java.nio.ByteBuffer;

import java.util.Date;
import java.util.LinkedList;
import java.util.List;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;
import org.bn.mq.net.ITransportMessageCoder;
import org.bn.mq.protocol.AliveRequest;
import org.bn.mq.protocol.MessageBody;
import org.bn.mq.protocol.MessageEnvelope;

public class WriterStorage {
    protected LinkedList<TransportPacket> queue = new LinkedList<TransportPacket>();
    protected final Lock awaitPacketLock = new ReentrantLock();
    protected final Condition awaitPacketEvent  = awaitPacketLock.newCondition(); 
    private boolean finishThread = false;
    protected ITransportMessageCoder messageCoder;
    
    protected LinkedList<Transport> aliveRequestCheckList = new LinkedList<Transport>();
    
    
    public WriterStorage() {
    }
    
    public TransportPacket getPacket() {
        synchronized(queue) {
            if(!queue.isEmpty()) {
                TransportPacket result = queue.getFirst();
                queue.removeFirst();
                return result;
            }
        }
        return null;
    }

    public TransportPacket waitPacket() {
        awaitPacketLock.lock();
        TransportPacket result = null;
        do {
            synchronized(queue) {
                if(!queue.isEmpty()) {
                    result = queue.getFirst();
                    queue.removeFirst();
                }
            }
            if(result==null) {
                try {
                    if(!awaitPacketEvent.await(10,TimeUnit.SECONDS)) {
                        pushAliveReqForRecipients();
                    }
                }
                catch(Exception ex) {ex =null; }
            }
        }
        while(result==null && !finishThread);
        awaitPacketLock.unlock();
        return result;
    }
    
    public void pushPacket(Transport transport, ByteBuffer data) {
        TransportPacket packet = new TransportPacket();
        packet.setData(data);
        packet.setTransport(transport);
        pushPacket(packet);
    }
    
    public void pushPacket(TransportPacket packet) {
        awaitPacketLock.lock();
        synchronized(queue) {
            queue.add(packet);
        }        
        awaitPacketEvent.signal();
        awaitPacketLock.unlock();
    }
    
    public void addAliveReqInspection(Transport transport) {
        synchronized(aliveRequestCheckList) {
            aliveRequestCheckList.add(transport);
        }        
    }

    public void delAliveReqInspection(Transport transport) {
        synchronized(aliveRequestCheckList) {
            aliveRequestCheckList.remove(transport);
        }
    }
    
    public void pushAliveReqForRecipients() {
        if(this.messageCoder!=null) {
            synchronized(aliveRequestCheckList) {
                MessageEnvelope envelope = new MessageEnvelope();
                MessageBody body = new MessageBody();                    
                AliveRequest req = new AliveRequest();
                envelope.setId("-PING-");
                req.setTimestamp(new Date().getTime());
                body.selectAliveRequest(req);
                envelope.setBody(body);
                ByteBuffer buffer;
                try {
                    buffer = messageCoder.encode(envelope);
                    for(Transport transport : aliveRequestCheckList) {
                        pushPacket(transport,buffer);
                    }
                }
                catch (Exception e) {
                    e.printStackTrace();
                }
                
            }
        }
    }
    
    public void setMessageCoder(ITransportMessageCoder coder) {
        this.messageCoder = coder;
    }
    
    public void close() {
        awaitPacketLock.lock();
        synchronized(queue) {
            queue.clear();
            //queue = null;
            finishThread = true;
        }            
        awaitPacketEvent.signal();
        awaitPacketLock.unlock();
    }
}
