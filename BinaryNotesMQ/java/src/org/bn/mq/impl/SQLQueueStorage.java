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

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutput;

import java.io.ObjectOutputStream;

import java.sql.Connection;

import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

import java.util.LinkedList;
import java.util.List;

import org.bn.mq.IConsumer;
import org.bn.mq.IMessage;
import org.bn.mq.IPersistenceQueueStorage;

public class SQLQueueStorage<T> implements IPersistenceQueueStorage<T> {
    protected Connection connection;
    protected String queueStorageName;
    protected PreparedStatement persistenceSubscribeCmd = null;
    protected PreparedStatement checkPersistenceSubscribeCmd = null;
    protected PreparedStatement persistenceUnsubscribeCmd = null;
    protected PreparedStatement registerMessageCmd = null;
    protected PreparedStatement removeMessageCmd = null;
    protected PreparedStatement getMessagesCmd = null;
    
    public SQLQueueStorage(Connection con, String queueStorageName) throws Exception {
        this.connection = con;
        this.queueStorageName = queueStorageName;
        prepareTables();
    }
    
    public List<IMessage<T>> getMessagesToSend(IConsumer<T> consumer)  {
        List<IMessage<T>> result = new LinkedList<IMessage<T>>();
        try {
            synchronized(connection) {
                this.getMessagesCmd.setString(1,consumer.getId());
                ResultSet set = this.getMessagesCmd.executeQuery();
                while(set.next()) {
                    byte[] serializedObj = set.getBytes(1);
                    ObjectInputStream is = new ObjectInputStream(new ByteArrayInputStream(serializedObj));
                    IMessage<T> message = (IMessage<T>)is.readObject();
                    result.add(message);
                }
            }            
        }
        catch(Exception ex) {
            ex.printStackTrace();
        }
        return result;
    }

    public void persistenceSubscribe(IConsumer<T> consumer) throws Exception {
        synchronized(connection) {
            try {
                this.checkPersistenceSubscribeCmd.setString(1,consumer.getId());
                ResultSet exists = this.checkPersistenceSubscribeCmd.executeQuery();
                if(!exists.next()) {
                    this.persistenceSubscribeCmd.setString(1,consumer.getId());
                    this.persistenceSubscribeCmd.execute();
                }
            }
            catch(Exception ex) {
                ex.printStackTrace();
                throw ex;
            }
        }
    }

    public void persistenceUnsubscribe(IConsumer<T> consumer) throws Exception {
        synchronized(connection) {
            this.persistenceUnsubscribeCmd.setString(1,consumer.getId());
            this.persistenceUnsubscribeCmd.execute();
        }    
    }

    public void registerPersistenceMessage(IMessage<T> message) throws Exception {
        synchronized(connection) {
            ByteArrayOutputStream byteOutput = new ByteArrayOutputStream();
            ObjectOutputStream output = new ObjectOutputStream(byteOutput);
            output.writeObject(message);        
            this.registerMessageCmd.setString(1,message.getId());
            this.registerMessageCmd.setBytes(2,byteOutput.toByteArray());
            this.registerMessageCmd.execute();
        }            
    }

    public void removeDeliveredMessage(String consumerId, String messageId) throws SQLException {
        synchronized(connection) {
            this.removeMessageCmd.setString(1,messageId);
            this.removeMessageCmd.setString(2,consumerId);
            this.removeMessageCmd.execute();
        }        
    }

    private void prepareTables() throws Exception {
        checkTable(
            "select consumerId from "+queueStorageName+"_subscriptions where consumerId=''",
            "create table "+queueStorageName+"_subscriptions (consumerId varchar(512) PRIMARY KEY)"
        );
        checkTable(
            "select consumerId from "+queueStorageName+"_messages where consumerId=''",
            "create table "+queueStorageName+"_messages (id varchar(512), consumerId varchar(512), message BINARY, CONSTRAINT uniqueMessage PRIMARY KEY (id,consumerId) )  "
        );
        
        persistenceSubscribeCmd = connection.prepareStatement("insert into "+queueStorageName+"_subscriptions (consumerId) values(?) ");
        checkPersistenceSubscribeCmd = connection.prepareStatement("select * from "+queueStorageName+"_subscriptions where consumerId = ? ");
        persistenceUnsubscribeCmd = connection.prepareStatement("delete from "+queueStorageName+"_subscriptions where consumerId = ? ");
        registerMessageCmd = connection.prepareStatement(
            "insert into "+queueStorageName+"_messages (id,consumerId,message) "+
            "select ?, consumerId, ? from  "+queueStorageName+"_subscriptions "
        );
        removeMessageCmd = connection.prepareStatement("delete from "+queueStorageName+"_messages where id = ? and consumerId = ? ");
        getMessagesCmd = connection.prepareStatement("select message from "+queueStorageName+"_messages where consumerId = ?");        
    }

    private void checkTable(String checkCommandSql, String executeCmdSql) throws Exception{
        Statement statement = connection.createStatement();
        try {
            statement.execute(checkCommandSql);
        }
        catch(Exception ex) {
            ex = null;
            statement.execute(executeCmdSql);
        }
        //connection.commit();
    }
    
    public void dispose() {
        close();
    }

    public void close() {
        try {
            this.connection.close();
        }
        catch (SQLException e) {
            e = null;
        }
    }
}
