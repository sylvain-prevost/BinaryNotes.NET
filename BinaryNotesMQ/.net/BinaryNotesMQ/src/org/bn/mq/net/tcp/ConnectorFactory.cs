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
using System.Collections.Generic;
using org.bn.mq.protocol;
using org.bn.mq.net;

namespace org.bn.mq.net.tcp
{
	
	public class ConnectorFactory:SocketFactory
	{
		protected internal ConnectorStorage connectorStorage = new ConnectorStorage();
        virtual public ConnectorStorage ConnectorStorage
        {
            get
            {
                return this.connectorStorage;
            }
        }


		protected internal Connector connector;
		protected IList< ConnectorTransport > createdTransports = new List< ConnectorTransport >();
		protected internal Thread connectorThread;
		
		
		public ConnectorFactory(WriterStorage writerStorage, TransportFactory factory):base(writerStorage, factory)
		{
            connector = new Connector(connectorStorage);
            connectorThread = new Thread(new System.Threading.ThreadStart(connector.Run));
			connectorThread.Name = "BNMQ-Connector";
			connectorThread.Start();
		}
		
        protected internal virtual ConnectorTransport createTransport(Uri addr)
		{
			ConnectorTransport transport = new ConnectorTransport(addr, this);
			createdTransports.Add(transport);
			return transport;
		}

        public void removeTransport(ConnectorTransport transport)
        {
            lock (createdTransports)
            {
                createdTransports.Remove(transport);
            }
        }

        public virtual ITransport getTransport(Uri addr)
		{
			ConnectorTransport transport = null;
			bool created = false;
			lock (createdTransports)
			{
				transport = createTransport(addr);
				created = true;
			}
			/*if (created)
			{
				connectorStorage.addAwaitingTransport(transport);
				transport.finishConnect();
			}*/
			return transport;
		}
		
		public virtual void  reconnect(ConnectorTransport transport)
		{
			connectorStorage.addAwaitingTransport(transport);
		}

        public void close()
        {
            IList<ConnectorTransport> tempCreatedTransports = null;
            lock (createdTransports)
            {
                tempCreatedTransports = new List<ConnectorTransport>(createdTransports);
                createdTransports.Clear();
            }
            foreach (ConnectorTransport item in tempCreatedTransports)
            {
                item.close();
            }

            connectorStorage.close();
            connector.stop();
            try
            {
                //if(connectorThread.IsAlive) - CF is not supported!
                connectorThread.Join();
            }
            catch (Exception e)
            {
                // TODO
            }
        }
		
		~ConnectorFactory()
		{
            close();
		}
	}
}