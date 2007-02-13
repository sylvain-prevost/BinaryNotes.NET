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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using org.bn.mq.protocol;
using org.bn.mq.net;

namespace org.bn.mq.net.tcp
{
	
	public class ConnectorTransport:Transport
	{
		protected internal AutoResetEvent awaitConnectEvent = new AutoResetEvent(false);
		protected internal ConnectorFactory factory;
		
		public ConnectorTransport(Uri addr, ConnectorFactory factory):base(addr, factory)
		{
			this.factory = factory;
		}
		
		public virtual bool connect()
		{
            IPHostEntry ipHostInfo = Dns.Resolve(getAddr().Host);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint ipe = new IPEndPoint(ipAddress, getAddr().Port);
            Socket s = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            s.Connect(ipe);
            setSocket(s);
            return true;
		}
		
		protected internal virtual void  onConnected()
		{
			awaitConnectEvent.Set();
			fireConnectedEvent();
		}
		
		protected internal virtual void  onNotConnected()
		{
            awaitConnectEvent.Set();
			factory.reconnect(this);
		}
		
		protected internal virtual void  onDisconnect()
		{
            lock (addr)
            {
                fireDisconnectedEvent();
                if (getSocket() != null)
                {
                    setSocket(null);
                    factory.reconnect(this);
                }
            }
		}

        public override void start()
        {
            factory.ConnectorStorage.addAwaitingTransport(this);
            this.finishConnect();
        }

        public override void close()
        {
            base.close();
            lock (addr) // Compact Framework doesn't supported RW-Mutex & Semaphores :(
            {
                factory.removeTransport(this);
            }            
        }
		
		protected internal override void onTransportClosed()
		{
			factory.ConnectorStorage.addDisconnectedTransport(this);
		}
		
		public virtual bool finishConnect()
		{
			try
			{
				awaitConnectEvent.WaitOne();
			}
			catch (Exception e)
			{
				// TODO
			}
			return isAvailable();
		}
	}
}