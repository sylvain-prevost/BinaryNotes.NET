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

package org.bn.mq.net.tcp;

import java.io.IOException;
import java.util.List;

public class Acceptor implements Runnable {
    private AcceptorStorage storage;
    private boolean finish = false;
    
    public Acceptor(AcceptorStorage storage) {
        this.storage = storage;
    }

    public void run() {
        while(!finish ) {
            try {
                List<ServerTransport> serverTransports =  storage.waitTransports();
                if(!finish && serverTransports!=null) {
                    for(ServerTransport serverTransport: serverTransports) {
                        if(!serverTransport.isAvailable()) {
                            serverTransport.startListener();
                        }
                        else {
                            serverTransport.acceptClient();
                        }
                    }
                }
            }
            catch (IOException ex) {
                ex = null;
            }            
        }
    }
    
    public void stop() {
        finish = true;
    }
}
