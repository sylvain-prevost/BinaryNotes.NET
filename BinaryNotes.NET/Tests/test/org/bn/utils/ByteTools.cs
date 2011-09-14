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

namespace test.org.bn.utils
{
	
	public class ByteTools
	{
        internal static System.String byteArrayToHexString(byte[] buffer)
        {
            byte ch = (byte)(0x00);
            int i = 0;

            if (buffer == null || buffer.Length <= 0)
                return null;
            System.String[] pseudo = new System.String[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
            System.Text.StringBuilder out_Renamed = new System.Text.StringBuilder(buffer.Length * 3);
            while (i < buffer.Length)
            {
                ch = (byte)(buffer[i] & 0xF0);
                ch = (byte)(ch >> 4);
                ch = (byte)(ch & 0x0F);
                out_Renamed.Append(pseudo[(int)ch]);
                ch = (byte)(buffer[i] & 0x0F);
                out_Renamed.Append(pseudo[(int)ch]);
                i++;
                if (i != buffer.Length)
                    out_Renamed.Append("-");
            }

            System.String rslt = new System.String(out_Renamed.ToString().ToCharArray());
            return rslt;
        }
		
		public static void  checkBuffers(byte[] src, byte[] standard)
		{
			Assert.Equals(src.Length, standard.Length);
			for (int i = 0; i < src.Length; i++)
			{
                Assert.Equals(src[i], standard[i]);
			}
		}
		
		public static System.String byteArrayToHexString(System.IO.MemoryStream stream)
		{
			return byteArrayToHexString(stream.ToArray());
		}
	}
}