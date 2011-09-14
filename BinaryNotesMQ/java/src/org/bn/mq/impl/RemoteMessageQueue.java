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

import java.util.HashMap;
import java.util.Map;

import org.bn.mq.IConsumer;
import org.bn.mq.IRemoteConsumer;
import org.bn.mq.IMQConnection;
import org.bn.mq.IMessage;
import org.bn.mq.IRemoteMessageQueue;
import org.bn.mq.IRemoteSupplier;
import org.bn.mq.net.ITransport;
import org.bn.mq.net.ITransportConnectionListener;
import org.bn.mq.net.ITransportReader;
import org.bn.mq.protocol.DeliveredStatus;
import org.bn.mq.protocol.DeliveryReport;
import org.bn.mq.protocol.LookupRequest;
import org.bn.mq.protocol.LookupResultCode;
import org.bn.mq.protocol.MessageBody;
import org.bn.mq.protocol.MessageEnvelope;
import org.bn.mq.protocol.SubscribeRequest;
import org.bn.mq.protocol.SubscribeResultCode;
import org.bn.mq.protocol.UnsubscribeRequest;
import org.bn.mq.protocol.UnsubscribeResultCode;

public class RemoteMessageQueue<T> implements IRemoteMessageQueue<T>, ITransportReader {
    private RemoteSupplier supplier;
    private String queuePath;
    private final int subscribeTimeout = 60;
    protected Map<String,IConsumer<T> > consumers = new HashMap<String,IConsumer<T> >();    
    private Class<T> messageClass;
    
    public RemoteMessageQueue(String queuePath, RemoteSupplier supplier, Class<T> messageClass) {
        this.supplier = supplier;
        this.queuePath = queuePath;
        this.messageClass = messageClass;
        start();
    }

    public void addConsumer(IConsumer<T> consumer)  throws Exception  {
        addConsumer(consumer, false);
    }

    public void addConsumer(IConsumer<T> consumer, Boolean persistence)  throws Exception  {
        addConsumer(consumer, persistence, null);
    }

    public void addConsumer(IConsumer<T> consumer, Boolean persistence, String filter) throws Exception {
        SubscribeRequest request = new SubscribeRequest();
        request.setConsumerId(consumer.getId());
        request.setFilter(filter);
        if(persistence)
            request.setPersistence(true);
        request.setQueuePath(getQueuePath());
        
        MessageEnvelope message = new MessageEnvelope();
        MessageBody body = new MessageBody();
        body.selectSubscribeRequest(request);
        message.setBody(body);
        message.setId(this.toString());
        MessageEnvelope result = supplier.getConnection().call(message,subscribeTimeout);
        if (result.getBody().getSubscribeResult().getCode().getValue() != SubscribeResultCode.EnumType.success ) {
            throw new Exception("Error when accessing to queue '"+queuePath+"' for supplier '"+supplier.getId()+"': "+ result.getBody().getSubscribeResult().getCode().getValue().toString());
        }
        else {
            synchronized(consumers) {
                consumers.put(consumer.getId(),consumer);
            }
        }
    }

    public void delConsumer(IConsumer<T> consumer) throws Exception {
        synchronized(consumers) {
            consumers.remove(consumer.getId());
        }
    
        UnsubscribeRequest request = new UnsubscribeRequest();
        request.setConsumerId(consumer.getId());
        request.setQueuePath(getQueuePath());
        
        MessageEnvelope message = new MessageEnvelope();
        MessageBody body = new MessageBody();
        body.selectUnsubscribeRequest(request);
        message.setBody(body);
        message.setId(this.toString());
        MessageEnvelope result = supplier.getConnection().call(message,subscribeTimeout);
        if (result.getBody().getUnsubscribeResult().getCode().getValue() != UnsubscribeResultCode.EnumType.success ) {
            throw new Exception("Error when accessing to queue '"+queuePath+"' for supplier '"+supplier.getId()+"': "+ result.getBody().getUnsubscribeResult().getCode().getValue().toString());
        }        
    }

    public String getQueuePath() {
        return queuePath;
    }

    public boolean onReceive(MessageEnvelope message, ITransport transport) {
        if(message.getBody().isMessageUserBodySelected() && message.getBody().getMessageUserBody().getQueuePath().equalsIgnoreCase(this.getQueuePath())) {
            synchronized(consumers) {
                IConsumer<T> consumer = consumers.get(message.getBody().getMessageUserBody().getConsumerId());
                if(consumer!=null) {
                    Message<T> msg = new Message<T>(this.messageClass);
                    try {
                        msg.fillFromEnvelope(message);
                    }
                    catch (Exception e) {
                        e.printStackTrace();
                    }
                    
                    T result = consumer.onMessage( msg );
                    if(msg.isMandatory()) {
                        MessageEnvelope deliveryReportMessage = new MessageEnvelope();
                        MessageBody deliveryReportBody = new MessageBody();
                        DeliveryReport deliveryReportData = new DeliveryReport();
                        deliveryReportMessage.setBody(deliveryReportBody);
                        deliveryReportMessage.setId("/report-for/"+msg.getId());                        
                        deliveryReportBody.selectDeliveryReport(deliveryReportData);
                        deliveryReportData.setConsumerId(consumer.getId());
                        deliveryReportData.setMessageId(msg.getId());
                        deliveryReportData.setQueuePath(this.queuePath);                                                                        
                        DeliveredStatus status = new DeliveredStatus();
                        status.setValue(DeliveredStatus.EnumType.delivered);
                        deliveryReportData.setStatus(status);
                        
                        try {
                            transport.sendAsync(deliveryReportMessage);
                        }
                        catch (Exception e) {
                            e.printStackTrace();
                        }                        
                    }
                    if(result!=null) {
                        Message<T> resultMsg = new Message<T>(this.messageClass);
                        resultMsg.setId(msg.getId());
                        resultMsg.setBody(result);
                        resultMsg.setQueuePath(msg.getQueuePath());                        
                        MessageEnvelope resultMsgEnv;
                        try {
                            resultMsgEnv = resultMsg.createEnvelope();
                            resultMsgEnv.getBody().getMessageUserBody().setConsumerId(consumer.getId());
                            transport.sendAsync(resultMsgEnv);
                        }
                        catch (Exception e) {
                            e.printStackTrace();
                        }
                    }
                }
            }
            return true;
        }
        return false;
    }

    public void start() {
        this.supplier.getConnection().addReader(this);
    }

    public void stop() {
        this.supplier.getConnection().delReader(this);
    }
}
