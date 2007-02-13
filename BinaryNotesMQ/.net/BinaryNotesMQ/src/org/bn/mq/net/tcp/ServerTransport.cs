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
	
	public class ServerTransport:Transport
	{
		virtual public Socket ServerSocket
		{
			get
			{
				return serverChannel;
			}
			
			set
			{
				this.serverChannel = value;
			}
			
		}
        private Socket serverChannel = null;
		protected List < ServerClientTransport > clients = new List < ServerClientTransport >();
		protected internal AcceptorFactory acceptorFactory;
		
		public ServerTransport(Uri addr, AcceptorFactory acceptorFactory):base(addr, null)
		{
			setSocket(null);
			this.acceptorFactory = acceptorFactory;
			this.socketFactory = acceptorFactory;
		}
		
		public virtual void  startListener()
		{
			lock (this)
			{
				close();
                IPHostEntry ipHostInfo = Dns.Resolve(getAddr().Host);
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint localEndPoint = new IPEndPoint(ipAddress, getAddr().Port);
                serverChannel = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
                serverChannel.Bind(localEndPoint);
                serverChannel.Listen(100);
                serverChannel.BeginAccept(this.acceptClient, serverChannel);


				/*serverChannel = ServerSocketChannel.open();
				serverChannel.configureBlocking(false);
				serverChannel.socket().setReuseAddress(true);
				serverChannel.socket().bind(new InetSocketAddress(getAddr().getHost(), getAddr().getPort()));
                 */
			}
		}

        public override void start()
        {
            startListener();
        }
		
		public override void  close()
		{
			if (isAvailable())
			{
				try
				{
                    lock (addr)
                    {
                        //serverChannel.Shutdown(SocketShutdown.Both);
                        //serverChannel.Disconnect(true);
                        serverChannel.Close();
                        serverChannel = null;
                    }
				}
				catch (Exception e) {
                    Console.WriteLine(e);
                }
			}
			
			lock (clients)
			{
				foreach(ServerClientTransport transport in clients)
				{
					transport.close();
				}
				clients.Clear();
			}
		}
		
				
		public override bool isAvailable()
		{
			lock (addr)
			{
                return serverChannel != null 
#if !PocketPC
                    && serverChannel.IsBound
#endif
                ;
			}
		}
		
		public virtual void acceptClient(IAsyncResult asyncResult)
		{
            lock (addr)
            {
                if (isAvailable())
                {
                    try
                    {
                        Socket listener = (Socket)asyncResult.AsyncState;
                        if (asyncResult.IsCompleted)
                        {
                            Socket clientSocket = listener.EndAccept(asyncResult);
                            if (clientSocket != null)
                            {
                                ServerClientTransport transport =
                                    new ServerClientTransport(
                                        new Uri("bnmq://" + clientSocket.RemoteEndPoint.ToString()),
                                    this,
                                    acceptorFactory
                                );
                                transport.setSocket(clientSocket);
                                lock (clients)
                                {
                                    clients.Add(transport);
                                    fireConnectedEvent(transport);
                                }
                            }
                        }
                    }
                    finally
                    {
                        serverChannel.BeginAccept(this.acceptClient, serverChannel);
                    }
                }
            }
		}
		
		public virtual void  removeClient(ServerClientTransport transport)
		{
			lock (clients)
			{
				clients.Remove(transport);
				fireDisconnectedEvent(transport);
			}
		}
		
		protected internal virtual void  fireReceivedData(MessageEnvelope message, ServerClientTransport client)
		{
			doProcessReceivedData(message, client);
		}
		
		protected internal virtual void  fireConnectedEvent(ServerClientTransport client)
		{
            lock(listeners)
            {
				foreach(ITransportConnectionListener listener in listeners)
				{
					listener.onConnected(client);
				}
			}		
        }
		
		protected internal virtual void  fireDisconnectedEvent(ServerClientTransport client)
		{
            lock(listeners)
            {
                foreach (ITransportConnectionListener listener in listeners)
                {
					listener.onDisconnected(client);
				}
            }
		}
		
		
		~ServerTransport()
		{
			close();
		}
		
		protected internal override void  onTransportClosed()
		{
		}
	}
}