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
using org.bn.mq.protocol;
using org.bn;

namespace org.bn.mq.net
{

    public class ASN1TransportMessageCoder : ITransportMessageCoder
    {
        protected internal IEncoder encoder;
        protected internal int coderSchemeDefVal = (short)0x0101;

        protected internal const byte coderVersion = 0x10;
        protected internal const int headerSize = 4 + 2 + 1; // length packet + coder schema + coderVersion;
        protected internal IDictionary<int, IDecoder> coderSchemaMap = new Dictionary<int, IDecoder>();
        protected internal System.IO.MemoryStream outputByteStream = new System.IO.MemoryStream();

        
        protected internal bool headerIsReaded = false;
        protected internal int crDecodedSchema = 0;
        protected internal byte crDecodedVersion = 0;
        protected internal int crDecodedLen = 0;

        protected internal ByteBuffer currentDecoded = null;
        protected ByteBuffer bufferOne = ByteBuffer.allocate(65535);
        protected ByteBuffer bufferTwo = ByteBuffer.allocate(65535);
        private int currentBufferIdx = 0;


        public ASN1TransportMessageCoder()
        {
            try
            {
                switchStaticBuffer();
                coderSchemaMap.Add(0x0000, CoderFactory.getInstance().newDecoder("BER"));
                coderSchemaMap.Add(0x0100, CoderFactory.getInstance().newDecoder("PER"));
                coderSchemaMap.Add(0x0101, CoderFactory.getInstance().newDecoder("PER/U"));
                IEncoder defEnc = CoderFactory.getInstance().newEncoder("PER/U");
                setDefaultEncoder(coderSchemeDefVal, defEnc);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public virtual void setDefaultEncoder(int coderSchemaVal, IEncoder encoder)
        {
            this.coderSchemeDefVal = coderSchemaVal;
            this.encoder = encoder;
        }

        public virtual ByteBuffer encode(MessageEnvelope message)
        {
            outputByteStream.Position = 0; //reset();
            encoder.encode<MessageEnvelope>(message, outputByteStream);
            ByteBuffer buffer = ByteBuffer.allocate((int)outputByteStream.Length + headerSize);
            buffer.putShort((short)coderSchemeDefVal);
            buffer.put(coderVersion);
            buffer.putInt((int)outputByteStream.Length);
            buffer.put(outputByteStream.ToArray());
            buffer.Position = 0;
            return buffer;
        }

        /*internal static System.String byteArrayToHexString(byte[] buffer)
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
        }*/


        public virtual IList<MessageEnvelope> decode(ByteBuffer buffer)
        {
            lock (this)
            {
                //Console.WriteLine("+++ Received " +buffer.Limit+ " bytes nBID:@" + byteArrayToHexString(buffer.Value));

                try
                {
                    IList<MessageEnvelope> result = new List<MessageEnvelope>();
                    int readedBytes = buffer.Limit;
                    if (currentDecoded.Remaining < readedBytes)
                    {
                        currentDecoded.growTo(readedBytes + 65535);
                    }
                    currentDecoded.put(buffer.Value, 0, buffer.Limit);

                    bool decodedMessage = false;
                    do
                    {
                        decodedMessage = false;
                        // if header presents
                        if (currentDecoded.Limit > headerSize && !headerIsReaded)
                        {
                            int savePos = currentDecoded.Position;
                            currentDecoded.Position = 0;
                            crDecodedSchema = currentDecoded.getShort();
                            crDecodedVersion = currentDecoded.get();
                            crDecodedLen = currentDecoded.getInt();
                            headerIsReaded = true;
                            currentDecoded.Position = savePos;
                        }

                        if (headerIsReaded)
                        {
                            IDecoder decoder = coderSchemaMap[crDecodedSchema];
                            if (crDecodedLen <= currentDecoded.Position - headerSize)
                            {
                                currentDecoded.Position = headerSize;
                                byte[] content = new byte[crDecodedLen];
                                currentDecoded.get(content);
                                //currentDecoded = currentDecoded.slice();
                                MessageEnvelope decodedObj = decoder.decode<MessageEnvelope>(new System.IO.MemoryStream(content));
                                //Console.WriteLine("--- Decoded to "+result!=null ? result.Id:"null"+"nBID:@" + byteArrayToHexString(buffer.Value) );

                                headerIsReaded = false;

                                if (decodedObj != null )
                                {
                                    result.Add(decodedObj);
                                }

                                if (currentDecoded.Limit > currentDecoded.Position)
                                {
                                    byte[] data = currentDecoded.Value;
                                    int currentPosition = currentDecoded.Position;
                                    int curLimit = currentDecoded.Limit;
                                    switchStaticBuffer();// ByteBuffer.allocate(data.length);
                                    currentDecoded.put(data, currentPosition, curLimit - currentPosition);
                                    //currentDecoded.Position = 0;
                                    decodedMessage = true;
                                }
                                else
                                {
                                    currentDecoded.clear();
                                }
                            }
                        }
                    }
                    while (decodedMessage);
                    return result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Decode problem!: "+ex.ToString());
                    throw ex;
                }
            }
        }

        private void switchStaticBuffer()
        {
            if (currentBufferIdx == 0)
            {
                currentBufferIdx++;
                bufferTwo.clear();
                currentDecoded = bufferTwo;
            }
            else
            {
                currentBufferIdx = 0;
                bufferOne.clear();
                currentDecoded = bufferOne;
            }
        }
    }
}