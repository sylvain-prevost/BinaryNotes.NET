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
using System.Threading;
using org.bn.mq.protocol;


namespace org.bn.mq.net.tcp
{
	
	public class AsyncCallManager
	{				
		protected IDictionary< String, AsyncCallItem > asyncCalls = new Dictionary< String, AsyncCallItem >();
        
        // Note! System.Threading.Timer uses instead of powerfull System.Timers.Timer 
        // because is not are supported by Compact Framework
        protected internal Timer asyncCallTimer = null;
		
		public AsyncCallManager()
		{
			start();
		}
		
		public virtual void  start()
		{
			lock (this)
			{
                asyncCallTimer = new Timer(
                    new TimerCallback(this.handleNotification), null, 5000, 5000
                ); // every 5 sec
				//asyncCallTimer.addNotificationListener(this, null, null);
				//asyncCallTimer.start();
			}
		}
		
		public virtual void  stop()
		{
			lock (this)
			{
                if (asyncCallTimer != null)
                {
                    asyncCallTimer.Dispose();
                    asyncCallTimer = null;
                    lock (asyncCalls)
                    {
                        asyncCalls.Clear();
                    }
                }
			}
		}
		
		public virtual void  storeRequest(MessageEnvelope request, ITransportCallListener listener, int timeout)
		{
			AsyncCallItem item = new AsyncCallItem();
			item.Request = request;
			item.Listener = listener;
            item.Timeout = timeout;
			lock (asyncCalls)
			{
				asyncCalls.Add(request.Id, item);
			}
		}
		
		public virtual AsyncCallItem getAsyncCall(System.String id)
		{
			AsyncCallItem res = null;
			lock (asyncCalls)
			{
                if(asyncCalls.ContainsKey(id))
				    res = asyncCalls[id];
				if (res != null)
					asyncCalls.Remove(id);
			}
			return res;
		}
		
		public virtual AsyncCallItem getAsyncCall(MessageEnvelope result)
		{
			return getAsyncCall(result.Id);
		}

        private long toUnixTime(DateTime time)
        {
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            double unixTime = ts.TotalSeconds;
            return (long)unixTime;
        }

        public virtual void handleNotification(Object notification)
        {
            IList<AsyncCallItem> expiredResult = new List<AsyncCallItem>();
            lock (asyncCalls)
            {
                foreach (AsyncCallItem item in asyncCalls.Values)
                {
                    long nowTicks = (DateTime.Now.Ticks/10000);
                    if (item.Started.Ticks/10000 + item.Timeout > nowTicks )
                    {
                        expiredResult.Add(item);
                    }
                }
            }

            foreach (AsyncCallItem item in expiredResult)
            {
                bool removed = false;
                lock (asyncCalls)
                {
                    if (asyncCalls.ContainsKey(item.Request.Id))
                    {
                        removed = true;
                        asyncCalls.Remove(item.Request.Id);
                    }
                }
                if (removed)
                {
                    item.Listener.onCallTimeout(item.Request);
                }
            }
        }
		
		~AsyncCallManager()
		{
			stop();
		}	
	}
}