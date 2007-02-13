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
package test.org.bn.mq.net.tcp;

import java.io.IOException;

import java.net.URI;
import java.net.URISyntaxException;

import junit.framework.TestCase;

import org.bn.mq.net.ITransport;
import org.bn.mq.net.ASN1TransportMessageCoderFactory;
import org.bn.mq.net.tcp.TransportFactory;

public class TransportFactoryTest extends TestCase {
    public TransportFactoryTest(String sTestName) {
        super(sTestName);
    }
       
    public void testGetServerTransport() throws URISyntaxException, 
                                          InterruptedException, IOException, 
                                          Throwable {
        final String connectionString = "bnmq://localhost:3333";
        TransportFactory conFactory = new TransportFactory();
        try {
            conFactory.setTransportMessageCoderFactory(new ASN1TransportMessageCoderFactory());
            ITransport transport = conFactory.getServerTransport(new URI(connectionString));            
            assertNotNull(transport);
            transport.start();
            Thread.sleep(500);
            transport.close();
        }
        finally {
            conFactory.close();
        }
        System.out.println("Finished: testGetServerTransport");
    }
    
    public void testSendRecvServerTransport() throws URISyntaxException, 
                                          InterruptedException, IOException, 
                                          Throwable {
        final String connectionString = "bnmq://localhost:3333";
        TransportFactory conFactory = new TransportFactory();
        try {
            conFactory.setTransportMessageCoderFactory(new ASN1TransportMessageCoderFactory());
            ITransport server = conFactory.getServerTransport(new URI(connectionString));
            assertNotNull(server);
            server.start();
            ITransport client = conFactory.getClientTransport(new URI(connectionString));
            assertNotNull(client);
            client.start();
            Thread.sleep(500);
            final byte[] buffer = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            for(int i=0;i<255;i++) {
                client.sendAsync(buffer);
            }
            Thread.sleep(500);
            client.close();
            server.close();
        }
        finally {
            conFactory.close();
        }
        System.out.println("Finished: testSendRecvServerTransport");
    }    
}
