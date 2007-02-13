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

package org.bn.mq.net;

import java.net.URI;

import java.util.LinkedList;
import java.util.List;

import org.bn.mq.net.tcp.TransportFactory;


public class MessagingBusNet {
    private static MessagingBusNet instance;
    
    protected MessagingBusNet() {
        initDefaultFactories();
    }
    
    protected List<ITransportFactory> factories = new LinkedList<ITransportFactory>();
    
    protected void initDefaultFactories() {
        addFactory(new TransportFactory());
    }
    
    public static MessagingBusNet getInstance() {
        return instance;
    }
    
    
    public void addFactory(ITransportFactory factory) {
        synchronized(factories) {
            factories.add(factory);
        }
    }
    
    public void removeFactory(ITransportFactory factory) {
        synchronized(factories) {
            factories.remove(factory);
        }
    }
    
    public ITransportFactory getFactory(URI addr) throws Exception {
        synchronized(factories) {
            for(ITransportFactory factory: factories) {
                if(factory.checkURISupport(addr)) {
                    return factory;
                }
            }
        }
        throw new Exception("Unable to find supported factory for URI"+addr);
    }
}
