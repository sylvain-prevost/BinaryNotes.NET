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
using System;
using System.Collections.Generic;
using System.Text;

namespace org.bn.types
{
    public class BitString
    {
        private byte[] bitStrValue = new byte[0];

        public byte[] Value
        {
            get { return bitStrValue; }
            set { bitStrValue = value; }
        }
        private int trailBitsCnt = 0; // count of buffer bit's trail

        public int TrailBitsCnt
        {
            get { return trailBitsCnt; }
            set { trailBitsCnt = value; }
        }

        public BitString()
        {
        }

        public BitString(BitString src)
        {
            this.Value = src.Value;
            this.TrailBitsCnt = src.getTrailBitsCnt();
        }

        public BitString(byte[] bitStrValue)
        {
            this.Value = bitStrValue;
        }

        public BitString(byte[] bitStrValue, int trailBitsCnt)
        {
            this.Value = bitStrValue;
            this.TrailBitsCnt = trailBitsCnt;
        }

        public int getLength()
        {
            return this.Value.Length;
        }

        public int getTrailBitsCnt()
        {
            return this.TrailBitsCnt;
        }

    	public int getLengthInBits() {
	    return getLength()*8 - getTrailBitsCnt();
    	}
    }
}
