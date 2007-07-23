using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using org.bn.mq;
using org.bn.mq.impl;
using org.bn.mq.net;
using org.bn.mq.protocol;
using org.bn.mq.net.tcp;

namespace BNP2PExample
{
    class TalkerSession<T> : IPTPSessionListener<T> 
    {
        public enum SessionTypeEnum { Server, Client };    

        public delegate void MessageReceiverDelegate(TalkerSession<T> talkerSession, string message);
        private event MessageReceiverDelegate messageReceivedEvent;

        private IMessagingBus bus = null;
        private TalkerConnectionListener talkerConnectionListener = null;
        private IMQConnection talkerConnection = null;
        private IPTPSession<T> ptpTalkerSession = null;

        private SessionTypeEnum sessionType;

        private object topLevelTreeNode; 

        public void SubscribeMessageReceived(MessageReceiverDelegate handler)
        {
            messageReceivedEvent += handler;
        }

        public void UnsubscribeMessageReceived(MessageReceiverDelegate handler)
        {
            messageReceivedEvent -= handler;
        }

        public TalkerSession(SessionTypeEnum sessionType, string busAddress)
        {
            topLevelTreeNode = null;
            bus = MQFactory.Instance.createMessagingBus();
            if (sessionType == SessionTypeEnum.Server)
            {
                talkerConnection = bus.create(new Uri(busAddress));
                ptpTalkerSession = talkerConnection.createPTPSession<T>("serverPTP", "ptpSimpleSession");
            }
            else if (sessionType == SessionTypeEnum.Client)
            {
                talkerConnection = bus.connect(new Uri(busAddress));
                ptpTalkerSession = talkerConnection.createPTPSession<T>("clientPTP", "ptpSimpleSession");
            }
            else
            {
                throw new Exception("Session Type must be either Server or Client");
            }
            this.sessionType = sessionType;

            talkerConnectionListener = new TalkerConnectionListener();
            talkerConnection.addListener(talkerConnectionListener);
            ptpTalkerSession.addListener(this);

            talkerConnection.start();
        }

        public void Close()
        {
            if (bus != null)
            {
                bus.close();
                bus = null;
            }
            if (talkerConnection != null)
            {
                talkerConnection.close(); // not sure why this locks up 
                talkerConnection = null;
            }
            if (talkerConnectionListener != null)
            {
                talkerConnectionListener.Close();
                talkerConnectionListener = null;
            }
            if (ptpTalkerSession != null)
            {
                ptpTalkerSession.close();
                ptpTalkerSession = null;
            }
        }

        ~TalkerSession()
        {
            Close();
        }

        public SessionTypeEnum SessionType { get { return sessionType; } }

        public void SendMessageToAllPeers(string msg)
        {
            Hashtable transportCollectionClone = talkerConnectionListener.GetTransportCollectionClone(); // Make a clone in case collection is modified during iteration
            foreach (DictionaryEntry de in transportCollectionClone)
            {
                ITransport transport = (ITransport)de.Value;
                SendMessageToOnePeer(msg, transport);
            }
        }

        private void SendMessageToOnePeer(string msg, ITransport transport)
        {            
            System.Net.Sockets.Socket socket = ((Transport)transport).getSocket();
            System.Net.EndPoint endPoint = socket.LocalEndPoint;
            StringBuilder sb = new StringBuilder(100);
            sb.Append(msg);
            sb.Append(" From " + endPoint);
            sb.Append(" To " + transport.getAddr());
            T t = (T)(object)sb.ToString();
            IMessage<T> tmsg = ptpTalkerSession.createMessage(t);
            ptpTalkerSession.sendMessage(tmsg, transport);
        }

        public T onMessage(IPTPSession<T> session, ITransport transport, IMessage<T> message)
        {
            messageReceivedEvent(this, message.Body.ToString());
            return default(T);
        }

        public object TopLevelTreeNode
        {
            get { return topLevelTreeNode; }
            set { topLevelTreeNode = value; }
        }
    }

    class TalkerConnectionListener : IMQConnectionListener
    {
        private Hashtable transportCollection = new Hashtable();

        public void onConnected(IMQConnection connection, ITransport networkTransport)
        {
            if (transportCollection.ContainsKey(networkTransport.getAddr()) == false)
            {
                transportCollection.Add(networkTransport.getAddr(), networkTransport);
            }
        }

        public void onDisconnected(IMQConnection connection, ITransport networkTransport)
        {
            if (transportCollection.ContainsKey(networkTransport.getAddr()) == true)
            {
                transportCollection.Remove(networkTransport.getAddr());
                networkTransport.close();
            }
        }

        public void Close()
        {
            Hashtable transportCollectionClone = (Hashtable)transportCollection.Clone(); // Make a clone in case collection is modified during iteration
            foreach (DictionaryEntry de in transportCollectionClone)
            {
                ITransport transport = (ITransport)de.Value;
                transport.close();
            }
            transportCollection = new Hashtable(); // keeps this from happening more than once 
        }

        ~TalkerConnectionListener()
        {
            Close();
        }

        // A clone is better because sometimes the collection will be modified during iteration.
        // For example, if something disconnects while you are iterating, and that item is 
        // removed from the collection then the iterator becomes invalid and an exception 
        // results. In that situation the collection clone will not be modified. Instead 
        // a message will be sent to a non-existant recipent which is not a problem.  
        public Hashtable GetTransportCollectionClone()
        {
            return (Hashtable)transportCollection.Clone();
        }
    }
}