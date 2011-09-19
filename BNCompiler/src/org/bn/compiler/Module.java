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

package org.bn.compiler;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.InputStream;

import java.util.ArrayList;

import javax.xml.transform.ErrorListener;
import javax.xml.transform.Transformer;
import javax.xml.transform.TransformerConfigurationException;
import javax.xml.transform.TransformerException;
import javax.xml.transform.TransformerFactory;
import javax.xml.transform.stream.StreamResult;
import javax.xml.transform.stream.StreamSource;

public class Module {
    private File[] moduleFiles = null;
    private String moduleName, outputDir, modulesPath;


    public Module(String modulesPath, String name, String outputDir)
            throws Exception {
        setModuleName(name);
        setOutputDir(outputDir);
        setModulesPath(modulesPath);
        loadTransformations();
    }

    private File createOutputFileForInput(File input) {
        String fileName = input.getName().substring(0,
                              input.getName().lastIndexOf(".")) + "."
                                  + getModuleName();
        File result = new File(getOutputDir(), fileName);

        return result;
    }

    private void loadTransformations() throws Exception {
        File basePath = new File(getModulesPath() + File.separator
                                 + getModuleName());

        if (basePath.isDirectory()) {
            moduleFiles = basePath.listFiles();
        } else {
            throw new FileNotFoundException("Modules must be directory!");
        }
    }

    public void translate(InputStream stream) throws Exception {
        TransformerFactory factory = TransformerFactory.newInstance();

        for (File file : moduleFiles) {
            if (file.isFile()) {
                Transformer transformer =
                    factory.newTransformer(new StreamSource(file));

                transformer.setErrorListener(new ErrorListener() {
                    public void warning(TransformerException exception) {
                        System.err.println("[W] Warning:" + exception);
                    }
                    public void error(TransformerException exception) {
                        System.err.println("[!] Error:" + exception);
                    }
                    public void fatalError(TransformerException exception) {
                        System.err.println("[!!!] Fatal error:" + exception);
                    }
                });

                File outputFile = createOutputFileForInput(file);

                transformer.transform(new StreamSource(stream),
                                      new StreamResult(outputFile));
            }
        }
    }

    public String getModuleName() {
        return moduleName;
    }

    public String getModulesPath() {
        return modulesPath;
    }

    public String getOutputDir() {
        return outputDir;
    }

    private void setModuleName(String moduleName) {
        this.moduleName = moduleName;
    }

    private void setModulesPath(String modulesPath) {
        this.modulesPath = modulesPath;
    }

    private void setOutputDir(String outputDir) {
        this.outputDir = outputDir;
    }
}

