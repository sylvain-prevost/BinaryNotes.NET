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

using org.bn.mq.impl;

namespace org.bn.mq
{

	public class MQFactory
	{
		public static MQFactory Instance
		{
			get
			{
				return instance;
			}
			
		}
		private static MQFactory instance = new MQFactory();
		
		protected internal MQFactory()
		{
		}
		
		public virtual IMessagingBus createMessagingBus()
		{
			return new MessagingBus();
		}
		
		public virtual IQueue<T> createQueue<T>()
		{
			return createQueue<T>("simple");
		}

        public virtual IQueue<T> createQueue<T>(string algorithm)
		{
			if (algorithm == null || (algorithm != null && algorithm.ToUpper().Equals("simple".ToUpper())))
			{
				return new Queue<T>();
			}
			/*else if (algorithm.ToUpper().Equals("priority".ToUpper()))
			{
				return new PriorityQueue();
			}*/
			else
			    return null;
		}
		
		public virtual IPersistenceStorage<T> createPersistenceStorage<T>(string storageType, System.Collections.Generic.IDictionary<String,Object> properties)
		{
			if (storageType == null || (storageType != null && storageType.Length == 0))
			{
                return new NullStorage<T>(properties);
			}
			else if (storageType.ToUpper().Equals("InMemory".ToUpper()))
			{
                return new InMemoryStorage<T>(properties);
			}
#if !PocketPC
			else if (storageType.ToUpper().Equals("SQL".ToUpper()))
			{
                return new SQLStorage<T>(properties);
			}
#endif
			else
				return null;
		}
	}
}