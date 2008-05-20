../bncompiler.cmd -m java -o output/ldapv3 -ns ldapv3 -mp ../modules -f ldapv3.asn
javac -cp output/;../../lib/java/binarynotes.jar ./output/ldapv3/*.java
