
# BinaryNotes.NET

This is a set of 2 tools:  
- C# library enabling marshalling/unmarshalling of asn.1 data buffers into/from objects according to the classes generated by the compiler.  
- ASN.1 Compiler that generates C# classes from an asn.1 file.  

It can be used to decode & parse asn.1 DER/BER-encoded binary files/buffers in a coding-friendly manner.  
It can also do the reverse and generate asn.1 compliant buffers from easily constructed C# objects.  

From Wikipedia: https://en.wikipedia.org/wiki/ASN.1#Example

```
FooProtocol DEFINITIONS ::= BEGIN

    FooQuestion ::= SEQUENCE {
        trackingNumber INTEGER,
        question       IA5String
    }

END
```

This asn.1 module can be fed to the compiler to generate corresponding C# class.  

```
call bncompiler\bnc.cmd -mp cs_xsl_modules -o [outdir] -ns [C# namespace] -f [asn1 module]
```

The generated class can now be used (with help from BinaryNotes library) to easily decode DER/BER data.  


```
IDecoder decoder = CoderFactory.getInstance().newDecoder("DER");

// asn1 data obtained from wikipedia asn.1 example
byte[] asn1Data = new byte[] { 
    0x30,0x13, 
        0x02,0x01,0x05, 
        0x16,0x0e,0x41,0x6e,0x79,0x62,0x6f,0x64,0x79,0x20,0x74,0x68,0x65,0x72,0x65,0x3f
};

using (MemoryStream memoryStream = new MemoryStream(asn1Data))
{
    // request decoding of memorystream using FooQuestion class
    FooQuestion fooQuestion = decoder.decode<FooQuestion>(memoryStream);

    // Display content of FooQuestion object
    Console.WriteLine("\tDecoded trackingNumber : {0}", fooQuestion.TrackingNumber);
    Console.WriteLine("\tDecoded question : {0}", fooQuestion.Question);
}
```

Result:
```
    Decoded trackingNumber : 5
    Decoded question : Anybody there?
```

And conversly the same class can also be used to easily generate DER/BER-encoded data.


```
IEncoder encoder = CoderFactory.getInstance().newEncoder("DER");

using (MemoryStream memoryStream = new MemoryStream())
{
    // create FooQuestion object and populate it
    FooQuestion fooQuestion = new FooQuestion();
    fooQuestion.TrackingNumber = 5;
    fooQuestion.Question = "Anybody there?";

    // request DER encoding
    encoder.encode<FooQuestion>(fooQuestion, memoryStream);
    byte[] result = memoryStream.ToArray();

    // display result
    Console.WriteLine("\tDER encoded result : {0}", BitConverter.ToString(result).Replace("-", " "));
}
```

Result:
```
    DER encoded result : 30 13 02 01 05 16 0E 41 6E 79 62 6F 64 79 20 74 68 65 72 65 3F
```


<br>
This project was developed and maintained by Abdulla G. Abdurakhmanov (http://bnotes.sf.net) until 2007. IMHO, he created a little jewel.  

<br>
<br>
Currently focusing on fixes & features additions to enable complete CryptographicMessageSyntax asn.1 decoding/encoding.
See [ePassportLibrary](https://github.com/sylvain-prevost/ePassportLibrary) for more concrete examples


<br>
<br>

## Included in 1.5.4.3

<br>
<br>

### New Feature(s)
- Added encoding/decoding support for large asn.1 integer (size > 8 bytes).  
- Enabled BER indefinite-length decoding.  
- Added .NET assembly StrongName.  
- Now targetting both .NET 4.0 & .NET Core 2.1.  
- Added 'SET OF ANY' support when present within sequence.  
- Added missing EXPLICIT keyword support.  

<br>
<br>

### Bugfixe(s)
- Fixed OID parsing when not last in sequence.  
- Fixed C# class generation when BOOLEAN element is annotated with a DEFAULT value.  
- Fixed C# class generation when MAX keyword is used in asn.1 constraint.  
- Fixed encoding of elements whose values are matching asn.1 DEFAULT definition (ie, it should NOT be encoded for DER).  

<br>
<br>

### Note(s)
- .NET Framework 4.0 or .NET Core 2.1 are the minimum version required (for BigInteger support).  


<br>
<br>
<br>
This project is a fork of the original: http://bnotes.sourceforge.net  

BinaryNotes v1.5.3 or greater is licensed under Apache Licence v2  http://www.apache.org/licenses/LICENSE-2.0.html  
(c) 2006-2011 Abdulla G. Abdurakhmanov (abdulla.abdurakhmanov@gmail.com)






