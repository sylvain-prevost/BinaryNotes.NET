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

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;

import org.bn.annotations.ASN1String;
import org.bn.coders.CoderUtils;
import org.bn.coders.DecodedObject;
import org.bn.coders.ElementInfo;
import org.bn.coders.UniversalTag;
import org.bn.utils.BitArrayInputStream;
import org.bn.utils.BitArrayOutputStream;

public class PERUnalignedDecoder extends PERAlignedDecoder {

    protected void skipAlignedBits(InputStream stream) {
        // Do nothing! Unaligned encoding ;)
    }
    
    protected long decodeConstraintNumber(long min, long max, BitArrayInputStream stream) throws Exception {
     int result = 0;
     long valueRange = max - min;
     // !!! int narrowedVal = value - min; !!!
     int maxBitLen = PERCoderUtils.getMaxBitLength(valueRange);
    
     if(valueRange == 0) {
         return max;
     }     
     //For the UNALIGNED variant the value is always encoded in the minimum 
     // number of bits necessary to represent the range (defined in 10.5.3). 
     int currentBit = maxBitLen;
     while(currentBit > 7 ) {
         currentBit-=8;
         result |= stream.read() << currentBit;         
     }
     if(currentBit > 0) {
        result |= stream.readBits(currentBit);
     }
     result+=min;
     return result;
    }
    
    public DecodedObject decodeString(DecodedObject decodedTag, Class objectClass, 
                                         ElementInfo elementInfo, 
                                  InputStream stream) throws IOException, 
                                                                    Exception {
     if(!PERCoderUtils.is7BitEncodedString(elementInfo))
         return super.decodeString(decodedTag, objectClass, elementInfo, stream);
     else {
            DecodedObject<String> result = new DecodedObject<String>();
         int strLen = decodeLength(elementInfo, stream);

         if(strLen <= 0) {
             result.setValue("");
             return result;
         }
     
         BitArrayInputStream bitStream = (BitArrayInputStream)stream;
         byte[] buffer = new byte[strLen];
         // 7-bit decoding of string
         for(int i=0;i<strLen;i++)
            buffer[i]=(byte)bitStream.readBits(7);
        result.setValue( new String(buffer) );
        return result;
     }
     
    }    
    

}
