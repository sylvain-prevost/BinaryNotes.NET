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

using System;
using System.Collections.Generic;
using org.bn.mq;
using org.bn.mq.net;
using org.bn.mq.examples.protocol;

namespace org.bn.mq.examples 
{
    public class BNMQConsumer {
        IMQConnection clientConnection  = null;
        IRemoteMessageQueue<ExampleMessage> queue = null;

        public BNMQConsumer() {
        }
        
        protected class MQConnectionListener : IMQConnectionListener {
            private BNMQConsumer parent;
            public MQConnectionListener(BNMQConsumer consumer)
            {
                parent = consumer;
            }

            public void onDisconnected(IMQConnection connection, 
                                       ITransport networkTransport) {
                Console.WriteLine("Disconnected from: "+connection.Addr+"/"+networkTransport.ToString());
            }

            public void onConnected(IMQConnection connection, 
                                    ITransport networkTransport) {
                Console.WriteLine("Connected to: "+connection.Addr+"/"+networkTransport.ToString());
                try {
                    parent.doSubscribe();
                }
                catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
            }
        }
        
        protected class ExampleConsumer : IConsumer<ExampleMessage> {
            public String Id {
                get { return ".NetExampleConsumer"; }
            }
            public ExampleMessage onMessage(IMessage<ExampleMessage> message) {
                Console.WriteLine("Received message #"+message.Id+" from supplier. Has body:"+message.Body);
                return null;
            }
        }
        
        protected void doSubscribe() {
            Console.WriteLine("Trying to lookup supplier");
            IRemoteSupplier remSupplier =  clientConnection.lookup("ExampleSupplier");
            queue = remSupplier.lookupQueue<ExampleMessage>("myqueues/queue");            
            Console.WriteLine("Trying to lookup & persistence subscribe consumer");
            queue.addConsumer(new ExampleConsumer(),true);
            Console.WriteLine("Subscribed procedure finished");
        }

        public void start() {
            IMessagingBus bus = MQFactory.Instance.createMessagingBus();

            try {
                Console.WriteLine("Creating connector");
                clientConnection  = bus.connect(new Uri("bnmq://127.0.0.1:3333"));            
                clientConnection.addListener(new MQConnectionListener(this));
                clientConnection.start();
                
                //doSubscribe();
                Console.WriteLine("Please enter to exit");
                Console.ReadKey();
                //System.in.read();
                Console.WriteLine("Trying to stop example queue dispatcher");
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }        
            finally {
                if(queue!=null) {
                    Console.WriteLine("Trying to stop receiveing from remote queue");
                    queue.stop();
                }
                if(clientConnection!=null) {
                    Console.WriteLine("Trying to close connector");
                    clientConnection.close();
                }
                    
                if(bus!=null) {
                    Console.WriteLine("Trying to finallize messaging bus");
                    bus.close();
                }            
            }
        }
        
        
    }
}