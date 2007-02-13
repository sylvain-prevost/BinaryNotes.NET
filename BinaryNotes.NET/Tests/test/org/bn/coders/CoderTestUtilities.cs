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
using test.org.bn.coders.test_asn;
using org.bn.types;

namespace test.org.bn.coders
{
	
	public abstract class CoderTestUtilities
	{
		
		public virtual NullSequence createNullSeq()
		{
			NullSequence seq = new NullSequence();
			return seq;
		}
		public abstract byte[] createNullSeqBytes();
		
		public virtual TaggedNullSequence createTaggedNullSeq()
		{
			TaggedNullSequence seq = new TaggedNullSequence();
			return seq;
		}
		public abstract byte[] createTaggedNullSeqBytes();
		
		
		public virtual ContentSchema createEnum()
		{
			ContentSchema schema = new ContentSchema();
			schema.Value = (ContentSchema.EnumType.multipart_mixed);
			return schema;
		}
		public abstract byte[] createEnumBytes();
		
		
		public virtual ITUSequence createITUSeq()
		{
			ITUSequence seq = new ITUSequence();
			seq.Type1 = "aaaaa";
			seq.Type2 = new ITUType1("bbbbb");
			ITUType1 type1 = new ITUType1("ccccc");
			ITUType2 type2 = new ITUType2();
			type2.Value = type1;
			seq.Type3 = type2;
			ITUType3 type3 = new ITUType3();
			type3.Value = type2;
			seq.Type4 = type3;
			seq.Type5 = type2;
			seq.Type6 = "ddddd";
			ITUType6 type6 = new ITUType6();
			type6.Value = "eeeee";
			seq.Type7 = type6;
			return seq;
		}
		public abstract byte[] createITUSeqBytes();
		
		
		public virtual DataSeq createDataSeq()
		{
			DataSeq seq = new DataSeq();
			seq.Binary = new TestOCT(new byte[]{});
			seq.SimpleType = "aaaaaaa";
			seq.BooleanType = (false);
			Data dt = new Data();
			dt.selectPlain(new TestPRN("eeeeeee"));
            List<Data> lstDt = new List<Data>();
			lstDt.Add(dt);
			seq.DataArray = (lstDt);
			seq.IntBndType = (0x44);
			seq.Plain = new TestPRN("");
			seq.SimpleOctType = new byte[]{(byte) (0xBA)};
			seq.IntType = (0);
            List<String> list = new List<String>();
			list.Add("bbbbbb");
			list.Add("ccccc");
			seq.StringArray = (list);
			seq.Extension = new byte[]{(byte) (0xFF)};
			return seq;
		}
		public abstract byte[] createDataSeqBytes();
		
		public virtual DataSeqMO createDataSeqMO()
		{
			DataSeqMO seq = new DataSeqMO();
			seq.Binary = new TestOCT(new byte[]{});
			seq.SimpleType = "aaaaaaa";
			seq.BooleanType=  true;
			
			Data dt = new Data();
			dt.selectPlain(new TestPRN("eeeeeee"));
			Data dt2 = new Data();
			dt2.selectPlain(new TestPRN("ffff"));
            List<Data> lstDt = new List<Data>();
			lstDt.Add(dt);
			lstDt.Add(dt2);
			seq.DataArray = (lstDt);
			
			seq.IntBndType = (0);
			seq.Plain = new TestPRN("");
			seq.SimpleOctType = new byte[]{(byte) (0xAB)};
			seq.IntType = (0);

            List<String> list = new List<String>();
			list.Add("bbbbbb");
			list.Add("ccccc");
			seq.StringArray = (list);

            List<Data> listData = new List<Data>();
			Data choice = new Data();
			choice.selectSimpleType("dddd");
			listData.Add(choice);
			seq.DataArray2 = (listData);
			
			seq.IntBndType2 = (0xAA);
			
			return seq;
		}
		public abstract byte[] createDataSeqMOBytes();
		
		public virtual SequenceWithEnum createSequenceWithEnum()
		{
			SequenceWithEnum seq = new SequenceWithEnum();
			seq.Enval = createEnum();
			seq.Item = "aaaaa";
			seq.TaggedEnval = createEnum();
			return seq;
		}
		public abstract byte[] createSequenceWithEnumBytes();
		
		public virtual TestIR createTestIntegerR()
		{
			TestIR value_Renamed = new TestIR();
			value_Renamed.Value = 3;
			return value_Renamed;
		}
		public abstract byte[] createTestIntegerRBytes();
		
