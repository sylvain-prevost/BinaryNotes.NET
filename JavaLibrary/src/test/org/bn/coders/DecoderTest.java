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
package test.org.bn.coders;

import java.io.ByteArrayInputStream;

import java.io.ByteArrayOutputStream;

import java.util.Collection;

import junit.framework.TestCase;

import org.bn.CoderFactory;
import org.bn.IDecoder;
import org.bn.IEncoder;
import org.bn.coders.ber.BERDecoder;
import org.bn.coders.ber.BEREncoder;
import org.bn.coders.Decoder;

import org.bn.types.BitString;

import org.bn.types.ObjectIdentifier;

import test.org.bn.utils.ByteTools;
import test.org.bn.coders.test_asn.*;

public abstract class DecoderTest extends TestCase {

    private CoderTestUtilities coderTestUtils;

    public DecoderTest(String sTestName, CoderTestUtilities coderTestUtils) {
        super(sTestName);
        this.coderTestUtils = coderTestUtils;
    }

    protected abstract IDecoder newDecoder() throws Exception;

    private void checkData(Data dec, Data std) {
        if (std.isBinarySelected()) {
            assertTrue(dec.isBinarySelected());
            ByteTools.checkBuffers(dec.getBinary().getValue(), 
                                   std.getBinary().getValue());
        } else if (std.isPlainSelected()) {
            assertTrue(dec.isPlainSelected());
            assertEquals(dec.getPlain().getValue(), std.getPlain().getValue());
        } else if (std.isIntTypeSelected()) {
            assertTrue(dec.isIntTypeSelected());
            assertEquals(dec.getIntType(), std.getIntType());
        } else if (std.isSimpleOctTypeSelected()) {
            assertTrue(dec.isSimpleOctTypeSelected());
            ByteTools.checkBuffers(dec.getSimpleOctType(), 
                                   std.getSimpleOctType());
        }
    }

    protected void checkDataArray(Collection<Data> dec, Collection<Data> std) {
        assertEquals(dec.size(), std.size());
        for (int i = 0; i < std.size(); i++) {
            checkData((Data)(dec.toArray())[i], (Data)(std.toArray()[i]));
        }
    }
    
    protected  <T> void checkArray(Collection<T> dec, Collection<T> std) {
        assertEquals(dec.size(), std.size());
        for (int i = 0; i < std.size(); i++) {
            assertEquals(dec.toArray()[i], std.toArray()[i]);
        }
    }

