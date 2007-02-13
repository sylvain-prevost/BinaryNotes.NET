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
	
	public class RemoteMessageQueue<T> : IRemoteMessageQueue<T>, ITransportReader
	{
		virtual public string QueuePath
		{
			get
			{
				return queuePath;
			}
			
		}
		private RemoteSupplier supplier;
		private string queuePath;
		private const int subscribeTimeout = 60;
		protected internal IDictionary<String, IConsumer<T>> consumers = new Dictionary<String, IConsumer<T>>();
		
		public RemoteMessageQueue(string queuePath, RemoteSupplier supplier)
		{
			this.supplier = supplier;
			this.queuePath = queuePath;
			start();
		}
		
		public virtual void addConsumer(IConsumer<T> consumer)
		{
			addConsumer(consumer, false);
		}
		
		public virtual void addConsumer(IConsumer<T> consumer, bool persistence)
		{
			addConsumer(consumer, persistence, null);
		}
		
		public virtual void addConsumer(IConsumer<T> consumer, bool persistence, string filter)
		{
			SubscribeRequest request = new SubscribeRequest();
			request.ConsumerId = consumer.Id;
			request.Filter = filter;
			if (persistence)
				request.Persistence = true;
			request.QueuePath = QueuePath;
			
			MessageEnvelope message = new MessageEnvelope();
			MessageBody body = new MessageBody();
			body.selectSubscribeRequest(request);
			message.Body = (body);
			message.Id = this.ToString();
            // !!!!!
			//MessageEnvelope result = supplier.Connection.call(message, 1000);
            MessageEnvelope result = supplier.Connection.call(message, subscribeTimeout);
			if (result.Body.SubscribeResult.Code.Value != SubscribeResultCode.EnumType.success)
			{
				throw new System.Exception("Error when accessing to queue '" + queuePath + "' for supplier '" + supplier.Id + "': " 
                    + result.Body.SubscribeResult.Code.Value.ToString()
                );
			}
			else
			{
				lock (consumers)
				{
					consumers[consumer.Id] = consumer;
				}
			}
		}
		
		public virtual void  delConsumer(IConsumer<T> consumer)
		{
			lock (consumers)
			{
				consumers.Remove(consumer.Id);
			}
			
			UnsubscribeRequest request = new UnsubscribeRequest();
			request.ConsumerId = consumer.Id;
			request.QueuePath = QueuePath;
			
			MessageEnvelope message = new MessageEnvelope();
			MessageBody body = new MessageBody();
			body.selectUnsubscribeRequest(request);
			message.Body = body;
			message.Id = this.ToString();
			MessageEnvelope result = supplier.Connection.call(message, subscribeTimeout);
			if (result.Body.UnsubscribeResult.Code.Value != UnsubscribeResultCode.EnumType.success)
			{
				throw new System.Exception("Error when accessing to queue '" + queuePath + "' for supplier '" + supplier.Id + "': " + result.Body.UnsubscribeResult.Code.Value.ToString());
			}
		}
		
		public virtual bool onReceive(MessageEnvelope message, ITransport transport)
		{
			if (message.Body.isMessageUserBodySelected() && message.Body.MessageUserBody.QueuePath.ToUpper().Equals(this.QueuePath.ToUpper()))
			{
				lock (consumers)
				{
                    IConsumer<T> consumer = null;
                    if(consumers.ContainsKey(message.Body.MessageUserBody.ConsumerId))
                        consumer = consumers[message.Body.MessageUserBody.ConsumerId];

					if (consumer != null)
					{
                        Message<T> msg = new Message<T>();
						try
						{
							msg.fillFromEnvelope(message);
						}
						catch (Exception e)
						{
                            Console.WriteLine(e.ToString());
						}
						
						T result = consumer.onMessage(msg);
                        if (msg.Mandatory)
                        {
                            MessageEnvelope deliveryReportMessage = new MessageEnvelope();
                            MessageBody deliveryReportBody = new MessageBody();
                            DeliveryReport deliveryReportData = new DeliveryReport();
                            deliveryReportMessage.Body = (deliveryReportBody);
                            deliveryReportMessage.Id = ("/report-for/" + msg.Id);
                            deliveryReportBody.selectDeliveryReport(deliveryReportData);
                            deliveryReportData.ConsumerId = (consumer.Id);
                            deliveryReportData.MessageId = (msg.Id);
                            deliveryReportData.QueuePath = (this.queuePath);
                            DeliveredStatus status = new DeliveredStatus();
                            status.Value = DeliveredStatus.EnumType.delivered;
                            deliveryReportData.Status = status;

                            try
                            {
                                transport.sendAsync(deliveryReportMessage);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                            }
                        }

						if (result != null)
						{
                            Message<T> resultMsg = new Message<T>();
							resultMsg.Id = msg.Id;
							resultMsg.Body = result;
							resultMsg.QueuePath = msg.QueuePath;
							MessageEnvelope resultMsgEnv;
							try
							{
								resultMsgEnv = resultMsg.createEnvelope();
								resultMsgEnv.Body.MessageUserBody.ConsumerId = consumer.Id;
								transport.sendAsync(resultMsgEnv);
							}
							catch (Exception e)
							{
                                Console.WriteLine(e.ToString());
							}
						}
					}
				}
				return true;
			}
			return false;
		}
		
		public virtual void  start()
		{
			this.supplier.Connection.addReader(this);
		}
		
		public virtual void  stop()
		{
			this.supplier.Connection.delReader(this);
		}
	}
}