
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
    [ASN1Sequence ( Name = "SequenceWithDefault", IsSet = false  )]
    public class SequenceWithDefault : IASN1PreparedElement {
                    
	private BigInteger nodefault_ ;
	[ASN1Integer( Name = "" )]
    
        [ASN1Element ( Name = "nodefault", IsOptional =  false , HasTag =  true, Tag = 0 , HasDefaultValue =  false )  ]
    
        public BigInteger Nodefault
        {
            get { return nodefault_; }
            set { nodefault_ = value;  }
        }
        
                
          
	private string withDefault_ ;
	[ASN1String( Name = "", 
        StringType =  UniversalTags.PrintableString , IsUCS = false )]
        [ASN1Element ( Name = "withDefault", IsOptional =  false , HasTag =  true, Tag = 1 , HasDefaultValue =  true )  ]
    
        public string WithDefault
        {
            get { return withDefault_; }
            set { withDefault_ = value;  }
        }
        
                
          
	private BigInteger withIntDef_ ;
	[ASN1Integer( Name = "" )]
    
        [ASN1Element ( Name = "withIntDef", IsOptional =  false , HasTag =  true, Tag = 2 , HasDefaultValue =  true )  ]
    
        public BigInteger WithIntDef
        {
            get { return withIntDef_; }
            set { withIntDef_ = value;  }
        }
        
                
          
	private WithSeqDefSequenceType withSeqDef_ ;
	
       [ASN1PreparedElement]
       [ASN1Sequence ( Name = "withSeqDef", IsSet = false  )]
       public class WithSeqDefSequenceType : IASN1PreparedElement {
                        
	private string name_ ;
	[ASN1String( Name = "", 
        StringType =  UniversalTags.PrintableString , IsUCS = false )]
        [ASN1Element ( Name = "name", IsOptional =  false , HasTag =  false  , HasDefaultValue =  false )  ]
    
        public string Name
        {
            get { return name_; }
            set { name_ = value;  }
        }
        
                
          
	private string email_ ;
	[ASN1String( Name = "", 
        StringType =  UniversalTags.PrintableString , IsUCS = false )]
        [ASN1Element ( Name = "email", IsOptional =  false , HasTag =  false  , HasDefaultValue =  false )  ]
    
        public string Email
        {
            get { return email_; }
            set { email_ = value;  }
        }
        
                
  
                
                public void initWithDefaults() {
            		
                }

            private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(WithSeqDefSequenceType));
            public IASN1PreparedElementData PreparedData {
            	get { return preparedData; }
            }

                
       }
                
        [ASN1Element ( Name = "withSeqDef", IsOptional =  false , HasTag =  true, Tag = 3 , HasDefaultValue =  true )  ]
    
        public WithSeqDefSequenceType WithSeqDef
        {
            get { return withSeqDef_; }
            set { withSeqDef_ = value;  }
        }
        
                
          
	private TestOCT withOctDef_ ;
	
        [ASN1Element ( Name = "withOctDef", IsOptional =  false , HasTag =  true, Tag = 4 , HasDefaultValue =  true )  ]
    
        public TestOCT WithOctDef
        {
            get { return withOctDef_; }
            set { withOctDef_ = value;  }
        }
        
                
          
	private byte[] withOctDef2_ ;
	[ASN1OctetString( Name = "" )]
    
        [ASN1Element ( Name = "withOctDef2", IsOptional =  false , HasTag =  true, Tag = 5 , HasDefaultValue =  true )  ]
    
        public byte[] WithOctDef2
        {
            get { return withOctDef2_; }
            set { withOctDef2_ = value;  }
        }
        
                
          
	private System.Collections.Generic.ICollection<string> withSeqOf_ ;
	[ASN1String( Name = "", 
        StringType =  UniversalTags.PrintableString , IsUCS = false )]
[ASN1SequenceOf( Name = "withSeqOf", IsSetOf = false  )]

    
        [ASN1Element ( Name = "withSeqOf", IsOptional =  false , HasTag =  true, Tag = 6 , HasDefaultValue =  true )  ]
    
        public System.Collections.Generic.ICollection<string> WithSeqOf
        {
            get { return withSeqOf_; }
            set { withSeqOf_ = value;  }
        }
        
                
          
	private System.Collections.Generic.ICollection<TestPRN> withSeqOf2_ ;
	
[ASN1SequenceOf( Name = "withSeqOf2", IsSetOf = false  )]

    
        [ASN1Element ( Name = "withSeqOf2", IsOptional =  false , HasTag =  true, Tag = 7 , HasDefaultValue =  true )  ]
    
        public System.Collections.Generic.ICollection<TestPRN> WithSeqOf2
        {
            get { return withSeqOf2_; }
            set { withSeqOf2_ = value;  }
        }
        
                
          
	private StringArray withSeqOf3_ ;
	
        [ASN1Element ( Name = "withSeqOf3", IsOptional =  false , HasTag =  true, Tag = 8 , HasDefaultValue =  true )  ]
    
        public StringArray WithSeqOf3
        {
            get { return withSeqOf3_; }
            set { withSeqOf3_ = value;  }
        }
        
                
  

            public void initWithDefaults() {
            	string param_WithDefault =         
            "dd";
        WithDefault = param_WithDefault;
    BigInteger param_WithIntDef =         
            new BigInteger ( 120);
        WithIntDef = param_WithIntDef;
    WithSeqDefSequenceType param_WithSeqDef =         
            
                new WithSeqDefSequenceType();
                {
                
                    param_WithSeqDef.Name = 
                        "Name"
                    ;
                
                    param_WithSeqDef.Email = 
                        "Email"
                    ;
                
                }
            ;
        WithSeqDef = param_WithSeqDef;
    TestOCT param_WithOctDef =         
            new TestOCT (CoderUtils.defStringToOctetString("'01101100'B"));
        WithOctDef = param_WithOctDef;
    byte[] param_WithOctDef2 =         
            CoderUtils.defStringToOctetString("'FFEEAA'H").Value;
        WithOctDef2 = param_WithOctDef2;
    System.Collections.Generic.ICollection<string> param_WithSeqOf =         
            
                                
                new System.Collections.Generic.List<string>();
                                
                {                    
                    
                    param_WithSeqOf.Add(
                        "aa"
                    );
                    
                    param_WithSeqOf.Add(
                        "dd"
                    );
                    
                }
            ;
        WithSeqOf = param_WithSeqOf;
    System.Collections.Generic.ICollection<TestPRN> param_WithSeqOf2 =         
            
                                
                new System.Collections.Generic.List<TestPRN>();
                                
                {                    
                    
                    param_WithSeqOf2.Add(
                        new TestPRN ("cc")
                    );
                    
                    param_WithSeqOf2.Add(
                        new TestPRN ("ee")
                    );
                    
                }
            ;
        WithSeqOf2 = param_WithSeqOf2;
    StringArray param_WithSeqOf3 =         
            
                                
                new StringArray();
                
                    param_WithSeqOf3.initValue();
                                
                {                    
                    
                    param_WithSeqOf3.Add(
                        "fff"
                    );
                    
                    param_WithSeqOf3.Add(
                        "ggg"
                    );
                    
                }
            ;
        WithSeqOf3 = param_WithSeqOf3;
    
            }


            private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(SequenceWithDefault));
            public IASN1PreparedElementData PreparedData {
            	get { return preparedData; }
            }

            
    }
            
}
