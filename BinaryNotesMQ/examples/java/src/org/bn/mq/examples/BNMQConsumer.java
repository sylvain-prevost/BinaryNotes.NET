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
import org.bn.mq.*;
import org.bn.mq.examples.protocol.*;
import org.bn.mq.net.ITransport;

public class BNMQConsumer {
    IMQConnection clientConnection  = null;
    IRemoteMessageQueue<ExampleMessage> queue = null;

    public BNMQConsumer() {
    }

    public static void main(String[] args) {
        BNMQConsumer app = new BNMQConsumer();
        app.start();
    }
    
    protected class MQConnectionListener implements IMQConnectionListener {

        public void onDisconnected(IMQConnection connection, 
                                   ITransport networkTransport) {
            System.out.println("Disconnected from: "+connection.getAddr()+"/"+networkTransport.toString());
        }

        public void onConnected(IMQConnection connection, 
                                ITransport networkTransport) {
            System.out.println("Connected to: "+connection.getAddr()+"/"+networkTransport.toString());
            try {
                doSubscribe();
            }
            catch (Exception e) {
                e.printStackTrace();
            }
        }
    }
    
    protected class ExampleConsumer implements IConsumer<ExampleMessage> {
        public String getId() {
            return "JavaExampleConsumer";
        }
        public ExampleMessage onMessage(IMessage<ExampleMessage> message) {
            System.out.println("Received message #"+message.getId()+" from supplier. Has body:"+message.getBody());
            return null;
        }
    }
    
    protected void doSubscribe() throws Exception {
        System.out.println("Trying to lookup supplier");
        IRemoteSupplier remSupplier =  clientConnection.lookup("ExampleSupplier");
        queue = remSupplier.lookupQueue("myqueues/queue", ExampleMessage.class);            
        System.out.println("Trying to lookup & persistence subscribe consumer");
        queue.addConsumer(new ExampleConsumer(),true);
    }
    private void start() {
        IMessagingBus bus = MQFactory.getInstance().createMessagingBus();

        try {
            System.out.println("Creating connector");
            clientConnection  = bus.connect(new URI("bnmq://127.0.0.1:3333"));            
            clientConnection.addListener(new MQConnectionListener());
            clientConnection.start();
            
            //doSubscribe();
            System.out.println("Please enter to exit");
            System.in.read();
            System.out.println("Trying to stop example queue dispatcher");
        }
        catch (Exception e) {
            e.printStackTrace();
        }        
        finally {
            if(queue!=null) {
                System.out.println("Trying to stop receiveing from remote queue");
                queue.stop();
            }
            if(clientConnection!=null) {
                System.out.println("Trying to close connector");
                clientConnection.close();
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
