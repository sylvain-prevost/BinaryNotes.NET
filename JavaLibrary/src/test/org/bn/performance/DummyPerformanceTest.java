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
package test.org.bn.performance;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;

import java.io.OutputStream;

import java.util.LinkedList;
import java.util.List;

import junit.framework.Assert;
import junit.framework.TestCase;

import org.bn.CoderFactory;
import org.bn.IDecoder;
import org.bn.IEncoder;
import org.bn.coders.ber.BEREncoder;
import org.bn.coders.Encoder;

import test.org.bn.coders.CoderTestUtilities;
import test.org.bn.coders.ber.BERCoderTestUtils;
import test.org.bn.coders.per.PERAlignedCoderTestUtils;
import test.org.bn.coders.per.PERUnalignedCoderTestUtils;
import test.org.bn.utils.ByteTools;
import test.org.bn.coders.test_asn.*;
public class DummyPerformanceTest extends TestCase {
    public DummyPerformanceTest(String sTestName) {
        super(sTestName);
    }
    
    protected void runEncoderPerfTest(String encoding) throws Exception {
        IEncoder encoder = CoderFactory.getInstance().newEncoder(encoding);
        assertNotNull(encoder);
        // Create test structure
        DataSeq dt = new BERCoderTestUtils().createDataSeq();
        ByteArrayOutputStream stream = new ByteArrayOutputStream();
        // Start test
        long startTime = System.currentTimeMillis();
        for (int i = 0; i < 100; i++)
        {
            encoder.encode(dt, stream);
        }
        long endTime = System.currentTimeMillis();
        long interval = (endTime-startTime);
        System.out.println("Encode elapsed time for " + encoding + ": " + interval/1000.0 );
    }

    protected void runDecoderPerfTest(String encoding, CoderTestUtilities coderUtils) throws Exception {
        IDecoder decoder = CoderFactory.getInstance().newDecoder(encoding);
        assertNotNull(decoder);
        // Create test structure
        ByteArrayInputStream stream = new ByteArrayInputStream( coderUtils.createDataSeqBytes() );
        // Start test
        long startTime = System.currentTimeMillis();
        for (int i = 0; i < 100; i++)
        {
            DataSeq dt = decoder.decode(stream, DataSeq.class);
            stream.reset();
        }
        long endTime = System.currentTimeMillis();
        long interval = (endTime-startTime);
        System.out.println("Decode elapsed time for " + encoding + ": " + interval/1000.0 );
    }
    
    public void testEncodePerf() throws Exception {
        runEncoderPerfTest("BER");
        runEncoderPerfTest("PER");
        runEncoderPerfTest("PER/Unaligned");
    }

    public void testDecodePerf() throws Exception {
        runDecoderPerfTest("BER", new BERCoderTestUtils());
        runDecoderPerfTest("PER", new PERAlignedCoderTestUtils());
        runDecoderPerfTest("PER/Unaligned", new PERUnalignedCoderTestUtils());
    }
    
}
