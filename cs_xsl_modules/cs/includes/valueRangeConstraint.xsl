<?xml version="1.0" encoding="utf-8"?>
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

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsltc="http://xml.apache.org/xalan/xsltc" xmlns:redirect="http://xml.apache.org/xalan/redirect" extension-element-prefixes="xsltc redirect">
    <xsl:import href="doDeterminateEndValue.xsl" />

    <xsl:output method="text" encoding="UTF-8" indent="no" />

    <xsl:template name="valueRangeConstraint">
        <xsl:if test="elemSetSpec/intersectionList/cnsElemList/lEndValue">
            <xsl:if test="elemSetSpec/intersectionList/cnsElemList/uEndValue or elemSetSpec/intersectionList/cnsElemList/isMaxKw = 'true'">
            <xsl:text>&#xA;&#x9;&#x9;[ASN1ValueRangeConstraint(</xsl:text>
                
                <xsl:for-each select="elemSetSpec/intersectionList/cnsElemList/lEndValue">
                    <xsl:variable name="val"><xsl:call-template name="doDeterminateEndValue"/></xsl:variable>
                    <xsl:choose>
                        <xsl:when test ="number($val) &gt;= number('-2147483648') and number($val) &lt;= number('2147483647')">
                            <xsl:text>Min = </xsl:text>
                            <xsl:call-template name="doDeterminateEndValue"/></xsl:when>
                        <xsl:when test ="number($val) &gt;= number('-9223372036854775808') and number($val) &lt;= number('9223372036854775807')">
                            <xsl:text>Min = </xsl:text>
                            <xsl:call-template name="doDeterminateEndValue"/>
                            <xsl:text>L</xsl:text>
                        </xsl:when>
                    </xsl:choose>
                </xsl:for-each>
                <xsl:if test="elemSetSpec/intersectionList/cnsElemList/uEndValue">
                    <xsl:for-each select="elemSetSpec/intersectionList/cnsElemList/uEndValue">
                        <xsl:variable name="val"><xsl:call-template name="doDeterminateEndValue"/></xsl:variable>
                        <xsl:choose>
                            <xsl:when test ="number($val) &gt;= number('-2147483648') and number($val) &lt;= number('2147483647')">
                                <xsl:text>, Max = </xsl:text>
                                
                                <xsl:call-template name="doDeterminateEndValue" /></xsl:when>
                            <xsl:when test ="number($val) &gt;= number('-9223372036854775808') and number($val) &lt;= number('9223372036854775807')">
                                <xsl:text>, Max = </xsl:text>
                                <xsl:call-template name="doDeterminateEndValue" />
                                <xsl:text>L</xsl:text>
                            </xsl:when>
                        </xsl:choose>
                    </xsl:for-each>
                </xsl:if>
                <xsl:if test="elemSetSpec/intersectionList/cnsElemList/isMaxKw = 'true'">
                <xsl:text>, Max = long.MaxValue</xsl:text>
            </xsl:if>
                <xsl:text>)]</xsl:text>
            </xsl:if>
        </xsl:if>
    </xsl:template>
</xsl:stylesheet>