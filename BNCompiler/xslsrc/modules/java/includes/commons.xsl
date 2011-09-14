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
    <xsl:output method="text" encoding="UTF-8" indent="no"/>

    <xsl:template name="toLower">
        <xsl:param name="input"/>
        <xsl:value-of select="translate($input,'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz')"/>
    </xsl:template>
    
    <xsl:template name="toUpper">
        <xsl:param name="input"/>
        <xsl:value-of select="translate($input,'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')"/>
    </xsl:template>

    <xsl:template name="toUpperFirstLetter">
        <xsl:param name="input"/>
	<xsl:value-of select="concat ( translate( substring($input,1,1),'abcdefghijklmnopqrstuvwxyz-[]', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ___'), translate(substring($input,2),'-[]','___') )"/>
        <!-- <xsl:value-of select="concat ( translate( substring($input,1,1),'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'), substring($input,2) )"/> -->
    </xsl:template>

    <xsl:template name="doMangleIdent">
        <xsl:param name="input"/>
	<xsl:variable name='linput'>
	<xsl:call-template name="toLower">
		<xsl:with-param name= "input" select="$input"/>
	</xsl:call-template>	
	</xsl:variable>
	<xsl:variable name='loutput'>
	<xsl:value-of select="$input"/>
	<xsl:choose>		
		<xsl:when test="$linput = 'class'">_</xsl:when>
		<xsl:when test="$input = 'public'">_</xsl:when>
		<xsl:when test="$input = 'protected'">_</xsl:when>
		<xsl:when test="$input = 'private'">_</xsl:when>
		<xsl:when test="$input = 'void'">_</xsl:when>
		<xsl:when test="$input = 'null'">_</xsl:when>
		<xsl:when test="$input = 'int'">_</xsl:when>
		<xsl:when test="$input = 'long'">_</xsl:when>
		<xsl:when test="$input = 'short'">_</xsl:when>
		<xsl:when test="$input = 'double'">_</xsl:when>
		<xsl:when test="$input = 'float'">_</xsl:when>
		<xsl:when test="$input = 'byte'">_</xsl:when>
		<xsl:when test="$input = 'String'">_</xsl:when>
		<xsl:when test="$input = 'interface'">_</xsl:when>
		<xsl:when test="$input = 'import'">_</xsl:when>
		<xsl:when test="$input = 'package'">_</xsl:when>
		<xsl:when test="$input = 'unit'">_</xsl:when>
		<xsl:when test="$input = 'using'">_</xsl:when>
		<xsl:when test="$input = 'static'">_</xsl:when>
		<xsl:when test="$input = 'return'">_</xsl:when>
		<xsl:when test="$input = 'final'">_</xsl:when>
		<xsl:when test="$input = 'const'">_</xsl:when>
	</xsl:choose>
	</xsl:variable>
	<xsl:value-of select="translate($loutput,'-[]','___')"/>
    </xsl:template>

    
</xsl:stylesheet>
