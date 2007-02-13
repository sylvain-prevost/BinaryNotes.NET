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

import java.io.IOException;

import org.bn.mq.IRemoteConsumer;
import org.bn.mq.IMessage;
import org.bn.mq.net.ITransport;
import org.bn.mq.protocol.MessageEnvelope;

public class RemoteConsumer<T> implements IRemoteConsumer<T> {
    protected ITransport transport = null;
    protected String consumerId = null;
    protected Class<T> messageClass;
    
    public RemoteConsumer (String consumerId, ITransport transport, Class<T> messageClass) {
        this.consumerId = consumerId;
        this.transport = transport;
        this.messageClass = messageClass;
    }
    
    public String getId() {
        return this.consumerId;
    }

    public T onMessage(IMessage<T> message) {
        Message<T> msgImpl = new Message<T>(message, this.messageClass);
        try {
            MessageEnvelope envelope = msgImpl.createEnvelope();
            envelope.getBody().getMessageUserBody().setConsumerId(getId());
            transport.sendAsync(envelope);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
        return null;
    }

    public ITransport getNetworkTransport() {
        return transport;
    }
}