		public virtual TestI8 createTestInteger1()
		{
			TestI8 value_Renamed = new TestI8();
			value_Renamed.Value = 0x0F;
			return value_Renamed;
		}
		public abstract byte[] createTestInteger1Bytes();
		
		public virtual TestI14 createTestInteger2_12()
		{
			TestI14 value_Renamed = new TestI14();
			value_Renamed.Value = 0x1FF1;
			return value_Renamed;
		}
		public abstract byte[] createTestInteger2_12Bytes();
		
		public virtual TestI16 createTestInteger2()
		{
			TestI16 value_Renamed = new TestI16();
			value_Renamed.Value = 0x0FF0;
			return value_Renamed;
		}
		public abstract byte[] createTestInteger2Bytes();
		
		public virtual TestI16 createTestInteger3()
		{
			TestI16 value_Renamed = new TestI16();
			value_Renamed.Value = 0xFFF0;
			return value_Renamed;
		}
		public abstract byte[] createTestInteger3Bytes();
		
		public virtual TestI32 createTestInteger4()
		{
			TestI32 value_Renamed = new TestI32();
			value_Renamed.Value = 0xF0F0F0;
			return value_Renamed;
		}
		public abstract byte[] createTestInteger4Bytes();
		
		
		public virtual SequenceWithNull createSeqWithNull()
		{
			SequenceWithNull seq = new SequenceWithNull();
			seq.Test = "sss";
			seq.Test2 = "ddd";
			return seq;
		}
		public abstract byte[] createSeqWithNullBytes();
		
		public virtual TestRecursiveDefinetion createTestRecursiveDefinition()
		{
			TestRecursiveDefinetion result = new TestRecursiveDefinetion();
			TestRecursiveDefinetion subResult = new TestRecursiveDefinetion();
			result.Name = "aaaaa";
			subResult.Name = "bbbbb";
			result.Value = subResult;
			return result;
		}
		public abstract byte[] createTestRecursiveDefinitionBytes();
		
		public virtual TestI createUnboundedTestInteger()
		{
			TestI value_Renamed = new TestI();
			value_Renamed.Value = 0xFAFBFC;
			return value_Renamed;
		}
		public abstract byte[] createUnboundedTestIntegerBytes();
		
		public virtual TestPRN createTestPRN()
		{
			TestPRN value_Renamed = new TestPRN();
			value_Renamed.Value = "Hello";
			return value_Renamed;
		}
		public abstract byte[] createTestPRNBytes();
		
		public virtual TestOCT createTestOCT()
		{
			TestOCT value_Renamed = new TestOCT();
			value_Renamed.Value = new byte[]{(byte) (0x01), (byte) (0x02), (byte) (0xFF), (byte) (0x03), (byte) (0x04)};
			return value_Renamed;
		}
		public abstract byte[] createTestOCTBytes();
		
		public abstract byte[] createDataChoiceTestOCTBytes();
		
		public abstract byte[] createDataChoiceSimpleTypeBytes();
		
		public abstract byte[] createDataChoiceBooleanBytes();
		
		public abstract byte[] createDataChoiceIntBndBytes();
		
		public abstract byte[] createDataChoicePlainBytes();
		
		public virtual StringArray createStringArray()
		{
			StringArray sequenceOfString = new StringArray();
            List<String> list = new List<String>();
			list.Add("bbbbbb");
			list.Add("ccccc");
			sequenceOfString.Value = list;
			return sequenceOfString;
		}
		
		public abstract byte[] createStringArrayBytes();

        public TestNI createTestNI()
        {
            return new TestNI(-8);
        }
        public abstract byte[] createTestNIBytes();

        public TestNI2 createTestNI2()
        {
            return new TestNI2(-2000);
        }
        public abstract byte[] createTestNI2Bytes();

        public SetWithDefault createSet()
        {
            SetWithDefault result = new SetWithDefault();
            result.Nodefault = (0xAA);
            result.Nodefault2 = (new TestPRN("aaaa"));
            result.Default3 = ("bbbb");
            return result;
        }
        public abstract byte[] createSetBytes();


        public TestBitStr createTestBitStr()
        {
            TestBitStr result = new TestBitStr();
            result.Value = (new BitString(new byte[] { (byte)0xAA, (byte)0xBB, (byte)0xCC, (byte)0xDD, (byte)0xF0 } , 4));
            return result;
        }
        public abstract byte[] createTestBitStrBytes();

