
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
    [ASN1Sequence ( Name = "TestRecursiveDefinetion", IsSet = false  )]
    public class TestRecursiveDefinetion : IASN1PreparedElement {
                    
	private string name_ ;
	[ASN1String( Name = "", 
        StringType =  UniversalTags.PrintableString , IsUCS = false )]
        [ASN1Element ( Name = "name", IsOptional =  false , HasTag =  true, Tag = 1 , HasDefaultValue =  false )  ]
    
        public string Name
        {
            get { return name_; }
            set { name_ = value;  }
        }
        
                
          
	private TestRecursiveDefinetion value_ ;
	
        private bool  value_present = false ;
	
        [ASN1Element ( Name = "value", IsOptional =  true , HasTag =  true, Tag = 2 , HasDefaultValue =  false )  ]
    
        public TestRecursiveDefinetion Value
        {
            get { return value_; }
            set { value_ = value; value_present = true;  }
        }
        
                
  
        public bool isValuePresent () {
            return this.value_present == true;
        }
        

            public void initWithDefaults() {
            	
            }


            private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(TestRecursiveDefinetion));
            public IASN1PreparedElementData PreparedData {
            	get { return preparedData; }
            }

            
    }
            
}
