<?xml version="1.0" encoding="utf-8" ?>
<!--
/*
 Copyright 2006-2011 Abdulla Abdurakhmanov (abdulla@latestbit.com)
 Original sources are available at www.latestbit.com

 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at

 http://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.
 */
-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xsltc="http://xml.apache.org/xalan/xsltc"
    xmlns:redirect="http://xml.apache.org/xalan/redirect"
    extension-element-prefixes="xsltc redirect"
>
    <xsl:import href="stringDecl.xsl"/>
    <xsl:import href="octetStringDecl.xsl"/>
    <xsl:import href="bitStringDecl.xsl"/>
    <xsl:import href="objectIdentifierDecl.xsl"/>
    <xsl:import href="booleanDecl.xsl"/>
    <xsl:import href="integerDecl.xsl"/>
    <xsl:import href="realDecl.xsl"/>
    <xsl:import href="anyDecl.xsl"/>
    <xsl:import href="sequenceOfDecl.xsl"/>
    <xsl:import href="nullDecl.xsl"/>
    <xsl:import href="sequenceDecl.xsl"/>
    <xsl:import href="choiceDecl.xsl"/>
    <xsl:import href="enumDecl.xsl"/>
    
    <xsl:import href="constraint.xsl"/>

    <xsl:output method="text" encoding="UTF-8" indent="no"/>

    <xsl:template name="typeDecl">
	<xsl:param name="parentElementName"/>
	<xsl:variable name="elementName">
		<xsl:choose>
            		<xsl:when test="string-length(name) > 0"><xsl:value-of select="name"/></xsl:when>
			<xsl:otherwise><xsl:value-of select="$parentElementName"/></xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
        <xsl:choose>
            <xsl:when test="typeReference/BUILTINTYPE = 'CHARACTER STRING'"><xsl:call-template name="stringDecl"/> </xsl:when>
            <xsl:when test="typeReference/BUILTINTYPE = 'OCTET STRING'"><xsl:call-template name="octetStringDecl"/> </xsl:when>
            <xsl:when test="typeReference/BUILTINTYPE = 'BIT STRING'"><xsl:call-template name="bitStringDecl"/> </xsl:when>
	    <xsl:when test="typeReference/BUILTINTYPE = 'OBJECT IDENTIFIER'"><xsl:call-template name="objectIdentifierDecl"/> </xsl:when>
            <xsl:when test="typeReference/BUILTINTYPE = 'BOOLEAN'"><xsl:call-template name="booleanDecl"/> </xsl:when>
            <xsl:when test="typeReference/BUILTINTYPE = 'INTEGER'"><xsl:call-template name="integerDecl"/> </xsl:when>
            <xsl:when test="typeReference/BUILTINTYPE = 'REAL'"><xsl:call-template name="realDecl"/> </xsl:when>
            <xsl:when test="typeReference/isSequenceOf = 'true' or typeReference/isSequenceOf = 'false'"><xsl:call-template name="sequenceOfDecl"><xsl:with-param name="elementName" select="$elementName"/></xsl:call-template></xsl:when>
            <xsl:when test="typeReference/isSequence = 'true' or typeReference/isSequence = 'false'"><xsl:call-template name="sequenceDecl"><xsl:with-param name="elementName" select="$elementName"/></xsl:call-template></xsl:when>
            <xsl:when test="typeReference/isChoice = 'true'"><xsl:call-template name="choiceDecl"><xsl:with-param name="elementName" select="$elementName"/></xsl:call-template></xsl:when>
	    <xsl:when test="typeReference/isEnum = 'true'"><xsl:call-template name="enumDecl"><xsl:with-param name="elementName" select="$elementName"/></xsl:call-template></xsl:when>
            <xsl:when test="typeReference/isNull = 'true'"><xsl:call-template name="nullDecl"/></xsl:when>
            <xsl:when test="string-length(typeName) > 0"></xsl:when>
            <xsl:otherwise><xsl:call-template name="anyDecl"/> </xsl:otherwise>
        </xsl:choose>
        <xsl:for-each select="typeReference/constraint">
            <xsl:call-template name="constraint"/>
        </xsl:for-each>
    </xsl:template>
</xsl:stylesheet>
