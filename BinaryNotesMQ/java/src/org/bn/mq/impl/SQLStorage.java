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

import java.sql.Connection;
import java.sql.DriverManager;

import java.util.Map;

import org.bn.mq.IPersistenceQueueStorage;
import org.bn.mq.IPersistenceStorage;

public class SQLStorage<T> implements IPersistenceStorage<T> {    
    private String storageName;
    private Map<String,Object> storageProperties;
    
    public SQLStorage(Map<String,Object> storageProperties) throws Exception {
        this.storageProperties = storageProperties;
        if(!this.storageProperties.containsKey("dbConnectionString")) {
            throw new Exception("Unable to present property: 'dbConnectionString'");
        }
        else {
            this.storageName = (String)this.storageProperties.get("dbConnectionString");
        }
    }
    
    protected Connection getConnection() throws Exception {
        return DriverManager.getConnection(storageName);
    }
    
    public IPersistenceQueueStorage<T> createQueueStorage(String queueStorageName) throws Exception {
        Connection con = getConnection();        
        con.setAutoCommit(true);
        return new SQLQueueStorage<T>(con,queueStorageName);
    }        
}
