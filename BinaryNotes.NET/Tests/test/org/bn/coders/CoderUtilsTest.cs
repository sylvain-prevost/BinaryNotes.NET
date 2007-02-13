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

using System;
using System.Collections.Generic;
using System.Text;
using csUnit;
using org.bn.coders;
using org.bn.types;
using test.org.bn.utils;


namespace test.org.bn.coders
{
    [TestFixture]
    class CoderUtilsTest
    {
        /**
         * @see CoderUtils#defStringToOctetString(String)
         */
        public void testDefStringToOctetString() {
            BitString result = CoderUtils.defStringToOctetString("'FFAABBEE'H");
            ByteTools.checkBuffers(result.Value, new byte[] { (byte)0xFF, (byte)0xAA, (byte)0xBB, (byte)0xEE });

            result = CoderUtils.defStringToOctetString("'FFAABBEEC'H");
            ByteTools.checkBuffers(result.Value, new byte[] { (byte)0xFF, (byte)0xAA, (byte)0xBB, (byte)0xEE, (byte)0xC0 });
            
            result = CoderUtils.defStringToOctetString("'111100001111000010011001'B");
            ByteTools.checkBuffers(result.Value, new byte[] { (byte)0xF0, (byte)0xF0, (byte)0x99});

            result = CoderUtils.defStringToOctetString("'1111000011110000100110011'B");
            ByteTools.checkBuffers(result.Value, new byte[] { (byte)0xF0, (byte)0xF0, (byte)0x99, (byte)0x80 });        

        }

    }
}
