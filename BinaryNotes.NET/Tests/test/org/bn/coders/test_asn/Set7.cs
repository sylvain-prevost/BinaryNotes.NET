
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
    [ASN1BoxedType ( Name = "Set7") ]
    public class Set7: IASN1PreparedElement {
            
           
        private Set7SequenceType  val;

        
       [ASN1PreparedElement]
       [ASN1Sequence ( Name = "Set7", IsSet = true  )]
       public class Set7SequenceType : IASN1PreparedElement {
                        
	private Set6 set6_ ;
	
        [ASN1Element ( Name = "set6", IsOptional =  false , HasTag =  false  , HasDefaultValue =  false )  ]
    
        public Set6 Set6
        {
            get { return set6_; }
            set { set6_ = value;  }
        }
        
                
  
                
                public void initWithDefaults() {
            		
                }

            private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(Set7SequenceType));
            public IASN1PreparedElementData PreparedData {
            	get { return preparedData; }
            }

                
       }
                
        [ASN1Element ( Name = "Set7", IsOptional =  false , HasTag =  true, Tag = 90, 
        TagClass =  TagClasses.Application  , HasDefaultValue =  false )  ]
    
        public Set7SequenceType Value
        {
                get { return val; }        
                    
                set { val = value; }
                        
        }            

                    
        
        public Set7 ()
        {
        }

            public void initWithDefaults()
	    {
	    }


            private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(Set7));
            public IASN1PreparedElementData PreparedData {
            	get { return preparedData; }
            }

        
    }
            
}
