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

using org.bn;
using org.bn.coders;
using csUnit;
using test.org.bn.utils;
using test.org.bn.coders;
using test.org.bn.coders.ber;
using test.org.bn.coders.per;
using test.org.bn.coders.der;
using test.org.bn.performance;

namespace Tests
{
    class Program
    {
        static void runEncoderTest(EncoderTest test)
        {
            test.testEncode();
            test.testEncodeChoice();
            test.testEncodeInteger();
            test.testEncodeString();
            test.testEnum();
            test.testITUEncode();
            test.testNullEncode();
            test.testRecursiveDefinition();
            test.testSequenceOfString();
            test.testSequenceWithEnum();
            test.testSequenceWithNull();
            test.testTaggedNullEncode();
            test.testNegativeInteger();
            test.testEncodeSet();
            test.testEncodeBitString();
            test.testEncodeBitStringSmall();
            test.testEncodeUnicodeString();
            test.testEncodeBitStringBnd();
            test.testEncodeVersion1_2();
            test.testEncodeChoiceInChoice();
            test.testEncodeTaggedSeqInSeq();
            test.testEncodeReals();            
            test.testEncodeTaggedSequence();
            test.testEncodeLongTag();
            test.testEncodeLongTag2();
            test.testSequenceOfUTFString();
            test.testEncodeOID();
        }

        static void runDecoderTest(DecoderTest test)
        {
            test.testDecode();
            test.testDecodeChoice();
            test.testDecodeInteger();
            test.testDecodeString();
            test.testDecodeStringArray();
            test.testEnum();
            test.testITUDeDecode();
            test.testNullDecode();
            test.testRecursiveDefinition();
            test.testSequenceWithEnum();
            test.testSequenceWithNullDecode();
            test.testTaggedNullDecode();
            test.testDecodeNegativeInteger();
            test.testDecodeSet();
            test.testDecodeBitStr();
            test.testDecodeUnicodeStr();
            test.testDecodeVersion1_2();
            test.testDecodeChoiceInChoice();
            test.testDecodeTaggedSeqInSeq();
            test.testDecodeReal();
            test.testDecodeLongTag();
            test.testDecodeLongTag2();
            test.testDecodeCSEnum();
        }

        [STAThread]
        static void Main(string[] args)
        {
            new BitArrayInputStreamTest("").testRead();
            new BitArrayOutputStreamTest("").testWrite();
            new CoderUtilsTest().testDefStringToOctetString();

            runEncoderTest(new BEREncoderTest(""));
            runEncoderTest(new PERAlignedEncoderTest(""));
            runEncoderTest(new PERUnalignedEncoderTest(""));
            runEncoderTest(new DEREncoderTest(""));

            runDecoderTest(new BERDecoderTest(""));
            runDecoderTest(new PERAlignedDecoderTest(""));
            runDecoderTest(new PERUnalignedDecoderTest(""));
            runDecoderTest(new DERDecoderTest(""));

            new DummyPerformanceTest().testEncodePerf();
            new DummyPerformanceTest().testDecodePerf();
        }
    }
}
