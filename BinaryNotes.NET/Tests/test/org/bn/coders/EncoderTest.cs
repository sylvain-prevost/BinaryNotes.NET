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
using org.bn;
using org.bn.coders;
using csUnit;
using test.org.bn.coders;
using test.org.bn.coders.test_asn;
using test.org.bn.utils;

namespace test.org.bn.coders
{
	[TestFixture]
    public abstract class EncoderTest
	{
		protected internal virtual void  printEncoded(System.String details, IEncoder encoder, System.Object obj)
		{
			System.IO.MemoryStream outputStream = new System.IO.MemoryStream();
			encoder.encode(obj, outputStream);
			System.Console.Out.WriteLine("Encoded by "+encoder.ToString()+" (" + details + ") : " + ByteTools.byteArrayToHexString(outputStream.ToArray()));
		}

        private CoderTestUtilities coderTestUtils;
		
		public EncoderTest(System.String sTestName, CoderTestUtilities coderTestUtils)
		{
			this.coderTestUtils = coderTestUtils;
		}
		
		protected abstract IEncoder newEncoder();
		
		/// <seealso cref="Encoder.encode(T,OutputStream)">
		/// </seealso>
		public virtual void  testEncodeChoice()
		{
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
            IEncoder encoder = newEncoder();
			Assert.NotNull(encoder);
			Data choice = new Data();
			
			choice.selectBinary(new TestOCT(new byte[]{(byte) (0xFF)}));
			printEncoded("Choice boxed octet", encoder, choice);
			checkEncoded(encoder, choice, coderTestUtils.createDataChoiceTestOCTBytes());
			
			choice.selectSimpleType("aaaaaaa");
			printEncoded("Choice string", encoder, choice);
			checkEncoded(encoder, choice, coderTestUtils.createDataChoiceSimpleTypeBytes());
			
			choice.selectBooleanType(true);
			printEncoded("Choice boolean", encoder, choice);
			checkEncoded(encoder, choice, coderTestUtils.createDataChoiceBooleanBytes());
			
			choice.selectIntBndType(7);
			printEncoded("Choice boxed int", encoder, choice);
			checkEncoded(encoder, choice, coderTestUtils.createDataChoiceIntBndBytes());
			
			choice.selectPlain(new TestPRN("bbbbbb"));
			printEncoded("Choice plain", encoder, choice);
			checkEncoded(encoder, choice, coderTestUtils.createDataChoicePlainBytes());
			
			choice.selectSimpleOctType(new byte[10]);
			printEncoded("Choice simple octet", encoder, choice);
			choice.selectIntType(7);
			printEncoded("Choice simple int", encoder, choice);
		}
		
		/// <seealso cref="Encoder.encode(T,OutputStream)">
		/// </seealso>
		public virtual void  testEncode()
		{
            IEncoder encoder = newEncoder();
			Assert.NotNull(encoder);
			printEncoded("SequenceMO test", encoder, coderTestUtils.createDataSeqMO());
			checkEncoded(encoder, coderTestUtils.createDataSeqMO(), coderTestUtils.createDataSeqMOBytes());
			
			printEncoded("Sequence test", encoder, coderTestUtils.createDataSeq());
			checkEncoded(encoder, coderTestUtils.createDataSeq(), coderTestUtils.createDataSeqBytes());
		}
		
		/// <seealso cref="Encoder.encode(T,OutputStream)">
		/// </seealso>
		public virtual void  testITUEncode()
		{
            IEncoder encoder = newEncoder();
			Assert.NotNull(encoder);
			printEncoded("ITUSequence test", encoder, coderTestUtils.createITUSeq());
			checkEncoded(encoder, coderTestUtils.createITUSeq(), coderTestUtils.createITUSeqBytes());
		}
		
