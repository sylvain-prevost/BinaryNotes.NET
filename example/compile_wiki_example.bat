SET OUTPUT_DIR=FooProtocol
mkdir %OUTPUT_DIR%
call ..\bncompiler\bnc.cmd -mp ..\cs_xsl_modules -o %OUTPUT_DIR% -ns FooProtocolExample -f FooProtocol.asn




