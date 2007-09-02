/*
 * Copyright 2007 Abdulla G. Abdurakhmanov (abdulla.abdurakhmanov@gmail.com).
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

package org.bn.types;

public class ObjectIdentifier {
	
	private String oidString;
	
    public ObjectIdentifier(String oidAsStr) 
    {
        setValue(oidAsStr);
    }
    
    public ObjectIdentifier() 
    {
    	oidString = null;
    }
    
    public String getValue() 
    {
        return oidString;
    }
    
    public void setValue(String value) 
    {
    	oidString = value;
    }

    public int[] getIntArray()
    {
        String[] sa = oidString.split("\\.");
        int[] ia = new int[sa.length];
        for (int i=0; i < sa.length; i++)
        {
        	ia[i] = new Integer(sa[i]).intValue();
        }
        return ia;
    }
}
