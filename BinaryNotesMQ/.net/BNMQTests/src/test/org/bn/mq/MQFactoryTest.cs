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
using csUnit;

using org.bn.mq.net;
using org.bn.mq.net.tcp;
using org.bn.mq.protocol;
using org.bn.mq;

namespace test.org.bn.mq
{
    [TestFixture]
    public class MQFactoryTest {

        public void testCreatingObjects() {
            IMessagingBus bus = MQFactory.Instance.createMessagingBus();
            IMQConnection serverConnection  = null;
            IMQConnection clientConnection  = null;
            IMessageQueue<String> queue = null;
            try {
                serverConnection  = bus.create(new Uri("bnmq://127.0.0.1:3333"));
                ISupplier supplier =  serverConnection.createSupplier("TestSupplier");
                queue = supplier.createQueue<String>("myqueues/queue");
                serverConnection.addListener(new TestMQConnectionListener());
                serverConnection.start();
                
                clientConnection  = bus.connect(new Uri("bnmq://127.0.0.1:3333"));
                clientConnection.addListener(new TestMQConnectionListener());
                clientConnection.start();

                IRemoteSupplier remSupplier =  clientConnection.lookup("TestSupplier");
                IRemoteMessageQueue<String> remQueue = remSupplier.lookupQueue<String>("myqueues/queue");
                remQueue.addConsumer(new TestConsumer());
                for(int i=0;i<100;i++) {                            
                    IMessage<String> message = queue.createMessage("Hello"+i);
                    queue.sendMessage(message);
                }
                Thread.Sleep(1000);
                
                try {
                    IRemoteMessageQueue<String> unknownRemQueue = remSupplier.lookupQueue<String>("myqueues/queue1");
                    unknownRemQueue.addConsumer(new TestConsumer());
                    Assert.True(false);
                }
                catch(Exception ex) {
                    Assert.True(ex.ToString().Contains("unknownQueue"));
                }
            }
            finally {
                if(queue!=null) {
                    queue.stop();
                }
                if(clientConnection!=null)
                    clientConnection.close();        
                if(serverConnection!=null)
                    serverConnection.close();
                if(bus!=null) {
                    bus.close();
                }
            }
        }
        
        public void testRPCStyle() {
            IMessagingBus bus = MQFactory.Instance.createMessagingBus();
            IMQConnection serverConnection  = null;
            IMQConnection clientConnection  = null;
            IMessageQueue<String> queue = null;
            try {
                serverConnection  = bus.create(new Uri("bnmq://127.0.0.1:3333"));
                ISupplier supplier =  serverConnection.createSupplier("TestSupplier");
                queue = supplier.createQueue<String>("myqueues/queue");
                serverConnection.addListener(new TestMQConnectionListener());
                serverConnection.start();
                
                clientConnection  = bus.connect(new Uri("bnmq://127.0.0.1:3333"));
                clientConnection.addListener(new TestMQConnectionListener());
                clientConnection.start();
                IRemoteSupplier remSupplier =  clientConnection.lookup("TestSupplier");
                IRemoteMessageQueue<String> remQueue = remSupplier.lookupQueue<String>("myqueues/queue");
                remQueue.addConsumer(new TestRPCConsumer());

                String result = queue.call("Hello from Server","RPC-Consumer",20);
                Assert.Equals(result,"Hello from RPC Consumer");
                
                queue.callAsync("Hello from Server 2","RPC-Consumer", new TestRPCAsyncCallBack(),20);
                Assert.Equals(result,"Hello from RPC Consumer");
                Thread.Sleep(2000);
                
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
                throw e;
            }
            finally {
                if(queue!=null) {
                    queue.stop();
                }
                if(clientConnection!=null)
                    clientConnection.close();        
                if(serverConnection!=null)
                    serverConnection.close();
                if(bus!=null) {
                    bus.close();
                }
            }
        }

