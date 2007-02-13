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

package org.bn.mq;

import org.bn.mq.protocol.MessageEnvelope;

/**
 * Interface for implementation listener for handling async calls
 */
public interface ICallAsyncListener<T> {
    /**
     * The event invokes when result for call has been received
     * @param request Source user request (handback)
     * @param result Result from consumer
     */
    void onCallResult(T request, T result);
    
    /**
     * The event invokes when timeout for call is expired
     * @param request Source user request (handback)
     */
    void onCallTimeout(T request);
}
