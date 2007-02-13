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
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using org.bn.mq.protocol;
using org.bn.mq.net;

namespace org.bn.mq.impl
{

    public class SQLStorage<T> : IPersistenceStorage<T>
	{		
        IDictionary<String, Object> storageProperties;

        public SQLStorage(IDictionary<String, Object> storageProperties)
		{
            this.storageProperties = storageProperties;
		}

        virtual protected internal DbConnection getConnection()
        {
            DbConnection connection = null;
            Assembly asm = null;
            if (storageProperties.ContainsKey("dbAssemblyName"))
            {
                asm = Assembly.Load((string)storageProperties["dbAssemblyName"]);
            }
            else
                throw new Exception("Unable to present property: 'dbAssemblyName'!");
            
            string dbConClassName = null;
            if (storageProperties.ContainsKey("dbConnectionClass"))
            {
                dbConClassName = (string)storageProperties["dbConnectionClass"];
            }
            else
                throw new Exception("Unable to present property: 'dbConnectionClass'!");

            Type dbConClass = asm.GetType(dbConClassName);
            connection = (DbConnection)Activator.CreateInstance(dbConClass);

            string dbConString = null;
            if (storageProperties.ContainsKey("dbConnectionString"))
            {
                dbConString = (string)storageProperties["dbConnectionString"];
            }
            else
                throw new Exception("Unable to present property: 'dbConnectionString'!");
            connection.ConnectionString = dbConString;
            connection.Open();
            return connection;
        }

		
		public virtual IPersistenceQueueStorage<T> createQueueStorage(string queueStorageName)
		{
            return new SQLQueueStorage<T>(getConnection(), queueStorageName);
		}
	}
}