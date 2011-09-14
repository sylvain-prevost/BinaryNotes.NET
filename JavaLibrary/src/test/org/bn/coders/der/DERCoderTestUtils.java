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
 package test.org.bn.coders.der;

import test.org.bn.coders.ber.BERCoderTestUtils;

public class DERCoderTestUtils extends BERCoderTestUtils {
    public DERCoderTestUtils() {
    }


    public byte[] createSetBytes() {
        return new byte[] { 0x31,0x10, (byte)0x81, 0x04,0x61,0x61,0x61,0x61, (byte)0x82, 0x02,0x00, (byte)0xAA, (byte)0x83, 
                0x04,0x62,0x62,0x62,0x62 } ;
    }
}
