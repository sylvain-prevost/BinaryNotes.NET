/*
 * Copyright 2007 Abdulla G. Abdurakhmanov (abdulla.abdurakhmanov@gmail.com).
 * 
 * Licensed under the LGPL, Version 2 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.gnu.org/copyleft/lgpl.html
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

using System;
using System.Collections.Generic;
using System.Text;
using org.bn.attributes;

namespace org.bn.types
{
    [ASN1Element(Name = "ObjectIdentifier")]
    [ASN1ObjectIdentifier(Name = "ObjectIdentifier")]
    public class ObjectIdentifier
    {
        private string oidString;

        public ObjectIdentifier(string oidString)
        {
            setValue(oidString);
        }

        public string getValue()
        {
            return oidString;
        }

        public void setValue(string oidString)
        {
            this.oidString = oidString;
        }

        public int[] getIntArray()
        {
            string[] sa = oidString.Split('.');
            int[] ia = new int[sa.Length];
            for (int i=0; i < sa.Length; i++)
            {
                ia[i] = int.Parse(sa[i]);
            }
            return ia;
        }

        public override string ToString()
        {
            return oidString;
        }
    }
}
