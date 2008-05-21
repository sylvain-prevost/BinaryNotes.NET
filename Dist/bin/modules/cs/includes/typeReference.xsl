<?xml version="1.0" encoding="utf-8" ?>
<!--
/*
 * Copyright 2006 Abdulla G. Abdurakhmanov (abdulla.abdurakhmanov@gmail.com).
 * 
 * Licensed under the GPL, Version 2 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.gnu.org/copyleft/gpl.html
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * With any your questions welcome to my e-mail 
 * or blog at http://abdulla-a.blogspot.com.
 */
-->

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xsltc="http://xml.apache.org/xalan/xsltc"
    xmlns:redirect="http://xml.apache.org/xalan/redirect"
    extension-element-prefixes="xsltc redirect"
>
    <xsl:import href="integerTypeDecl.xsl"/>
    <xsl:output method="text" encoding="UTF-8" indent="no"/>
    

    <xsl:template name="typeReference">
	<xsl:param name="elementName"/>
        <xsl:param name="instanceable"/>
        <xsl:choose>
            <xsl:when test="typeReference/BUILTINTYPE = 'CHARACTER STRING'">string</xsl:when>
            <xsl:when test="typeReference/BUILTINTYPE = 'OCTET STRING'">byte[]</xsl:when>
            <xsl:when test="typeReference/BUILTINTYPE = 'BIT STRING'">BitString</xsl:when>
	    <xsl:when test="typeReference/BUILTINTYPE = 'OBJECT IDENTIFIER'">ObjectIdentifier</xsl:when>
            <xsl:when test="typeReference/BUILTINTYPE = 'BOOLEAN'">bool</xsl:when>
            <xsl:when test="typeReference/BUILTINTYPE = 'REAL'">double</xsl:when>
            <xsl:when test="typeReference/BUILTINTYPE = 'INTEGER'"><xsl:for-each select="typeReference"><xsl:call-template name="integerTypeDecl"/></xsl:for-each></xsl:when>
            <xsl:when test="typeReference/isSequenceOf = 'true' or typeReference/isSequenceOf = 'false'"><xsl:if test="$instanceable != 'yes'">System.Collections.Generic.ICollection</xsl:if><xsl:if test="$instanceable = 'yes'">System.Collections.Generic.List</xsl:if>&lt;<xsl:for-each select="typeReference"><xsl:call-template name="elementType"><xsl:with-param name="parentElementName" select="$elementName"/></xsl:call-template></xsl:for-each>&gt;</xsl:when>
	    <xsl:when test="typeReference/isSequence = 'true' or typeReference/isSequence = 'false'"><xsl:call-template name="toUpperFirstLetter"><xsl:with-param name="input" select="$elementName"/></xsl:call-template>SequenceType</xsl:when>
            <xsl:when test="typeReference/isChoice = 'true'"><xsl:call-template name="toUpperFirstLetter"><xsl:with-param name="input" select="$elementName"/></xsl:call-template>ChoiceType</xsl:when>
	    <xsl:when test="typeReference/isEnum = 'true'"><xsl:call-template name="toUpperFirstLetter"><xsl:with-param name="input" select="$elementName"/></xsl:call-template>EnumType</xsl:when>
            <xsl:when test="typeReference/isNull = 'true'">NullObject</xsl:when>
            <xsl:otherwise>byte[]</xsl:otherwise>
        </xsl:choose>
    </xsl:template>
</xsl:stylesheet>
