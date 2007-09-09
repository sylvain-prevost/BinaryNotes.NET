#!/bin/sh
BN_HOME=`dirname $0`
java -classpath $BN_HOME/../depends/lineargs.jar:$BN_HOME/../depends/antlr.jar:$BN_HOME/../depends/activation.jar:$BN_HOME/../depends/java5/jaxb\-api.jar:$BN_HOME/../depends/java5/jaxb\-impl.jar:$BN_HOME/../depends/java5/jaxb1\-impl.jar:$BN_HOME/../depends/java5/jsr173_1.0_api.jar:$BN_HOME/../lib/java/binarynotes.jar:$BN_HOME/../bin/bncompiler.jar org.bn.compiler.Main $*


