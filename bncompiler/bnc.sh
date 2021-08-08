#!/bin/sh
BN_HOME=`dirname $0`
java -classpath $BN_HOME/bncompiler.jar org.bn.compiler.Main -m cs $*


