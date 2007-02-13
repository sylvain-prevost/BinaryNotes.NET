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

    public delegate void CallAsyncDelegateResult<T>(T request, T result);
    public delegate void CallAsyncDelegateTimeout<T>(T request);

	public interface IMessageQueue<T> : IRemoteMessageQueue<T>
	{
		IQueue<T> Queue
		{
			get;			
			set;			
		}

		IPersistenceQueueStorage<T> PersistenceStorage
		{
			set;
            get;
		}

		IMessage<T> createMessage(T body);
		IMessage<T> createMessage();
		
		void  sendMessage(IMessage<T> message);
		
        T call(T args, string consumerId);
		T call(T args, string consumerId, int timeout);
		
        void  callAsync(T args, string consumerId, ICallAsyncListener<T> listener);
		void  callAsync(T args, string consumerId, ICallAsyncListener<T> listener, int timeout);

        void callAsync(T args, string consumerId, CallAsyncDelegateResult<T> resultDelegate, CallAsyncDelegateTimeout<T> timeoutDelegate);
        void callAsync(T args, string consumerId, CallAsyncDelegateResult<T> resultDelegate, CallAsyncDelegateTimeout<T> timeoutDelegate, int timeout);

	}
}