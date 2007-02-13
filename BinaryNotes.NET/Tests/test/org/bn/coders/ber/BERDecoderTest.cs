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
using org.bn;
using org.bn.coders;
using csUnit;
using test.org.bn.coders.test_asn;

namespace test.org.bn.coders.ber
{
	
	public class BERDecoderTest:DecoderTest
	{
		protected CoderFactory coderFactory = new CoderFactory();
		
		
		public BERDecoderTest(System.String sTestName):base(sTestName, new BERCoderTestUtils())
		{
		}

        public BERDecoderTest(System.String sTestName, CoderTestUtilities coderTestUtils)
            : base(sTestName, coderTestUtils)
        {
        }

		
		protected override IDecoder newDecoder()
		{
			return coderFactory.newDecoder("BER");
		}
	}
}