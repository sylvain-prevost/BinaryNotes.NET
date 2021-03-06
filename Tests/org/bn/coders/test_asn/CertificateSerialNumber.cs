
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
    [ASN1BoxedType(Name = "CertificateSerialNumber")]
    public class CertificateSerialNumber: IASN1PreparedElement 
    {
    
        private BigInteger val;
        
        [ASN1Integer(Name = "CertificateSerialNumber")]
        
        public BigInteger Value
        {
            get { return val; }
            set { val = value; }
        }
        
        public CertificateSerialNumber()
        {
        }

        public CertificateSerialNumber(BigInteger value)
        {
            this.Value = value;
        }

        public void initWithDefaults()
        {
        }

        private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(CertificateSerialNumber));
        public IASN1PreparedElementData PreparedData 
        {
            get { return preparedData; }
        }

    }
            
}
