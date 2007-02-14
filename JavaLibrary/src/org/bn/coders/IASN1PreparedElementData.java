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

package org.bn.coders;

import java.lang.reflect.Field;

import org.bn.metadata.ASN1ElementMetadata;
import org.bn.metadata.ASN1Metadata;
import org.bn.metadata.constraints.IASN1ConstraintMetadata;

public interface IASN1PreparedElementData {
    ASN1Metadata getTypeMetadata();
    
    IASN1ConstraintMetadata getConstraint();    
    boolean hasConstraint();
    
    
    Field[] getFields();
    Field getField(int index);
    ASN1PreparedElementData getFieldMetadata(int index);
    
    Field getValueField();
    ASN1PreparedElementData getValueMetadata();    
    
    ASN1ElementMetadata getASN1ElementInfo();
    boolean hasASN1ElementInfo();
    
    Object invokeSetterMethod(Object object, Object param) throws Exception;
    Object invokeDoSelectMethod(Object object, Object param) throws Exception;
    Object invokeGetterMethod(Object object, Object param) throws Exception;
    Object invokeIsSelectedMethod(Object object, Object param) throws Exception;
    
    boolean isMemberClass();
    Object newInstance() throws Exception;
}
