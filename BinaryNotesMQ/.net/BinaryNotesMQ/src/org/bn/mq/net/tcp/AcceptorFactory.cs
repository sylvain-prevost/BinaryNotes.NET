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

namespace org.bn.mq.net.tcp
{
	
	
	public class AcceptorFactory:SocketFactory
	{
		
		//protected internal AcceptorStorage acceptorStorage = new AcceptorStorage();
        //protected internal System.Threading.Thread acceptorThread = null;
		//protected internal Acceptor acceptorThreadBody;
        //protected IDictionary<String, ServerTransport> storage = new Dictionary<String, ServerTransport>();
			
		public AcceptorFactory(WriterStorage writerStorage, TransportFactory factory):base(writerStorage, factory)
		{
			//initAcceptors();
		}
				
		protected internal virtual ServerTransport createTransport(Uri addr)
		{
			ServerTransport transport = new ServerTransport(addr, this);
			//transport.startListener();
			return transport;
		}
		
		public virtual ITransport getTransport(Uri addr)
		{
			ServerTransport transport = null;
				//transport = getCreatedTransport(addr);
				//if(transport == null) {
				transport = createTransport(addr);
				//}
			return transport;
		}
		
		private void  initAcceptors()
		{
			//acceptorThreadBody = new Acceptor(acceptorStorage);
			//acceptorThread = new System.Threading.Thread(new System.Threading.ThreadStart(acceptorThreadBody.Run));
			//acceptorThread.Name = "BNMQ-Acceptor";
			//acceptorThread.Start();
		}

        public void close()
        {

        }
		
		~AcceptorFactory()
		{
			//acceptorThreadBody.stop();
			//acceptorThread.Join();
			//acceptorStorage.Finalize();
		}
	}
}