		protected internal virtual void  checkEncoded(IEncoder encoder, System.Object obj, byte[] standard)
		{
			System.IO.MemoryStream outputStream = new System.IO.MemoryStream();
			encoder.encode(obj, outputStream);
			byte[] array = (outputStream.ToArray());
			Assert.Equals(array.Length, standard.Length);
			for (int i = 0; i < array.Length; i++)
			{
				Assert.Equals(array[i], standard[i]);
			}
		}
		
		public virtual void  testNullEncode()
		{
            IEncoder encoder = newEncoder();
			Assert.NotNull(encoder);
			printEncoded("NullSequence test", encoder, coderTestUtils.createNullSeq());
			checkEncoded(encoder, coderTestUtils.createNullSeq(), coderTestUtils.createNullSeqBytes());
		}
		
		public virtual void  testTaggedNullEncode()
		{
            IEncoder encoder = newEncoder();
			Assert.NotNull(encoder);
			printEncoded("TaggedNullSequence test", encoder, coderTestUtils.createTaggedNullSeq());
			checkEncoded(encoder, coderTestUtils.createTaggedNullSeq(), coderTestUtils.createTaggedNullSeqBytes());
		}
		
		public virtual void  testSequenceWithNull()
		{
            IEncoder encoder = newEncoder();
			Assert.NotNull(encoder);
			printEncoded("SequenceWithNull test", encoder, coderTestUtils.createSeqWithNull());
			checkEncoded(encoder, coderTestUtils.createSeqWithNull(), coderTestUtils.createSeqWithNullBytes());
		}
		
		public virtual void  testEnum()
		{
            IEncoder encoder = newEncoder();
			Assert.NotNull(encoder);
			printEncoded("Enum test", encoder, coderTestUtils.createEnum());
			checkEncoded(encoder, coderTestUtils.createEnum(), coderTestUtils.createEnumBytes());
		}
		
		public virtual void  testSequenceWithEnum()
		{
            IEncoder encoder = newEncoder();
			Assert.NotNull(encoder);
			printEncoded("Sequence Enum test", encoder, coderTestUtils.createSequenceWithEnum());
			checkEncoded(encoder, coderTestUtils.createSequenceWithEnum(), coderTestUtils.createSequenceWithEnumBytes());
		}
		
		public virtual void  testSequenceOfString()
		{
            IEncoder encoder = newEncoder();
			Assert.NotNull(encoder);
			printEncoded("Sequence Of String", encoder, coderTestUtils.createStringArray());
			checkEncoded(encoder, coderTestUtils.createStringArray(), coderTestUtils.createStringArrayBytes());
		}

        public virtual void testSequenceOfUTFString()
        {
            IEncoder encoder = newEncoder();
            Assert.NotNull(encoder);
            printEncoded("Sequence Of UTF8String", encoder, coderTestUtils.createUTF8StringArray());
            checkEncoded(encoder, coderTestUtils.createUTF8StringArray(), coderTestUtils.createUTF8StringArrayBytes());
        }		

		
		public virtual void  testRecursiveDefinition()
		{
            IEncoder encoder = newEncoder();
			Assert.NotNull(encoder);
			printEncoded("Recursive test", encoder, coderTestUtils.createTestRecursiveDefinition());
			checkEncoded(encoder, coderTestUtils.createTestRecursiveDefinition(), coderTestUtils.createTestRecursiveDefinitionBytes());
		}
		
		
		public virtual void  testEncodeInteger()
		{
            IEncoder encoder = newEncoder();
			Assert.NotNull(encoder);
			printEncoded("Unbounded integer test", encoder, coderTestUtils.createUnboundedTestInteger());
			checkEncoded(encoder, coderTestUtils.createUnboundedTestInteger(), coderTestUtils.createUnboundedTestIntegerBytes());
			printEncoded("Integer2_12 test", encoder, coderTestUtils.createTestInteger2_12());
			checkEncoded(encoder, coderTestUtils.createTestInteger2_12(), coderTestUtils.createTestInteger2_12Bytes());
			
			printEncoded("IntegerR test", encoder, coderTestUtils.createTestIntegerR());
			checkEncoded(encoder, coderTestUtils.createTestIntegerR(), coderTestUtils.createTestIntegerRBytes());
			
			printEncoded("Integer4 test", encoder, coderTestUtils.createTestInteger4());
			checkEncoded(encoder, coderTestUtils.createTestInteger4(), coderTestUtils.createTestInteger4Bytes());
			printEncoded("Integer3 test", encoder, coderTestUtils.createTestInteger3());
			checkEncoded(encoder, coderTestUtils.createTestInteger3(), coderTestUtils.createTestInteger3Bytes());
			printEncoded("Integer2 test", encoder, coderTestUtils.createTestInteger2());
			checkEncoded(encoder, coderTestUtils.createTestInteger2(), coderTestUtils.createTestInteger2Bytes());
			
			
			printEncoded("Integer1 test", encoder, coderTestUtils.createTestInteger1());
			checkEncoded(encoder, coderTestUtils.createTestInteger1(), coderTestUtils.createTestInteger1Bytes());
		}
		
