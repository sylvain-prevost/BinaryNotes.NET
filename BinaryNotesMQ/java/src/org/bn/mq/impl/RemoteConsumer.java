/*
 Copyright 2006-2011 Abdulla Abdurakhmanov (abdulla@latestbit.com)
 Original sources are available at www.latestbit.com

 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at

 http://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.
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
