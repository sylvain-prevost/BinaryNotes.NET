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
using System.Numerics;
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
            seq.Binary = new TestOCT(new byte[] { });
            seq.SimpleType = "aaaaaaa";
            seq.BooleanType = (false);
            Data dt = new Data();
            dt.selectPlain(new TestPRN("eeeeeee"));
            List<Data> lstDt = new List<Data>();
            lstDt.Add(dt);
            seq.DataArray = (lstDt);
            seq.IntBndType = (0x44);
            seq.Plain = new TestPRN("");
            seq.SimpleOctType = new byte[] { (byte)(0xBA) };
            seq.IntType = (0);
            List<String> list = new List<String>();
            list.Add("bbbbbb");
            list.Add("ccccc");
            seq.StringArray = (list);
            seq.Extension = new byte[] { (byte)(0xFF) };
            return seq;
        }
        public abstract byte[] createDataSeqBytes();

        public virtual TestSequenceWithOid createSequenceWithOid()
        {
            TestSequenceWithOid seq = new TestSequenceWithOid();
            seq.Simple_oid = new ObjectIdentifier("2.5.4.6");
            seq.Simple_integer = 0x112233;
            return seq;
        }

        public abstract byte[] createSequenceWithOidBytes();

        public virtual TestBasicSequence createBasicSequence()
        {
            TestBasicSequence seq = new TestBasicSequence();
            seq.Simple_integer = 0x112233;
            seq.Simple_octet_string = new byte[] { (byte)(0x4A), (byte)(0x6F), (byte)(0x6E), (byte)(0x65), (byte)(0x73) };
            return seq;
        }

        public abstract byte[] createDefiniteLengthBasicSequenceBytes();

        public abstract byte[] createIndefiniteLengthBasicSequenceBytes();

        public virtual DataSeqMO createDataSeqMO()
        {
            DataSeqMO seq = new DataSeqMO();
            seq.Binary = new TestOCT(new byte[] { });
            seq.SimpleType = "aaaaaaa";
            seq.BooleanType = true;

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
            seq.SimpleOctType = new byte[] { (byte)(0xAB) };
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

        public virtual TestI64 createTestInteger8()
        {
            TestI64 value_Renamed = new TestI64();
            value_Renamed.Value = 0xF0F0F0F0F0F0F0;
            return value_Renamed;
        }
        public abstract byte[] createTestInteger8Bytes();


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
            value_Renamed.Value = new byte[] { (byte)(0x01), (byte)(0x02), (byte)(0xFF), (byte)(0x03), (byte)(0x04) };
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

        public virtual UTF8StringArray createUTF8StringArray()
        {
            UTF8StringArray sequenceOfString = new UTF8StringArray();
            List<String> list = new List<String>();
            list.Add("bbbbbb");
            list.Add("ccccc");
            sequenceOfString.Value = list;
            return sequenceOfString;
        }
        public abstract byte[] createUTF8StringArrayBytes();

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

        public TestNI4 createTestNI4()
        {
            return new TestNI4(-131071999);
        }
        public abstract byte[] createTestNI4Bytes();

        public TestNI8 createTestNI8()
        {
            return new TestNI8(-562949953421311999);
        }
        public abstract byte[] createTestNI8Bytes();

        public TestNI16 createTestNI16()
        {
            BigInteger bigInt = BigInteger.Parse("-10384593717069655257060992658440191999");
            return new TestNI16(bigInt);
        }
        public abstract byte[] createTestNI16Bytes();


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
            result.Value = (new BitString(new byte[] { (byte)0xAA, (byte)0xBB, (byte)0xCC, (byte)0xDD, (byte)0xF0 }, 4));
            return result;
        }
        public abstract byte[] createTestBitStrBytes();

        public TestBitStr createTestBitStrSmall()
        {
            TestBitStr result = new TestBitStr();
            result.Value = (new BitString(new byte[] { (byte)0xAA, (byte)0xB0 }, 4));
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
            result.Value = (new BitString(new byte[] { (byte)0xF0 }, 4));
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
            OK = 1,
            CANCEL = 2
        }
        public Object createCSEnum()
        {
            TestCSEnum item;
            item = TestCSEnum.OK;
            return item;
        }
        public abstract byte[] createCSEnumBytes();

        public TestOID createTestOID1()
        {
            TestOID result = new TestOID();
            result.Value = new ObjectIdentifier("2.5.4.6");  // CountryName
            return result;
        }

        public abstract byte[] createTestOID1Bytes();

        public TestOID createTestOID2()
        {
            TestOID result = new TestOID();
            result.Value = new ObjectIdentifier("1.2.840.113549.1.1.5");  // sha1withRSAEncryption
            return result;
        }

        public abstract byte[] createTestOID2Bytes();

        public TestOID createTestOID3()
        {
            TestOID result = new TestOID();
            result.Value = new ObjectIdentifier("1.0.8571.2");  // example from ASN1 Complete (excellent free book) 
            return result;
        }

        public abstract byte[] createTestOID3Bytes();

        // This test added because BER encode had bug with handling a zero exclusive of first two arcs
        public TestOID createTestOID4()
        {
            TestOID result = new TestOID();
            result.Value = new ObjectIdentifier("2.23.42.3.0");  // SET attribute cert 
            return result;
        }

        public abstract byte[] createTestOID4Bytes();

        public Config createTaggedSet()
        {
            Config config_bn = new Config();
            config_bn.Value = (new Config.ConfigSequenceType());
            config_bn.Value.Major_config = (new Major());
            config_bn.Value.Major_config.Value = (0xCCL);


            test_asn.Version version_bn = new test_asn.Version();
            version_bn.Value = (new test_asn.Version.VersionSequenceType());
            version_bn.Value.Major = (new Major());
            version_bn.Value.Major.Value = (0xAAL);
            version_bn.Value.Minor = (new Minor());
            version_bn.Value.Minor.Value = (0xBBL);

            config_bn.Value.LstVersion = (new LstVersion());

            config_bn.Value.LstVersion.Value = (new List<test_asn.Version>());
            config_bn.Value.LstVersion.Value.Add(version_bn);

            return config_bn;
        }

        private Config2 createTaggedSet2()
        {
            Config2 config_bn = new Config2();
            config_bn.Value = (new Config2.Config2SequenceType());
            config_bn.Value.Major_config = (new Major());
            config_bn.Value.Major_config.Value = (0xCCL);


            test_asn.Version version_bn = new test_asn.Version();
            version_bn.Value = (new test_asn.Version.VersionSequenceType());
            version_bn.Value.Major = (new Major());
            version_bn.Value.Major.Value = (0xAAL);
            version_bn.Value.Minor = (new Minor());
            version_bn.Value.Minor.Value = (0xBBL);

            config_bn.Value.LstVersion = (new LstVersion());

            config_bn.Value.LstVersion.Value = (new List<test_asn.Version>());
            config_bn.Value.LstVersion.Value.Add(version_bn);

            return config_bn;
        }

        public abstract byte[] createTaggedSetBytes();

        public TestTaggedSetInSet createTaggedSetInSet()
        {
            TestTaggedSetInSet result = new TestTaggedSetInSet();
            result.Value = (new TestTaggedSetInSet.TestTaggedSetInSetSequenceType());
            result.Value.Config1 = (createTaggedSet());
            result.Value.Config2 = (createTaggedSet2());
            return result;
        }

        public abstract byte[] createTaggedSetInSetBytes();

        public Set7 createSet7()
        {
            Set7 set7 = new Set7();
            set7.Value = (new Set7.Set7SequenceType());
            set7.Value.Set6 = (new Set6());
            set7.Value.Set6.Value = (new Set6.Set6SequenceType());
            set7.Value.Set6.Value.Set4 = (new Set4());
            set7.Value.Set6.Value.Set4.Value = (new List<Set3>());
            Set3 set3 = new Set3();
            set3.Value = (new Set3.Set3SequenceType());
            set3.Value.Set2 = (new Set2());
            set3.Value.Set2.Value = (new List<Set1>());
            Set1 set1 = new Set1();
            set1.Value = (new Set1.Set1SequenceType());
            set1.Value.Set1ID = 0x44L;
            set3.Value.Set2.Value.Add(set1);
            set7.Value.Set6.Value.Set4.Value.Add(set3);
            set7.Value.Set6.Value.Set5 = (new Set5());
            set7.Value.Set6.Value.Set5.Value = (new List<Set3>());
            set7.Value.Set6.Value.Set5.Value.Add(set3);
            return set7;
        }

        public abstract byte[] createSet7Bytes();

        public Test128Tag createTest128Tag()
        {
            Test128Tag tag = new Test128Tag();
            tag.Value = 10L;
            return tag;
        }

        public abstract byte[] createTest128TagBytes();


        public test_asn.Attribute createAttribute()
        {
            test_asn.Attribute attribute = new test_asn.Attribute();
            attribute.Type = new AttributeType();
            attribute.Type.Value = new ObjectIdentifier("1.2.840.113549.1.9.4");
            attribute.Values = new List<byte[]>();
            attribute.Values.Add(new byte[] {0x04, 0x14,
                0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A,
                0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x11, 0x22, 0x33, 0x44, 0x55 });
            return attribute;
        }

        public abstract byte[] createAttributeBytes();

        public test_asn.Extension createExtensionWithDefaultMismatch()
        {
            test_asn.Extension extension = new test_asn.Extension();
            extension.ExtnID = new ObjectIdentifier("2.5.29.15");
            extension.Critical = true; // this does not matches the DEFAULT of the asn.1 definition (it should be encoded).
            extension.ExtnValue = new byte[] { 0x03, 0x02, 0x04, 0xF0 };
            return extension;
        }

        public abstract byte[] createExtensionWithDefaultMismatchBytes();

        public test_asn.Extension createExtensionWithDefaultMatch()
        {
            test_asn.Extension extension = new test_asn.Extension();
            extension.ExtnID = new ObjectIdentifier("2.5.29.32");
            extension.Critical = false; // this matches the DEFAULT of the asn.1 definition (it should not be encoded under DER).
            extension.ExtnValue = new byte[] { 0x30, 0x3B, 0x30, 0x39, 0x06, 0x0A, 0x2A, 0x81, 0x7A, 0x01, 0x81, 0x1F, 0x01, 0x01, 0x01, 0x02, 0x30, 0x2B, 0x30, 0x29, 0x06, 0x08, 0x2B, 0x06, 0x01, 0x05, 0x05, 0x07, 0x02, 0x01, 0x16, 0x1D, 0x68, 0x74, 0x74, 0x70, 0x3A, 0x2F, 0x2F, 0x69, 0x6E, 0x74, 0x65, 0x72, 0x69, 0x65, 0x75, 0x72, 0x2E, 0x67, 0x6F, 0x75, 0x76, 0x2E, 0x66, 0x72, 0x2F, 0x63, 0x73, 0x63, 0x61 };
            return extension;
        }

        public abstract byte[] createExtensionWithDefaultMatchBytesBER();

        public abstract byte[] createExtensionWithDefaultMatchBytesDER();

        public test_asn.SequenceWithExplicitFields createSequenceWithExplicitFields()
        {
            test_asn.SequenceWithExplicitFields sequenceWithExplicitFields = new test_asn.SequenceWithExplicitFields();
            sequenceWithExplicitFields.Version = new CertVersion(1);
            sequenceWithExplicitFields.SerialNumber = new CertificateSerialNumber(1);
            return sequenceWithExplicitFields;
        }

        public abstract byte[] createSequenceWithExplicitFieldsBytes();

    }
}