		public virtual void  testEncodeString()
		{
            IEncoder encoder = newEncoder();
			Assert.NotNull(encoder);
			printEncoded("TestPRN", encoder, coderTestUtils.createTestPRN());
			checkEncoded(encoder, coderTestUtils.createTestPRN(), coderTestUtils.createTestPRNBytes());
			
			printEncoded("TestOCT", encoder, coderTestUtils.createTestOCT());
			checkEncoded(encoder, coderTestUtils.createTestOCT(), coderTestUtils.createTestOCTBytes());
		}

        public virtual void testNegativeInteger() 
        {
            IEncoder encoder = newEncoder();
            Assert.NotNull(encoder);
            printEncoded("Negative integer test",encoder, coderTestUtils.createTestNI());
            checkEncoded(encoder, coderTestUtils.createTestNI(), coderTestUtils.createTestNIBytes());
            printEncoded("Negative integer test 2",encoder, coderTestUtils.createTestNI2());
            checkEncoded(encoder, coderTestUtils.createTestNI2(), coderTestUtils.createTestNI2Bytes());        
        }

        public virtual void testEncodeSet()
        {
            IEncoder encoder = newEncoder();
            Assert.NotNull(encoder);
            printEncoded("Set test", encoder, coderTestUtils.createSet());
            checkEncoded(encoder, coderTestUtils.createSet(), coderTestUtils.createSetBytes());
        }

        public virtual void testEncodeBitString() {
            IEncoder encoder = newEncoder();
            Assert.NotNull(encoder);
            printEncoded("TestBitStr test",encoder, coderTestUtils.createTestBitStr());            
            checkEncoded(encoder, coderTestUtils.createTestBitStr(), coderTestUtils.createTestBitStrBytes());
        }

        public virtual void testEncodeBitStringSmall() {
            IEncoder encoder = newEncoder();
            Assert.NotNull(encoder);
            printEncoded("TestBitStrSmall test",encoder, coderTestUtils.createTestBitStrSmall());
            checkEncoded(encoder, coderTestUtils.createTestBitStrSmall(), coderTestUtils.createTestBitStrSmallBytes());
        }
        
        public virtual void testEncodeUnicodeString() {
            IEncoder encoder = newEncoder();
            Assert.NotNull(encoder);
            printEncoded("TestUnicode test",encoder, coderTestUtils.createUnicodeStr());            
            checkEncoded(encoder, coderTestUtils.createUnicodeStr(), coderTestUtils.createUnicodeStrBytes());
        }

        public void testEncodeBitStringBnd() {
            IEncoder encoder = newEncoder();
            Assert.NotNull(encoder);
            printEncoded("TestBitStrBnd test",encoder, coderTestUtils.createTestBitStrBnd());            
            checkEncoded(encoder, coderTestUtils.createTestBitStrBnd(), coderTestUtils.createTestBitStrBndBytes());
        }

