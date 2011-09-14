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

package org.bn.mq;

import java.net.URI;

/**
 * Transport connection abstraction
 */
public interface IMQConnection {
    /**
     * Add new connection listener
     * @param listener connection listener
     */
    void addListener(IMQConnectionListener listener);
    
    /**
     * Del the connection listener
     * @param listener connection listener
     */    
    void delListener(IMQConnectionListener listener);    
    
    /**
     * Lookup remote supplier with specified name
     * @param supplierName supplier name
     * @return RemoteSupplier proxy object
     */
    IRemoteSupplier lookup(String supplierName) throws Exception;
    
    /**
     * Create/register supplier on connection with specified name
     * @param supplierName supplier name
     * @return supplier instance
     */
    ISupplier createSupplier(String supplierName);
    
    /**
     * Create/register supplier on connection with specified name
     * @param sessionName PTP session Id
     * @param pointName PTP endpoint Id
     * @return PTP session instance
     */
    <T> IPTPSession<T> createPTPSession(String pointName, String sessionName, Class<T> messageClass);
    
    /**
     * Remove registered supplier
     * @param supplier supplier instance
     */
    void removeSupplier(ISupplier supplier);
    
    /**
     * Get connection addr
     * @return connection addr
     */
    URI getAddr();
    
    /**
     * Start connection
     */
    void start() throws Exception;
    
    /**
     * Close & destroy current connection
     */
    void close();
}
