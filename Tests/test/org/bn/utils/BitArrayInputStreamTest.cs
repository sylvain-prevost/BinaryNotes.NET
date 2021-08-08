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
using csUnit;
using org.bn.utils;

namespace test.org.bn.utils
{
	[TestFixture]
	public class BitArrayInputStreamTest
	{
		
		public BitArrayInputStreamTest(System.String sTestName)
		{
		}
		
		protected internal virtual BitArrayInputStream newStream()
		{
			System.IO.MemoryStream btStream = new System.IO.MemoryStream( new byte[] {0xAB,0xCD, 0xEF,0x33, 0xFE, 0xDC, 0xBA });
			BitArrayInputStream stream = new BitArrayInputStream(btStream);
			return stream;
		}
		
		/// <seealso cref="BitArrayInputStream.read()">
		/// </seealso>
		public virtual void  testRead()
		{
			BitArrayInputStream stream = newStream();
			int bt = stream.ReadByte();
			Assert.Equals(0xAB, bt);
			bt = stream.ReadByte();
			Assert.Equals(0xCD, bt);
			bt = stream.readBit();
			Assert.Equals(1, bt);
			bt = stream.readBit();
			Assert.Equals(1, bt);
			bt = stream.ReadByte();
			Assert.Equals(0xBC, bt);
			bt = stream.ReadByte();
			Assert.Equals(0xCF, bt);
			stream.skipUnreadedBits();
			bt = stream.ReadByte();
			Assert.Equals(0xDC, bt);
			bt = stream.readBits(4);
			Assert.Equals(0x0B, bt);
			bt = stream.readBits(4);
			Assert.Equals(0x0A, bt);
		}
	}
}