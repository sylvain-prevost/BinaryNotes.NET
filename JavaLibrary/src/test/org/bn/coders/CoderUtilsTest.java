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
package test.org.bn.coders;

import junit.framework.TestCase;

import org.bn.coders.CoderUtils;
import org.bn.types.BitString;

import test.org.bn.utils.ByteTools;

public class CoderUtilsTest extends TestCase {
    public CoderUtilsTest(String sTestName) {
        super(sTestName);
    }

    /**
     * @see CoderUtils#defStringToOctetString(String)
     */
    public void testDefStringToOctetString() {
        BitString result = CoderUtils.defStringToOctetString("'FFAABBEE'H");
        ByteTools.checkBuffers(result.getValue(), new byte[] { (byte)0xFF, (byte)0xAA, (byte)0xBB, (byte)0xEE });

        result = CoderUtils.defStringToOctetString("'FFAABBEEC'H");
        ByteTools.checkBuffers(result.getValue(), new byte[] { (byte)0xFF, (byte)0xAA, (byte)0xBB, (byte)0xEE, (byte)0xC0 });
        
        result = CoderUtils.defStringToOctetString("'111100001111000010011001'B");
        ByteTools.checkBuffers(result.getValue(), new byte[] { (byte)0xF0, (byte)0xF0, (byte)0x99});        
        
        result = CoderUtils.defStringToOctetString("'1111000011110000100110011'B");
        ByteTools.checkBuffers(result.getValue(), new byte[] { (byte)0xF0, (byte)0xF0, (byte)0x99, (byte)0x80});        
        
    }
    
    public void testIntegerLengthCalc() {
        assertEquals(CoderUtils.getIntegerLength(0xF0F0F0),4);
    }
}
