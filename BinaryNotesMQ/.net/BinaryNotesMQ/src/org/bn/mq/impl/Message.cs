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
using org.bn.mq.net;

namespace org.bn.mq.impl
{	
#if !PocketPC
	[Serializable]
#endif
	public class Message<T> : IMessage<T>
	{	
		private string id;
        virtual public string Id
        {
            get
            {
                return id;
            }
            set
            {
                this.id = value;
            }
        }

		private string senderId;
        virtual public string SenderId
        {
            get
            {
                return senderId;
            }
            set
            {
                this.senderId = value;
            }

        }

		private string queuePath;
        virtual public string QueuePath
        {
            get
            {
                return queuePath;
            }
            set
            {
                this.queuePath = value;
            }

        }

        private int priority;
        virtual public int Priority
        {
            get
            {
                return priority;
            }
            set
            {
                this.priority = value;
            }

        }

        private bool mandatoryFlag = false;
        virtual public bool Mandatory
        {
            get
            {
                return mandatoryFlag;
            }

            set
            {
                this.mandatoryFlag = value;
            }

        }

        private DateTime lifeTime;
        virtual public DateTime LifeTime
        {
            get
            {
                return lifeTime;
            }

            set
            {
                this.lifeTime = value;
            }

        }

        private T body;
        virtual public T Body
        {
            get
            {
                return body;
            }

            set
            {
                this.body = value;
            }

        }

		//private Type messageClass;
		
		public Message(/*System.Type messageClass*/)
		{
			//this.messageClass = messageClass;
		}

        public Message(IMessage<T> src)/*, System.Type messageClass)//:this(messageClass)*/
		{
			Id = src.Id;
			SenderId = src.SenderId;
			Priority = src.Priority;
			Mandatory = src.Mandatory;
			LifeTime = src.LifeTime;
			Body = src.Body;
			QueuePath = src.QueuePath;
		}				
		
		public virtual void  fillFromEnvelope(MessageEnvelope messageEnvelope)
		{
			byte[] userBody = messageEnvelope.Body.MessageUserBody.UserBody;
			Body = decoder.decode<T>(new System.IO.MemoryStream(userBody));
			Id = messageEnvelope.Id;
			SenderId = messageEnvelope.Body.MessageUserBody.SenderId;
			QueuePath = messageEnvelope.Body.MessageUserBody.QueuePath;
            Mandatory = messageEnvelope.DeliveryReportReq;
		}
		
		public virtual MessageEnvelope createEnvelope()
		{
			MessageEnvelope result = new MessageEnvelope();
			MessageBody messageBody = new MessageBody();
			MessageUserBody userBody = new MessageUserBody();
			System.IO.MemoryStream output = new System.IO.MemoryStream();
			encoder.encode<T>(Body, output);
			userBody.UserBody = output.ToArray();
			userBody.SenderId = SenderId;
			userBody.QueuePath = QueuePath;
			messageBody.selectMessageUserBody(userBody);
			result.Body = messageBody;
			result.Id = this.Id;
            result.DeliveryReportReq = true;
			
			return result;
		}
		
		private static IDecoder decoder = null;
		private static IEncoder encoder = null;
		static Message()
		{
			{
				try
				{
					decoder = CoderFactory.getInstance().newDecoder("PER/U");
					encoder = CoderFactory.getInstance().newEncoder("PER/U");
				}
				catch (System.Exception e)
				{
					e = null;
				}
			}
		}
	}
}
