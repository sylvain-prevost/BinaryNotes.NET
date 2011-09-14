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
<xsl:stylesheet version="1.0" 
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xsltc="http://xml.apache.org/xalan/xsltc"
    xmlns:redirect="http://xml.apache.org/xalan/redirect"
    extension-element-prefixes="xsltc redirect"
>
    <xsl:import href="includes/header.xsl"/>
    <xsl:import href="includes/footer.xsl"/>
    <xsl:import href="includes/choice.xsl"/>
    <xsl:import href="includes/sequence.xsl"/>    
    <xsl:import href="includes/enum.xsl"/>
    <xsl:import href="includes/boxedSequenceOfType.xsl"/>
    <xsl:import href="includes/boxedStringType.xsl"/>
    <xsl:import href="includes/boxedOctetStringType.xsl"/>
    <xsl:import href="includes/boxedBooleanType.xsl"/>
    <xsl:import href="includes/boxedBitStringType.xsl"/>
    <xsl:import href="includes/boxedObjectIdentifierType.xsl"/>
    <xsl:import href="includes/boxedIntegerType.xsl"/>
    <xsl:import href="includes/boxedRealType.xsl"/>
    <xsl:import href="includes/boxedType.xsl"/>
    <xsl:import href="includes/boxedNullType.xsl"/>
    <xsl:import href="includes/packageInfo.xsl"/>
    
    <xsl:output method="text" encoding="UTF-8" indent="no"/>
    
    <xsl:variable name="outputDirectory"><xsl:value-of select="//outputDirectory"/></xsl:variable>
    <xsl:variable name="moduleName"><xsl:value-of select="//moduleNS"/></xsl:variable>
    
    <xsl:template match="/">
        <xsl:call-template name="header"/>
        <xsl:apply-templates/>
        <xsl:call-template name="footer"/>
    </xsl:template>
    
    <xsl:template match="//module/moduleIdentifier" >
        <xsl:call-template name="packageInfo"/>
        <xsl:apply-templates/>
    </xsl:template>
    
    <xsl:template match="//module/asnTypes/choices">
        <xsl:call-template name="choice"/>
        <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="//module/asnTypes/sequenceSets">
        <xsl:call-template name="sequence"/>
        <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="//module/asnTypes/enums">
        <xsl:call-template name="enum"/>
        <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="//module/asnTypes/sequenceSetsOf">
        <xsl:call-template name="boxedSequenceOfType"/>
        <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="//module/asnTypes/characterStrings">
        <xsl:call-template name="boxedStringType"/>
        <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="//module/asnTypes/octetStrings">
        <xsl:call-template name="boxedOctetStringType"/>
        <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="//module/asnTypes/booleans">
        <xsl:call-template name="boxedBooleanType"/>
        <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="//module/asnTypes/bitStrings">
        <xsl:call-template name="boxedBitStringType"/>
        <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="//module/asnTypes/objectIdentifiers">
        <xsl:call-template name="boxedObjectIdentifierType"/>
        <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="//module/asnTypes/integers">
        <xsl:call-template name="boxedIntegerType"/>
        <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="//module/asnTypes/reals">
        <xsl:call-template name="boxedRealType"/>
        <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="//module/asnTypes/taggeds">
        <xsl:call-template name="boxedType"/>
        <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="//module/asnTypes/defineds">
        <xsl:call-template name="boxedType"/>
        <xsl:apply-templates/>
    </xsl:template>

    <xsl:template match="//module/asnTypes/nulls">
        <xsl:call-template name="boxedNullType"/>
        <xsl:apply-templates/>
    </xsl:template>
    
    <xsl:template match="text()"><xsl:apply-templates/></xsl:template>
</xsl:stylesheet>
