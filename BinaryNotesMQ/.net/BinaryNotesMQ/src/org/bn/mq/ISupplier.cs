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

namespace org.bn.mq
{
	
	public interface ISupplier : IRemoteSupplier
	{
		IMessageQueue<T> createQueue<T>(System.String queuePath);
        IMessageQueue<T> createQueue<T>(System.String queuePath, IQueue<T> queueImpl);
        IMessageQueue<T> createQueue<T>(System.String queuePath, IQueue<T> queueImpl, IPersistenceQueueStorage<T> storage);
        IMessageQueue<T> createQueue<T>(System.String queuePath, IPersistenceQueueStorage<T> storage);
        void removeQueue<T>(IMessageQueue<T> queue);
	}
}
