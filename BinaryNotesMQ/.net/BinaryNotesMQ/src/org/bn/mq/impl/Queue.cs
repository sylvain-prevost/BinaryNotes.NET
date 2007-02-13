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
using org.bn.mq.protocol;
using org.bn.mq.net;

namespace org.bn.mq.impl
{
	
	public class Queue<T> : IQueue<T>
	{
        protected internal System.Collections.Generic.Queue<IMessage<T>> simpleQueue = new System.Collections.Generic.Queue<IMessage<T>>();

        virtual public IMessage<T> getNext()
        {
            lock (simpleQueue)
            {
                if (simpleQueue.Count > 0)
                    return simpleQueue.Dequeue();
                else
                    return null;
            }
        }

		
		public virtual void  push(IMessage<T> message)
		{
			lock (simpleQueue)
			{
                simpleQueue.Enqueue(message);
			}
		}
		
		public virtual void push(System.Collections.Generic.IList<IMessage<T>> messages)
		{
			lock (simpleQueue)
			{
                foreach (IMessage<T> message in messages)
                {
                    simpleQueue.Enqueue(message);
                }
			}
		}
	}
}
