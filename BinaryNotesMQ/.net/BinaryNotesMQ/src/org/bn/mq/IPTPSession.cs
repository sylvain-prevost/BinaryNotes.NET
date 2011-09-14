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
using System.Text;
using org.bn.mq;
using org.bn.mq.net;

namespace org.bn.mq
{
    public interface IPTPSession<T>
    {
        IMessage<T> createMessage(T body);
        IMessage<T> createMessage();

        void sendMessage(IMessage<T> message);
        void sendMessage(IMessage<T> message, ITransport forTransport);

        T call(T args, ITransport forTransport) ;
        T call(T args, ITransport forTransport, int timeout);
        T call(T args);
        T call(T args, int timeout);

        void callAsync(T args, ITransport forTransport, ICallAsyncListener<T> listener);
        void callAsync(T args, ITransport forTransport, ICallAsyncListener<T> listener, int timeout);
        void callAsync(T args, ICallAsyncListener<T> listener);
        void callAsync(T args, ICallAsyncListener<T> listener, int timeout);
        
        void callAsync(T args, ITransport forTransport, CallAsyncDelegateResult<T> resultDelegate, CallAsyncDelegateTimeout<T> timeoutDelegate);
        void callAsync(T args, ITransport forTransport, CallAsyncDelegateResult<T> resultDelegate, CallAsyncDelegateTimeout<T> timeoutDelegate, int timeout);
        void callAsync(T args, CallAsyncDelegateResult<T> resultDelegate, CallAsyncDelegateTimeout<T> timeoutDelegate);
        void callAsync(T args, CallAsyncDelegateResult<T> resultDelegate, CallAsyncDelegateTimeout<T> timeoutDelegate, int timeout);

        void addListener(IPTPSessionListener<T> listener);
        void delListener(IPTPSessionListener<T> listener);
        void close();
    }
}
