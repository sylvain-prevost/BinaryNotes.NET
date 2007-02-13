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
	
	public class InMemoryStorage<T> : IPersistenceStorage<T>
	{
        private IDictionary<String, InMemoryQueueStorage<T>> storages = new Dictionary<String, InMemoryQueueStorage<T>>();
		private string storageName;
        IDictionary<String, Object> storageProperties;

        public InMemoryStorage(IDictionary<String, Object> storageProperties)
		{
			//this.storageName = storageName;
            this.storageProperties = storageProperties;
            if (!this.storageProperties.ContainsKey("storageName"))
            {
                throw new Exception("Unable to present property: 'storageName'");
            }
            else
            {
                this.storageName = (string)this.storageProperties["storageName"];
            }

		}
		
		public virtual IPersistenceQueueStorage<T> createQueueStorage(string queueStorageName)
		{
            InMemoryQueueStorage<T> result = null;
			lock (storages)
			{
                string key = storageName + "/" + queueStorageName;
                if(!storages.ContainsKey(key)) 
                {
                    result = new InMemoryQueueStorage<T>(key);
					storages[key] = result;
                }
                else 
                {
                    result = storages[key];
                }
			}
            return result;
		}
	}
}