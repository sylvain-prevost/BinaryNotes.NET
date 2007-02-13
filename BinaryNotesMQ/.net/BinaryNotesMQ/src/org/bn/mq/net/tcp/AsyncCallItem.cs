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

namespace org.bn.mq.net.tcp
{
	
	public class AsyncCallItem
	{
		virtual public MessageEnvelope Request
		{
			get
			{
				return request;
			}
			
			set
			{
				this.request = value;
			}
			
		}
		virtual public ITransportCallListener Listener
		{
			get
			{
				return listener;
			}
			
			set
			{
				this.listener = value;
			}
			
		}
		virtual public DateTime Started
		{
			get
			{
                return startedTime;
			}
			
		}

        virtual public int Timeout
        {
            get
            {
                return timeout;
            }

            set
            {
                this.timeout = value;
            }

        }


        public AsyncCallItem()
		{
            this.startedTime = DateTime.Now;
		}
		
		private MessageEnvelope request;
		private ITransportCallListener listener;
		private DateTime startedTime;
        private int timeout;
	}
}