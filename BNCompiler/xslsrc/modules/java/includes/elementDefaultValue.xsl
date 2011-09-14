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

    <xsl:template name="elementDefaultValue">
	<xsl:param name="elementName"/>
	<xsl:param name="elementType"/>        
        <xsl:param name="instElementType"/>
        <xsl:param name="elementInfo"/>        
        
        <xsl:choose>
            <xsl:when test="isCString = 'true'">
                <xsl:if test="$instElementType!='null'">new <xsl:value-of select="$instElementType"/> (</xsl:if> <xsl:value-of select="cStr"/> <xsl:if test="$instElementType!='null'">)</xsl:if>
            </xsl:when>
            <xsl:when test="isDefinedValue = 'true'">
		<xsl:variable name="typeName" select="definedValue/name"/>
		<xsl:choose>
			<xsl:when test="$typeName = 'true'"><xsl:value-of select="$typeName"/> </xsl:when>
			<xsl:when test="$typeName = 'false'"><xsl:value-of select="$typeName"/> </xsl:when>
			<xsl:otherwise>
    			<xsl:for-each select="//module/asnValues">
				<xsl:if test="name = $typeName">
					<xsl:call-template name="elementDefaultValue">
						<xsl:with-param name="elementName" select ="$elementName"/>
						<xsl:with-param name="elementType" select ="$elementType"/>        
        					<xsl:with-param name="instElementType" select ="$instElementType"/>
			        		<xsl:with-param name="elementInfo" select ="$elementInfo"/>        
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
			</xsl:otherwise>
		</xsl:choose>
            </xsl:when>
            <xsl:when test="isSignedNumber = 'true'">
                <xsl:if test="$instElementType!='null'">new <xsl:value-of select="$instElementType"/> ( </xsl:if> <xsl:if test="signedNumber/positive != 'true'">-</xsl:if><xsl:value-of select="signedNumber/num"/> <xsl:if test="$instElementType!='null'">)</xsl:if>
            </xsl:when>        
            <xsl:when test="isCStrValue = 'true'">            		
                <xsl:if test="$instElementType != 'byte[]' and $instElementType != 'null'">new <xsl:value-of select="$instElementType"/> (</xsl:if>org.bn.coders.CoderUtils.defStringToOctetString("<xsl:value-of select="bStrValue/bhStr"/>")<xsl:if test="$instElementType = 'byte[]'">.getValue()</xsl:if> <xsl:if test="$instElementType != 'byte[]' and $instElementType != 'null'">)</xsl:if>                
            </xsl:when>
            <xsl:when test="isSequenceOfValue = 'true'">  
                <xsl:variable name="sqOfTypeName" select="$instElementType"/>
                
                <xsl:variable name="sqOfElementTypeName">                            
                <xsl:for-each select="$elementInfo">                                
                <xsl:choose>
                    <xsl:when test="typeReference/isSequenceOf = 'true'">
                         <xsl:for-each select="typeReference"><xsl:call-template name="elementType"/></xsl:for-each>
                    </xsl:when>
                    <xsl:otherwise>
                        <xsl:value-of select="'null'"/>
                    </xsl:otherwise>
                </xsl:choose>            
                </xsl:for-each>
                </xsl:variable>
                                
                new <xsl:value-of select="$sqOfTypeName"/>();
                <xsl:if test="$sqOfElementTypeName = 'null'">
                    param_<xsl:value-of select="$elementName"/>.initValue();
                </xsl:if>                
                {                    
                    <xsl:for-each select="seqOfVal/value">
                    param_<xsl:value-of select="$elementName"/>.add(
                        <xsl:call-template name="elementDefaultValue">
                            <xsl:with-param name="elementName" select="$elementName"/>
                            <xsl:with-param name="elementType" select="$elementType"/>
                            <xsl:with-param name="instElementType" select="$sqOfElementTypeName"/>
                            <xsl:with-param name="elementInfo" select="$elementInfo"/>                                
                        </xsl:call-template>
                    );
                    </xsl:for-each>
                }
            </xsl:when>
            <xsl:when test="isSequenceValue = 'true'">
                new <xsl:value-of select="$instElementType"/>();
                {
                <xsl:for-each select="seqval/namedValueList">
                    <xsl:variable name="fieldName"><xsl:call-template name="toUpperFirstLetter"><xsl:with-param name="input" select="name"/></xsl:call-template></xsl:variable>
                    param_<xsl:value-of select="$elementName"/>.set<xsl:value-of select="$fieldName"/> (
                        <xsl:for-each select="value">
                            <xsl:call-template name="elementDefaultValue">
                                <xsl:with-param name="elementName" select="$elementName"/>
                                <xsl:with-param name="elementType" select="$elementType"/>
                                <xsl:with-param name="instElementType" select="'null'"/>
                                <xsl:with-param name="elementInfo" select="$elementInfo"/>                                
                            </xsl:call-template>                            
                        </xsl:for-each>
                    );
                </xsl:for-each>
                }
            </xsl:when>
            <xsl:otherwise>null</xsl:otherwise>
        </xsl:choose>
            
    </xsl:template>
</xsl:stylesheet>
