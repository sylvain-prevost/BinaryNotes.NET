<?xml version="1.0" encoding="utf-8" ?>
<!--
/*
 * Copyright 2006 Abdulla G. Abdurakhmanov (abdulla.abdurakhmanov@gmail.com).
 * 
 * Licensed under the GPL, Version 2 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.gnu.org/copyleft/gpl.html
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * With any your questions welcome to my e-mail 
 * or blog at http://abdulla-a.blogspot.com.
 */
-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xsltc="http://xml.apache.org/xalan/xsltc"
    xmlns:redirect="http://xml.apache.org/xalan/redirect"
    extension-element-prefixes="xsltc redirect"
>
    <xsl:import href="header.xsl"/>
    <xsl:import href="footer.xsl"/>

    <xsl:output method="text" encoding="UTF-8" indent="no"/>

    <xsl:template name="boxedOctetStringType">
        <xsl:variable name="boxedName"><xsl:call-template name="doMangleIdent"> <xsl:with-param name='input'><xsl:value-of select="name"/></xsl:with-param> </xsl:call-template></xsl:variable>
        <xsltc:output file="{$outputDirectory}/{$boxedName}.cs">
            <xsl:call-template name="header"/>

    [ASN1PreparedElement]
    [ASN1BoxedType ( Name = "<xsl:value-of select='$boxedName'/>") ]
    public class <xsl:value-of select="$boxedName"/>: IASN1PreparedElement {
    
            private byte[] val = null;

            [ASN1OctetString( Name = "<xsl:value-of select='name'/>") ]            
            <xsl:for-each select="constraint">
                <xsl:call-template name="constraint"/>
            </xsl:for-each>
            public byte[] Value
            {
                get { return val; }
                set { val = value; }
            }            
            
            public <xsl:value-of select="$boxedName"/>() {
            }

            public <xsl:value-of select="$boxedName"/>(byte[] value) {
                this.Value = value;
            }            
            
            public <xsl:value-of select="$boxedName"/>(BitString value) {
                this.Value = value.Value;
            }                        

            public void initWithDefaults()
	    {
	    }


            private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(<xsl:value-of select='$boxedName'/>));
            public IASN1PreparedElementData PreparedData {
            	get { return preparedData; }
            }

    }
            <xsl:call-template name="footer"/>
        </xsltc:output>        
    </xsl:template>
</xsl:stylesheet>
