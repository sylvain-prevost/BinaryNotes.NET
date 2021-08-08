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
	public class BitArrayOutputStreamTest
	{
		
		public BitArrayOutputStreamTest(System.String sTestName)
		{
		}
		
		/// <seealso cref="BitArrayOutputStream.write(int)">
		/// </seealso>
		public virtual void  testWrite()
		{
			BitArrayOutputStream stream = new BitArrayOutputStream();
			stream.WriteByte(0xFF);
			stream.writeBit(true);
			stream.writeBit(false);
			stream.writeBit(true);
			stream.writeBit(false);
			stream.WriteByte(0xFF);
			stream.WriteByte(0x0F);
			stream.writeBit(true);
			stream.writeBit(true);
			stream.writeBit(true);
			stream.writeBit(true);
			System.Console.Out.WriteLine("Write " + ByteTools.byteArrayToHexString(stream.ToArray()));
            ByteTools.checkBuffers(stream.ToArray(), new byte[] { 0xFF, 0xAF, 0xF0, 0xFF });
			
			stream.writeBit(true);
			stream.writeBit(false);
			stream.writeBit(true);
			stream.writeBit(false);
			byte[] temp_byteArray;
			temp_byteArray = new byte[]{ (0xCC),  (0xFF),  (0xFF),  (0xBB)};
			stream.Write(temp_byteArray, 0, temp_byteArray.Length);
			System.Console.Out.WriteLine("After buf write " + ByteTools.byteArrayToHexString(stream.ToArray()));
			ByteTools.checkBuffers(stream.ToArray(), new byte[]{ (0xFF),  (0xAF),  (0xF0),  (0xFF),  (0xAC),  (0xCF),  (0xFF),  (0xFB),  (0xB0)});
			stream.align();
			stream.writeBit(true);
			stream.writeBit(true);
			stream.align();
			stream.WriteByte(0xFF);
			
			System.Console.Out.WriteLine("After align " + ByteTools.byteArrayToHexString(stream.ToArray()));
			ByteTools.checkBuffers(stream.ToArray(), new byte[]{ (0xFF),  (0xAF),  (0xF0),  (0xFF),  (0xAC),  (0xCF),  (0xFF),  (0xFB),  (0xB0),  (0xC0),  (0xFF)});
		}
	}
}