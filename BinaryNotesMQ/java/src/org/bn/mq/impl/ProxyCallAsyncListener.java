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

package org.bn.mq.impl;

import org.bn.mq.ICallAsyncListener;
import org.bn.mq.IMessageQueue;
import org.bn.mq.net.ITransportCallListener;
import org.bn.mq.protocol.MessageEnvelope;

public class ProxyCallAsyncListener<T> implements ITransportCallListener {
    private ICallAsyncListener<T> listener;
    private Class<T> messageClass;
    //private IMessageQueue queue;
    
    public ProxyCallAsyncListener(ICallAsyncListener<T> listener, Class<T> messageClass) {
        this.listener = listener;
        //this.queue = queue;
        this.messageClass = messageClass;
    }

    public void onCallResult(MessageEnvelope requestEnv, MessageEnvelope resultEnv) {
        if(listener!=null) {
            Message<T> request = new Message<T>(messageClass);
            Message<T> result = new Message<T>(messageClass);
            try {
                request.fillFromEnvelope(requestEnv);
                result.fillFromEnvelope(resultEnv);
                listener.onCallResult(request.getBody(),result.getBody());
            }
            catch (Exception e) {
                e.printStackTrace();
            }            
        }
    }

    public void onCallTimeout(MessageEnvelope requestEnv) {
        if(listener!=null) {
            Message<T> request = new Message<T>(messageClass);
            try {
                request.fillFromEnvelope(requestEnv);
                listener.onCallTimeout(request.getBody());
            }
            catch (Exception e) {
                e.printStackTrace();
            }            
        }    
    }
}
