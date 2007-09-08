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
package test.org.bn;

import junit.framework.Test;
import junit.framework.TestSuite;

import junit.swingui.TestRunner;

import test.org.bn.coders.CoderUtilsTest;
import test.org.bn.coders.ber.BERDecoderTest;
import test.org.bn.coders.ber.BEREncoderTest;
import test.org.bn.coders.der.DERDecoderTest;
import test.org.bn.coders.der.DEREncoderTest;
import test.org.bn.coders.per.PERAlignedDecoderTest;
import test.org.bn.coders.per.PERAlignedEncoderTest;
import test.org.bn.coders.per.PERUnalignedDecoderTest;
import test.org.bn.coders.per.PERUnalignedEncoderTest;
import test.org.bn.performance.DummyPerformanceTest;
import test.org.bn.utils.BitArrayInputStreamTest;
import test.org.bn.utils.BitArrayOutputStreamTest;

public class AllTests {
	
    public static Test suite() {
        TestSuite suite;
        suite = new TestSuite("AllTests");
        suite.addTestSuite(BEREncoderTest.class);
        suite.addTestSuite(BERDecoderTest.class);
        suite.addTestSuite(DEREncoderTest.class);
        suite.addTestSuite(DERDecoderTest.class);        
        suite.addTestSuite(PERAlignedEncoderTest.class);
        suite.addTestSuite(PERAlignedDecoderTest.class);        
        suite.addTestSuite(PERUnalignedEncoderTest.class);
        suite.addTestSuite(PERUnalignedDecoderTest.class);
        suite.addTestSuite(BitArrayOutputStreamTest.class);
        suite.addTestSuite(BitArrayInputStreamTest.class);
        suite.addTestSuite(CoderUtilsTest.class);
        suite.addTestSuite(DummyPerformanceTest.class);
        return suite;
    }

    public static void main(String args[]) {
        String args2[] = {"-noloading", "test.org.bn.AllTests"};
        TestRunner.main(args2);
    }
}
