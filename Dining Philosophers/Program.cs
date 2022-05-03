using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace v3
{
    internal class Program
    {
        private static bool[] forks = new bool[] { false, false, false, false, false };
        private static readonly object locker = new object();
        private static Random rnd = new Random();

        static void Main(string[] args)
        {
            Thread p0 = new Thread(Eat);
            Thread p1 = new Thread(Eat);
            Thread p2 = new Thread(Eat);
            Thread p3 = new Thread(Eat);
            Thread p4 = new Thread(Eat);

            p0.Start(0);
            p1.Start(1);
            p2.Start(2);
            p3.Start(3);
            p4.Start(4);

            p0.Join();
            p1.Join();
            p2.Join();
            p3.Join();
            p4.Join();
        }
        static void Eat(object i)
        {
            while (true)
            {
                int id = Convert.ToInt32(i);

                bool hasBeenEating = false;
                int forkRight = id;
                int forkLeft = (id + 1) % 5;


                while (hasBeenEating == false)
                {
                    Monitor.Enter(locker);

                    if (forks[forkRight] == false && forks[forkLeft] == false)
                    {
                        try
                        {
                            forks[forkRight] = true;
                            forks[forkLeft] = true;
                            Console.WriteLine($"Philosopher{id} is eating...");
                            break;
                        }
                        finally
                        {
                            Monitor.Exit(locker);

                            Thread.Sleep(rnd.Next(3000, 5000));

                            hasBeenEating = true;
                            forks[forkRight] = false;
                            forks[forkLeft] = false;

                        }
                    }
                    else
                    {
                        try
                        {
                            Console.WriteLine($"Philosopher{id} is waiting...");
                        }
                        finally
                        {
                            Monitor.Exit(locker);
                            Thread.Sleep(rnd.Next(3000, 6000));
                        }
                    }
                }
                while (hasBeenEating == true)
                {
                    Console.WriteLine($"Philosopher{id} is thinking...");
                    Thread.Sleep(rnd.Next(3000, 6000));
                    break;
                }
            }
        }
    }
}
