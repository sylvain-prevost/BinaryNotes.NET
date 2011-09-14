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
    <!-- tagClass = "<xsl:value-of select ='tag/clazz'/>" -->
    <xsl:template name="tagClass">
        tagClass = <xsl:choose>
            <xsl:when test="tag/clazz = 'APPLICATION'"> TagClass.Application </xsl:when>
            <xsl:when test="tag/clazz = 'PRIVATE'"> TagClass.Private </xsl:when>
            <xsl:otherwise>TagClass.ContextSpecific</xsl:otherwise>
        </xsl:choose>
    </xsl:template>
</xsl:stylesheet>
