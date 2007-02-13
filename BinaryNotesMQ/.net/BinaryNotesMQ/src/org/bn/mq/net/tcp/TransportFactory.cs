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
using System.Threading;
using System.Collections.Generic;
using org.bn.mq.protocol;
using org.bn.mq.net;

namespace org.bn.mq.net.tcp
{
	
	public class TransportFactory : ITransportFactory
	{
		virtual public ITransportMessageCoderFactory TransportMessageCoderFactory
		{
			get
			{
				return messageCoderFactory;
			}
			
			set
			{
				this.messageCoderFactory = value;
				if (this.messageCoderFactory != null)
				{
					writerStorage.MessageCoder = this.messageCoderFactory.newCoder(null);
				}
			}
			
		}

		virtual public AsyncCallManager AsyncCallManager
		{
			get
			{
				return this.asyncCallMgr;
			}
			
		}
		virtual public ConnectorFactory ConnectorFactory
		{
			get
			{
				return this.conFactory;
			}
			
		}
		virtual public AcceptorFactory AcceptorFactory
		{
			get
			{
				return this.acpFactory;
			}
			
		}
		
		private const string scheme = "bnmq";
		
		protected internal WriterStorage writerStorage = new WriterStorage();
		//protected internal ReaderStorage readerStorage = new ReaderStorage();		
		protected internal ConnectorFactory conFactory;
		protected internal AcceptorFactory acpFactory;
		
		protected internal Thread writerThread;
		protected internal Writer writerThreadBody;
		
		protected internal Thread readerThread;
		//protected internal Reader readerThreadBody;

		protected internal ITransportMessageCoderFactory messageCoderFactory;
		protected internal AsyncCallManager asyncCallMgr = new AsyncCallManager();
		
		public TransportFactory()
		{
            conFactory = new ConnectorFactory(writerStorage, this);
            acpFactory = new AcceptorFactory(writerStorage, this);
            startAsyncDispatchers();
		}
		
		public virtual ITransport getClientTransport(Uri addr)
		{
			return conFactory.getTransport(addr);
		}

        public virtual ITransport getServerTransport(Uri addr)
		{
			return acpFactory.getTransport(addr);
		}

        public virtual bool checkURISupport(Uri addr)
		{
			return addr.Scheme.ToUpper().Equals(scheme.ToUpper());
		}
		
		protected internal virtual void  startAsyncDispatchers()
		{
			writerThreadBody = new Writer(writerStorage);
			writerThread = new Thread(new ThreadStart(writerThreadBody.Run));
			writerThread.Name = "BNMQ-TCPWriter";
            writerThread.Start();
			/*readerThreadBody = new Reader(readerStorage);
			readerThread = new Thread(new ThreadStart(readerThreadBody.Run));
			readerThread.Name = "BNMQ-TCPReader";
			readerThread.Start();*/
		}

        public void close()
        {
            writerThreadBody.stop();
            //readerThreadBody.stop();
            writerStorage.close();
            //if(writerThread.IsAlive) - not supported on CF
                writerThread.Join();

            //readerThread.Join();
            //readerStorage.Finalize();

            conFactory.close();
            acpFactory.close();
            asyncCallMgr.stop();
        }
		
		~TransportFactory()
		{
            close();
		}
	}
}