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
 package org.bn.coders.der;

import java.io.OutputStream;

import java.lang.reflect.Field;

import java.util.Map;
import java.util.SortedMap;

import org.bn.annotations.ASN1Sequence;
import org.bn.coders.CoderUtils;
import org.bn.coders.ElementInfo;
import org.bn.coders.ElementType;
import org.bn.coders.TagClass;
import org.bn.coders.UniversalTag;
import org.bn.coders.ber.BERCoderUtils;
import org.bn.coders.ber.BEREncoder;

public class DEREncoder<T> extends BEREncoder<T> {
    public DEREncoder() {
    }

    public int encodeSequence(Object object, OutputStream stream, 
                                 ElementInfo elementInfo) throws Exception {
        ASN1Sequence seqInfo = elementInfo.getAnnotatedClass().getAnnotation(ASN1Sequence.class);
        if(!CoderUtils.isSequenceSet(elementInfo)) {
            return super.encodeSequence(object, stream, elementInfo);
        }
        else {
            int resultSize = 0;
            Field[] fields = null;
            if(elementInfo.hasPreparedInfo()) {
                fields = elementInfo.getPreparedInfo().getFields();
            }
            else {
                SortedMap<Integer,Field> fieldOrder = CoderUtils.getSetOrder(object.getClass());
                fields = new Field[0];
                fields = fieldOrder.values().toArray(fields);
            }
            
            for(int i=0; i < fields.length; i++) {
                resultSize+=encodeSequenceField(object, fields.length - 1 - i, fields[fields.length - 1 - i], stream, elementInfo);
            }
            resultSize += encodeHeader (BERCoderUtils.getTagValueForElement (elementInfo,TagClass.Universal, ElementType.Constructed, UniversalTag.Set), resultSize, stream );
            return resultSize;            
        }
    }
}
