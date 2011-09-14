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
    <xsl:import href="elementType.xsl"/>
    <xsl:import href="elementDefaultValue.xsl"/>
    <xsl:output method="text" encoding="UTF-8" indent="no"/>

    <xsl:template name="elementDefault">
        <xsl:variable name="elementName"><xsl:call-template name="doMangleIdent"><xsl:with-param name='input'><xsl:call-template name="toUpperFirstLetter"><xsl:with-param name="input" select="name"/></xsl:call-template></xsl:with-param></xsl:call-template></xsl:variable>
        <xsl:variable name="elementType"><xsl:call-template name="elementType"/></xsl:variable>
        <xsl:variable name="instElementType"><xsl:call-template name="elementType"> <xsl:with-param name="instanceable" select="'yes'"/></xsl:call-template> </xsl:variable>
        <xsl:variable name="elementInfo" select="."/>

        <xsl:value-of select="$elementType"/> param_<xsl:value-of select="$elementName"/> =         
            <xsl:for-each select="value">
            <xsl:call-template name="elementDefaultValue">
                <xsl:with-param name="elementName" select="$elementName"/>
                <xsl:with-param name="elementType" select="$elementType"/>
                <xsl:with-param name="elementInfo" select="$elementInfo"/>
                <xsl:with-param name="instElementType" select="$instElementType"/>
            </xsl:call-template>
            </xsl:for-each>;
        <xsl:value-of select="$elementName"/> = param_<xsl:value-of select="$elementName"/>;
    </xsl:template>
</xsl:stylesheet>
