/*
 Copyright 2006-2011 Abdulla Abdurakhmanov (abdulla@latestbit.com)
 Original sources are available at www.latestbit.com

 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at

 http://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.
 */

package org.bn.mq.impl;

import java.util.Map;

import org.bn.mq.IMessageQueue;
import org.bn.mq.IPersistenceQueueStorage;
import org.bn.mq.IPersistenceStorage;

public class NullStorage<T> implements IPersistenceStorage<T> {
    public NullStorage(Map<String,Object> storageProperties) {
    }
    
    private NullQueueStorage<T> nullQueueStorage = new NullQueueStorage<T>();
    public IPersistenceQueueStorage<T> createQueueStorage(String queueStorageName) {
        return nullQueueStorage;
    }
}
