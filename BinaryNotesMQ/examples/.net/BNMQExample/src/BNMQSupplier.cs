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
using System.Threading;
using org.bn.mq;
using org.bn.mq.net;
using org.bn.mq.examples.protocol;

namespace org.bn.mq.examples 
{
    public class BNMQSupplier {
        public BNMQSupplier() {
        }

        
        protected class MQConnectionListener : IMQConnectionListener {

            public void onDisconnected(IMQConnection connection, 
                                       ITransport networkTransport) {
                Console.WriteLine("Disconnected from: "+connection.Addr+"/"+networkTransport.ToString());
            }

            public void onConnected(IMQConnection connection, 
                                    ITransport networkTransport) {
                Console.WriteLine("Connected from: "+connection.Addr+"/"+networkTransport.ToString());
            }
        }
        
        protected class QueueDispatcher {
            private bool stopFlag = false;
            private Thread thread = null;
            private IMessageQueue<ExampleMessage> queue ;
            
            public QueueDispatcher(IMessageQueue<ExampleMessage> queue ) {
                this.queue = queue;
                thread = new Thread(new ThreadStart(this.run));
                thread.Name = "ExampleQueueDispatcher";
            }
            
            public void start() {
                stopFlag = false;
                thread.Start();
            }
            
            public void run() {
                while (!stopFlag)
                {
                    try {
                        Thread.Sleep(2000);
                        IMessage<ExampleMessage> message = queue.createMessage();
                        ExampleMessage messageBody = new ExampleMessage();
                        messageBody.Field1 = ("Field1Content");
                        messageBody.Field2 = (0xffffL);
                        Console.WriteLine("Queue: Trying to send message #"+message.Id);
                        message.Body = (messageBody);
                        message.Mandatory = true;
                        queue.sendMessage(message);
                    }
                    catch (Exception e) {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
            
            public void stop() {
                if (!stopFlag)
                {
                    stopFlag = true;
                    thread.Join();
                }
            }
        }


        public void start() {
            IMessagingBus bus = MQFactory.Instance.createMessagingBus();
            
            
            IMQConnection serverConnection  = null;
            IMessageQueue<ExampleMessage> queue = null;
            IPersistenceQueueStorage<ExampleMessage> queueStorage = null;
            QueueDispatcher dispatcher = null;
            try {
                serverConnection  = bus.create(new Uri("bnmq://127.0.0.1:3333"));
                serverConnection.addListener(new MQConnectionListener());                
                ISupplier supplier =  serverConnection.createSupplier("ExampleSupplier");
                Console.WriteLine("Supplier created successfully");

                IDictionary<String,Object> storProps = new Dictionary<String,Object>();
                storProps.Add("storageName","MyMemoryStorage");
                // For InMemoryDB (It's not HSQLDB!)        
                IPersistenceStorage< ExampleMessage > persistStorage =  MQFactory.Instance.createPersistenceStorage<ExampleMessage>("InMemory",storProps);
                
                queueStorage = persistStorage.createQueueStorage("MyQueue");
                Console.WriteLine("QueueStorage created successfully");
                
                queue = supplier.createQueue<ExampleMessage>("myqueues/queue", queueStorage);
                Console.WriteLine("MessageQueue created successfully");

                serverConnection.start();
                Console.WriteLine("Listener created successfully");
                
                dispatcher = new QueueDispatcher(queue);
                dispatcher.start();
                Console.WriteLine("Example queue dispatcher started successfully");
                
                Console.WriteLine("Please enter to exit");
                Console.ReadKey();
                Console.WriteLine("Trying to stop example queue dispatcher");
                dispatcher.stop();
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
            finally {
                if (serverConnection != null)
                {
                    Console.WriteLine("Trying to close listener");
                    serverConnection.close();
                }

                if(queue!=null) 
                {
                    Console.WriteLine("Trying to stop queue");
                    queue.stop();
                }

                if(queueStorage!=null) 
                {
                    Console.WriteLine("Trying to close queue storage");
                    queueStorage.close();
                }
                    
                if(bus!=null) {
                    Console.WriteLine("Trying to finallize messaging bus");
                    bus.close();
                }            
            }
        }
    }
}
