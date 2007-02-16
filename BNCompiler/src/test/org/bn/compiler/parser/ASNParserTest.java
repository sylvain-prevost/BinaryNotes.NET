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
package test.org.bn.compiler.parser;

import junit.framework.TestCase;

import org.bn.compiler.parser.ASNLexer;
import org.bn.compiler.parser.ASNParser;
import org.bn.compiler.parser.model.ASN1Model;
import org.bn.compiler.parser.model.ASNModule;

//~--- JDK imports ------------------------------------------------------------

import java.io.File;
import java.io.FileOutputStream;
import java.io.InputStream;

import javax.xml.bind.JAXBContext;
import javax.xml.bind.Marshaller;

public class ASNParserTest extends TestCase {
    public ASNParserTest(String sTestName) {
        super(sTestName);
    }

    private ASN1Model createFromStream() throws Exception {
        InputStream stream = getClass().getResourceAsStream(
                                 "/test/org/bn/compiler/parser/test.asn");
        ASNLexer  lexer  = new ASNLexer(stream);
        ASNParser parser = new ASNParser(lexer);
        ASNModule module = new ASNModule();

        parser.module_definition(module);

        ASN1Model model = new ASN1Model();

        model.module = module;

        return model;
    }

    public void testJaxb() throws Exception {
        JAXBContext jc =
            JAXBContext.newInstance("org.bn.compiler.parser.model");
        Marshaller marshaller = jc.createMarshaller();
        ASN1Model  model      = createFromStream();

        model.runtimeArguments = new String[] { "-inputFileName", "test.asn" };
        model.moduleDirectory  = "modules" + File.separator + "java";
        model.outputDirectory  = "output";
        model.moduleNS = "test_asn";
        marshaller.setProperty(Marshaller.JAXB_FORMATTED_OUTPUT, Boolean.TRUE);
        marshaller.marshal(model, System.out);

        marshaller.marshal(model,new FileOutputStream("temp.xml"));
    }

    /**
     * @see ASNParser#module_definition(ASNModule)
     */
    public void testModule_definition() throws Exception {
        ASN1Model model = createFromStream();

        assertEquals(model.module.moduleIdentifier.name, "TEST_ASN");
        assertEquals(model.module.asnTypes.sequenceSets.size(), 17);
        assertEquals(model.module.asnTypes.enums.size(), 2);
        assertEquals(model.module.asnTypes.characterStrings.size(), 6);
        assertEquals(model.module.asnTypes.octetStrings.size(), 1);
        assertEquals(model.module.asnTypes.sequenceSetsOf.size(), 7);
    }
}
