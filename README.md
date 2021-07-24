
## Included in 1.5.4



### New Feature(s)
- Added encoding/decoding support for large asn.1 integer (size > 8 bytes).  
- Enabled BER indefinite-length decoding.  
- Added .NET assembly StrongName.  
### Bugfixe(s)
- Fixed OID parsing when not last in sequence.  
- Fixed C# class generation when BOOLEAN element is annotated with a DEFAULT value (adjust modules/cs/includes/elementDefaultValue.xsl).  



### Note(s)
- Reference  .NET Framework 4.0 in order to include BigInteger support.  


<br>
<br>
<br>
This project is a fork of the original:

http://bnotes.sourceforge.net/

