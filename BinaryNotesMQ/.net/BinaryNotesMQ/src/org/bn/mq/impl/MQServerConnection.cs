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
	
	public class MQServerConnection:MQConnection
	{
        protected internal IList<ITransport> clients = new List<ITransport>();
		
		public MQServerConnection(ITransport transport):base(transport)
		{
		}
		
		public override IRemoteSupplier lookup(System.String supplierName)
		{
			MessageEnvelope message = new MessageEnvelope();
			MessageBody body = new MessageBody();

            LookupRequest request = new LookupRequest();
            request.SupplierName = supplierName;

			body.selectLookupRequest(request);
			message.Body = body;
			message.Id = this.ToString();

			IRemoteSupplier supplier = null;
			lock (clients)
			{
				foreach(ITransport client in clients)
				{
					MessageEnvelope result = client.call(message, callTimeout);
					if (result.Body.LookupResult.Code.Value == LookupResultCode.EnumType.success)
					{
						supplier = new RemoteSupplier(supplierName, client);
						break;
					}
				}
			}

			if (supplier == null)
				throw new Exception("Error when accessing to supplier '" + supplierName + "'! Unable to find any suitable supplier!");
			return supplier;
		}
		
		public override void  onConnected(ITransport client)
		{
			lock (clients)
			{
				clients.Add(client);

                lock (listeners)
                {
                    foreach (IMQConnectionListener listener in listeners)
                    {
                        listener.onConnected(this, client);
                    }
                }

			}
		}
		
		public override void  onDisconnected(ITransport client)
		{
			lock (clients)
			{
				clients.Remove(client);
                lock (listeners)
                {
                    foreach (IMQConnectionListener listener in listeners)
                    {
						listener.onDisconnected(this, client);
					}
				}
			}
		}
	}
}