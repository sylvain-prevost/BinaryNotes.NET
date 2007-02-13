package test.org.bn.utils;

import java.io.ByteArrayInputStream;

import java.io.IOException;

import junit.framework.TestCase;

import org.bn.utils.BitArrayInputStream;

import test.org.bn.AllTests;

public class BitArrayInputStreamTest extends TestCase {

    public BitArrayInputStreamTest(String sTestName) {
        super(sTestName);
    }
    
    protected BitArrayInputStream newStream() {
        ByteArrayInputStream btStream = new ByteArrayInputStream( 
                    new byte[] { (byte)0xAB, (byte)0xCD, (byte)0xEF, 
                    0x33, (byte)0xFE, (byte)0xDC, (byte)0xBA } 
        );
        BitArrayInputStream stream = new BitArrayInputStream(btStream);
        return stream;
    }

    /**
     * @see BitArrayInputStream#read()
     */
    public void testRead() throws IOException {
        BitArrayInputStream stream = newStream();
        int bt = stream.read();
        assertEquals( 0xAB, bt );
        bt = stream.read();
        assertEquals( 0xCD, bt );
        bt = stream.readBit();
        assertEquals( 1, bt );
        bt = stream.readBit();
        assertEquals( 1, bt );
        bt = stream.read();
        assertEquals( 0xBC, bt );
        bt = stream.read();
        assertEquals( 0xCF, bt );
        stream.skipUnreadedBits();
        bt = stream.read();
        assertEquals( 0xDC, bt );
        bt = stream.readBits(4);
        assertEquals( 0x0B, bt );        
        bt = stream.readBits(4);
        assertEquals( 0x0A, bt ); 
    }
}