        public TestBitStr createTestBitStrSmall()
        {
            TestBitStr result = new TestBitStr();
            result.Value = (new BitString(new byte[] { (byte)0xAA, (byte)0xB0 } , 4));
            return result;
        }
        public abstract byte[] createTestBitStrSmallBytes();

        public TestUnicodeStr createUnicodeStr()
        {
            TestUnicodeStr result = new TestUnicodeStr();
            result.Value = ("\u0465\u0464\u0466");
            return result;
        }
        public abstract byte[] createUnicodeStrBytes();

        public TestBitStrBnd createTestBitStrBnd()
        {
            TestBitStrBnd result = new TestBitStrBnd();
            result.Value = (new BitString(new byte[] { (byte)0xF0 } , 4));
            return result;
        }
        public abstract byte[] createTestBitStrBndBytes();

        public TestSequenceV12 createTestSequenceV12()
        {
            TestSequenceV12 result = new TestSequenceV12();
            result.AttrSimple = ("aba");
            ICollection<String> array = new List<String>();
            array.Add("aaaa");
            array.Add("bbb");
            result.AttrArr = (array);
            result.AttrBitStr = (new BitString(new byte[] { (byte)0x99, (byte)0x80 }, 1));
            result.AttrBitStrBnd = (new BitString(new byte[] { (byte)0xF0 }, 4));
            result.AttrBoxBitStr = (new TestBitStrBnd(new BitString(new byte[] { (byte)0xA0 }, 4)));
            result.AttrStr = ("cccc");
            result.AttrStr2 = (new TestPRN("dddddd"));
            result.AttrStrict = (new byte[] { 0x0A, 0x0B, 0x0C, 0x0D });
            return result;
        }
        public abstract byte[] createTestSequenceV12Bytes();

        public BugValueType createChoiceInChoice()
        {
            BugPrimitive primitive = new BugPrimitive();
            primitive.selectBugInteger(5);

            BugValueType valueType = new BugValueType();
            valueType.selectBugPrimitive(primitive);

            return valueType;
        }
        public abstract byte[] createChoiceInChoiceBytes();

        public TaggedSeqInSeq createTaggedSeqInSeq() {
            TaggedSeqInSeq result = new TaggedSeqInSeq();
            TaggedSeqInSeq.TaggedSeqInSeqSequenceType resSeq = new TaggedSeqInSeq.TaggedSeqInSeqSequenceType();
            PlainParamsMap field = new PlainParamsMap();
            field.Param_name = ("aaaa");
            field.Param_value = ("bbbb");
            resSeq.Field = (field);
            result.Value = (resSeq);
            return result;
        }
        public abstract byte[] createTaggedSeqInSeqBytes();

        public TestReal createTestReal0_5()
        {
            return new TestReal(0.5);
        }
        public abstract byte[] createTestReal0_5Bytes();

        public TestReal createTestReal1_5()
        {
            return new TestReal(1.5);
        }
        public abstract byte[] createTestReal1_5Bytes();

        public TestReal createTestReal2()
        {
            return new TestReal(2D);
        }
        public abstract byte[] createTestReal2Bytes();

        public TestReal createTestRealBig()
        {
            return new TestReal(200100.125);
        }
        public abstract byte[] createTestRealBigBytes();

        public TaggedSequence createTaggedSequence()
        {
            TaggedSequence result = new TaggedSequence();
            result.Value = new TaggedSequence.TaggedSequenceSequenceType();
            result.Value.Type1 = "AAA";
            return result;
        }
        public abstract byte[] createTaggedSequenceBytes();

        public TestLongTag createTestLongTag()
        {
            TestLongTag result = new TestLongTag();
            result.Value = (0xAAL);
            return result;
        }
        public abstract byte[] createTestLongTagBytes();

        public TestLongTag2 createTestLongTag2()
        {
            TestLongTag2 result = new TestLongTag2();
            TestLongTag2Choice resultChoice = new TestLongTag2Choice();
            resultChoice.Testb = (0xFEEDL);
            result.selectTesta(resultChoice);
            return result;
        }
        public abstract byte[] createTestLongTag2Bytes();

        public enum TestCSEnum
        {
            OK=1,
            CANCEL=2
        }
        public Object createCSEnum()
        {
            TestCSEnum item;
            item = TestCSEnum.OK;
            return item;
        }
        public abstract byte[] createCSEnumBytes();

	}
}