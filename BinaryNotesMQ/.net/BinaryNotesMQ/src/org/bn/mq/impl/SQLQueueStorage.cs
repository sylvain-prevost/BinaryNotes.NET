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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using org.bn.mq.protocol;
using org.bn.mq.net;

namespace org.bn.mq.impl
{
	
	public class SQLQueueStorage<T> : IPersistenceQueueStorage<T>
	{
		protected internal DbConnection connection;
		protected internal string queueStorageName;
		protected internal DbCommand persistenceSubscribeCmd = null;
		protected internal DbCommand checkPersistenceSubscribeCmd = null;
		protected internal DbCommand persistenceUnsubscribeCmd = null;
		protected internal DbCommand registerMessageCmd = null;
		protected internal DbCommand removeMessageCmd = null;
		protected internal DbCommand getMessagesCmd = null;
		
		public SQLQueueStorage(DbConnection con, string queueStorageName)
		{
			this.connection = con;
			this.queueStorageName = queueStorageName;
			prepareTables();
		}

        private void prepareTables()
        {
            checkTable(
                "select consumerId from " 
                + queueStorageName + "_subscriptions where consumerId=''", 
                "create table " + queueStorageName + "_subscriptions (consumerId varchar(512) PRIMARY KEY)"
            );
            checkTable(
                "select consumerId from " + queueStorageName + "_messages where consumerId=''", 
                "create table " + queueStorageName + "_messages (id varchar(512), consumerId varchar(512), message BINARY, CONSTRAINT uniqueMessage PRIMARY KEY (id,consumerId) )  "
            );

            getMessagesCmd = prepareStatement("select message from " + queueStorageName + "_messages where consumerId = @consumerId ");
            DbParameter consumerId = getMessagesCmd.CreateParameter();
            consumerId.ParameterName = "@consumerId";
            getMessagesCmd.Parameters.Add(consumerId);

            persistenceSubscribeCmd = prepareStatement("insert into " + queueStorageName + "_subscriptions (consumerId) values(@consumerId) ");
            consumerId = persistenceSubscribeCmd.CreateParameter();
            consumerId.ParameterName = "@consumerId";
            persistenceSubscribeCmd.Parameters.Add(consumerId);

            checkPersistenceSubscribeCmd = prepareStatement("select * from " + queueStorageName + "_subscriptions where consumerId = @consumerId ");
            consumerId = checkPersistenceSubscribeCmd.CreateParameter();
            consumerId.ParameterName = "@consumerId";
            checkPersistenceSubscribeCmd.Parameters.Add(consumerId);

            persistenceUnsubscribeCmd = prepareStatement("delete from " + queueStorageName + "_subscriptions where consumerId = @consumerId ");
            consumerId = persistenceUnsubscribeCmd.CreateParameter();
            consumerId.ParameterName = "@consumerId";
            persistenceUnsubscribeCmd.Parameters.Add(consumerId);

            registerMessageCmd = prepareStatement(
                "insert into " + queueStorageName + "_messages (id,consumerId,message) " 
                + "select @messageId, consumerId, @message from  " + queueStorageName + "_subscriptions ");
            DbParameter message = registerMessageCmd.CreateParameter();
            message.ParameterName = "@message";
            registerMessageCmd.Parameters.Add(message);
            DbParameter messageId = registerMessageCmd.CreateParameter();
            messageId.ParameterName = "@messageId";
            registerMessageCmd.Parameters.Add(messageId);

            removeMessageCmd = prepareStatement("delete from " + queueStorageName + "_messages where id = @messageId and consumerId = @consumerId ");
            consumerId = removeMessageCmd.CreateParameter();
            consumerId.ParameterName = "@consumerId";
            removeMessageCmd.Parameters.Add(consumerId);
            messageId = removeMessageCmd.CreateParameter();
            messageId.ParameterName = "@messageId";
            removeMessageCmd.Parameters.Add(messageId);
            
        }

