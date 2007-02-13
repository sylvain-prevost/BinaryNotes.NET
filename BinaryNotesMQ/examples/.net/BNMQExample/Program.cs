using System;
using System.Collections.Generic;
using System.Text;

namespace BNMQExample
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Please specified type: server/client!\n");
            }
            else
            {
                if (args[0].Equals("client"))
                {
                    new org.bn.mq.examples.BNMQConsumer().start();
                }
                else
                if (args[0].Equals("server"))
                {
                    new org.bn.mq.examples.BNMQSupplier().start();
                }

            }
        }
    }
}
