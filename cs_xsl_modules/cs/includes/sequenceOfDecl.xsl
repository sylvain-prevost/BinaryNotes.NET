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

    <xsl:template name="sequenceOfDecl">
<xsl:param name="elementName"/>
<xsl:variable name="sequenceOfName">
<xsl:choose>
            <xsl:when test="string-length(typeReference/name) > 0"><xsl:value-of select="typeReference/name"/></xsl:when>
<xsl:otherwise><xsl:value-of select="$elementName"/></xsl:otherwise>
</xsl:choose>
</xsl:variable>
<xsl:for-each select="typeReference">
    <xsl:call-template name="typeDecl"><xsl:with-param name="parentElementName" select="$sequenceOfName"/></xsl:call-template>
</xsl:for-each>
    <xsl:text>&#xA;&#x9;&#x9;[ASN1SequenceOf(Name = "</xsl:text>
    <xsl:value-of select='$sequenceOfName'/>", IsSetOf = <xsl:choose><xsl:when test="typeReference/isSequenceOf = 'false'">true</xsl:when><xsl:otherwise>false</xsl:otherwise></xsl:choose>)]
    </xsl:template>
</xsl:stylesheet>
