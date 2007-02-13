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

import org.bn.mq.net.ITransportCallListener;
import org.bn.mq.protocol.MessageEnvelope;

public class AsyncCallItem {
    public AsyncCallItem() {
    }
    
    private MessageEnvelope request;
    private ITransportCallListener listener;
    private Integer timerId;

    public MessageEnvelope getRequest() {
        return request;
    }

    public void setRequest(MessageEnvelope request) {
        this.request = request;
    }

    public ITransportCallListener getListener() {
        return listener;
    }

    public void setListener(ITransportCallListener listener) {
        this.listener = listener;
    }

    public int getTimerId() {
        return timerId;
    }

    public void setTimerId(int timerId) {
        this.timerId = timerId;
    }
    
}
