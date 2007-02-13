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

import java.util.LinkedList;
import java.util.List;

import org.bn.mq.IMQConnectionListener;
import org.bn.mq.IRemoteSupplier;
import org.bn.mq.net.ITransport;
import org.bn.mq.protocol.LookupRequest;
import org.bn.mq.protocol.LookupResultCode;
import org.bn.mq.protocol.MessageBody;
import org.bn.mq.protocol.MessageEnvelope;

public class MQServerConnection extends MQConnection {
    protected List<ITransport> clients = new LinkedList<ITransport>();

    public MQServerConnection(ITransport transport) {
        super(transport);
    }

    public IRemoteSupplier lookup(String supplierName) throws Exception {
        LookupRequest request = new LookupRequest();
        request.setSupplierName(supplierName);
        MessageEnvelope message = new MessageEnvelope();
        MessageBody body = new MessageBody();
        body.selectLookupRequest(request);
        message.setBody(body);
        message.setId(this.toString());
        IRemoteSupplier supplier = null;
        synchronized(clients) {
            for(ITransport client : clients) {
                MessageEnvelope result = client.call(message,callTimeout);                
                if (result.getBody().getLookupResult().getCode().getValue() == LookupResultCode.EnumType.success ) {
                    supplier = new RemoteSupplier(supplierName,client);
                    break;
                }
            }
            
        }
        if(supplier==null)
            throw new Exception("Error when accessing to supplier '"+supplierName+"'! Unable to find any suitable supplier!");        
        return supplier;
    }

    public void onConnected(ITransport client) {
        synchronized(clients) {
            clients.add(client);
        }
        synchronized(listeners) {
            for(IMQConnectionListener listener: listeners) {
                listener.onConnected(this,client);
            }
        }
    }

    public void onDisconnected(ITransport client) {
        synchronized(clients) {
            clients.remove(client);
            synchronized(listeners) {
                for(IMQConnectionListener listener: listeners) {
                    listener.onDisconnected(this,client);
                }
            }            
        }    
    }

}
