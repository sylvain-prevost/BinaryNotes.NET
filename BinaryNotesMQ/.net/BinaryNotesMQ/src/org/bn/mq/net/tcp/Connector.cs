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
	
	public class Connector 
	{
		private ConnectorStorage storage;
		private bool finish = false;
		
		public Connector(ConnectorStorage storage)
		{
			this.storage = storage;
		}
		
		public virtual void  Run()
		{
			do 
			{
				ConnectorStorage.ConnectorStorageEvent ev = storage.getAwaitingEvent();
                if (ev != null)
				{
                    ConnectorTransport transport = ev.TransportToConnect;
					if (transport != null)
					{
						System.Console.Out.WriteLine("Connecting started for " + transport.getAddr());
						bool connected = false;
						try
						{
							connected = transport.connect();
							if (connected)
							{
								transport.onConnected();
							}
						}
						catch (Exception ex)
						{
							connected = false;
						}
						if (!connected)
						{
							System.Console.Out.WriteLine("Unable to connect for " + transport.getAddr());
							transport.setSocket(null);
							transport.onNotConnected();
						}
					}
					else if (ev.DisconnectedTransport != null)
					{
						transport = ev.DisconnectedTransport;
						transport.onDisconnect();
					}
				}
			}
			while (!finish);
		}
		
		public virtual void  stop()
		{
			finish = true;
		}
	}
}