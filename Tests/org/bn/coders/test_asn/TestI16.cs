
//
// This file was generated by the BinaryNotes compiler (created by Abdulla Abdurakhmanov, modified by Sylvain Prevost).
// See http://bnotes.sourceforge.net 
// Any modifications to this file will be lost upon recompilation of the source ASN.1. 
//

using System;
using System.Numerics;

using org.bn.attributes;
using org.bn.attributes.constraints;
using org.bn.coders;
using org.bn.types;
using org.bn;

namespace test.org.bn.coders.test_asn {


    [ASN1PreparedElement]
    [ASN1BoxedType(Name = "TestI16")]
    public class TestI16: IASN1PreparedElement 
    {
    
        private int val;
        
        [ASN1Integer(Name = "TestI16")]
        
		[ASN1ValueRangeConstraint(Min = 0, Max = 65535)]
        public int Value
        {
            get { return val; }
            set { val = value; }
        }
        
        public TestI16()
        {
        }

        public TestI16(int value)
        {
            this.Value = value;
        }

        public void initWithDefaults()
        {
        }

        private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(TestI16));
        public IASN1PreparedElementData PreparedData 
        {
            get { return preparedData; }
        }

    }
            
}
