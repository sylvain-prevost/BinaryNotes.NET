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

package org.bn.mq.net.tcp;

import java.util.Calendar;
import java.util.Date;
import java.util.HashMap;

import javax.management.InstanceNotFoundException;
import javax.management.ListenerNotFoundException;
import javax.management.Notification;
import javax.management.timer.Timer;
import javax.management.NotificationListener;

import org.bn.mq.net.ITransportCallListener;
import org.bn.mq.protocol.MessageEnvelope;

public class AsyncCallManager implements NotificationListener {

    protected HashMap<String,AsyncCallItem> asyncCalls = new HashMap<String,AsyncCallItem>();
    protected Timer asyncCallTimer = new Timer();

    public AsyncCallManager() {
        start();
    }
    
    public synchronized void start() {
        asyncCallTimer.addNotificationListener(this,null,null);
        asyncCallTimer.start();        
    }
    
    public synchronized void stop() {
        asyncCallTimer.stop();
        asyncCallTimer.removeAllNotifications();
        try {
            asyncCallTimer.removeNotificationListener(this);
        }
        catch (ListenerNotFoundException e) { e = null; }
        synchronized(asyncCalls) {
            asyncCalls.clear();
        }        
    }
    
    public void storeRequest(MessageEnvelope request, ITransportCallListener listener, int timeout) {
        AsyncCallItem item = new AsyncCallItem();
        item.setRequest(request);
        item.setListener(listener);        
        synchronized(asyncCalls) {
            asyncCalls.put(request.getId(),item);
        }
        Date tm = new Date();
        tm.setTime(tm.getTime()+timeout*1000);
        item.setTimerId(asyncCallTimer.addNotification("AsyncCall","RequestId",item.getRequest().getId(),tm));
    }
    
    public AsyncCallItem getAsyncCall(String id) {
        AsyncCallItem res = null;
        synchronized(asyncCalls) {
            res = asyncCalls.get(id);
            if(res!=null)            
                asyncCalls.remove(id);
        }
        if(res!=null) {
            try {
                asyncCallTimer.removeNotification(res.getTimerId());
            }
            catch (InstanceNotFoundException e) { e = null; }
        }
        return res;
    }
    
    public AsyncCallItem getAsyncCall(MessageEnvelope result) {
        return getAsyncCall(result.getId());
    }
        
    
    public void dispose() {
        stop();
    }

    public void handleNotification(Notification notification, Object handback) {
        AsyncCallItem res = null;
        synchronized(asyncCalls) {
            res = asyncCalls.get(notification.getUserData());
            if(res!=null)
                asyncCalls.remove(notification.getUserData());
        }
        
        if(res!=null) {
            if(res.getListener()!=null)
                res.getListener().onCallTimeout(res.getRequest());
        }
        
    }
}
