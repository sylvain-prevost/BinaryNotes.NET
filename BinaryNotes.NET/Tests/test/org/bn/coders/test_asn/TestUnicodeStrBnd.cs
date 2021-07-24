
//
// This file was generated by the BinaryNotes compiler.
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
    [ASN1BoxedType ( Name = "TestUnicodeStrBnd") ]
    public class TestUnicodeStrBnd: IASN1PreparedElement {

            private String val;
    
            [ASN1String( Name = "TestUnicodeStrBnd", 
        StringType =  UniversalTags.UTF8String , IsUCS = false) ]
            
            [ASN1SizeConstraint ( Max = 12L )]
        
            public String Value
            {
                get { return val; }
                set { val = value; }
            }
            
            public TestUnicodeStrBnd() {
            }

            public TestUnicodeStrBnd(String val) {
                this.val = val;
            }            

            public void initWithDefaults()
	    {
	    }


            private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(TestUnicodeStrBnd));
            public IASN1PreparedElementData PreparedData {
            	get { return preparedData; }
            }

    }
            
}
