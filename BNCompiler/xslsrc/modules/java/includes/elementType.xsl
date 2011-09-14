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
    <xsl:import href="typeReference.xsl"/>
    <xsl:output method="text" encoding="UTF-8" indent="no"/>

    <xsl:template name="elementType">
        <xsl:param name="instanceable" select="'no'"/>
	<xsl:param name="parentElementName"/>
	<xsl:variable name="currentNodeName"><xsl:call-template name="doMangleIdent"><xsl:with-param name='input' select="name"/></xsl:call-template></xsl:variable>
        <xsl:variable name="elementName">		
		<xsl:choose>
			<xsl:when test="$currentNodeName!=''"><xsl:value-of select="$currentNodeName"/></xsl:when>
			<xsl:otherwise><xsl:value-of select="$parentElementName"/></xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
        <xsl:choose>
            <xsl:when test="string-length(typeName) > 0"><xsl:call-template name="doMangleIdent"><xsl:with-param name='input' select="typeName"/></xsl:call-template></xsl:when>
            <xsl:when test="typeReference"><xsl:call-template name="typeReference"><xsl:with-param name="elementName" select="$elementName"/><xsl:with-param name="instanceable" select="$instanceable"/></xsl:call-template></xsl:when>
        </xsl:choose>
    </xsl:template>
</xsl:stylesheet>
