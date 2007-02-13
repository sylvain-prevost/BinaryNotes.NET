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

import java.util.LinkedList;

import java.util.List;

import org.bn.mq.IMessage;
import org.bn.mq.IQueue;
import org.bn.mq.protocol.MessageEnvelope;

public class Queue<T> implements IQueue<T> {
    protected java.util.Queue< IMessage<T> > simpleQueue = new LinkedList< IMessage<T> >();
    
    public void push(IMessage<T> message) {
        synchronized(simpleQueue) {
            simpleQueue.add(message);
        }
    }
    
    public void push(List<IMessage<T>> messages) {
        synchronized(simpleQueue) {
            simpleQueue.addAll(messages);
        }
    }
    

    public IMessage<T> getNext() {
        synchronized(simpleQueue) {
            return simpleQueue.poll();
        }
    }
}
