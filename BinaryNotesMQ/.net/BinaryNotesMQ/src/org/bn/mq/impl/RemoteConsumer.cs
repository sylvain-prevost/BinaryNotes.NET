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
using org.bn.mq.protocol;
using org.bn.mq.net;

namespace org.bn.mq.impl
{
	public class RemoteConsumer<T> : IRemoteConsumer<T>
	{
        protected internal string consumerId = null;
		virtual public string Id
		{
			get
			{
				return this.consumerId;
			}
			
		}
        
        protected internal ITransport transport = null;
		virtual public ITransport NetworkTransport
		{
			get
			{
				return transport;
			}			
		}
		
		
		public RemoteConsumer(string consumerId, ITransport transport)
		{
			this.consumerId = consumerId;
			this.transport = transport;
		}
		
		public virtual T onMessage(IMessage<T> message)
		{
			Message<T> msgImpl = new Message<T>(message);
			try
			{
				MessageEnvelope envelope = msgImpl.createEnvelope();
				envelope.Body.MessageUserBody.ConsumerId = Id;
				transport.sendAsync(envelope);
			}
			catch (Exception e)
			{
                Console.WriteLine(e.ToString());
			}
			return default(T);
		}
	}
}
