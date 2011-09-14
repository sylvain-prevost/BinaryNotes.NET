/*
 Copyright 2006-2011 Abdulla Abdurakhmanov (abdulla@latestbit.com)
 Original sources are available at www.latestbit.com

 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at

 http://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.
 */
using System;
using System.Collections.Generic;
using org.bn.mq.protocol;
using org.bn.mq.net;
using org.bn.mq;

namespace org.bn.mq.impl
{
	
	public class MQConnection : IMQConnection, ITransportConnectionListener, ITransportReader
	{
		virtual public Uri Addr
		{
			get
			{
				return transport.getAddr();
			}
			
		}
		protected internal ITransport transport;
		protected internal const int callTimeout = 30;
        protected internal IDictionary<String, ISupplier> suppliers = new Dictionary<String, ISupplier>();

        protected internal IList<IMQConnectionListener> listeners = new List<IMQConnectionListener>();
		
		public MQConnection(ITransport transport)
		{
			this.transport = transport;
			transport.addConnectionListener(this);
			transport.addReader(this);
		}
		
		public virtual IRemoteSupplier lookup(string supplierName)
		{

			MessageEnvelope message = new MessageEnvelope();
			MessageBody body = new MessageBody();

            LookupRequest request = new LookupRequest();
            request.SupplierName = supplierName;

			body.selectLookupRequest(request);
			message.Body = body;
			message.Id = this.ToString();
			MessageEnvelope result = transport.call(message, callTimeout);
			if (result.Body.LookupResult.Code.Value == LookupResultCode.EnumType.success)
			{
				return new RemoteSupplier(supplierName, transport);
			}
			else
				throw new System.Exception("Error when accessing to supplier '" + supplierName + "': " + result.Body.LookupResult.Code.Value.ToString());
		}
		
		public virtual ISupplier createSupplier(System.String supplierName)
		{
			Supplier supplier = new Supplier(supplierName, transport);
			lock (suppliers)
			{
				suppliers[supplier.Id] = supplier;
			}
			return supplier;
		}
		
		public virtual void  removeSupplier(ISupplier supplier)
		{
			lock (suppliers)
			{
				suppliers.Remove(supplier.Id);
			}
		}

        public void start()
        {
            transport.start();
        }
		
		public virtual void  close()
		{
			transport.delConnectionListener(this);
			transport.delReader(this);
			transport.close();
		}
		
		public virtual bool onReceive(MessageEnvelope message, ITransport replyTransport)
		{
			if (message.Body.isLookupRequestSelected())
			{
				MessageEnvelope result = new MessageEnvelope();
				result.Id = message.Id;
				MessageBody body = new MessageBody();
				result.Body = body;
				LookupResult lResult = new LookupResult();
				body.selectLookupResult(lResult);
				try
				{
					lock (suppliers)
					{
						ISupplier supplier = suppliers[message.Body.LookupRequest.SupplierName];
						LookupResultCode resCode = new LookupResultCode();
						if (supplier != null)
						{
							resCode.Value = LookupResultCode.EnumType.success;
						}
						else
							resCode.Value = LookupResultCode.EnumType.notFound;
						lResult.Code = resCode;
					}
					replyTransport.sendAsync(result);
				}
				catch (Exception e)
				{
                    Console.WriteLine(e);
                }
				return true;
			}
			else
				return false;
		}
		
		public virtual void  onConnected(ITransport transport)
		{
			lock (listeners)
			{
				foreach(IMQConnectionListener listener in listeners)
				{
					listener.onConnected(this, transport);
				}
			}
		}
		
		public virtual void  onDisconnected(ITransport transport)
		{
			lock (listeners)
			{
				foreach(IMQConnectionListener listener in listeners)
				{
					listener.onDisconnected(this, transport);
				}
			}
		}
		
		public virtual void  addListener(IMQConnectionListener listener)
		{
			lock (listeners)
			{
				listeners.Add(listener);
			}
		}
		
		public virtual void  delListener(IMQConnectionListener listener)
		{
			lock (listeners)
			{
				listeners.Remove(listener);
			}
		}

        public IPTPSession<T> createPTPSession<T>(String pointName, String sessionName)
        {
            return new PTPSession<T>(pointName,sessionName, transport);
        }
	}
}