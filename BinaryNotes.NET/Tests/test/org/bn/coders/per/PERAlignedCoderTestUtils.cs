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
using System;
using org.bn;
using org.bn.coders;
using csUnit;
using test.org.bn.coders.test_asn;

namespace test.org.bn.coders.per
{
	
	public class PERAlignedCoderTestUtils:CoderTestUtilities
	{
		public override byte[] createNullSeqBytes()
		{
			return new byte[0];
		}
		
		public override byte[] createTaggedNullSeqBytes()
		{
			return new byte[0];
		}
		
		public override byte[] createEnumBytes()
		{
			return new byte[]{(byte) (0x20)};
		}
		
		public override byte[] createITUSeqBytes()
		{
			return new byte[]{(byte) (0x80), (byte) (0x05), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x05), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x05), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x05), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x05), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x05), (byte) (0x64), (byte) (0x64), (byte) (0x64), (byte) (0x64), (byte) (0x64), (byte) (0x05), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65)};
		}
		
		public override byte[] createDataSeqBytes()
		{
			return new byte[]{(byte) (0x40), (byte) (0x00), (byte) (0x00), (byte) (0x07), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x01), (byte) (0xBA), (byte) (0x00), (byte) (0x01), (byte) (0x00), (byte) (0x44), (byte) (0x02), (byte) (0x06), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x05), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x01), (byte) (0x00), (byte) (0x07), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0xFF)};
		}
		
		public override byte[] createSequenceWithEnumBytes()
		{
			return new byte[]{(byte) (0x05), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x20), (byte) (0x20)};
		}
		
		public override byte[] createTestInteger1Bytes()
		{
			return new byte[]{(byte) (0x0F)};
		}
		
		public override byte[] createTestInteger2Bytes()
		{
			return new byte[]{(byte) 0x0F, (byte) (0xF0)};
		}
		
		public override byte[] createTestInteger3Bytes()
		{
			return new byte[]{(byte) (0xFF), (byte) (0xF0)};
		}
		
		public override byte[] createTestInteger4Bytes()
		{
			return new byte[]{(byte) (0x60), (byte) (0x00), (byte) (0xF0), (byte) (0xF0), (byte) (0xF0)};
		}
		
		public override byte[] createSeqWithNullBytes()
		{
			return new byte[]{(byte) (0x03), (byte) (0x73), (byte) (0x73), (byte) (0x73), (byte) (0x03), (byte) (0x64), (byte) (0x64), (byte) (0x64)};
		}
		
		public override byte[] createTestRecursiveDefinitionBytes()
		{
			return new byte[]{(byte) (0x80), (byte) (0x05), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x00), (byte) (0x05), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62)};
		}
		
		public override byte[] createUnboundedTestIntegerBytes()
		{
			return new byte[]{(byte) (0x04), (byte) (0x00), (byte) (0xFA), (byte) (0xFB), (byte) (0xFC)};
		}
		
		public override byte[] createTestIntegerRBytes()
		{
			return new byte[]{(byte) (0x40)};
		}
		
		public override byte[] createTestInteger2_12Bytes()
		{
			return new byte[]{(byte) (0x1F), (byte) (0xF1)};
		}
		
		public override byte[] createTestPRNBytes()
		{
			return new byte[]{(byte) (0x05), (byte) (0x48), (byte) (0x65), (byte) (0x6C), (byte) (0x6C), (byte) (0x6F)};
		}
		
		public override byte[] createTestOCTBytes()
		{
			return new byte[]{(byte) (0x05), (byte) (0x01), (byte) (0x02), (byte) (0xFF), (byte) (0x03), (byte) (0x04)};
		}
		
		public override byte[] createDataSeqMOBytes()
		{
			return new byte[]{(byte) (0x7F), (byte) (0x01), (byte) (0x40), (byte) (0x00), (byte) (0x00), (byte) (0x07), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x01), (byte) (0xAB), (byte) (0x80), (byte) (0x01), (byte) (0x00), (byte) (0x00), (byte) (0x02), (byte) (0x06), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x05), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x02), (byte) (0x00), (byte) (0x07), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x00), (byte) (0x04), (byte) (0x66), (byte) (0x66), (byte) (0x66), (byte) (0x66), (byte) (0xAA), (byte) (0x01), (byte) (0x60), (byte) (0x04), (byte) (0x64), (byte) (0x64), (byte) (0x64), (byte) (0x64)};
		}
		
		public override byte[] createDataChoiceTestOCTBytes()
		{
			return new byte[]{(byte) (0x40), (byte) (0x01), (byte) (0xFF)};
		}
		
		public override byte[] createDataChoiceSimpleTypeBytes()
		{
			return new byte[]{(byte) (0x60), (byte) (0x07), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61)};
		}
		
		public override byte[] createDataChoiceBooleanBytes()
		{
			return new byte[]{(byte) (0xB0)};
		}
		
		public override byte[] createDataChoiceIntBndBytes()
		{
			return new byte[]{(byte) (0xE0), (byte) (0x07)};
		}
		
		public override byte[] createDataChoicePlainBytes()
		{
			return new byte[]{(byte) (0x00), (byte) (0x06), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62)};
		}
		
		public override byte[] createStringArrayBytes()
		{
			return new byte[]{(byte) (0x02), (byte) (0x06), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x05), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63)};
		}

        public override byte[] createUTF8StringArrayBytes()
        {
            return new byte[] { 0x02, 0x06, 0x62, 0x62, 0x62, 0x62, 0x62, 0x62, 0x05, 0x63, 0x63, 0x63, 0x63, 0x63 };
        }

        public override byte[] createTestNIBytes()
        {
            return new byte[] { 0x00, 0x78 };
        }

        public override byte[] createTestNI2Bytes()
        {
            return new byte[] { 0x00, 0x30 };
        }

        public override byte[] createSetBytes()
        {
            return new byte[] { (byte)0x80, 0x04, 0x61, 0x61, 0x61, 0x61, 0x02, 0x00, (byte)0xAA, 0x04, 0x62, 0x62, 0x62, 0x62 };
        }

        public override byte[] createTestBitStrBytes()
        {
            return new byte[] { 0x24, (byte)0xAA, (byte)0xBB, (byte)0xCC, (byte)0xDD, (byte)0xF0 };
        }

        public override byte[] createTestBitStrSmallBytes()
        {
            return new byte[] { 0x0C, (byte)0xAA, (byte)0xB0 };
        }

        public override byte[] createUnicodeStrBytes()
        {
            return new byte[] { 0x06, (byte)0xD1, (byte)0xA5, (byte)0xD1, (byte)0xA4, (byte)0xD1, (byte)0xA6 };
        }

        public override byte[] createTestSequenceV12Bytes()
        {
            return new byte[] { (byte)0xB0, 0x03,0x61,0x62,0x61, (byte)0xC0, 
                0x63,0x63,0x63,0x63,0x06,0x64,0x64,0x64,0x64,0x64,0x64,0x20,0x04,0x61,0x61,0x61,0x61,0x03,0x62,0x62,0x62,0x0F, (byte)0x99, (byte)0x80, 
                0x0C, (byte)0xF0, 0x30, (byte)0xA0, 
                0x0A,0x0B,0x0C,0x0D };
        }

        public override byte[] createTestBitStrBndBytes()
        {
            return new byte[] { 0x30, (byte)0xF0 };
        }

        public override byte[] createChoiceInChoiceBytes()
        {
            return new byte[] { 0x00, (byte)0x80, 0x01, 0x05 };
        }

        public override byte[] createTaggedSeqInSeqBytes()
        {
            return new byte[] { 0x04, 0x61, 0x61, 0x61, 0x61, 0x04, 0x62, 0x62, 0x62, 0x62 };
        }

        public override byte[] createTestReal0_5Bytes()
        {
            return new byte[] { 0x03, (byte)0x80, (byte)0xFF, 0x01 };
        }

        public override byte[] createTestReal1_5Bytes()
        {
            return new byte[] { 0x03, (byte)0x80, (byte)0xFF, 0x03 };
        }

        public override byte[] createTestReal2Bytes()
        {
            return new byte[] { 0x03, (byte)0x80, (byte)0x01, 0x01 };
        }

        public override byte[] createTestRealBigBytes()
        {
            return new byte[] { 0x05, (byte)0x80, (byte)0xFD, 0x18, 0x6D, 0x21 };
        }

 
        public override byte[] createTaggedSequenceBytes()
        {
            return new byte[] { 0x80, 0x03, 0x41, 0x41, 0x41 };
        }

        public override byte[] createTestLongTagBytes()
        {
            return new byte[] { 0x02, 0x00, (byte)0xAA };
        }

        public override byte[] createTestLongTag2Bytes()
        {
            return new byte[] { 0x03, 0x00, (byte)0xFE, (byte)0xED };
        }

        public override byte[] createCSEnumBytes()
        {
            return new byte[] { 0x01, 0x01 };
        }

        public override byte[] createTestOID1Bytes()
        {
            return new byte[] { 0x03, 0x55, 0x04, 0x06 };
        }

        public override byte[] createTestOID2Bytes()
        {
            return new byte[] { 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x05 };
        }

        public override byte[] createTestOID3Bytes()
        {
            return new byte[] { 0x04, 0x28, 0xC2, 0x7B, 0x02 };
        }

        public override byte[] createTestOID4Bytes()
        {
            return new byte[] { 0x04, 0x67, 0x2A, 0x03, 0x00 };
        }

        public override byte[] createTaggedSetBytes()
        {
            return new byte[] { 0x01, (byte)0xC0, 0x02, 0x00, (byte)0xAA, 0x02, 0x00, (byte)0xBB, 0x02, 0x00, (byte)0xCC };
        }

        public override byte[] createTaggedSetInSetBytes()
        {
            return new byte[] { 0x01, (byte)0xC0, 0x02, 0x00, (byte)0xAA, 0x02, 0x00, (byte)0xBB, 0x02, 0x00, (byte)0xCC, 0x01, (byte)0xC0, 0x02, 0x00, (byte)0xAA, 0x02, 0x00, (byte)0xBB, 0x02, 0x00, (byte)0xCC };
        }

        public override byte[] createSet7Bytes()
        {
            return new byte[] { 0x01, 0x01, 0x01, 0x44, 0x01, 0x01, 0x01, 0x44 };
        }

        public override byte[] createTest128TagBytes()
        {
            return new byte[] { 0x01, 0x0A };
        }

	}
}
