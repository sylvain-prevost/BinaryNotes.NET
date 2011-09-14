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

    <xsl:import href="header.xsl"/>
    <xsl:import href="footer.xsl"/>
    <xsl:import href="elements.xsl"/>
    <xsl:import href="sequenceFunctions.xsl"/>
    <xsl:import href="elementDefaults.xsl"/>

    <xsl:output method="text" encoding="UTF-8" indent="no"/>
    
    <xsl:variable name="outputDirectory"><xsl:value-of select="//outputDirectory"/></xsl:variable>
    
    <xsl:template name="sequence">
	<xsl:variable name="sequenceName"><xsl:call-template name="doMangleIdent"><xsl:with-param name='input' select="name"/></xsl:call-template></xsl:variable>
        <xsltc:output file="{$outputDirectory}/{$sequenceName}.cs">
            <xsl:call-template name="header"/>

    [ASN1PreparedElement]
    [ASN1Sequence ( Name = "<xsl:value-of select='$sequenceName'/>", IsSet = <xsl:choose><xsl:when test="isSequence = 'false'">true</xsl:when><xsl:otherwise>false</xsl:otherwise></xsl:choose>  )]
    public class <xsl:value-of select="$sequenceName"/> : IASN1PreparedElement {
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
            <xsl:call-template name="footer"/>
        </xsltc:output>        
    </xsl:template>    
</xsl:stylesheet>
