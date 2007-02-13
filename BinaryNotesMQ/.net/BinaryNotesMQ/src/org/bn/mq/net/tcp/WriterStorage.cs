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
	
	public class WriterStorage
	{
        protected LinkedList<TransportPacket> queue = new LinkedList<TransportPacket>();
        protected internal AutoResetEvent awaitPacketEvent = new AutoResetEvent(false);
        private bool finishThread = false;
        protected internal ITransportMessageCoder messageCoder;
        protected LinkedList<Transport> aliveRequestCheckList = new LinkedList<Transport>();

		virtual public TransportPacket getPacket()
		{
			lock (queue)
			{
				if (queue.Count > 0)
				{
					TransportPacket result = queue.First.Value;
					queue.RemoveFirst();
					return result;
				}
			}
			return null;			
		}
		
        virtual public ITransportMessageCoder MessageCoder
		{
			set
			{
				this.messageCoder = value;
			}
			
		}					
		
		
		public WriterStorage()
		{
			
		}
		
		public virtual TransportPacket waitPacket()
		{
			TransportPacket result = null;
			do 
			{
				lock (queue)
				{
					if (queue.Count > 0 )
					{
						result = queue.First.Value;
						queue.RemoveFirst();
					}
				}
				if (result == null)
				{
					try
					{
						if (!awaitPacketEvent.WaitOne(10000, false))
						{
							pushAliveReqForRecipients();
						}
					}
					catch (System.Exception ex)
					{
						ex = null;
					}
				}
			}
			while (result == null && !finishThread);
			return result;
		}
		
		public virtual void  pushPacket(Transport transport, ByteBuffer data)
		{
			TransportPacket packet = new TransportPacket();
			packet.Data = data;
			packet.Transport = transport;
			pushPacket(packet);
		}
		
		public virtual void  pushPacket(TransportPacket packet)
		{
			lock (queue)
			{
				queue.AddLast(packet);
			}
			awaitPacketEvent.Set();
		}
		
		public virtual void  addAliveReqInspection(Transport transport)
		{
			lock (aliveRequestCheckList)
			{
				aliveRequestCheckList.AddLast(transport);
			}
		}
		
		public virtual void  delAliveReqInspection(Transport transport)
		{
			lock (aliveRequestCheckList)
			{
				aliveRequestCheckList.Remove(transport);
			}
		}
		
		public virtual void  pushAliveReqForRecipients()
		{
			if (this.messageCoder != null)
			{
				lock (aliveRequestCheckList)
				{
					MessageEnvelope envelope = new MessageEnvelope();
					MessageBody body = new MessageBody();
					AliveRequest req = new AliveRequest();
					envelope.Id = ("-PING-");
					req.Timestamp = (System.DateTime.Now.Ticks);
					body.selectAliveRequest(req);
					envelope.Body = (body);
					ByteBuffer buffer;
					try
					{
						buffer = messageCoder.encode(envelope);
						foreach(Transport transport in aliveRequestCheckList)
						{
							pushPacket(transport, buffer);
						}
					}
					catch (System.Exception e)
					{
                        Console.WriteLine(e.ToString());
					}
				}
			}
		}

        public void close()
        {
            lock (queue)
            {
                queue.Clear();
                finishThread = true;
            }
            awaitPacketEvent.Set();
        }
		
		~WriterStorage()
		{
            close();
		}
	}
}