        public virtual void testEncodeVersion1_2() {
            IEncoder encoder = newEncoder();
            Assert.NotNull(encoder);
            printEncoded("TestEncodeVersion_1_2: ",encoder, coderTestUtils.createTestSequenceV12());            
            checkEncoded(encoder, coderTestUtils.createTestSequenceV12(), coderTestUtils.createTestSequenceV12Bytes());        
        }

        public void testEncodeChoiceInChoice() {
            IEncoder encoder = newEncoder();
            Assert.NotNull(encoder);
            printEncoded("EncodeChoiceInChoice: ",encoder, coderTestUtils.createChoiceInChoice());            
            checkEncoded(encoder, coderTestUtils.createChoiceInChoice(), coderTestUtils.createChoiceInChoiceBytes());        
        }

        public void testEncodeTaggedSeqInSeq() {
            IEncoder encoder = newEncoder();
            Assert.NotNull(encoder);
            printEncoded("EncodeTaggedSeqInSeq: ",encoder, coderTestUtils.createTaggedSeqInSeq());            
            checkEncoded(encoder, coderTestUtils.createTaggedSeqInSeq(), coderTestUtils.createTaggedSeqInSeqBytes());        
        }

        public void testEncodeReals() {
            IEncoder encoder = newEncoder();
            Assert.NotNull(encoder);
            printEncoded("EncodeTestReal1.5: ",encoder, coderTestUtils.createTestReal1_5());            
            checkEncoded(encoder, coderTestUtils.createTestReal1_5(), coderTestUtils.createTestReal1_5Bytes());        
            printEncoded("EncodeTestReal0.5: ",encoder, coderTestUtils.createTestReal0_5());            
            checkEncoded(encoder, coderTestUtils.createTestReal0_5(), coderTestUtils.createTestReal0_5Bytes());                
            printEncoded("EncodeTestReal2: ",encoder, coderTestUtils.createTestReal2());            
            checkEncoded(encoder, coderTestUtils.createTestReal2(), coderTestUtils.createTestReal2Bytes());        
            printEncoded("EncodeTestRealBig: ",encoder, coderTestUtils.createTestRealBig());            
            checkEncoded(encoder, coderTestUtils.createTestRealBig(), coderTestUtils.createTestRealBigBytes());            
        }


        public void testEncodeTaggedSequence()
        {
            IEncoder encoder = newEncoder();
            Assert.NotNull(encoder);
            printEncoded("EncodeTestTaggedSequence: ", encoder, coderTestUtils.createTaggedSequence());
            checkEncoded(encoder, coderTestUtils.createTaggedSequence(), coderTestUtils.createTaggedSequenceBytes());
        }

        public void testEncodeLongTag()
        {
            IEncoder encoder = newEncoder();
            Assert.NotNull(encoder);
            printEncoded("EncodeTestLongTag: ", encoder, coderTestUtils.createTestLongTag());
            checkEncoded(encoder, coderTestUtils.createTestLongTag(), coderTestUtils.createTestLongTagBytes());
        }

        public void testEncodeLongTag2()
        {
            IEncoder encoder = newEncoder();
            Assert.NotNull(encoder);
            printEncoded("EncodeTestLongTag2: ", encoder, coderTestUtils.createTestLongTag2());
            checkEncoded(encoder, coderTestUtils.createTestLongTag2(), coderTestUtils.createTestLongTag2Bytes());
        }

        public void testEncodeCSEnum()
        {
            IEncoder encoder = newEncoder();
            Assert.NotNull(encoder);
            printEncoded("EncodeCSEnum: ", encoder, coderTestUtils.createCSEnum());
            checkEncoded(encoder, coderTestUtils.createCSEnum(), coderTestUtils.createCSEnumBytes());
        }


	}
}