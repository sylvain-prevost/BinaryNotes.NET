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
using System.Threading;
using csUnit;

using org.bn.mq.net;
using org.bn.mq.net.tcp;

namespace test.org.bn.mq.net.tcp {

    [TestFixture]
    public class TransportFactoryTest {

        public TransportFactoryTest() {        
        }
           
        public void testGetServerTransport() {
            const string connectionString = "bnmq://localhost:3333";

            TransportFactory conFactory = new TransportFactory();
            try {
                conFactory.TransportMessageCoderFactory = new ASN1TransportMessageCoderFactory();
                ITransport transport = conFactory.getServerTransport(new Uri(connectionString));
                Assert.NotNull(transport);
                transport.start();
                Thread.Sleep(500);
                transport.close();
            }
            finally {
                conFactory.close();
            }
            Console.WriteLine("Finished: testGetServerTransport");
        }
        
        public void testSendRecvServerTransport() {
            const String connectionString = "bnmq://localhost:3333";
            TransportFactory conFactory = new TransportFactory();
            try {
                conFactory.TransportMessageCoderFactory = new ASN1TransportMessageCoderFactory();
                ITransport server = conFactory.getServerTransport(new Uri(connectionString));
                Assert.NotNull(server);
                server.start();
                ITransport client = conFactory.getClientTransport(new Uri(connectionString));
                Assert.NotNull(client);
                client.start();

                byte[] buffer = new byte[] { 0x01, 0x02, 0x03, 0x04 };
                for(int i=0;i<255;i++) {
                    client.sendAsync(buffer);
                }
                Thread.Sleep(500);                
                server.close();
                client.close();
            }
            finally {
                conFactory.close();
            }
            Console.WriteLine("Finished: testSendRecvServerTransport");
        }    
    }
}