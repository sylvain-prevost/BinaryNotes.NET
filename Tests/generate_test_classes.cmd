

SET OUTPUT_DIR=.\test\org\bn\coders\test_asn

mkdir %OUTPUT_DIR%

call ..\bncompiler\bnc.cmd -mp ..\cs_xsl_modules -o %OUTPUT_DIR% -ns test.org.bn.coders.test_asn -f .\test\test.asn




