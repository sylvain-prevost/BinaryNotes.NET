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
package test.org.bn.coders.per;

import org.bn.CoderFactory;
import org.bn.IEncoder;

import test.org.bn.coders.CoderTestUtilities;
import test.org.bn.coders.EncoderTest;
import test.org.bn.coders.ber.BEREncoderTest;
import test.org.bn.coders.test_asn.TaggedNullSequence;

public class PERAlignedEncoderTest extends EncoderTest {
    protected CoderFactory coderFactory = new CoderFactory();
    
    public PERAlignedEncoderTest(String sTestName) {
        super(sTestName, new PERAlignedCoderTestUtils());
    }
       
    protected <T> IEncoder<T> newEncoder() throws Exception {
        return coderFactory.newEncoder("PER/Aligned");
    }
    
    public void testTaggedNullEncode() throws Exception {
        // PER does not encode NULL value
    }
    
    public void testNullEncode() {
        // PER does not encode NULL value        
    }
    
}

