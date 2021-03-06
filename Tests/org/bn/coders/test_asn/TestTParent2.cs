
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
    [ASN1BoxedType(Name = "TestTParent2")]
    public class TestTParent2: IASN1PreparedElement 
    {

        private TestParent2 val;

        
		[ASN1Element(Name = "TestTParent2", IsOptional = false, HasTag = false, HasDefaultValue = false)]
        public TestParent2 Value
        {
            get { return val; }
            
            set { val = value; }
            
        }

        
        
        public TestTParent2 ()
        {
        }

        public void initWithDefaults()
        {
        }

        private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(TestTParent2));
        public IASN1PreparedElementData PreparedData 
        {
            get { return preparedData; }
        }

    }
            
}
