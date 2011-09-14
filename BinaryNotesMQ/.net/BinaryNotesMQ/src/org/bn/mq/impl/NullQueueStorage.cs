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

    public class NullQueueStorage<T> : IPersistenceQueueStorage<T>
	{
        private IList<IMessage<T>> nullMessages = new List<IMessage<T>>();

        public IList<IMessage<T>> getMessagesToSend(IConsumer<T> consumer)
        {
            return nullMessages;
        }

        public void persistenceSubscribe(IConsumer<T> consumer)
        {
        }

        public void persistenceUnsubscribe(IConsumer<T> consumer)
        {
        }

        public void registerPersistenceMessage(IMessage<T> message)
        {
        }

        public void removeDeliveredMessage(string consumerId, string messageId)
        {
        }

        public void close()
        {
        }
    }
}