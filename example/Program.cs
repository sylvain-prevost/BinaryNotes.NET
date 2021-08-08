
using System;
using org.bn;
using System.IO;

using FooProtocolExample;

namespace example
{
    class Program
    {
        static void DecodeExample()
        {
            Console.WriteLine("Decoding example");

            IDecoder decoder = CoderFactory.getInstance().newDecoder("DER");

            // asn1 data obtained from wikipedia asn.1 example
            byte[] asn1Data = new byte[] { 0x30, 0x13, 0x02, 0x01, 0x05, 0x16, 0x0e, 0x41, 0x6e, 0x79, 0x62, 0x6f, 0x64, 0x79, 0x20, 0x74, 0x68, 0x65, 0x72, 0x65, 0x3f };

            using (MemoryStream memoryStream = new MemoryStream(asn1Data))
            {
                // request decoding of memorystream using FooQuestion class
                FooQuestion fooQuestion = decoder.decode<FooQuestion>(memoryStream);

                // Display content of FooQuestion object
                Console.WriteLine("\tDecoded trackingNumber : {0}", fooQuestion.TrackingNumber);
                Console.WriteLine("\tDecoded question : {0}", fooQuestion.Question);                
            }
        }

        static void EncodeExample()
        {
            Console.WriteLine("Encoding example");

            IEncoder encoder = CoderFactory.getInstance().newEncoder("DER");

            using (MemoryStream memoryStream = new MemoryStream())
            {
                // create FooQuestion object and populate it
                FooQuestion fooQuestion = new FooQuestion();
                fooQuestion.TrackingNumber = 5;
                fooQuestion.Question = "Anybody there?";

                // request DER encoding
                encoder.encode<FooQuestion>(fooQuestion, memoryStream);
                byte[] result = memoryStream.ToArray();

                // display result
                Console.WriteLine("\tDER encoded result : {0}", BitConverter.ToString(result).Replace("-", " "));
            }
        }

        static void Main()
        {
            // wikipedia example: https://en.wikipedia.org/wiki/ASN.1#Example

            DecodeExample();

            EncodeExample();
        }
    }
}