        public void testPersistence() {
            IMessagingBus bus = MQFactory.Instance.createMessagingBus();

            IDictionary<string, object> storageProps = new Dictionary<string, object>();
            // For InMemoryDB (It's not HSQLDB!)
            //storageProps.Add("storageName", "MyMemoryStorage");
            //IPersistenceStorage<String> persistStorage =  MQFactory.Instance.createPersistenceStorage<String>("InMemory",storageProps);
            
            // For SQLite 
            //Object obj = Activator.CreateInstance("System.Data.SQLite.SQLiteConnection", "SQLiteConnection");
            //System.Data.SQLite.SQLiteConnection connection = null;
            storageProps.Add("dbAssemblyName", "System.Data.SQLite");
            storageProps.Add("dbConnectionClass", "System.Data.SQLite.SQLiteConnection");
            storageProps.Add("dbConnectionString", "Data Source=test.db3");
            IPersistenceStorage<String> persistStorage = MQFactory.Instance.createPersistenceStorage<String>("SQL", storageProps);
                        
            IMQConnection serverConnection  = null;
            IMQConnection clientConnection  = null;
            IMessageQueue<String> queue = null;
            IPersistenceQueueStorage<String> queueStorage = null;
            try {
                serverConnection  = bus.create(new Uri("bnmq://127.0.0.1:3333"));
                ISupplier supplier =  serverConnection.createSupplier("TestSupplier");            
                queueStorage = persistStorage.createQueueStorage("MyQueue");
                queue = supplier.createQueue<String>("myqueues/queue", queueStorage);
                serverConnection.addListener(new TestMQConnectionListener());
                serverConnection.start();
                
                clientConnection  = bus.connect(new Uri("bnmq://127.0.0.1:3333"));
                clientConnection.addListener(new TestMQConnectionListener());
                clientConnection.start();
                IRemoteSupplier remSupplier =  clientConnection.lookup("TestSupplier");
                IRemoteMessageQueue<String> remQueue = remSupplier.lookupQueue<String>("myqueues/queue");
                remQueue.addConsumer(new TestPersistenceConsumer(),true);
                clientConnection.close();
                clientConnection = null; 
                
                for(int i=0;i<10;i++) {
                    IMessage<String> mandatoryMessage = queue.createMessage("Mandatory message "+i);
                    mandatoryMessage.Mandatory = true;
                    queue.sendMessage(mandatoryMessage);
                }
                
                clientConnection  = bus.connect(new Uri("bnmq://127.0.0.1:3333"));
                clientConnection.addListener(new TestMQConnectionListener());
                clientConnection.start();
                remSupplier =  clientConnection.lookup("TestSupplier");
                remQueue = remSupplier.lookupQueue<String>("myqueues/queue");
                remQueue.addConsumer(new TestPersistenceConsumer(),true);
                Thread.Sleep(2000);
                //clientConnection.close();            
                
            }
            finally {
                if(queue!=null) {
                    queue.stop();
                }
                if(queueStorage!=null) {
                    queueStorage.close();
                }
                if(clientConnection!=null)
                    clientConnection.close();        
                if(serverConnection!=null)
                    serverConnection.close();
                if(bus!=null) {
                    bus.close();
                }
            }        
        }

        public void testPTPSession() {
            IMessagingBus bus = MQFactory.Instance.createMessagingBus();
            IMQConnection serverConnection  = null;
            IMQConnection clientConnection  = null;
            IPTPSession<String> ptpClientSession = null;
            IPTPSession<String> ptpServerSession = null;
            try {
                serverConnection  = bus.create(new Uri("bnmq://127.0.0.1:3333"));
                ptpServerSession = serverConnection.createPTPSession<String>("serverPTP","ptpSimpleSession");
                ptpServerSession.addListener(new TestPTPSessionListener());
                serverConnection.start();

                clientConnection = bus.connect(new Uri("bnmq://127.0.0.1:3333"));
                ptpClientSession = clientConnection.createPTPSession<String>("clientPTP","ptpSimpleSession");
                ptpClientSession.addListener(new TestPTPSessionListener());            
                clientConnection.start();

                string result = ptpClientSession.call("Hello from PTP Client",20);
                Assert.Equals(result,"Hello from RPC/PTP");            
                ptpClientSession.callAsync("Hello from Server 2",new TestRPCAsyncCallBack(),20);
                            
                Thread.Sleep(2000);
                
            }
            catch (Exception e) {
                Console.WriteLine(e.ToString());
                throw e;
            }
            finally {
                if(ptpClientSession!=null) {
                    ptpClientSession.close();
                }
                if(ptpServerSession!=null) {
                    ptpServerSession.close();
                }
                
                if(clientConnection!=null)
                    clientConnection.close();        
                if(serverConnection!=null)
                    serverConnection.close();
                if(bus!=null) {
                   bus.close();
                }
            }
        }
        
        
        protected class TestMQConnectionListener : IMQConnectionListener {

            public void onDisconnected(IMQConnection connection, 
                                       ITransport networkTransport) {
                Console.WriteLine("onDisconnected: "+connection.ToString()+"/"+networkTransport.ToString());
            }

            public void onConnected(IMQConnection connection, 
                                    ITransport networkTransport) {
                Console.WriteLine("onConnected: "+connection.ToString()+"/"+networkTransport.ToString());
            }
        }

        protected class TestConsumer : IConsumer<String> {
            public String Id
            {
                get { return this.ToString(); }
            }
            public String onMessage(IMessage<String> message)
            {
                Console.WriteLine("Received message: "+message.Body);
                return null;
            }
        }
        
        protected class TestRPCConsumer : IConsumer<String> {
            
            public String Id 
            {
                get { return "RPC-Consumer"; }
            }
            public String onMessage(IMessage<String> message) 
            {
                Console.WriteLine("Received Call: "+message.Body);
                return "Hello from RPC Consumer";
            }
        }    
        
        protected class TestRPCAsyncCallBack : ICallAsyncListener<String> {

            public void onCallResult(String request,String result) 
            {
                Console.WriteLine("Received call result: "+result);
            }

            public void onCallTimeout(String request)
            {
                Console.WriteLine("!!! Received call timeout for request: "+request);
            }
        }
        
        protected class TestPersistenceConsumer : IConsumer<String> {
            
            public string Id 
            {
                get { return "Persistence-Consumer"; }
            }
            
            public String onMessage(IMessage<String> message) 
            {
                Console.WriteLine("Persistence consumer received : "+message.Body);
                return null;
            }
        }

        protected class TestPTPSessionListener : IPTPSessionListener<String> {

            public String onMessage(IPTPSession<String> session, ITransport transport, IMessage<String> message) {
                Console.WriteLine ("Received PTP session call: "+message.Body);
                return "Hello from RPC/PTP";
            }
        }


    }
}