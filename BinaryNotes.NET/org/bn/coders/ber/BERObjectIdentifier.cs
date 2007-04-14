/*
* Copyright 2007 Abdulla G. Abdurakhmanov (abdulla.abdurakhmanov@gmail.com).
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

namespace org.bn.coders.ber
{
    /**
     * BER OID Encoding
     * Implemented by Alan Gutzeit.
     */
    static class BERObjectIdentifier
    {
        public static byte[] Encode(int[] oidArcArray)
        {
            int arcLength = oidArcArray.Length;
            if (arcLength < 2) throw new Exception("oidArcArray length below 2");
            byte[] result = new byte[(arcLength * 5)]; // 32-bit encoding cannot exceed 5 bytes each 
            int nextAvailable = 0;
            nextAvailable += EncodeFirstTwoArcs(oidArcArray[0], oidArcArray[1], result, nextAvailable);
            for (int i = 2; i < arcLength; i++)
            {
                nextAvailable += EncodeOneArc(oidArcArray[i], result, nextAvailable);
            }
            if (nextAvailable > 255) throw new Exception("Encoded length cannot exceed 255");
            return Truncate(result, nextAvailable);
        }

        private static int EncodeFirstTwoArcs(int topArc, int secondArc, byte[] result, int nextAvailable)
        {
            if (topArc < 0 || topArc > 2) throw new Exception("Top arc must be less than 3");
            int combinedArc = topArc * 40 + secondArc;
            return EncodeOneArc(combinedArc, result, nextAvailable);
        }

        // Encoding: 
        //         11112222333344445555666677778888 
        // 00001111x2222333x3444455x5566667x7778888

        // Decoding: 
        // xxxx1111x2222333x3444455x5566667x7778888
        //         11112222333344445555666677778888 
        //

        /// <summary>
        /// Adds encoding to passed result array. Note that result array must already have adequate capacity. 
        /// </summary>
        /// <returns>length of result</returns>
        private static int EncodeOneArc(int arc, byte[] result, int nextAvailable)
        {
            if (arc < 1) throw new Exception("arc must be greater then zero");

            long arc1 = (arc & 0x7f);
            long arc2 = (arc & 0x3f80) << 1;
            long arc3 = (arc & 0x1fc000) << 2;
            long arc4 = (arc & 0xfe00000) << 3;
            long arc5 = (arc & 0xf0000000) << 4;
            long all = arc1 | arc2 | arc3 | arc4 | arc5;

            byte[] temp = new byte[5]; 
            temp[4] = (byte)((all & 0xff));
            temp[3] = (byte)((all & 0xff00) >> 8);
            temp[2] = (byte)((all & 0xff0000) >> 16);
            temp[1] = (byte)((all & 0xff000000) >> 24);
            temp[0] = (byte)((all & 0xff00000000) >> 32);

            int resultLength = 0;
            if (temp[0] > 0) resultLength = 5;            
            else if (temp[1] > 0) resultLength = 4;
            else if (temp[2] > 0) resultLength = 3;
            else if (temp[3] > 0) resultLength = 2;
            else if (temp[4] > 0) resultLength = 1;
            if (resultLength < 1) throw new Exception("no result");

            // all bytes have high-order bit one except last byte has high-order bit zero 
            temp[0] |= 0x80; // high-bit set
            temp[1] |= 0x80; // high-bit set
            temp[2] |= 0x80; // high-bit set
            temp[3] |= 0x80; // high-bit set
            temp[4] &= 0x7f; // high-bit reset

            int sourceIndex = 5 - resultLength;
            System.Array.Copy(temp, sourceIndex, result, nextAvailable, resultLength);
            return resultLength; 
        }

        // return new array by truncating passed array.
        private static byte[] Truncate(byte[] b1, int nextAvailable)
        {
            byte[] b2 = new byte[nextAvailable];
            if (nextAvailable == 0) return b2;
            for (int i = 0; i < nextAvailable; i++)
            {
                b2[i] = b1[i];
            }
            return b2;
        }
    }
}
