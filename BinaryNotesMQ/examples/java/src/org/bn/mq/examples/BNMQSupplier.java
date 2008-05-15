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

package org.bn.mq.examples;


import java.net.URI;

import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.atomic.AtomicBoolean;

import org.bn.mq.*;
import org.bn.mq.examples.protocol.*;
import org.bn.mq.net.*;

//import test.org.bn.mq.MQFactoryTest;

public class BNMQSupplier {
    public BNMQSupplier() {
    }

    public static void main(String[] args) {
        BNMQSupplier app = new BNMQSupplier();        
        app.start();
    }
    
    protected class MQConnectionListener implements IMQConnectionListener {

        public void onDisconnected(IMQConnection connection, 
                                   ITransport networkTransport) {
            System.out.println("Disconnected from: "+connection.getAddr()+"/"+networkTransport.toString());
        }

        public void onConnected(IMQConnection connection, 
                                ITransport networkTransport) {
            System.out.println("Connected from: "+connection.getAddr()+"/"+networkTransport.toString());
        }
    }
    
    protected class QueueDispatcher implements Runnable {
        private AtomicBoolean stop = new AtomicBoolean();
        private Thread thread = new Thread(this,"ExampleQueueDispatcher");
        private IMessageQueue<ExampleMessage> queue ;
        
        public QueueDispatcher(IMessageQueue<ExampleMessage> queue ) {
            this.queue = queue;
        }
        
        public void start() {
            stop.set(false);
            thread.start();
        }
        
        public void run() {
            while(!stop.get()) {
                try {
                    Thread.sleep(2000);
                    IMessage<ExampleMessage> message = queue.createMessage();
                    ExampleMessage messageBody = new ExampleMessage();
                    messageBody.setField1("Field1Content");
                    messageBody.setField2(0xffffL);                    
                    System.out.println("Queue: Trying to send message #"+message.getId());
                    message.setBody(messageBody);
                    message.setMandatory(true);
                    queue.sendMessage(message);
                }
                catch (Exception e) {
                    e.printStackTrace();
                }
            }
        }
        
        public void stop() {
            if(!stop.get()) {
                stop.set(true);
                try {
                    thread.join();
                }
                catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
        }
    }


    private void start() {
        IMessagingBus bus = MQFactory.getInstance().createMessagingBus();
        
        
        IMQConnection serverConnection  = null;
        IMessageQueue<ExampleMessage> queue = null;
        IPersistenceQueueStorage<ExampleMessage> queueStorage = null;
        QueueDispatcher dispatcher = null;
        try {
            serverConnection  = bus.create(new URI("bnmq://127.0.0.1:3333"));
            serverConnection.addListener(new MQConnectionListener());
            
            ISupplier supplier =  serverConnection.createSupplier("ExampleSupplier");
            System.out.println("Supplier created successfully");

            Map<String,Object> storProps = new HashMap<String,Object>();
            storProps.put("storageName","MyMemoryStorage");
            // For InMemoryDB (It's not HSQLDB!)        
            IPersistenceStorage< ExampleMessage > persistStorage =  MQFactory.getInstance().createPersistenceStorage("InMemory",storProps, ExampleMessage.class);
            
            queueStorage = persistStorage.createQueueStorage("MyQueue");
            System.out.println("QueueStorage created successfully");
            
            queue = supplier.createQueue("myqueues/queue", ExampleMessage.class,queueStorage);
            System.out.println("MessageQueue created successfully");
            serverConnection.start();
            System.out.println("Listener created successfully");
            
            dispatcher = new QueueDispatcher(queue);
            dispatcher.start();
            System.out.println("Example queue dispatcher started successfully");
            
            System.out.println("Please enter to exit");
            System.in.read();
            System.out.println("Trying to stop example queue dispatcher");
            dispatcher.stop();
        }
        catch (Exception e) {
            e.printStackTrace();
        }
        finally {
            if(serverConnection!=null) {
                System.out.println("Trying to close listener");
                serverConnection.close();
            }
        
            if(queue!=null) {
                System.out.println("Trying to stop queue");
                queue.stop();
            }
            if(queueStorage!=null) {
                System.out.println("Trying to close queue storage");
                queueStorage.close();
            }
                
            if(bus!=null) {
                try {
                    System.out.println("Trying to finallize messaging bus");
                    bus.close();
                }
                catch (Throwable e) {e = null; }
            }            
        }
    }
}
