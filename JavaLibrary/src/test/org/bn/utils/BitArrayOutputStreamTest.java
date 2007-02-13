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
package test.org.bn.utils;

import java.io.ByteArrayOutputStream;

import java.io.IOException;

import junit.framework.TestCase;

import org.bn.utils.BitArrayOutputStream;


public class BitArrayOutputStreamTest extends TestCase {
   
    public BitArrayOutputStreamTest(String sTestName) {
        super(sTestName);
    }

    /**
     * @see BitArrayOutputStream#write(int)
     */
    public void testWrite() throws IOException {
        BitArrayOutputStream stream = new BitArrayOutputStream();
        stream.write(0xFF);
        stream.writeBit(true);
        stream.writeBit(false);
        stream.writeBit(true);
        stream.writeBit(false);
        stream.write(0xFF);
        stream.write(0x0F);
        stream.writeBit(true);
        stream.writeBit(true);
        stream.writeBit(true);
        stream.writeBit(true);
        System.out.println ("Write " + ByteTools.byteArrayToHexString(stream));
        ByteTools.checkBuffers(stream.toByteArray(),new byte[] { 
            (byte)0xFF, (byte)0xAF, (byte)0xF0, (byte)0xFF}
        );

        stream.writeBit(true);
        stream.writeBit(false);
        stream.writeBit(true);
        stream.writeBit(false);
        stream.write ( new byte[]{ (byte)0xCC, (byte)0xFF, (byte)0xFF, (byte)0xBB });
        System.out.println ("After buf write " + ByteTools.byteArrayToHexString(stream));
        ByteTools.checkBuffers(stream.toByteArray(),new byte[] { 
                (byte)0xFF, (byte)0xAF, (byte)0xF0, (byte)0xFF,
                (byte)0xAC, (byte)0xCF, (byte)0xFF,(byte)0xFB, (byte)0xB0
            }
        );
        stream.align();
        stream.writeBit(true);
        stream.writeBit(true);
        stream.align();
        stream.write(0xFF);
        
        System.out.println ("After align " + ByteTools.byteArrayToHexString(stream));
        ByteTools.checkBuffers(stream.toByteArray(),new byte[] { 
                (byte)0xFF, (byte)0xAF, (byte)0xF0, (byte)0xFF,
                (byte)0xAC, (byte)0xCF, (byte)0xFF,(byte)0xFB, (byte)0xB0,
                (byte)0xC0, (byte)0xFF
            }
        );
        
    }
        
}
