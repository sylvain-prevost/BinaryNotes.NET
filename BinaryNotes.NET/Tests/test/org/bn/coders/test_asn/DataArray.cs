
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
    [ASN1BoxedType ( Name = "DataArray" ) ]
    public class DataArray : IASN1PreparedElement {

	    private System.Collections.Generic.ICollection<Data> val = null; 
            
            
            [ASN1SequenceOf( Name = "DataArray", IsSetOf = false) ]

            public System.Collections.Generic.ICollection<Data> Value
            {
                get { return val; }
                set { val = value; }
            }
            
            public void initValue() {
                this.Value = new System.Collections.Generic.List<Data>();
            }
            
            public void Add(Data item) {
                this.Value.Add(item);
            }

            public void initWithDefaults()
	    {
	    }


            private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(DataArray));
            public IASN1PreparedElementData PreparedData {
            	get { return preparedData; }
            }

    }
            
}
