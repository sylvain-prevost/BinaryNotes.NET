/*
 * Copyright 2007 Abdulla G. Abdurakhmanov (abdulla.abdurakhmanov@gmail.com).
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

package org.bn.mq;

import org.bn.mq.impl.PTPSession;
import org.bn.mq.net.ITransport;

public interface IPTPSession<T> {
    /**
     * Creating message for delivery over this queue
     * @param body user content body
     * @return created message
     */
    IMessage<T> createMessage(T body);
    
    /**
     * Creating message for delivery over this queue     
     * @return created message
     */
    IMessage<T> createMessage();
    
    /**
     * Send message to remote object
     * @param message message to send
     */
    void sendMessage(IMessage<T> message) throws Exception;    

    /**
     * Send message to remote object
     * @param forTransport client transport instance (for MQ server listener)
     * @param message message to send
     */    
    void sendMessage(IMessage<T> message, ITransport forTransport) throws Exception;
    
    /**
     * Synchronized RPC-style call 
     * @param args call arguments
     * @param forTransport client transport instance (for MQ server listener)
     * @return call result
     */
    T call(T args, ITransport forTransport) throws Exception;
    
    /**
     * Synchronized RPC-style call 
     * @param args call arguments
     * @param forTransport client transport instance (for MQ server listener)
     * @param timeout call timeout
     * @return call result
     */
    T call(T args, ITransport forTransport, int timeout) throws Exception;

    /**
     * Synchronized RPC-style call 
     * @param args call arguments
     * @return call result
     */
    T call(T args) throws Exception;
    
    /**
     * Synchronized RPC-style call 
     * @param args call arguments
     * @param timeout call timeout
     * @return call result
     */
    T call(T args, int timeout) throws Exception;
    
    /**
     * RPC-style async call 
     * @param args call arguments
     * @param forTransport client transport instance (for MQ server listener)
     * @param listener Listener can handles result/timeout
     */    
    void callAsync(T args, ITransport forTransport, ICallAsyncListener<T> listener) throws Exception;
    /**
     * RPC-style async call 
     * @param args call arguments
     * @param forTransport client transport instance (for MQ server listener)
     * @param listener Listener can handles result/timeout
     * @param timeout call timeout     
     */        
    void callAsync(T args, ITransport forTransport, ICallAsyncListener<T> listener, int timeout) throws Exception;
    
    /**
     * RPC-style async call 
     * @param args call arguments     
     * @param listener Listener can handles result/timeout
     */    
    void callAsync(T args, ICallAsyncListener<T> listener) throws Exception;
    
    /**
     * RPC-style async call 
     * @param args call arguments
     * @param listener Listener can handles result/timeout
     * @param timeout call timeout     
     */        
    void callAsync(T args, ICallAsyncListener<T> listener, int timeout) throws Exception;    

    /**
    * Add new session listener
    * @param listener session listener
    */    
    void addListener(IPTPSessionListener<T> listener);

    /**
    * Remove session listener
    * @param listener session listener
    */
    void delListener(IPTPSessionListener<T> listener);
    
    /**
     * Close session
     */
     void close();
}
