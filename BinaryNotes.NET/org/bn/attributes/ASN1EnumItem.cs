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

namespace org.bn.attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ASN1EnumItem : Attribute
    {
        private string name = "";

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        bool hasTag = false;

        public bool HasTag
        {
            get { return hasTag; }
            set { hasTag = value; }
        }

        int tag = 0;

        public int Tag
        {
            get { return tag; }
            set { tag = value; }
        }

    }
}
