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
package org.bn.coders.per;

import java.lang.reflect.Field;

import java.util.LinkedList;
import java.util.List;
import java.util.SortedMap;
import java.util.TreeMap;

import org.bn.annotations.ASN1Element;
import org.bn.coders.CoderUtils;
import org.bn.coders.ElementInfo;
import org.bn.coders.UniversalTag;

public class PERCoderUtils {
    public static int getMaxBitLength(long value) {
        int bitCnt = 0;
        while( value !=0 ) {
            value >>>= 1;
            bitCnt++;
        }
        return bitCnt;
    }    
    
    public static int getRealFieldsCount(Class objectClass, ElementInfo info) {
        int result = 0;            
        if(info.hasPreparedInfo()) {
            result = info.getPreparedInfo().getFields().length;
        }
        else {
            for(Field item: objectClass.getDeclaredFields()) {
                if(!item.isSynthetic())
                    result++;
            }
        }
        return result;
    }
    
    public static List<Field> getRealFields(Class objectClass) {        
        List<Field> result = new LinkedList<Field>();        
        for(Field item: objectClass.getDeclaredFields()) {
            if(!item.isSynthetic())
                result.add(item);
        }
        return result;
    }
    
    public static boolean is7BitEncodedString(ElementInfo elementInfo) {
        int stringType = CoderUtils.getStringTagForElement(elementInfo);
        boolean is7Bit = 
            ( 
                stringType == UniversalTag.PrintableString || 
                stringType ==UniversalTag.VisibleString
            )
            ;
        return is7Bit;
    }
    
}
