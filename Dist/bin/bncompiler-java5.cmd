@echo off
set BN_HOME=%~dp0
set CMD_LINE_ARGS=%1
if ""%1""=="""" goto doneStart
shift
:setupArgs
	if ""%1""=="""" goto doneStart
	set CMD_LINE_ARGS=%CMD_LINE_ARGS% %1
	shift
goto setupArgs
:doneStart
java -classpath "%BN_HOME%/../depends/lineargs.jar;%BN_HOME%/../depends/antlr.jar;%BN_HOME%/../depends/activation.jar;%BN_HOME%/../depends/java5/jaxb-api.jar;%BN_HOME%/../depends/java5/jaxb-impl.jar;%BN_HOME%/../depends/java5/jaxb1-impl.jar;%BN_HOME%/../depends/java5/jsr173_1.0_api.jar;%BN_HOME%/../lib/java/binarynotes.jar;%BN_HOME%/../bin/bncompiler.jar" org.bn.compiler.Main %CMD_LINE_ARGS%
