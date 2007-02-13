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

namespace org.bn.mq.net
{
	
	public interface ITransport
	{
		void setUnhandledMessagesReader(ITransportReader reader);
		
        void send(ByteBuffer buffer);
		void sendAsync(ByteBuffer buffer);
		
		void send(byte[] buffer);
		void sendAsync(byte[] buffer);
		
		void send(MessageEnvelope message);
		void sendAsync(MessageEnvelope message);
		MessageEnvelope call(MessageEnvelope message);
		MessageEnvelope call(MessageEnvelope message, int timeout);

		void  callAsync(MessageEnvelope message, ITransportCallListener listener);
		void  callAsync(MessageEnvelope message, ITransportCallListener listener, int timeout);
		
		Uri getAddr();		
		void  addConnectionListener(ITransportConnectionListener listener);
		void  delConnectionListener(ITransportConnectionListener listener);		
		void  addReader(ITransportReader listener);
		void  delReader(ITransportReader listener);
		
		bool isAvailable();
        void start();
		void close();
	}
}