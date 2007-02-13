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

    public class ProxyCallAsyncListener<T> : ITransportCallListener
	{
		private ICallAsyncListener<T> listener = null;
        private CallAsyncDelegateResult<T> resultDelegate = null; 
        private CallAsyncDelegateTimeout<T> timeoutDelegate = null;
		
		public ProxyCallAsyncListener(ICallAsyncListener<T> listener)
		{
			this.listener = listener;
		}

        public ProxyCallAsyncListener(CallAsyncDelegateResult<T> resultDelegate, CallAsyncDelegateTimeout<T> timeoutDelegate)
        {
            this.timeoutDelegate = timeoutDelegate;
            this.resultDelegate = resultDelegate;
        }
		
		public virtual void  onCallResult(MessageEnvelope requestEnv, MessageEnvelope resultEnv)
		{
            try
            {

                Message<T> request = new Message<T>();
                Message<T> result = new Message<T>();
                request.fillFromEnvelope(requestEnv);
                result.fillFromEnvelope(resultEnv);

                if (listener != null)
                {
                    listener.onCallResult( request.Body, result.Body);
                }
                else
                {
                    if (resultDelegate != null)
                    {
                        resultDelegate.Invoke(request.Body, result.Body);
                    }                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

		}
		
		public virtual void  onCallTimeout(MessageEnvelope requestEnv)
		{
            Message<T> request = new Message<T>();
			try
			{
				request.fillFromEnvelope(requestEnv);
			    if (listener != null)
			    {
					listener.onCallTimeout(request.Body);
				}
                else
                if (timeoutDelegate != null)
                {
                    timeoutDelegate.Invoke(request.Body);
                }
			}
            catch (System.Exception e)
            {
                Console.WriteLine(e.ToString());
            }

		}
	}
}