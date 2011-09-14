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
using org.bn.mq.net.tcp;

namespace org.bn.mq.net
{
	public class MessagingBusNet
	{
		public static MessagingBusNet Instance
		{
			get
			{
				return instance;
			}
			
		}
		private static MessagingBusNet instance = new MessagingBusNet();
		
		protected internal MessagingBusNet()
		{
			initDefaultFactories();
		}

        protected internal IList<ITransportFactory> factories = new List<ITransportFactory>();
		
		protected internal virtual void  initDefaultFactories()
		{
			addFactory(new TransportFactory());
		}
		
		
		public virtual void  addFactory(ITransportFactory factory)
		{
			lock (factories)
			{
				factories.Add(factory);
			}
		}
		
		public virtual void  removeFactory(ITransportFactory factory)
		{
			lock (factories)
			{
				factories.Remove(factory);
			}
		}
		
		public virtual ITransportFactory getFactory(Uri addr)
		{
			lock (factories)
			{
				foreach(ITransportFactory factory in factories)
				{
					if (factory.checkURISupport(addr))
					{
						return factory;
					}
				}
			}
			throw new System.Exception("Unable to find supported factory for URI" + addr);
		}
	}
}