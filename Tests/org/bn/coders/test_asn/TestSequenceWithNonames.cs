
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
    [ASN1Sequence(Name = "TestSequenceWithNonames", IsSet = false)]
    public class TestSequenceWithNonames : IASN1PreparedElement 
    {
        
        private SeqSequenceType seq_;
        
    [ASN1PreparedElement]
    [ASN1Sequence(Name = "seq", IsSet = false)]
    public class SeqSequenceType : IASN1PreparedElement
    {
        
        private BigInteger it1_;
        
        private bool  it1_present = false;
        [ASN1Integer( Name = "" )]
    
		[ASN1Element(Name = "it1", IsOptional = true, HasTag = false, HasDefaultValue = false)]
        public BigInteger It1
        {
            get { return it1_; }
            set { it1_ = value; it1_present = true;  }
        }
  
        public bool isIt1Present()
        {
            return this.it1_present == true;
        }
        
        
        public void initWithDefaults() 
        {
            
        }

        private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(SeqSequenceType));
        public IASN1PreparedElementData PreparedData 
        {
            get { return preparedData; }
        }
    }
                
		[ASN1Element(Name = "seq", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public SeqSequenceType Seq
        {
            get { return seq_; }
            set { seq_ = value;  }
        }
  
        private ChChoiceType ch_;
        

    [ASN1PreparedElement]
    [ASN1Choice(Name = "ch")]
    public class ChChoiceType : IASN1PreparedElement  
    {
        
        private BigInteger it1_;
        private bool  it1_selected = false;

        [ASN1Integer( Name = "" )]
    
		[ASN1Element(Name = "it1", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public BigInteger It1
        {
            get { return it1_; }
            set { selectIt1(value); }
        }
  
        private byte[] it2_;
        private bool  it2_selected = false;

        [ASN1OctetString( Name = "" )]
    
		[ASN1Element(Name = "it2", IsOptional = false, HasTag = true, Tag = 1, HasDefaultValue = false)]
        public byte[] It2
        {
            get { return it2_; }
            set { selectIt2(value); }
        }
  
        public bool isIt1Selected()
        {
            return this.it1_selected;
        }

        

        public void selectIt1 (BigInteger val) 
        {
            this.it1_ = val;
            this.it1_selected = true;
            
            this.it2_selected = false;
            
        }
  
        public bool isIt2Selected()
        {
            return this.it2_selected;
        }

        

        public void selectIt2 (byte[] val) 
        {
            this.it2_ = val;
            this.it2_selected = true;
            
            this.it1_selected = false;
            
        }
  

        public void initWithDefaults()
        {
        }

        private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(ChChoiceType));
        public IASN1PreparedElementData PreparedData 
        {
            get { return preparedData; }
        }

    }
                
		[ASN1Element(Name = "ch", IsOptional = false, HasTag = true, Tag = 1, HasDefaultValue = false)]
        public ChChoiceType Ch
        {
            get { return ch_; }
            set { ch_ = value;  }
        }
  
        private System.Collections.Generic.ICollection<SeqfSequenceType> seqf_;
        
    [ASN1PreparedElement]
    [ASN1Sequence(Name = "seqf", IsSet = false)]
    public class SeqfSequenceType : IASN1PreparedElement
    {
        
        private BigInteger it1_;
        
        private bool  it1_present = false;
        [ASN1Integer( Name = "" )]
    
		[ASN1Element(Name = "it1", IsOptional = true, HasTag = false, HasDefaultValue = false)]
        public BigInteger It1
        {
            get { return it1_; }
            set { it1_ = value; it1_present = true;  }
        }
  
        public bool isIt1Present()
        {
            return this.it1_present == true;
        }
        
        
        public void initWithDefaults() 
        {
            
        }

        private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(SeqfSequenceType));
        public IASN1PreparedElementData PreparedData 
        {
            get { return preparedData; }
        }
    }
                
		[ASN1SequenceOf(Name = "seqf", IsSetOf = false)]
    
		[ASN1Element(Name = "seqf", IsOptional = false, HasTag = true, Tag = 2, HasDefaultValue = false)]
        public System.Collections.Generic.ICollection<SeqfSequenceType> Seqf
        {
            get { return seqf_; }
            set { seqf_ = value;  }
        }
  

        public void initWithDefaults() 
        {
            
        }

        private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(TestSequenceWithNonames));
        public IASN1PreparedElementData PreparedData 
        {
            get { return preparedData; }
        }

    }
            
}
