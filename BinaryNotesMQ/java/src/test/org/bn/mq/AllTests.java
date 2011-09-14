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

package test.org.bn.mq;

import junit.framework.Test;
import junit.framework.TestSuite;

import junit.swingui.TestRunner;

import test.org.bn.mq.net.MessageDecoderThreadTest;
import test.org.bn.mq.net.tcp.TransportFactoryTest;

public class AllTests {
    public static Test suite() {
        TestSuite suite;
        suite = new TestSuite("AllTests");
        suite.addTestSuite(TransportFactoryTest.class);
        suite.addTestSuite(MessageDecoderThreadTest.class);
        suite.addTestSuite(MQFactoryTest.class);
        return suite;
    }

    public static void main(String args[]) {
        String args2[] = {"-noloading", "test.org.bn.mq.AllTests"};
        TestRunner.main(args2);
    }
}
