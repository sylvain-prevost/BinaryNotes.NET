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

    <xsl:template name="sequenceDecl">
	<xsl:param name="elementName"/>
	<xsl:variable name="sequenceName"><xsl:call-template name="toUpperFirstLetter"><xsl:with-param name="input" select="$elementName"/></xsl:call-template>SequenceType</xsl:variable>

            <xsl:if test="typeReference/isSequence = 'true' or typeReference/isSequence = 'false'">
                <xsl:for-each select="typeReference">
       [ASN1PreparedElement]
       [ASN1Sequence ( Name = "<xsl:value-of select='$elementName'/>", IsSet = <xsl:choose><xsl:when test="isSequence = 'false'">true</xsl:when><xsl:otherwise>false</xsl:otherwise></xsl:choose>  )]
       public class <xsl:value-of select='$sequenceName'/> : IASN1PreparedElement {
                <xsl:call-template name="elements"/>
                <xsl:call-template name="sequenceFunctions"/>
                
                public void initWithDefaults() {
            		<xsl:call-template name="elementDefaults">
				<xsl:with-param name="typeName" select="$sequenceName"/>
            		</xsl:call-template>
                }

            private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(<xsl:value-of select='$sequenceName'/>));
            public IASN1PreparedElementData PreparedData {
            	get { return preparedData; }
            }

                
       }
                </xsl:for-each>
            </xsl:if>
    </xsl:template>
</xsl:stylesheet>
