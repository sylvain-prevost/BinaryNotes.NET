
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
    [ASN1Sequence(Name = "TestChild3", IsSet = false)]
    public class TestChild3 : IASN1PreparedElement 
    {
        
        private BigInteger field1_;
        [ASN1Integer( Name = "" )]
    
		[ASN1Element(Name = "field1", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public BigInteger Field1
        {
            get { return field1_; }
            set { field1_ = value;  }
        }
  
        private byte[] field2_;
        
        private bool  field2_present = false;
        [ASN1OctetString( Name = "" )]
    
		[ASN1Element(Name = "field2", IsOptional = true, HasTag = true, Tag = 1, HasDefaultValue = false)]
        public byte[] Field2
        {
            get { return field2_; }
            set { field2_ = value; field2_present = true;  }
        }
  
        private string field3_;
        
		[ASN1String(Name = "", StringType = UniversalTags.UTF8String, IsUCS = false)]
		[ASN1Element(Name = "field3", IsOptional = false, HasTag = true, Tag = 2, HasDefaultValue = true)]
        public string Field3
        {
            get { return field3_; }
            set { field3_ = value;  }
        }
  
        private BigInteger field4_;
        [ASN1Integer( Name = "" )]
    
		[ASN1Element(Name = "field4", IsOptional = false, HasTag = true, Tag = 3, HasDefaultValue = false)]
        public BigInteger Field4
        {
            get { return field4_; }
            set { field4_ = value;  }
        }
  
        public bool isField2Present()
        {
            return this.field2_present == true;
        }
        
        private string field5_;
        
		[ASN1String(Name = "", StringType = UniversalTags.UTF8String, IsUCS = false)]
		[ASN1Element(Name = "field5", IsOptional = false, HasTag = true, Tag = 4, HasDefaultValue = false)]
        public string Field5
        {
            get { return field5_; }
            set { field5_ = value;  }
        }
  
        private BigInteger field6_;
        [ASN1Integer( Name = "" )]
    
		[ASN1Element(Name = "field6", IsOptional = false, HasTag = true, Tag = 5, HasDefaultValue = true)]
        public BigInteger Field6
        {
            get { return field6_; }
            set { field6_ = value;  }
        }
  

        public void initWithDefaults() 
        {
            string param_Field3 =         
            "Sssdsd";
        Field3 = param_Field3;
    BigInteger param_Field6 =         
            new BigInteger ( 0);
        Field6 = param_Field6;
    
        }

        private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(TestChild3));
        public IASN1PreparedElementData PreparedData 
        {
            get { return preparedData; }
        }

    }
            
}
