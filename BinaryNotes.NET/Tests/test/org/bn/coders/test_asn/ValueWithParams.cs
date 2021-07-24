
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
    [ASN1Sequence ( Name = "ValueWithParams", IsSet = false  )]
    public class ValueWithParams : IASN1PreparedElement {
                    
	private string value_ ;
	[ASN1String( Name = "", 
        StringType =  UniversalTags.PrintableString , IsUCS = false )]
        [ASN1Element ( Name = "value", IsOptional =  false , HasTag =  true, Tag = 0 , HasDefaultValue =  false )  ]
    
        public string Value
        {
            get { return value_; }
            set { value_ = value;  }
        }
        
                
          
	private System.Collections.Generic.ICollection<PlainParamsMap> params_ ;
	
        private bool  params_present = false ;
	
[ASN1SequenceOf( Name = "params", IsSetOf = false  )]

    
        [ASN1Element ( Name = "params", IsOptional =  true , HasTag =  true, Tag = 1 , HasDefaultValue =  false )  ]
    
        public System.Collections.Generic.ICollection<PlainParamsMap> Params
        {
            get { return params_; }
            set { params_ = value; params_present = true;  }
        }
        
                
  
        public bool isParamsPresent () {
            return this.params_present == true;
        }
        

            public void initWithDefaults() {
            	
            }


            private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(ValueWithParams));
            public IASN1PreparedElementData PreparedData {
            	get { return preparedData; }
            }

            
    }
            
}