        private DbCommand prepareStatement(string sql)
        {
            DbCommand cmd = this.connection.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        private void checkTable(System.String checkCommandSql, System.String executeCmdSql)
        {
            DbCommand statement = prepareStatement(checkCommandSql);
            try
            {
                statement.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                ex = null;
                statement = prepareStatement(executeCmdSql);
                statement.ExecuteNonQuery();
            }
        }

        public object deserialize(System.IO.BinaryReader binaryReader)
        {
#if !PocketPC
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            return formatter.Deserialize(binaryReader.BaseStream);
#else
            throw new Exception("Unable to supported serialization of objects on CF!");
#endif
        }

        public void serialize(System.IO.Stream stream, object objectToSend)
        {
#if !PocketPC
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(stream, objectToSend);
#else
            throw new Exception("Unable to supported serialization of objects on CF!");
#endif

        }


		public virtual IList<IMessage<T>> getMessagesToSend(IConsumer<T> consumer)
		{
            IList<IMessage<T>> result = new List<IMessage<T>>();
			try
			{
				lock (connection)
				{
                    getMessagesCmd.Parameters["@consumerId"].Value = consumer.Id;
					DbDataReader resultSet = this.getMessagesCmd.ExecuteReader();
                    while (resultSet.Read())
					{
                        byte[] serializedObj = new byte[65535];
						long len = resultSet.GetBytes(0,0,serializedObj,0,65535);
						System.IO.BinaryReader inputStream = new System.IO.BinaryReader(
                            new System.IO.MemoryStream(serializedObj,0,(int)len)
                        );
                        IMessage<T> message = (IMessage<T>)deserialize(inputStream);
						result.Add(message);
					}
                    resultSet.Close();
				}
			}
			catch (System.Exception ex)
			{
                Console.WriteLine(ex.ToString());
			}
			return result;
		}
		
		public virtual void  persistenceSubscribe(IConsumer<T> consumer)
		{
			lock (connection)
			{
                DbTransaction trans =  connection.BeginTransaction();
				try
				{
                    checkPersistenceSubscribeCmd.Parameters["@consumerId"].Value = consumer.Id;
					DbDataReader exists = this.checkPersistenceSubscribeCmd.ExecuteReader();
					if (!exists.Read())
					{
                        exists.Close();
                        this.persistenceSubscribeCmd.Parameters["@consumerId"].Value = consumer.Id;
						this.persistenceSubscribeCmd.ExecuteNonQuery();
					}
                    else
                        exists.Close();
                    trans.Commit();
				}
				catch (System.Exception ex)
				{
                    Console.WriteLine(ex.ToString());
                    trans.Rollback();
					throw ex;
				}                
			}
		}
		
		public virtual void  persistenceUnsubscribe(IConsumer<T> consumer)
		{
			lock (connection)
			{
                DbTransaction trans =  connection.BeginTransaction();
                try
                {
                    this.persistenceUnsubscribeCmd.Parameters["@consumerId"].Value = consumer.Id;
                    this.persistenceUnsubscribeCmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    trans.Rollback();
                    throw ex;
                }                
            }
		}
		
		public virtual void  registerPersistenceMessage(IMessage<T> message)
		{
			lock (connection)
			{
                DbTransaction trans =  connection.BeginTransaction();
                try
                {
				    System.IO.MemoryStream byteOutput = new System.IO.MemoryStream();
				    System.IO.BinaryWriter output = new System.IO.BinaryWriter(byteOutput);
                    serialize(byteOutput, message);
                    this.registerMessageCmd.Parameters["@messageId"].Value = message.Id;
                    this.registerMessageCmd.Parameters["@message"].Value = byteOutput.ToArray();
                    this.registerMessageCmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    trans.Rollback();
                    throw ex;
                }                

			}
		}

        public void removeDeliveredMessage(string consumerId, string messageId)
		{
			lock (connection)
			{
                DbTransaction trans =  connection.BeginTransaction();
                try
                {
                    this.removeMessageCmd.Parameters["@messageId"].Value = messageId;
                    this.removeMessageCmd.Parameters["@consumerId"].Value = consumerId;
				    this.removeMessageCmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    trans.Rollback();
                    throw ex;
                }                

			}
		}

        ~SQLQueueStorage()
        {
            close();
        }
				
		public virtual void  close()
		{
            lock (connection)
            {
                if (this.connection.State == ConnectionState.Open)
                {
                    try
                    {
                        this.connection.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
		}
	}
}