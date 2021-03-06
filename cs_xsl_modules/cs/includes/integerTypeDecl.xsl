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
    <xsl:import href="doDeterminateEndValue.xsl"/>

    <xsl:output method="text" encoding="UTF-8" indent="no"/>

    <xsl:template name="integerTypeDecl">        
        <xsl:choose>
            <xsl:when test="constraint/elemSetSpec/intersectionList/cnsElemList/lEndValue">
                <xsl:if test= "constraint/elemSetSpec/intersectionList/cnsElemList/uEndValue">
                     <xsl:variable name="min"><xsl:for-each select="constraint/elemSetSpec/intersectionList/cnsElemList/lEndValue"><xsl:call-template name="doDeterminateEndValue"/></xsl:for-each></xsl:variable>
                     <xsl:variable name="max"><xsl:for-each select="constraint/elemSetSpec/intersectionList/cnsElemList/uEndValue"><xsl:call-template name="doDeterminateEndValue"/></xsl:for-each></xsl:variable>
                     <xsl:choose>
                        <xsl:when test ="number($min) &gt;= number('-2147483648') and number($max) &lt;= number('2147483647')">int</xsl:when>
                        <xsl:when test ="number($min) &gt;= number('-9223372036854775808') and number($max) &lt;= number('9223372036854775807')">long</xsl:when>
                        <xsl:otherwise>BigInteger</xsl:otherwise>
                     </xsl:choose>
               </xsl:if>
                <xsl:if test= "constraint/elemSetSpec/intersectionList/cnsElemList/isMaxKw = 'true'">long</xsl:if>
            </xsl:when>
            <xsl:otherwise>BigInteger</xsl:otherwise>
        </xsl:choose>
    </xsl:template>
</xsl:stylesheet>
