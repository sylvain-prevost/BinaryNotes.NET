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
using csUnit;
using test.org.bn.coders;
using test.org.bn.coders.ber;
using test.org.bn.coders.test_asn;
using test.org.bn.coders.per;

namespace test.org.bn.performance
{
    [TestFixture]
    class DummyPerformanceTest
    {
        protected void runEncoderPerfTest(string encoding)
        {
            IEncoder encoder = CoderFactory.getInstance().newEncoder(encoding);
            Assert.NotNull(encoder);
            // Create test structure
            DataSeq dt = new BERCoderTestUtils().createDataSeq();
            System.IO.Stream stream = new System.IO.MemoryStream();
            // Start test
            DateTime startTime = System.DateTime.Now;
            for (int i = 0; i < 100; i++)
            {
                encoder.encode<DataSeq>(dt, stream);
            }
            DateTime endTime = System.DateTime.Now;
            TimeSpan interval = (endTime-startTime);
            System.Console.WriteLine("Encode elapsed time for " + encoding + ": " + interval.TotalSeconds );
        }

        protected void runDecoderPerfTest(string encoding, CoderTestUtilities coderUtils)
        {
            IDecoder encoder = CoderFactory.getInstance().newDecoder(encoding);
            Assert.NotNull(encoder);
            // Create test structure
            System.IO.Stream stream = new System.IO.MemoryStream(
                    coderUtils.createDataSeqBytes()
            );
            // Start test
            DateTime startTime = System.DateTime.Now;
            for (int i = 0; i < 100; i++)
            {
                DataSeq dt = encoder.decode<DataSeq>(stream);
                stream.Position = 0;
            }
            DateTime endTime = System.DateTime.Now;
            TimeSpan interval = (endTime - startTime);
            System.Console.WriteLine("Decode elapsed time for " + encoding + ": " + interval.TotalSeconds);
        }

        public void testEncodePerf()
        {
            runEncoderPerfTest("BER");
            runEncoderPerfTest("PER");
            runEncoderPerfTest("PER/Unaligned");
        }

        public void testDecodePerf(){
            runDecoderPerfTest("BER", new BERCoderTestUtils());
            runDecoderPerfTest("PER", new PERAlignedCoderTestUtils());
            runDecoderPerfTest("PER/Unaligned", new PERUnalignedCoderTestUtils());
        }

    }
}
