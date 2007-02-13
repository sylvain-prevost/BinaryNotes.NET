/*
 * Copyright 2006 Abdulla G. Abdurakhmanov (abdulla.abdurakhmanov@gmail.com).
 * 
 * Licensed under the GPL, Version 2 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.gnu.org/copyleft/gpl.html
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
package test.org.bn.compiler;


import junit.framework.Test;
import junit.framework.TestSuite;

import junit.swingui.TestRunner;

import test.org.bn.compiler.parser.ASNParserTest;


public class AllTests {
    public static void main(String args[]) {
        String args2[] = { "-noloading", "test.org.bn.compiler.AllTests" };

        TestRunner.main(args2);
    }

    public static Test suite() {
        TestSuite suite;

        suite = new TestSuite("AllTests");
        suite.addTestSuite(ASNParserTest.class);
        suite.addTestSuite(MainTest.class);

        return suite;
    }
}

