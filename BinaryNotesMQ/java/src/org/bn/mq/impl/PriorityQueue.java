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

import java.util.Comparator;
import java.util.LinkedList;
import java.util.List;
import java.util.Queue;

import org.bn.mq.IMessage;
import org.bn.mq.IQueue;

public class PriorityQueue<T> implements IQueue<T> {
    private class MessageComparator implements Comparator< IMessage<T> > {
        public int compare(IMessage<T> msg1, IMessage<T> msg2) {
            return msg1.getPriority()-msg2.getPriority();
        }
    }
    protected java.util.Queue< IMessage<T> > priorityQueue = new java.util.PriorityQueue< IMessage<T> >(1024, new MessageComparator());
    
    public void push(IMessage<T> message) {
        synchronized(priorityQueue) {
            priorityQueue.add(message);
        }
    }
    
    public void push(List<IMessage<T>> messages) {
        synchronized(priorityQueue) {
            priorityQueue.addAll(messages);
        }
    }
    

    public IMessage<T> getNext() {
        synchronized(priorityQueue) {
            return priorityQueue.poll();
        }
    }
}
