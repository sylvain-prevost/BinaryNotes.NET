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
    <xsl:import href="choiceFunctions.xsl"/>

    <xsl:output method="text" encoding="UTF-8" indent="no"/>
    
    <xsl:variable name="outputDirectory"><xsl:value-of select="//outputDirectory"/></xsl:variable>
    
    <xsl:template name="choice">
        <xsl:variable name="choiceName"><xsl:call-template name="doMangleIdent"> <xsl:with-param name='input'><xsl:value-of select="name"/></xsl:with-param> </xsl:call-template></xsl:variable>

        <xsltc:output file="{$outputDirectory}/{$choiceName}.java">
            <xsl:call-template name="header"/>

    @ASN1PreparedElement
    @ASN1Choice ( name = "<xsl:value-of select='$choiceName'/>" )
    public class <xsl:value-of select="$choiceName"/> implements IASN1PreparedElement {
            <xsl:call-template name="elements"/>
            <xsl:call-template name="choiceFunctions"/>

	    public void initWithDefaults() {
	    }

        private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(<xsl:value-of select='$choiceName'/>.class);
        public IASN1PreparedElementData getPreparedData() {
            return preparedData;
        }


    }
            <xsl:call-template name="footer"/>
        </xsltc:output>        
    </xsl:template>    
</xsl:stylesheet>
