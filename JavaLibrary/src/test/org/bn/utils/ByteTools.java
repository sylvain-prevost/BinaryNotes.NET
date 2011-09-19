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

package test.org.bn.utils;

import java.io.ByteArrayOutputStream;

import junit.framework.TestCase;

public class ByteTools extends TestCase{
    public static String byteArrayToHexString(byte buffer[]) {
       byte ch = 0x00;
       int i = 0; 

       if (buffer == null || buffer.length <= 0)
           return null;
       String pseudo[] = {"0", "1", "2","3", "4", "5", "6", "7", "8","9", "A", "B", "C", "D", "E","F"};
       StringBuffer out = new StringBuffer(buffer.length * 3);
       while (i < buffer.length) {
           ch = (byte) (buffer[i] & 0xF0); 
           ch = (byte) (ch >>> 4); 
           ch = (byte) (ch & 0x0F);    
           out.append(pseudo[ (int) ch]); 
           ch = (byte) (buffer[i] & 0x0F);
           out.append(pseudo[ (int) ch]); 
           i++;
           if(i!=buffer.length)
            out.append("-");
       }

       String rslt = new String(out);
       return rslt;
    }    

    public static void checkBuffers(byte[] src, byte[] standard) {
        assertEquals( src.length , standard. length);
        for(int i=0;i<src.length;i++) {            
            assertEquals(src[i],standard[i]);
        }
    }

    public static String byteArrayToHexString(ByteArrayOutputStream stream) {
        return byteArrayToHexString(stream.toByteArray());
    }

}
