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

public class Writer implements Runnable{    
    protected WriterStorage storage;
    
    public Writer (WriterStorage storage) {
        this.storage = storage;
    }
    
    public void run() {
        TransportPacket packet = null;
        do {
            
            try {
                packet = storage.waitPacket();
                if(packet!=null)
                    packet.getTransport().send(packet.getData());
            }
            catch(IOException ex) {
                System.err.println("Unable to write packet for transport "+packet.getTransport());
                ex.printStackTrace();
            }
        }
        while(packet!=null);
    }

    void stop() {
    }
}
