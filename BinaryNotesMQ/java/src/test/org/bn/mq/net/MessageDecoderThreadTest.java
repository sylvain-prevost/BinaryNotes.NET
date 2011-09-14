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
package test.org.bn.mq.net;

import java.net.URI;
import junit.framework.TestCase;

import org.bn.mq.net.ITransport;
import org.bn.mq.net.ITransportConnectionListener;
import org.bn.mq.net.ASN1TransportMessageCoderFactory;
import org.bn.mq.net.ITransportCallListener;
import org.bn.mq.net.ITransportReader;
import org.bn.mq.net.tcp.TransportFactory;
import org.bn.mq.protocol.MessageBody;
import org.bn.mq.protocol.MessageEnvelope;
import org.bn.mq.protocol.MessageUserBody;


public class MessageDecoderThreadTest extends TestCase {

    public MessageDecoderThreadTest(String sTestName) {
        super(sTestName);
    }
    
    protected MessageEnvelope createMessage(String vl) {
        MessageEnvelope message = new MessageEnvelope();
        message.setId("MsgId-"+vl);
        MessageBody msgBody = new MessageBody();
        MessageUserBody userBody = new MessageUserBody();
        userBody.setUserBody(new byte[] { (byte)0xFF, (byte)0xFE });
        userBody.setConsumerId(this.toString());
        userBody.setQueuePath("testQueuePath/Temp");
        msgBody.selectMessageUserBody(userBody);
        message.setBody(msgBody);
        return message;
    }

    public void testTakeMessage() throws Exception {
        final String connectionString = "bnmq://localhost:3333";
        TransportFactory conFactory = new TransportFactory();
        try {
            conFactory.setTransportMessageCoderFactory(new ASN1TransportMessageCoderFactory());
            
            ITransport server = conFactory.getServerTransport(new URI(connectionString));
            assertNotNull(server);
            MessageListener ml = new MessageListener(this);
            server.addConnectionListener(ml);
            server.addReader(ml);
            server.start();
            
            ITransport client = conFactory.getClientTransport(new URI(connectionString));
            assertNotNull(client);            
            ml = new MessageListener(this);
            client.addConnectionListener(ml);
            client.addReader(ml);
            client.start();

                
            client.send(createMessage("AAAaasasasasassas"));
            client.sendAsync(createMessage("Two"));
            Thread.sleep(500);
            client.close();
            server.close();
            
        }
        finally {
            conFactory.close();
        }
        System.out.println("Finished: testTakeMessage");
    }
    
    public void testCall() throws Exception {
        final String connectionString = "bnmq://localhost:3333";
        TransportFactory conFactory = new TransportFactory();
        try {
            conFactory.setTransportMessageCoderFactory(new ASN1TransportMessageCoderFactory());
            System.out.println("[testCall] created server");
            ITransport server = conFactory.getServerTransport(new URI(connectionString));
            assertNotNull(server);
            CallMessageListener cl = new CallMessageListener(this);
            server.addConnectionListener(cl);
            server.addReader(cl);
            server.start();
            System.out.println("[testCall] server started");
            
            ITransport client = conFactory.getClientTransport(new URI(connectionString));
            assertNotNull(client);
            client.start();
            System.out.println("[testCall] client started");
            
            MessageEnvelope result = client.call(createMessage("Call"), 10);
            System.out.println("Result call received with Id:"+result.getId()+" has been received successfully");
            client.close();
            server.close();
            
        }
        finally {
            conFactory.close();
        }
        System.out.println("Finished: testCall");
    }    

    public void testAsyncCall() throws Exception {
        final String connectionString = "bnmq://localhost:3333";
        TransportFactory conFactory = new TransportFactory();
        try {
            conFactory.setTransportMessageCoderFactory(new ASN1TransportMessageCoderFactory());
            
            ITransport server = conFactory.getServerTransport(new URI(connectionString));
            assertNotNull(server);
            CallMessageListener cl = new CallMessageListener(this);
            server.addConnectionListener(cl);
            server.addReader(cl);
            server.start();
            
            ITransport client = conFactory.getClientTransport(new URI(connectionString));
            assertNotNull(client);
            client.start();
            
            client.callAsync(createMessage("CallAsync"), new AsyncCallMessageListener());
            Thread.sleep(500);
            client.close();
            server.close();
            
        }
        finally {
            conFactory.close();
        }
        System.out.println("Finished: testAsyncCall");
    }    

    private class MessageListener implements ITransportConnectionListener, ITransportReader {         
        private MessageDecoderThreadTest parent;
        private int counter = 0;
        public MessageListener(MessageDecoderThreadTest parent) {
            this.parent = parent;
        }
        public boolean onReceive(MessageEnvelope message, ITransport transport) {
            System.out.println("Message from "+transport+" with Id:"+message.getId()+" has been received successfully");
            try {
                if(counter<10) {
                    transport.send(parent.createMessage("Three" + counter++));
                    transport.sendAsync(parent.createMessage("Four" + counter++));
                }
                
            }
            catch (Exception e) {
                System.err.println(e);
                e.printStackTrace();
            }
            return true;
        }

        public void onConnected(ITransport transport) {
            System.out.println("Connected from "+transport+". Addr:"+transport.getAddr());
        }

        public void onDisconnected(ITransport transport) {
            System.out.println("Disconnected from "+transport+". Addr:"+transport.getAddr());
        }
    }
    
    private class CallMessageListener implements ITransportConnectionListener, ITransportReader {         
        private MessageDecoderThreadTest parent;
        private int counter = 0;
        public CallMessageListener(MessageDecoderThreadTest parent) {
            this.parent = parent;
        }
        public boolean onReceive(MessageEnvelope message, ITransport transport) {
            System.out.println("Call from "+transport+" with Id:"+message.getId()+" has been received successfully");
            try {
                MessageEnvelope result = createMessage("result");
                result.setId(message.getId());
                transport.sendAsync(result);
            }
            catch (Exception e) {
                System.err.println(e);
            }
            return true;
        }

        public void onConnected(ITransport transport) {
            System.out.println("Connected from "+transport+". Addr:"+transport.getAddr());
        }

        public void onDisconnected(ITransport transport) {
            System.out.println("Disconnected from "+transport+". Addr:"+transport.getAddr());
        }
    }    
    
    private class AsyncCallMessageListener implements ITransportCallListener {

        public void onCallResult(MessageEnvelope request, 
                                 MessageEnvelope result) {
            System.out.println("Call result received: " + result.toString());
        }

        public void onCallTimeout(MessageEnvelope request) {
            System.out.println("!! Call result timeout !!. Request: " + request.toString());
        }
    }
}
