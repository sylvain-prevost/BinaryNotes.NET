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

import junit.framework.TestCase;

import org.bn.compiler.Main;
import org.bn.compiler.Module;
import java.io.File;

public class MainTest extends TestCase {
    public MainTest(String sTestName) {
        super(sTestName);
    }

    /**
     * @see Main#start(String[])
     */
    public void testStart() throws Exception {
        Main compiler = new Main();

        compiler.start(new String[] {
            "--modulesPath", "xslsrc" + File.separator + "modules",
            "--moduleName", "java", 
            "--outputDir", "testworkdir" + File.separator + "output", 
            "--fileName", "testworkdir" + File.separator + "test.asn",
            "-ns", "test.org.bn.coders.test_asn"
        });
    }
    
    public void testCSStart() throws Exception {
        Main compiler = new Main();

        compiler.start(new String[] {
            "--modulesPath", "xslsrc" + File.separator + "modules",
            "--moduleName", "cs", 
            "--outputDir", "testworkdir" + File.separator + "output-cs", 
            "--fileName", "testworkdir" + File.separator + "test.asn",
            "-ns", "test.org.bn.coders.test_asn"
        });
    }
    
}
