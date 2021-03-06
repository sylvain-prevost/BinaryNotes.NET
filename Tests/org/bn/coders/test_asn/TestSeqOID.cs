
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
    [ASN1Sequence(Name = "TestSeqOID", IsSet = false)]
    public class TestSeqOID : IASN1PreparedElement 
    {
        
        private ObjectIdentifier field1_;
        [ASN1ObjectIdentifier( Name = "" )]
    
		[ASN1Element(Name = "field1", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public ObjectIdentifier Field1
        {
            get { return field1_; }
            set { field1_ = value;  }
        }
  
        private ObjectIdentifier field2_;
        
        private bool  field2_present = false;
        [ASN1ObjectIdentifier( Name = "" )]
    
		[ASN1Element(Name = "field2", IsOptional = true, HasTag = true, Tag = 1, HasDefaultValue = false)]
        public ObjectIdentifier Field2
        {
            get { return field2_; }
            set { field2_ = value; field2_present = true;  }
        }
  
        private BigInteger field3_;
        [ASN1Integer( Name = "" )]
    
		[ASN1Element(Name = "field3", IsOptional = false, HasTag = true, Tag = 2, HasDefaultValue = false)]
        public BigInteger Field3
        {
            get { return field3_; }
            set { field3_ = value;  }
        }
  
        public bool isField2Present()
        {
            return this.field2_present == true;
        }
        

        public void initWithDefaults() 
        {
            
        }

        private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(TestSeqOID));
        public IASN1PreparedElementData PreparedData 
        {
            get { return preparedData; }
        }

    }
            
}