    /**
     * @see Decoder#decode(InputStream,Class)
     */
    public void testDecode() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createDataSeqBytes());
        DataSeq seq = decoder.decode(stream, DataSeq.class);
        checkDataSeq(seq, coderTestUtils.createDataSeq());
    }

    protected void checkDataSeq(DataSeq decoded, 
                                DataSeq standard) throws Exception {ByteTools.checkBuffers(decoded.getBinary().getValue(), 
                               standard.getBinary().getValue());
        assertEquals(decoded.getBooleanType(), standard.getBooleanType());
        checkDataArray(decoded.getDataArray(), standard.getDataArray());
        assertEquals(decoded.getIntBndType(), standard.getIntBndType());
        assertEquals(decoded.getIntType(), standard.getIntType());
        assertEquals(decoded.getPlain().getValue(), 
                     standard.getPlain().getValue());
        assertEquals(decoded.getSimpleType(), standard.getSimpleType());
        ByteTools.checkBuffers(decoded.getSimpleOctType(), 
                               standard.getSimpleOctType());
    }
    
    protected void checkBitString(BitString decoded, BitString standard) {
        ByteTools.checkBuffers(decoded.getValue(),standard.getValue());
        assertEquals(decoded.getTrailBitsCnt(),standard.getTrailBitsCnt());
    }

    public void testITUDeDecode() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createITUSeqBytes());
        ITUSequence seq = decoder.decode(stream, ITUSequence.class);
        checkITUSeq(seq, coderTestUtils.createITUSeq());
    }

    private void checkITUSeq(ITUSequence decoded, ITUSequence standard) {
        assertEquals(decoded.getType1(), standard.getType1());
        assertEquals(decoded.getType2().getValue(), 
                     standard.getType2().getValue());
        assertEquals(decoded.getType3().getValue().getValue(), 
                     standard.getType3().getValue().getValue());
        assertEquals(decoded.getType4().getValue().getValue().getValue(), 
                     standard.getType4().getValue().getValue().getValue());
        assertEquals(decoded.getType5().getValue().getValue(), 
                     standard.getType5().getValue().getValue());
        assertEquals(decoded.getType6(), standard.getType6());
        assertEquals(decoded.getType7().getValue(), 
                     standard.getType7().getValue());
    }

    public void testNullDecode() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createNullSeqBytes());
        NullSequence seq = decoder.decode(stream, NullSequence.class);
        assertNotNull(seq);
    }

    public void testTaggedNullDecode() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createTaggedNullSeqBytes());
        TaggedNullSequence seq = 
            decoder.decode(stream, TaggedNullSequence.class);
        assertNotNull(seq);
    }

    public void testEnum() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createEnumBytes());
        ContentSchema enm = decoder.decode(stream, ContentSchema.class);
        assertNotNull(enm);
        checkContentSchema(enm, coderTestUtils.createEnum());
    }

    private void checkContentSchema(ContentSchema decoded, 
                                    ContentSchema standard) {
        assertEquals(decoded.getValue(), standard.getValue());
    }

    public void testSequenceWithEnum() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createSequenceWithEnumBytes());
        SequenceWithEnum seq = decoder.decode(stream, SequenceWithEnum.class);
        assertNotNull(seq);
    }

    public void testRecursiveDefinition() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createTestRecursiveDefinitionBytes());
        TestRecursiveDefinetion seq = 
            decoder.decode(stream, TestRecursiveDefinetion.class);
        assertNotNull(seq);
        checkRecursiveDefinition(seq, 
                                 coderTestUtils.createTestRecursiveDefinition());
    }

    private void checkRecursiveDefinition(TestRecursiveDefinetion decoded, 
                                          TestRecursiveDefinetion standard) {
        assertEquals(decoded.getName(), standard.getName());
        if (standard.getValue() != null) {
            assertNotNull(decoded.getValue());
            checkRecursiveDefinition(decoded.getValue(), standard.getValue());
        }
    }

    public void testDecodeInteger() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createTestInteger4Bytes());
            
        TestI32 val = decoder.decode(stream, TestI32.class);
        assertNotNull(val);
        assertEquals(val.getValue(), 
                     coderTestUtils.createTestInteger4().getValue());

        stream = 
                new ByteArrayInputStream(coderTestUtils.createTestInteger3Bytes());
        TestI16 val16 = decoder.decode(stream, TestI16.class);
        assertNotNull(val16);
        assertEquals(val16.getValue(), 
                     coderTestUtils.createTestInteger3().getValue());

        stream = 
                new ByteArrayInputStream(coderTestUtils.createTestInteger2Bytes());
        val16 = decoder.decode(stream, TestI16.class);
        assertNotNull(val16);
        assertEquals(val16.getValue(), 
                     coderTestUtils.createTestInteger2().getValue());

        stream = 
                new ByteArrayInputStream(coderTestUtils.createTestInteger1Bytes());
        TestI8 val8 = decoder.decode(stream, TestI8.class);
        assertNotNull(val8);
        assertEquals(val8.getValue(), 
                     coderTestUtils.createTestInteger1().getValue());

        stream = 
                new ByteArrayInputStream(coderTestUtils.createTestIntegerRBytes());
        TestIR valR = decoder.decode(stream, TestIR.class);
        assertNotNull(valR);
        assertEquals(valR.getValue(), 
                     coderTestUtils.createTestIntegerR().getValue());

        stream = 
                new ByteArrayInputStream(coderTestUtils.createTestInteger2_12Bytes());
        TestI14 val14 = decoder.decode(stream, TestI14.class);
        assertNotNull(val14);
        assertEquals(val14.getValue(), 
                     coderTestUtils.createTestInteger2_12().getValue());

    }
    
    public void testDecodeString() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createTestPRNBytes());            
        TestPRN val = decoder.decode(stream, TestPRN.class);
        assertNotNull(val);
        assertEquals(val.getValue(), 
                     coderTestUtils.createTestPRN().getValue());

        stream = new ByteArrayInputStream(coderTestUtils.createTestOCTBytes());            
        TestOCT valOct = decoder.decode(stream, TestOCT.class);
        assertNotNull(valOct);
        ByteTools.checkBuffers(valOct.getValue(), 
                     coderTestUtils.createTestOCT().getValue());
        
    }
    
    public void testDecodeStringArray()  throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createStringArrayBytes());
        StringArray val = decoder.decode(stream, StringArray.class);
        assertNotNull(val);
        checkArray(val.getValue(), coderTestUtils.createStringArray().getValue());
    }
    
    public void testDecodeChoice() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createDataChoicePlainBytes());
        Data choice = new Data();
        Data val = decoder.decode(stream, Data.class);
        assertNotNull(val);                
        choice.selectPlain(new TestPRN("bbbbbb"));
        checkData(val,choice);
        
        stream = new ByteArrayInputStream(coderTestUtils.createDataChoiceSimpleTypeBytes());
        val = decoder.decode(stream, Data.class);
        assertNotNull(val);                
        choice.selectSimpleType("aaaaaaa");        
        checkData(val,choice);

        stream = new ByteArrayInputStream(coderTestUtils.createDataChoiceTestOCTBytes());
        val = decoder.decode(stream, Data.class);
        assertNotNull(val);                
        choice.selectBinary( new TestOCT( new byte[] { (byte)0xFF } ) );
        checkData(val,choice);

        stream = new ByteArrayInputStream(coderTestUtils.createDataChoiceBooleanBytes());
        val = decoder.decode(stream, Data.class);
        assertNotNull(val);                
        choice.selectBooleanType(true);
        checkData(val,choice);

        stream = new ByteArrayInputStream(coderTestUtils.createDataChoiceIntBndBytes());
        val = decoder.decode(stream, Data.class);
        assertNotNull(val);                
        choice.selectIntBndType(7);
        checkData(val,choice);
                
    }
    
    public void testDecodeNegativeInteger() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createTestNIBytes());
        TestNI val = decoder.decode(stream, TestNI.class);
        assertEquals(val.getValue(), coderTestUtils.createTestNI().getValue());
        
        stream = 
            new ByteArrayInputStream(coderTestUtils.createTestNI2Bytes());
        TestNI2 val2 = decoder.decode(stream, TestNI2.class);
        assertEquals(val2.getValue(), coderTestUtils.createTestNI2().getValue());        
    }
    
    public void testDecodeSet() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createSetBytes());
        SetWithDefault val = decoder.decode(stream, SetWithDefault.class);
        assertEquals(val.getNodefault(), coderTestUtils.createSet().getNodefault());
        assertEquals(val.getNodefault2().getValue(), coderTestUtils.createSet().getNodefault2().getValue());
        assertEquals(val.getDefault3(), coderTestUtils.createSet().getDefault3());
    }
    
    public void testDecodeBitStr() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createTestBitStrBytes());
        TestBitStr val = decoder.decode(stream, TestBitStr.class);
        checkBitString(val.getValue(),coderTestUtils.createTestBitStr().getValue());
    }
    
    public void testDecodeUnicodeStr() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createUnicodeStrBytes());
        TestUnicodeStr val = decoder.decode(stream, TestUnicodeStr.class);
        assertEquals(val.getValue(), coderTestUtils.createUnicodeStr().getValue());        
    }

    public void testDecodeVersion1_2() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createTestSequenceV12Bytes());
        TestSequenceV12 val = decoder.decode(stream, TestSequenceV12.class);        
        //assertEquals(val.getAttrInt(), coderTestUtils.createTestSequenceV12().getAttrInt());        
        assertEquals(val.getAttrStr(), coderTestUtils.createTestSequenceV12().getAttrStr());
        assertEquals(val.getAttrStr2().getValue(), coderTestUtils.createTestSequenceV12().getAttrStr2().getValue());
        checkArray(val.getAttrArr(),coderTestUtils.createTestSequenceV12().getAttrArr());
        checkBitString(val.getAttrBitStr(), coderTestUtils.createTestSequenceV12().getAttrBitStr());
        TestSequenceV12 valWithDef = coderTestUtils.createTestSequenceV12();
        valWithDef.initWithDefaults();        
        checkBitString(val.getAttrBitStrDef(), valWithDef .getAttrBitStrDef());
        checkBitString(val.getAttrBoxBitStr().getValue(), coderTestUtils.createTestSequenceV12().getAttrBoxBitStr().getValue());
    }    
    
    public void testDecodeChoiceInChoice() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createChoiceInChoiceBytes());
        BugValueType val = decoder.decode(stream, BugValueType.class);        
        //assertEquals(val.getAttrInt(), coderTestUtils.createTestSequenceV12().getAttrInt());        
        assertEquals(val.isBugPrimitiveSelected(), coderTestUtils.createChoiceInChoice().isBugPrimitiveSelected());
        assertEquals(val.getBugPrimitive().isBugIntegerSelected(), coderTestUtils.createChoiceInChoice().getBugPrimitive().isBugIntegerSelected());
        assertEquals(val.getBugPrimitive().getBugInteger(), coderTestUtils.createChoiceInChoice().getBugPrimitive().getBugInteger());
    }

    public void testDecodeChoiceInChoice2() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createChoiceInChoice2Bytes());
        BugList val = decoder.decode(stream, BugList.class);        
        //assertEquals(val.getAttrInt(), coderTestUtils.createTestSequenceV12().getAttrInt());        
        assertNotNull(val.getValue());
        assertTrue("Is not empty",!val.getValue().isEmpty());
    }
   
    public void testDecodeReal() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createTestReal1_5Bytes());
        TestReal val = decoder.decode(stream, TestReal.class);
        assertEquals(val.getValue(), coderTestUtils.createTestReal1_5().getValue());
        
        stream = 
            new ByteArrayInputStream(coderTestUtils.createTestReal0_5Bytes());
        val = decoder.decode(stream, TestReal.class);
        assertEquals(val.getValue(), coderTestUtils.createTestReal0_5().getValue());
        
        stream = 
            new ByteArrayInputStream(coderTestUtils.createTestReal2Bytes());
        val = decoder.decode(stream, TestReal.class);
        assertEquals(val.getValue(), coderTestUtils.createTestReal2().getValue());

        stream = 
            new ByteArrayInputStream(coderTestUtils.createTestRealBigBytes());
        val = decoder.decode(stream, TestReal.class);
        assertEquals(val.getValue(), coderTestUtils.createTestRealBig().getValue());
        
    }

    public void testDecodeChoiceInChoice3() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createChoiceInChoice3Bytes());
        BugList val = decoder.decode(stream, BugList.class);        
        //assertEquals(val.getAttrInt(), coderTestUtils.createTestSequenceV12().getAttrInt());        
        assertNotNull(val.getValue());
        assertTrue("Is not empty",!val.getValue().isEmpty());
    }    
    
    public void testDecodeTaggedSeqInSeq() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createTaggedSeqInSeqBytes());
        TaggedSeqInSeq val = decoder.decode(stream, TaggedSeqInSeq.class);
        assertEquals(val.getValue().getField().getParam_name(), coderTestUtils.createTaggedSeqInSeq().getValue().getField().getParam_name());
        assertEquals(val.getValue().getField().getParam_value(), coderTestUtils.createTaggedSeqInSeq().getValue().getField().getParam_value());
    }
    
    public void testSequenceWithNullDecode() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createSeqWithNullBytes());
        SequenceWithNull seq = decoder.decode(stream, SequenceWithNull.class);
        assertNotNull(seq);
    }
    
    public void testDecodeLongTag2() throws Exception {
        IDecoder decoder = newDecoder();
        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createTestLongTag2Bytes());
        TestLongTag2 val = decoder.decode(stream, TestLongTag2.class);
        assertTrue(val.isTestaSelected());
        assertEquals(val.getTesta().getTestb(), coderTestUtils.createTestLongTag2().getTesta().getTestb());    
    }
    
    public void testDecodeOID() throws Exception {
        IDecoder decoder = newDecoder();
        assertNotNull(decoder);

        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createTestOID1Bytes());
        ObjectIdentifier oid1 = decoder.decode(stream, ObjectIdentifier.class);
        assertEquals(oid1.getValue(), coderTestUtils.createTestOID1().getValue().getValue());

        stream = new ByteArrayInputStream(coderTestUtils.createTestOID2Bytes());
        ObjectIdentifier oid2 = decoder.decode(stream, ObjectIdentifier.class);
        assertEquals(oid2.getValue(), coderTestUtils.createTestOID2().getValue().getValue());

        stream = new ByteArrayInputStream(coderTestUtils.createTestOID3Bytes());
        ObjectIdentifier oid3 = decoder.decode(stream, ObjectIdentifier.class);
        assertEquals(oid3.getValue(), coderTestUtils.createTestOID3().getValue().getValue());

        stream = new ByteArrayInputStream(coderTestUtils.createTestOID4Bytes());
        ObjectIdentifier oid4 = decoder.decode(stream, ObjectIdentifier.class);
        assertEquals(oid4.getValue(), coderTestUtils.createTestOID4().getValue().getValue());        
    }    
    
    public void testDecodeTaggedSet() throws Exception {
        IDecoder decoder = newDecoder();
        assertNotNull(decoder);

        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createTaggedSetBytes());
        Config tset = decoder.decode(stream, Config.class);
        assertEquals(tset.getValue().getLstVersion().getValue().size(), 
        		coderTestUtils.createTaggedSet().getValue().getLstVersion().getValue().size());
    	
    }

    public void testDecodeTaggedSetInSet() throws Exception {
        IDecoder decoder = newDecoder();
        assertNotNull(decoder);

        ByteArrayInputStream stream = 
            new ByteArrayInputStream(coderTestUtils.createTaggedSetInSetBytes());
        TestTaggedSetInSet tset = decoder.decode(stream, TestTaggedSetInSet.class);
        stream = 
            new ByteArrayInputStream(coderTestUtils.createSet7Bytes());
        Set7 set7 = decoder.decode(stream, Set7.class);
    }

}
