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

namespace test.org.bn.coders.ber
{
	
	public class BERCoderTestUtils:CoderTestUtilities
	{
		
		
		public override byte[] createDataSeqBytes()
		{
			return new byte[]{(byte) (0x30), (byte) (0x36), (byte) (0x80), (byte) (0x00), (byte) (0x82), (byte) (0x00), (byte) (0x83), (byte) (0x07), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x04), (byte) (0x01), (byte) (0xBA), (byte) (0x85), (byte) (0x01), (byte) (0x00), (byte) (0x86), (byte) (0x01), (byte) (0x00), (byte) (0x87), (byte) (0x01), (byte) (0x44), (byte) (0xA8), (byte) (0x0F), (byte) (0x13), (byte) (0x06), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x13), (byte) (0x05), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0xA9), (byte) (0x09), (byte) (0x80), (byte) (0x07), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0xFF)};
		}
		
		
		public override byte[] createITUSeqBytes()
		{
			return new byte[]{(byte) (0x30), (byte) (0x31), (byte) (0x1A), (byte) (0x05), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x43), (byte) (0x05), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x82), (byte) (0x05), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x47), (byte) (0x05), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x82), (byte) (0x05), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x87), (byte) (0x05), (byte) (0x64), (byte) (0x64), (byte) (0x64), (byte) (0x64), (byte) (0x64), (byte) (0x88), (byte) (0x05), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65)};
		}
		
		
		public override byte[] createNullSeqBytes()
		{
			return new byte[]{(byte) (0x05), (byte) (0x00)};
		}
		
		
		public override byte[] createTaggedNullSeqBytes()
		{
			return new byte[]{(byte) (0x81), (byte) (0x00)};
		}
		
		
		public override byte[] createSeqWithNullBytes()
		{
			return new byte[]{(byte) (0x30), (byte) (0x0C), (byte) (0x13), (byte) (0x03), (byte) (0x73), (byte) (0x73), (byte) (0x73), (byte) (0x05), (byte) (0x00), (byte) (0x81), (byte) (0x03), (byte) (0x64), (byte) (0x64), (byte) (0x64)};
		}
		
		
		public override byte[] createEnumBytes()
		{
			return new byte[]{(byte) (0x0A), (byte) (0x01), (byte) (0x6F)};
		}
		
		
		public override byte[] createSequenceWithEnumBytes()
		{
			return new byte[]{(byte) (0x30), (byte) (0x0D), (byte) (0x13), (byte) (0x05), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x0A), (byte) (0x01), (byte) (0x6F), (byte) (0x81), (byte) (0x01), (byte) (0x6F)};
		}
		
		
		public override byte[] createTestRecursiveDefinitionBytes()
		{
			return new byte[]{(byte) (0x30), (byte) (0x10), (byte) (0x81), (byte) (0x05), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0xA2), (byte) (0x07), (byte) (0x81), (byte) (0x05), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62)};
		}
		
		
		public override byte[] createTestInteger4Bytes()
		{
			return new byte[]{(byte) (0x02), (byte) (0x04), (byte) (0x00), (byte) (0xF0), (byte) (0xF0), (byte) (0xF0)};
		}
		
		
		public override byte[] createTestInteger3Bytes()
		{
			return new byte[]{(byte) (0x02), (byte) (0x03), (byte) (0x00), (byte) (0xFF), (byte) (0xF0)};
		}
		
		
		public override byte[] createTestInteger2Bytes()
		{
			return new byte[]{(byte) (0x02), (byte) (0x02), (byte) (0x0F), (byte) (0xF0)};
		}
		
		
		public override byte[] createTestInteger1Bytes()
		{
			return new byte[]{(byte) (0x02), (byte) (0x01), (byte) (0x0F)};
		}
		
		public override byte[] createUnboundedTestIntegerBytes()
		{
			return new byte[]{(byte) (0x02), (byte) (0x04), (byte) (0x00), (byte) (0xFA), (byte) (0xFB), (byte) (0xFC)};
		}
		
		public override byte[] createTestIntegerRBytes()
		{
			return new byte[]{(byte) (0x02), (byte) (0x01), (byte) (0x03)};
		}
		
		public override byte[] createTestInteger2_12Bytes()
		{
			return new byte[]{(byte) (0x02), (byte) (0x02), (byte) (0x1F), (byte) (0xF1)};
		}
		
		public override byte[] createTestPRNBytes()
		{
			return new byte[]{(byte) (0x13), (byte) (0x05), (byte) (0x48), (byte) (0x65), (byte) (0x6C), (byte) (0x6C), (byte) (0x6F)};
		}
		
		public override byte[] createTestOCTBytes()
		{
			return new byte[]{(byte) (0x04), (byte) (0x05), (byte) (0x01), (byte) (0x02), (byte) (0xFF), (byte) (0x03), (byte) (0x04)};
		}
		
		public override byte[] createDataSeqMOBytes()
		{
			return new byte[]{(byte) (0x30), (byte) (0x47), (byte) (0x80), (byte) (0x00), (byte) (0x82), (byte) (0x00), (byte) (0x83), (byte) (0x07), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x04), (byte) (0x01), (byte) (0xAB), (byte) (0x85), (byte) (0x01), (byte) (0xFF), (byte) (0x86), (byte) (0x01), (byte) (0x00), (byte) (0x87), (byte) (0x01), (byte) (0x00), (byte) (0xA8), (byte) (0x0F), (byte) (0x13), (byte) (0x06), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x13), (byte) (0x05), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0xA9), (byte) (0x0F), (byte) (0x80), (byte) (0x07), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x65), (byte) (0x80), (byte) (0x04), (byte) (0x66), (byte) (0x66), (byte) (0x66), (byte) (0x66), (byte) (0x8E), (byte) (0x02), (byte) (0x00), (byte) (0xAA), (byte) (0xB0), (byte) (0x06), (byte) (0x83), (byte) (0x04), (byte) (0x64), (byte) (0x64), (byte) (0x64), (byte) (0x64)};
		}
		
		public override byte[] createDataChoiceTestOCTBytes()
		{
			return new byte[]{(byte) (0x82), (byte) (0x01), (byte) (0xFF)};
		}
		
		public override byte[] createDataChoiceSimpleTypeBytes()
		{
			return new byte[]{(byte) (0x83), (byte) (0x07), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61), (byte) (0x61)};
		}
		
		public override byte[] createDataChoiceBooleanBytes()
		{
			return new byte[]{(byte) (0x85), (byte) (0x01), (byte) (0xFF)};
		}
		
		public override byte[] createDataChoiceIntBndBytes()
		{
			return new byte[]{(byte) (0x87), (byte) (0x01), (byte) (0x07)};
		}
		
		public override byte[] createDataChoicePlainBytes()
		{
			return new byte[]{(byte) (0x80), (byte) (0x06), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62)};
		}
		
		public override byte[] createStringArrayBytes()
		{
			return new byte[]{(byte) (0x30), (byte) (0x0F), (byte) (0x13), (byte) (0x06), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x62), (byte) (0x13), (byte) (0x05), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63), (byte) (0x63)};
		}

        public override byte[] createUTF8StringArrayBytes()
        {
            return new byte[] { (byte)(0x30), (byte)(0x0F), (byte)(0x0C), (byte)(0x06), (byte)(0x62), (byte)(0x62), (byte)(0x62), (byte)(0x62), (byte)(0x62), (byte)(0x62), (byte)(0x0C), (byte)(0x05), (byte)(0x63), (byte)(0x63), (byte)(0x63), (byte)(0x63), (byte)(0x63) };
        }

        public override byte[] createTestNIBytes()
        {
            return new byte[] { 0x02, 0x01, (byte)0xF8 };
        }

        public override byte[] createTestNI2Bytes()
        {
            return new byte[] { 0x02, 0x02, (byte)0xF8, 0x30 };
        }

        public override byte[] createSetBytes()
        {
            return new byte[] {0x31,0x10, (byte)0x81, 
                0x04,0x61,0x61,0x61,0x61, (byte)0x82, 0x02,0x00, (byte)0xAA, (byte)0x83, 
                0x04,0x62,0x62,0x62,0x62};
        }

        public override byte[] createTestBitStrBytes()
        {
            return new byte[] { 03, 0x06, 0x04, (byte)0xAA, (byte)0xBB, (byte)0xCC, (byte)0xDD, (byte)0xF0 };
        }

        public override byte[] createTestBitStrSmallBytes()
        {
            return new byte[] { 0x03, 0x03, 0x04, (byte)0xAA, (byte)0xB0 };
        }

        public override byte[] createUnicodeStrBytes()
        {
            return new byte[] { 0x0C, 0x06, (byte)0xD1, (byte)0xA5, (byte)0xD1, (byte)0xA4, (byte)0xD1, (byte)0xA6 };
        }

        public override byte[] createTestSequenceV12Bytes()
        {
            return new byte[] {0x30,0x33, (byte)0x80, 0x03,0x61,0x62,0x61, (byte)0x81, 
                0x04,0x63,0x63,0x63,0x63, (byte)0x82, 
                0x06,0x64,0x64,0x64,0x64,0x64,0x64, (byte)0xA3, 0x0B,0x13,0x04,0x61,0x61,0x61,0x61,0x13,0x03,0x62,0x62,0x62, (byte)0x84, 
                0x03,0x01, (byte)0x99, (byte)0x80, (byte)0x85, 
                0x02,0x04, (byte)0xF0, (byte)0x86, 
                0x02,0x04, (byte)0xA0, (byte)0x87, 0x04,0x0A,0x0B,0x0C,0x0D};
        }

        public override byte[] createTestBitStrBndBytes()
        {
            return new byte[] { 0x03, 0x02, 0x04, (byte)0xF0 };
        }

        public override byte[] createChoiceInChoiceBytes()
        {
            return new byte[] { (byte)0xA0, 0x03, (byte)0x81, 0x01, 0x05 };
        }

        public override byte[] createTaggedSeqInSeqBytes()
        {
            return new byte[] { 0x64, 0x0E, (byte)0xA0, 0x0C, (byte)0x81, 0x04, 0x61, 0x61, 0x61, 0x61, (byte)0x82, 0x04, 0x62, 0x62, 0x62, 0x62 };
        }

        public override byte[] createTestReal0_5Bytes()
        {
            return new byte[] { 0x09, 0x03, (byte)0x80, (byte)0xFF, 0x01 };
        }

        public override byte[] createTestReal1_5Bytes()
        {
            return new byte[] { 0x09, 0x03, (byte)0x80, (byte)0xFF, 0x03 };
        }

        public override byte[] createTestReal2Bytes()
        {
            return new byte[] { 0x09, 0x03, (byte)0x80, 0x01, 0x01 };
        }

        public override byte[] createTestRealBigBytes()
        {
            return new byte[] { 0x09, 0x05, (byte)0x80, (byte)0xFD, 0x18, 0x6D, 0x21 };
        }

        public override byte[] createTaggedSequenceBytes()
        {
            return new byte[] { 0x68, 0x05, 0x87, 0x03, 0x41, 0x41, 0x41 };
        }

        public override byte[] createTestLongTagBytes()
        {
            return new byte[] { 0x5F, (byte)0xF6, 0x13, 0x02, 0x00, (byte)0xAA };
        }

        public override byte[] createTestLongTag2Bytes()
        {
            return new byte[] { (byte)0xBF, 0x21, 0x05, (byte)0x80, 0x03, 0x00, (byte)0xFE, (byte)0xED };
        }

        public override byte[] createCSEnumBytes()
        {
            return new byte[] { 0x02, 0x01, 0x01 };
        }

        public override byte[] createTestOID1Bytes()
        {
            return new byte[] { 0x06, 0x03, 0x55, 0x04, 0x06 };
        }

        public override byte[] createTestOID2Bytes()
        {
            return new byte[] { 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x05  };
        }

        public override byte[] createTestOID3Bytes()
        {
            return new byte[] { 0x06, 0x04, 0x28, 0xC2, 0x7B, 0x02 };
        }

        public override byte[] createTestOID4Bytes()
        {
            return new byte[] { 0x06, 0x04, 0x67, 0x2A, 0x03, 0x00 };
        }

	    public override byte[] createTaggedSetBytes() 
        {
            return new byte[] { 0x7F, 0x4C, 0x15, 0x7F, 0x4B, 0x0D, 0x7F, 0x4A, 0x0A, 0x5F, 0x6A, 0x02, 0x00, 0xAA, 0x5F, 0x69, 0x02, 0x00, 0xBB, 0x5F, 0x6A, 0x02, 0x00, 0xCC };
	    }

	    public override byte[] createTaggedSetInSetBytes() 
        {
            return new byte[] { 0x7F, 0x4D, 0x30, 0x7F, 0x4F, 0x15, 0x7F, 0x4B, 0x0D, 0x7F, 0x4A, 0x0A, 0x5F, 0x6A, 0x02, 0x00, 0xAA, 0x5F, 0x69, 0x02, 0x00, 0xBB, 0x5F, 0x6A, 0x02, 0x00, 0xCC, 0x7F, 0x4C, 0x15, 0x7F, 0x4B, 0x0D, 0x7F, 0x4A, 0x0A, 0x5F, 0x6A, 0x02, 0x00, 0xAA, 0x5F, 0x69, 0x02, 0x00, 0xBB, 0x5F, 0x6A, 0x02, 0x00, 0xCC };
	    }

        public override byte[] createSet7Bytes()
        {
            return new byte[] { 0x7F, 0x5A, 0x23, 0x7F, 0x38, 0x20, 0x7F, 0x7F, 0x0D, 0x7F, 0x7C, 0x0A, 0x7F, (byte)0x81, 0x00, 0x06, 0x7F, 0x37, 0x03, 0x02, 0x01, 0x44, 0x7F, 0x3D, 0x0D, 0x7F, 0x7C, 0x0A, 0x7F, (byte)0x81, 0x00, 0x06, 0x7F, 0x37, 0x03, 0x02, 0x01, 0x44 };
        }

        public override byte[] createTest128TagBytes()
        {
            return new byte[] { 0x5F,0x81,0x00,0x01,0x0A };
        }

	}
}
