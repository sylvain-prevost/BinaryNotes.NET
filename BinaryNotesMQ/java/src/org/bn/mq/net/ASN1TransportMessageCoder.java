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

package org.bn.mq.net;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;

import java.nio.ByteBuffer;

import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

import org.bn.CoderFactory;
import org.bn.IDecoder;
import org.bn.IEncoder;
import org.bn.mq.net.ITransportMessageCoder;
import org.bn.mq.protocol.MessageEnvelope;

public class ASN1TransportMessageCoder implements ITransportMessageCoder {
    protected IEncoder<MessageEnvelope> encoder;
    protected int coderSchemeDefVal = (short)0x0101;
    
    protected final byte coderVersion = 0x10;
    protected final int headerSize = 4 + 2 + 1; // length packet + coder schema + coderVersion;
    protected final HashMap<Integer,IDecoder> coderSchemaMap = new HashMap<Integer, IDecoder>();
    
    protected ByteArrayOutputStream outputByteStream = new ByteArrayOutputStream();
    
    protected ByteBuffer currentDecoded = null;
    
    protected boolean headerIsReaded = false;
    protected int crDecodedSchema = 0;
    protected byte crDecodedVersion = 0;
    protected int crDecodedLen = 0;
    protected int currentBufferIdx = 0;
    protected ByteBuffer bufferOne =  ByteBuffer.allocate(65535);
    protected ByteBuffer bufferTwo =  ByteBuffer.allocate(65535);
    
    
    public ASN1TransportMessageCoder() {
        try {
            switchStaticBuffer();
            coderSchemaMap.put(0x0000, CoderFactory.getInstance().newDecoder("BER"));
            coderSchemaMap.put(0x0100, CoderFactory.getInstance().newDecoder("PER"));
            coderSchemaMap.put(0x0101, CoderFactory.getInstance().newDecoder("PER/U"));
            IEncoder<MessageEnvelope> defEnc =  CoderFactory.getInstance().newEncoder("PER/U");
            setDefaultEncoder(coderSchemeDefVal,defEnc);
            
        }
        catch (Exception e) {
            // TODO
        }
        
    }
    
    public void setDefaultEncoder(int coderSchemaVal, IEncoder<MessageEnvelope> encoder) {    
        this.coderSchemeDefVal = coderSchemaVal;
        this.encoder = encoder;
    }

    public ByteBuffer encode(MessageEnvelope message) throws Exception {
        outputByteStream.reset();
        encoder.encode(message, outputByteStream);
        ByteBuffer buffer = ByteBuffer.allocate(outputByteStream.size()+headerSize);        
        buffer.putShort((short)coderSchemeDefVal);
        buffer.put(coderVersion);
        buffer.putInt(outputByteStream.size());
        buffer.put(outputByteStream.toByteArray());
        buffer.position(0);
        return buffer;
    }

    public List<MessageEnvelope> decode(ByteBuffer buffer)  throws Exception {
        List<MessageEnvelope> result = new LinkedList<MessageEnvelope>();
        int readedBytes = buffer.limit();
        if(currentDecoded.remaining()<readedBytes) {
            byte[] data = currentDecoded.array();
            currentDecoded = ByteBuffer.allocate(data.length+65535 + readedBytes);
            currentDecoded.put(data);
        }
        currentDecoded.put(buffer.array(),currentDecoded.position(),buffer.limit());
        int lastCurrentDecodedPost = currentDecoded.position();
        
        boolean decodedMessage = false;
        do
        {
            decodedMessage = false;
            // if header presents
            if(lastCurrentDecodedPost > headerSize && !headerIsReaded) {
                currentDecoded.position(0);
                crDecodedSchema = currentDecoded.getShort();
                crDecodedVersion = currentDecoded.get();
                crDecodedLen = currentDecoded.getInt();
                headerIsReaded = true;
            }
            
            if(headerIsReaded) {
                IDecoder decoder = coderSchemaMap.get(crDecodedSchema);
                if(crDecodedLen <= lastCurrentDecodedPost - headerSize ) {
                    currentDecoded.position(headerSize);
                    byte[] content = new byte[crDecodedLen];
                    currentDecoded.get(content);
                    //currentDecoded = 
                    MessageEnvelope decodedObj = decoder.decode(new ByteArrayInputStream(content),MessageEnvelope.class);       
                    if(decodedObj!=null)
                        result.add(decodedObj);
                    headerIsReaded = false;
                    
                    if(lastCurrentDecodedPost > currentDecoded.position() ) {
                        byte[] data = currentDecoded.array();
                        int currentPosition = currentDecoded.position();
                        switchStaticBuffer();// ByteBuffer.allocate(data.length);
                        currentDecoded.put(data,currentPosition,lastCurrentDecodedPost - currentPosition);
                        lastCurrentDecodedPost = currentDecoded.position();
                        currentDecoded.position(0);
                        
                        if(decodedObj!=null)
                            decodedMessage = true;
                    }
                    else {
                        currentDecoded.clear();
                    }
                }
            }            
        }
        while(decodedMessage);
        return result;
    }
    
    private void switchStaticBuffer() {
        if(currentBufferIdx == 0 ) {
            currentBufferIdx++;
            bufferTwo.clear();            
            currentDecoded =  bufferTwo;
        }
        else {
            currentBufferIdx = 0;
            bufferOne.clear();            
            currentDecoded = bufferOne;            
        }
    }
}
