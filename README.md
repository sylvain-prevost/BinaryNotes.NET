
# BinaryNotes 

This is a set of 2 tools:  
- ASN.1 Compiler (written in Java) that generates C# classes from an asn.1 file.  
- C# library support enabling marhsalling/unmarshalling asn.1 data buffers into/from objects according to the classes gnerated by the compiler.  

For example, this is extremelly useful to decode & parse asn.1 DER/BER-encoded binary files/buffers in a coding-friendly manner.  
It can also do the reverse and generate asn.1 compliant buffers from easily constructed C# objects.  

<br>
This project was developed and maintained by Abdulla G. Abdurakhmanov (http://bnotes.sf.net) until 2007. IMHO, he created a little jewel.  

<br>
<br>
Currently focusing on fixes & features additions to enable complete CryptographicMessageSyntax asn.1 decoding/encoding (RFC 5652: https://datatracker.ietf.org/doc/html/rfc5652).  


<br>
<br>

## Included in 1.5.4

<br>
<br>

### New Feature(s)
- Added encoding/decoding support for large asn.1 integer (size > 8 bytes).  
- Enabled BER indefinite-length decoding.  
- Added .NET assembly StrongName.  
- Now targetting both .NET 4.0 & .NET Core 2.1.  

<br>
<br>

### Bugfixe(s)
- Fixed OID parsing when not last in sequence.  
- Fixed C# class generation when BOOLEAN element is annotated with a DEFAULT value.  

<br>
<br>

### Note(s)
- .NET Framework 4.0 or .NET Core 2.1 are required (for BigInteger support).  


<br>
<br>
<br>
This project is a fork of the original:

http://bnotes.sourceforge.net/

