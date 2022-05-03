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

            Console.WriteLine("1. No monitor");
            Console.WriteLine("2. With monitor");
            Thread producer;
            Thread consumer;
            switch (Console.ReadKey(true).KeyChar)
            {
                case '1':
                    producer = new Thread(Producer);
                    consumer = new Thread(Consumer);

                    producer.Start();
                    consumer.Start();
                    break;
                case '2':
                    producer = new Thread(ProducerMonitor);
                    consumer = new Thread(ConsumerMonitor);

                    producer.Start();
                    consumer.Start();
                    break;
            }

            while (keepRunning)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    keepRunning = false;
                }
            }

        }
        #region No Monitor
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
        #endregion

        #region With Monitor
        static void ProducerMonitor()
        {
            while (keepRunning)
            {
                if (Monitor.TryEnter(products))
                {

                    if (products.Count < 3)
                    {
                        products.Enqueue(new Product());
                        Console.WriteLine("Producer produces, inventory: {0}", products.Count);
                        Monitor.Exit(products);
                    }
                    else
                    {
                        Console.WriteLine("Producer waits...");
                        Monitor.PulseAll(products);
                        Monitor.Wait(products);
                    }
                }
                Thread.Sleep(100 / 15);
            }
        }


        static void ConsumerMonitor()
        {
            while (keepRunning)
            {
                if (Monitor.TryEnter(products))
                {

                    if (products.Count > 0)
                    {
                        while (products.Count > 0)
                        {
                            products.Dequeue();
                            Console.WriteLine("Consumer consumes, inventory: {0}", products.Count);
                            Thread.Sleep(100 / 15);
                        }
                        Monitor.Exit(products);
                    }
                    else
                    {
                        Console.WriteLine("Consumer waits...");
                        Monitor.PulseAll(products);
                        Monitor.Wait(products);
                    }

                }
                Thread.Sleep(100 / 15);


            }
        }

        #endregion
    }
}
