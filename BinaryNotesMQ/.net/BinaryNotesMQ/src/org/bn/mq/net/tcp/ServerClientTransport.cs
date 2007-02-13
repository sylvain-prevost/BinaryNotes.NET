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
	
	public class ServerClientTransport:Transport
	{
		protected internal AcceptorFactory factory;
		protected internal ServerTransport serverTransport;
		
		public ServerClientTransport(Uri addr, ServerTransport server, AcceptorFactory factory):base(addr, factory)
		{
			this.factory = factory;
			this.serverTransport = server;
		}


        protected internal override void fireReceivedData(ByteBuffer packet)
		{
			try
			{
                IList<MessageEnvelope> messages = messageCoder.decode(packet);
                if (messages != null)
                {
                    foreach (MessageEnvelope message in messages)
                    {
                       serverTransport.fireReceivedData(message, this);
                    }
                }

			}
			catch (System.Exception ex)
			{
				System.Console.Error.WriteLine("Pkt: " + packet);
				System.Console.Error.WriteLine("Pkt len" + packet.Limit);
				throw ex;
			}
		}

        public override void start()
        {
        }
		
		protected internal override void  onTransportClosed()
		{
			serverTransport.removeClient(this);
			//fireDisconnectedEvent();
			base.onTransportClosed();
		}
	}
}