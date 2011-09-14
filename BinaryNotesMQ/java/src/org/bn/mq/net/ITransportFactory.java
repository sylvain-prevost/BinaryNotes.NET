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

package org.bn.mq.net;

import java.io.IOException;

import java.net.URI;

public interface ITransportFactory {
    ITransport getClientTransport(URI addr) throws IOException;
    ITransport getServerTransport(URI addr) throws IOException;
    void setTransportMessageCoderFactory(ITransportMessageCoderFactory coderFactory);
    ITransportMessageCoderFactory getTransportMessageCoderFactory();
    boolean checkURISupport(URI addr);
    void close();
}
