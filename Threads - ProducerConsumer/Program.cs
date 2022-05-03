using System;
using System.Threading;


namespace Threads___ProducerConsumer
{
    public static class Program
    {
        public static Queue<Product> products = new Queue<Product> { };

        public static bool keepRunning = true;

        public static Random random = new Random();
        static void Main()
        {
            Thread producer = new Thread(Producer);
            Thread consumer = new Thread(Consumer);

            producer.Start();
            consumer.Start();


            while (keepRunning)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    keepRunning = false;
                }
            }

        }

        static void Producer()
        {
            int count = 0;
            while (keepRunning)
            {
                if (products.Count < 3)
                {
                    products.Enqueue(new Product());
                    Console.WriteLine("Producer produces, inventory: {0}", products.Count);
                    count = 0;
                }
                else
                {
                    count++;
                    Console.WriteLine("Producer can't produce");
                }

                if (count > 3)
                {
                    Thread.Sleep(3000);
                }
                Thread.Sleep(100 / 15);
            }
        }

        static void Consumer()
        {
            int count = 0;
            while (keepRunning)
            {
                if (products.Count > 0)
                {
                    while (products.Count > 0)
                    {
                        products.Dequeue();
                        Console.WriteLine("Consumer consumes, inventory: {0}", products.Count);
                        Thread.Sleep(100 / 15);
                        count = 0;
                    }
                }
                else
                {
                    count++;
                    Console.WriteLine("Consumer could not consume");
                }

                Thread.Sleep(2000);


            }
        }
    }
}
