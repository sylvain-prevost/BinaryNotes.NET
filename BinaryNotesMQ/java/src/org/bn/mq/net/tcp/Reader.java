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

package org.bn.mq.net.tcp;

import java.io.IOException;
import java.nio.ByteBuffer;
import java.util.List;


public class Reader implements Runnable {
    protected ReaderStorage storage;
    protected boolean finish = false;
    
    public Reader(ReaderStorage storage) {
        this.storage = storage;
    }
    
    public void stop() {
        finish = true;
    }

    public void run() {
        List<Transport> transports = null;
        do {
            try {
                transports = storage.waitTransports();
                if(transports!=null) {
                    for(Transport transport: transports) {
                        ByteBuffer buffer;
                        try {
                            buffer = transport.receiveAsync();
                            transport.fireReceivedData(buffer);
                        }
                        catch (Exception e) {
                            //System.err.println(e);
                            //e.printStackTrace();
                        }
                    }
                }
            }
            catch (Exception e) {
                e.printStackTrace();
            }            
        }
        while(!finish);
    }
    
    public void finalize() {
        stop();
    }
}
