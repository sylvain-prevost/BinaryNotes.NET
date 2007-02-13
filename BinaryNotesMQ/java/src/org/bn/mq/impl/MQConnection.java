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

package org.bn.mq.impl;

import java.net.URI;

import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

import org.bn.mq.IMQConnection;
import org.bn.mq.IMQConnectionListener;
import org.bn.mq.IPTPSession;
import org.bn.mq.IRemoteSupplier;
import org.bn.mq.ISupplier;
import org.bn.mq.net.ITransport;
import org.bn.mq.net.ITransportConnectionListener;
import org.bn.mq.net.ITransportReader;
import org.bn.mq.protocol.LookupRequest;
import org.bn.mq.protocol.LookupResult;
import org.bn.mq.protocol.LookupResultCode;
import org.bn.mq.protocol.MessageBody;
import org.bn.mq.protocol.MessageEnvelope;

public class MQConnection implements IMQConnection, ITransportConnectionListener, ITransportReader {
    protected ITransport transport;
    protected final int callTimeout = 30;
    protected Map<String,ISupplier> suppliers = new HashMap<String,ISupplier>();
    protected List < IMQConnectionListener> listeners = new LinkedList < IMQConnectionListener > ();
    
    public MQConnection(ITransport transport) {
        this.transport = transport;
        transport.addConnectionListener(this);
        transport.addReader(this);
    }
    
    public void start() throws Exception {
        this.transport.start();
    }

    public IRemoteSupplier lookup(String supplierName) throws Exception {
        LookupRequest request = new LookupRequest();
        request.setSupplierName(supplierName);
        MessageEnvelope message = new MessageEnvelope();
        MessageBody body = new MessageBody();
        body.selectLookupRequest(request);
        message.setBody(body);
        message.setId(this.toString());
        MessageEnvelope result = transport.call(message,callTimeout);
        if (result.getBody().getLookupResult().getCode().getValue() == LookupResultCode.EnumType.success ) {
            return new RemoteSupplier(supplierName,transport);
        }
        else
            throw new Exception("Error when accessing to supplier '"+supplierName+"': "+ result.getBody().getLookupResult().getCode().getValue().toString());
    }
  
    public ISupplier createSupplier(String supplierName) {
        Supplier supplier = new Supplier(supplierName,transport);
        synchronized(suppliers) {            
            suppliers.put(supplier.getId(),supplier);
        }        
        return supplier;
    }
    
    public void removeSupplier(ISupplier supplier) {
        synchronized(suppliers) {
            suppliers.remove(supplier.getId());
        }
    }    

    public URI getAddr() {
        return transport.getAddr();
    }

    public void close() {
        transport.delConnectionListener(this);
        transport.delReader(this);
        transport.close();
    }

    public boolean onReceive(MessageEnvelope message, ITransport replyTransport) {
        if(message.getBody().isLookupRequestSelected()) {
            MessageEnvelope result = new MessageEnvelope();
            result.setId(message.getId());
            MessageBody body = new MessageBody();
            result.setBody(body);
            LookupResult lResult = new LookupResult();
            body.selectLookupResult(lResult);
            try {
                synchronized(suppliers) {
                    ISupplier supplier = suppliers.get(message.getBody().getLookupRequest().getSupplierName());
                    LookupResultCode resCode = new LookupResultCode();                    
                    if(supplier!=null) {
                        resCode.setValue(LookupResultCode.EnumType.success);
                    }
                    else
                        resCode.setValue(LookupResultCode.EnumType.notFound);
                    lResult.setCode(resCode);
                }
                replyTransport.sendAsync(result);
            }
            catch (Exception e) {
                e.printStackTrace();
            }
            return true;
        }
        else
            return false;
    }

    public void onConnected(ITransport transport) {
        synchronized(listeners) {
            for(IMQConnectionListener listener: listeners) {
                listener.onConnected(this,transport);
            }
        }
    }

    public void onDisconnected(ITransport transport) {
        synchronized(listeners) {
            for(IMQConnectionListener listener: listeners) {
                listener.onDisconnected(this, transport);
            }
        }    
    }

    public void addListener(IMQConnectionListener listener) {
        synchronized(listeners) {
            listeners.add(listener);
        }
    }

    public void delListener(IMQConnectionListener listener) {
        synchronized(listeners) {
            listeners.remove(listener);
        }    
    }

    public <T> IPTPSession<T> createPTPSession(String pointName, String sessionName, Class<T> messageClass) {
        return new PTPSession<T>(pointName, sessionName, this.transport, messageClass);
    }
}
