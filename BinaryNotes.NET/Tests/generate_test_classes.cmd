

SET OUTPUT_DIR=.\test\org\bn\coders\test_asn

SET COMPILER=..\..\Dist\bin\bncompiler.cmd
SET MODULES=..\..\Dist\bin\modules
SET GENERATE_CS=-m cs -mp %MODULES%

mkdir %OUTPUT_DIR%

call %COMPILER% %GENERATE_CS% -o .\test\org\bn\coders\test_asn -ns test.org.bn.coders.test_asn -f .\test\test.asn




