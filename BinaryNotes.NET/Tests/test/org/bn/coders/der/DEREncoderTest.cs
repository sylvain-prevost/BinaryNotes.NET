/*
 * Copyright 2006 Abdulla G. Abdurakhmanov (abdulla.abdurakhmanov@gmail.com).
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
using org.bn;
using org.bn.attributes;
using org.bn.coders;
using test.org.bn.coders;
using test.org.bn.coders.ber;

namespace test.org.bn.coders.der
{
    class DEREncoderTest: BEREncoderTest
    {
        public DEREncoderTest(string testName)
            : base(testName, new DERCoderUtils())
        {
        }

        protected override IEncoder newEncoder()
        {
            return coderFactory.newEncoder("DER");
        }

    }
}
