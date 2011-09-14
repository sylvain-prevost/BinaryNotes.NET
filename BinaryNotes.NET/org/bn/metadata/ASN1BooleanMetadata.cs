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

using System;
using System.Reflection;
using System.IO;
using org.bn.attributes;
using org.bn.coders;

namespace org.bn.metadata
{
    public class ASN1BooleanMetadata : ASN1FieldMetadata {

        public ASN1BooleanMetadata() {
            
        }
        
        public ASN1BooleanMetadata(String name): base(name)
        {
        }

        public ASN1BooleanMetadata(ASN1Boolean annotation)
            : this(annotation.Name)
        {
        }
        
        public override int encode(IASN1TypesEncoder encoder, object obj, Stream stream, ElementInfo elementInfo) {
            return encoder.encodeBoolean(obj, stream, elementInfo);
        }

        public override DecodedObject<object> decode(IASN1TypesDecoder decoder, DecodedObject<object> decodedTag, Type objectClass, ElementInfo elementInfo, Stream stream)
        {
            return decoder.decodeBoolean(decodedTag,objectClass,elementInfo,stream);
        }
        
    }
}