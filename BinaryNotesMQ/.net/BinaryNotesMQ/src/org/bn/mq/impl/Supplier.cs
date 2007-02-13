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
using System.Collections.Generic;
using org.bn.mq.protocol;
using org.bn.mq.net;

namespace org.bn.mq.impl
{
	
	public class Supplier : ISupplier, ITransportReader
	{
		virtual public string Id
		{
			get
			{
				return supplierId;
			}			
		}
		private ITransport transport;
		private string supplierId;

		private IDictionary<String, object> queues = new Dictionary<String, object>();
		
		public Supplier(string supplierId, ITransport transport)
		{
			this.transport = transport;
			this.supplierId = supplierId;
			this.transport.setUnhandledMessagesReader ( this );
		}
		
		public virtual IRemoteMessageQueue<T> lookupQueue<T>(string queuePath)
		{
			IRemoteMessageQueue<T> queue = null;
			lock (queues)
			{
				queue = (IRemoteMessageQueue<T>) queues[queuePath];
			}
			return queue;
		}

        public virtual IMessageQueue<T> createQueue<T>(string queuePath, IQueue<T> queueImpl, IPersistenceQueueStorage<T> storage)
		{
            MessageQueue<T> queue = new MessageQueue<T>(queuePath, transport);
			queue.Queue = queueImpl;
			queue.PersistenceStorage = storage;
			lock (queues)
			{
				queues[queuePath] = queue;
			}
			return queue;
		}
		
		public virtual IMessageQueue<T> createQueue<T>(string queuePath, IQueue<T> queueImpl)
		{
            NullStorage<T> nullStorage = new NullStorage<T>(null);
			return createQueue(queuePath, queueImpl, nullStorage.createQueueStorage(queuePath));
		}
		
		public virtual IMessageQueue<T> createQueue<T>(string queuePath)
		{
            NullStorage<T> nullStorage = new NullStorage<T>(null);
            return createQueue(queuePath, new Queue<T>(), nullStorage.createQueueStorage(queuePath));
		}
		
		public virtual IMessageQueue<T> createQueue<T>(string queuePath, IPersistenceQueueStorage<T> storage)
		{
            return createQueue<T>(queuePath, new Queue<T>(), storage);
		}
		
		public virtual void removeQueue<T>(IMessageQueue<T> queue)
		{
			lock (queues)
			{
				queues.Remove(queue.QueuePath);
			}
		}
		
		public virtual bool onReceive(MessageEnvelope message, ITransport transport)
		{
			if (message.Body.isSubscribeRequestSelected())
			{
				MessageEnvelope resultMsg = new MessageEnvelope();
				MessageBody body = new MessageBody();
				SubscribeResult subscribeResult = new SubscribeResult();
				SubscribeResultCode subscribeResultCode = new SubscribeResultCode();
				body.selectSubscribeResult(subscribeResult);
				resultMsg.Body = (body);
				resultMsg.Id = message.Id;
				subscribeResultCode.Value = SubscribeResultCode.EnumType.unknownQueue;
				subscribeResult.Code = subscribeResultCode;
				try
				{
					transport.sendAsync(resultMsg);
				}
				catch (System.Exception ex)
				{
                    Console.WriteLine(ex.ToString());
				}
			}
			else if (message.Body.isUnsubscribeRequestSelected())
			{
				MessageEnvelope resultMsg = new MessageEnvelope();
				MessageBody body = new MessageBody();
				UnsubscribeResult unsubscribeResult = new UnsubscribeResult();
				UnsubscribeResultCode unsubscribeResultCode = new UnsubscribeResultCode();
				body.selectUnsubscribeResult(unsubscribeResult);
				resultMsg.Body = (body);
				resultMsg.Id = message.Id;
				unsubscribeResultCode.Value = (UnsubscribeResultCode.EnumType.unknownQueue);
				unsubscribeResult.Code = (unsubscribeResultCode);
				try
				{
					transport.sendAsync(resultMsg);
				}
				catch (System.Exception ex)
				{
                    Console.WriteLine(ex.ToString());
				}
			}
			
			return true;
		}
	}
}