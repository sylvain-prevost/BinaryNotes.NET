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

import java.util.HashMap;
import java.util.Map;

import org.bn.mq.IMessageQueue;
import org.bn.mq.IPersistenceQueueStorage;
import org.bn.mq.IPersistenceStorage;
import org.bn.mq.MQFactory;

public class InMemoryStorage<T> implements IPersistenceStorage<T> {
    private Map<String, InMemoryQueueStorage<T> > storages = new HashMap<String, InMemoryQueueStorage<T> > ();
    private String storageName;
    private Map<String,Object> storageProperties;
    
    public InMemoryStorage(Map<String,Object> storageProperties) throws Exception {
        this.storageProperties = storageProperties;
        if(!this.storageProperties.containsKey("storageName")) {
            throw new Exception("Unable to present property: 'storageName'");
        }
        else {
            this.storageName = (String)this.storageProperties.get("storageName");
        }
    }
    
    public IPersistenceQueueStorage<T> createQueueStorage(String queueStorageName) {
        synchronized(storages) {
            InMemoryQueueStorage<T> result = storages.get(storageName+"/"+queueStorageName);
            if(result==null) {
                result = new InMemoryQueueStorage<T>(storageName+"/"+queueStorageName);
                storages.put(storageName+"/"+queueStorageName,result);
            }
            return result;
        }
    }
}
