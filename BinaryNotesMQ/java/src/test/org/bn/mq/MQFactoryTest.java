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

package test.org.bn.mq;

import java.net.URI;

import java.util.Calendar;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;

import junit.framework.TestCase;

import org.bn.mq.ICallAsyncListener;
import org.bn.mq.IConsumer;
import org.bn.mq.IMQConnection;
import org.bn.mq.IMQConnectionListener;
import org.bn.mq.IMessage;
import org.bn.mq.IMessageQueue;
import org.bn.mq.IMessagingBus;
import org.bn.mq.IPTPSession;
import org.bn.mq.IPTPSessionListener;
import org.bn.mq.IPersistenceQueueStorage;
import org.bn.mq.IPersistenceStorage;
import org.bn.mq.IRemoteMessageQueue;
import org.bn.mq.IRemoteSupplier;
import org.bn.mq.ISupplier;
import org.bn.mq.MQFactory;
import org.bn.mq.net.ITransport;

import test.org.bn.coders.test_asn.TestSimpleSequence;


public class MQFactoryTest extends TestCase {
    public MQFactoryTest(String sTestName) {
        super(sTestName);
    }

    /**
     * @see MQFactory#createMessagingBus()
     */
    public void testCreatingObjects() {
        IMessagingBus bus = MQFactory.getInstance().createMessagingBus();
        IMQConnection serverConnection  = null;
        IMQConnection clientConnection  = null;
        IMessageQueue<String> queue = null;
        try {
            serverConnection  = bus.create(new URI("bnmq://127.0.0.1:3333"));
            ISupplier supplier =  serverConnection.createSupplier("TestSupplier");
            queue = supplier.createQueue("myqueues/queue", String.class);
            serverConnection.addListener(new TestMQConnectionListener());
            serverConnection.start();
            
            clientConnection  = bus.connect(new URI("bnmq://127.0.0.1:3333"));
            clientConnection.addListener(new TestMQConnectionListener());
            clientConnection.start();
            IRemoteSupplier remSupplier =  clientConnection.lookup("TestSupplier");
            IRemoteMessageQueue<String> remQueue = remSupplier.lookupQueue("myqueues/queue", String.class);            
            remQueue.addConsumer(new TestConsumer());
            for(int i=0;i<100;i++) {                            
                IMessage<String> message = queue.createMessage("Hello"+i);
                queue.sendMessage(message);
            }
            Thread.sleep(1000);
            
            try {
                IRemoteMessageQueue unknownRemQueue = remSupplier.lookupQueue("myqueues/queue1", String.class);
                unknownRemQueue.addConsumer(new TestConsumer());
                assertTrue(false);
            }
            catch(Exception ex) {
                assertTrue(ex.toString().contains("unknownQueue"));
            }
        }
        catch (Exception e) {
            e.printStackTrace();
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
                try {
                    bus.close();
                }
                catch (Throwable e) {e = null; }
            }
        }
    }
    
    public void testRPCStyle() throws Exception {
        IMessagingBus bus = MQFactory.getInstance().createMessagingBus();
        IMQConnection serverConnection  = null;
        IMQConnection clientConnection  = null;
        IMessageQueue<String> queue = null;
        try {
            serverConnection  = bus.create(new URI("bnmq://127.0.0.1:3333"));
            ISupplier supplier =  serverConnection.createSupplier("TestSupplier");
            queue = supplier.createQueue("myqueues/queue", String.class);
            serverConnection.addListener(new TestMQConnectionListener());
            serverConnection.start();
            
            clientConnection  = bus.connect(new URI("bnmq://127.0.0.1:3333"));
            clientConnection.addListener(new TestMQConnectionListener());
            clientConnection.start();
            IRemoteSupplier remSupplier =  clientConnection.lookup("TestSupplier");
            IRemoteMessageQueue<String> remQueue = remSupplier.lookupQueue("myqueues/queue", String.class);
            remQueue.addConsumer(new TestRPCConsumer());

            String result = queue.call("Hello from Server","RPC-Consumer",20);
            assertEquals(result,"Hello from RPC Consumer");
            
            queue.callAsync("Hello from Server 2","RPC-Consumer", new TestRPCAsyncCallBack(),20);
            assertEquals(result,"Hello from RPC Consumer");
            Thread.sleep(2000);
            
        }
        catch (Exception e) {
            e.printStackTrace();
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
                try {
                    bus.close();
                }
                catch (Throwable e) {e = null; }
            }
        }
    }

    public void testPersistence() throws Exception {
        IMessagingBus bus = MQFactory.getInstance().createMessagingBus();
        
        
        IMQConnection serverConnection  = null;
        IMQConnection clientConnection  = null;
        IMessageQueue<String> queue = null;
        IPersistenceQueueStorage<String> queueStorage = null;
        try {
            serverConnection  = bus.create(new URI("bnmq://127.0.0.1:3333"));
            ISupplier supplier =  serverConnection.createSupplier("TestSupplier");            

            Map<String,Object> storProps = new HashMap<String,Object>();
            // For InMemoryDB (It's not HSQLDB!)
            storProps.put("storageName","MyMemoryStorage");
            IPersistenceStorage<String> persistStorage =  MQFactory.getInstance().createPersistenceStorage("InMemory",storProps,String.class);
            
            // For HSQLDB
            //Class.forName("org.hsqldb.jdbcDriver");
            //storProps.put("dbConnectionString","jdbc:hsqldb:mem:aname");            
            //IPersistenceStorage<String> persistStorage =  MQFactory.getInstance().createPersistenceStorage("SQL",storProps,String.class);
            
            queueStorage = persistStorage.createQueueStorage("MyQueue");
            queue = supplier.createQueue("myqueues/queue", String.class,queueStorage);
            serverConnection.addListener(new TestMQConnectionListener());
            serverConnection.start();
            
            clientConnection  = bus.connect(new URI("bnmq://127.0.0.1:3333"));
            clientConnection.addListener(new TestMQConnectionListener());
            clientConnection.start();
            
            IRemoteSupplier remSupplier =  clientConnection.lookup("TestSupplier");
            IRemoteMessageQueue<String> remQueue = remSupplier.lookupQueue("myqueues/queue", String.class);
            remQueue.addConsumer(new TestPersistenceConsumer(),true);
            clientConnection.close();
            clientConnection = null; 
            
            for(int i=0;i<10;i++) {
                IMessage<String> mandatoryMessage = queue.createMessage("Mandatory message "+i);
                mandatoryMessage.setMandatory(true);
                queue.sendMessage(mandatoryMessage);
            }
            
            clientConnection  = bus.connect(new URI("bnmq://127.0.0.1:3333"));
            clientConnection.addListener(new TestMQConnectionListener());
            clientConnection.start();
            remSupplier =  clientConnection.lookup("TestSupplier");
            remQueue = remSupplier.lookupQueue("myqueues/queue", String.class);
            remQueue.addConsumer(new TestPersistenceConsumer(),true);
            Thread.sleep(2000);
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
                try {
                    bus.close();
                }
                catch (Throwable e) {e = null; }
            }
        }        
    }
    
    public void testPTPSession() throws Exception {
        IMessagingBus bus = MQFactory.getInstance().createMessagingBus();
        IMQConnection serverConnection  = null;
        IMQConnection clientConnection  = null;
        IPTPSession<String> ptpClientSession = null;
        IPTPSession<String> ptpServerSession = null;
        try {
            serverConnection  = bus.create(new URI("bnmq://127.0.0.1:3333"));
            ptpServerSession = serverConnection.createPTPSession("serverPTP","ptpSimpleSession",String.class);
            ptpServerSession.addListener(new TestPTPSessionListener());
            serverConnection.start();
            
            clientConnection  = bus.connect(new URI("bnmq://127.0.0.1:3333"));
            ptpClientSession = clientConnection.createPTPSession("clientPTP","ptpSimpleSession",String.class);
            ptpClientSession.addListener(new TestPTPSessionListener());            
            clientConnection.start();

            String result = ptpClientSession.call("Hello from PTP Client",20);
            assertEquals(result,"Hello from RPC/PTP");            
            ptpClientSession.callAsync("Hello from Server 2",new TestRPCAsyncCallBack(),20);
                        
            Thread.sleep(2000);
            
        }
        catch (Exception e) {
            e.printStackTrace();
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
                try {
                    bus.close();
                }
                catch (Throwable e) {e = null; }
            }
        }
    }
    
    
    /*public void testPTPSessionPerf() throws Exception {
        IMessagingBus bus = MQFactory.getInstance().createMessagingBus();
        IMQConnection serverConnection  = null;
        IMQConnection clientConnection  = null;
        IPTPSession<TestSimpleSequence> ptpClientSession = null;
        IPTPSession<TestSimpleSequence> ptpServerSession = null;
        try {
            serverConnection  = bus.create(new URI("bnmq://127.0.0.1:3333"));
            
            ptpServerSession = serverConnection.createPTPSession("serverPTP","ptpSimpleSession",TestSimpleSequence.class);
            ptpServerSession.addListener(new TestPTPSessionPerfServerListener());
            serverConnection.start();
            
            clientConnection  = bus.connect(new URI("bnmq://127.0.0.1:3333"));
            ptpClientSession = clientConnection.createPTPSession("clientPTP","ptpSimpleSession",TestSimpleSequence.class);
            clientConnection.start();

            int currentSec = Calendar.getInstance().get(Calendar.SECOND);            
            int msgPerSec = 0;
            TestSimpleSequence sequence = new TestSimpleSequence();            
            sequence.setField2("2Hello!");
            sequence.setField3("3Hello!");
            TestPTPPerfRPCAsyncCallBack callBack = new TestPTPPerfRPCAsyncCallBack();
            for(int i=0;i<100000;i++) {
                sequence.setField1((long)i);
                ptpClientSession.callAsync(sequence , callBack ,20);
                int nowSec = Calendar.getInstance().get(Calendar.SECOND);
                if(nowSec!=currentSec) {
                    System.out.println("Send msg per sec: "+msgPerSec);
                    msgPerSec= 0;                    
                    currentSec = nowSec;
                }
                else
                    msgPerSec++;
            }
                        
            Thread.sleep(1000);
            
        }
        catch (Exception e) {
            e.printStackTrace();
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
                try {
                    bus.close();
                }
                catch (Throwable e) {e = null; }
            }
        }
    }*/
    
    
    protected class TestMQConnectionListener implements IMQConnectionListener {

        public void onDisconnected(IMQConnection connection, 
                                   ITransport networkTransport) {
            System.out.println("onDisconnected: "+connection.toString()+"/"+networkTransport.toString());
        }

        public void onConnected(IMQConnection connection, 
                                ITransport networkTransport) {
            System.out.println("onConnected: "+connection.toString()+"/"+networkTransport.toString());
        }
    }

    protected class TestConsumer implements IConsumer<String> {
        public String getId() {
            return this.toString();
        }
        public String onMessage(IMessage<String> message) {
            System.out.println("Received message: "+message.getBody());
            return null;
        }
    }
    
    protected class TestRPCConsumer implements IConsumer<String> {
        
        public String getId() {
            return "RPC-Consumer";
        }
        public String onMessage(IMessage<String> message) {
            System.out.println("Received Call: "+message.getBody());
            return "Hello from RPC Consumer";
        }
    }    
    
    protected class TestRPCAsyncCallBack implements ICallAsyncListener<String> {

        public void onCallResult(String request,String result) {
            System.out.println("Received call result: "+result);
        }

        public void onCallTimeout(String request) {
            System.out.println("!!! Received call timeout for request: "+request);
        }
    }
    
    protected class TestPersistenceConsumer implements IConsumer<String> {
        
        public String getId() {
            return "Persistence-Consumer";
        }
        public String onMessage(IMessage<String> message) {
            System.out.println("Persistence consumer received : "+message.getBody());
            return null;
        }
    }
    
    protected class TestPTPSessionListener implements IPTPSessionListener<String> {

        public String onMessage(IPTPSession<String> session, ITransport transport, 
                                IMessage<String> message) {
            System.out.println("Received PTP session call: "+message.getBody());
            return "Hello from RPC/PTP";
        }
    }

    protected class TestPTPSessionPerfServerListener implements IPTPSessionListener<TestSimpleSequence> {
        private TestSimpleSequence sequence = new TestSimpleSequence();

        public TestSimpleSequence onMessage(IPTPSession<TestSimpleSequence> session, 
                                            ITransport transport, 
                                            IMessage<TestSimpleSequence> message) {
            sequence.setField1((long)1);
            sequence.setField2("2Hello!");
            sequence.setField3("3Hello!");
            return sequence;
        }
    }
    
    protected class TestPTPPerfRPCAsyncCallBack implements ICallAsyncListener<TestSimpleSequence> {
        int currentSec = Calendar.getInstance().get(Calendar.SECOND);            
        int msgPerSec = 0;

        public void onCallResult(TestSimpleSequence request, TestSimpleSequence result) {
            int nowSec = Calendar.getInstance().get(Calendar.SECOND);
            if(nowSec!=currentSec) {
                System.out.println("Recv msg per sec: "+msgPerSec);
                msgPerSec= 0;
                currentSec = nowSec;
            }
            else
                msgPerSec++;        
        }

        public void onCallTimeout(TestSimpleSequence request) {
            System.out.println("Call timeout!!! ");
        }
    }
}
