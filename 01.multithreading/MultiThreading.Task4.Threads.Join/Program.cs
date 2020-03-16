/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private const int ThreadCount = 10;
        private static Semaphore semaphore = new Semaphore(1, 1);

        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            WorkerA(ThreadCount);
            WorkerB(ThreadCount);

            Console.ReadLine();
        }

        private static void WorkerA(int number)
        {
            var numb = number - 1;

            if (numb >= 0)
            {
                Console.WriteLine(numb);
                var th = new Thread(() => WorkerA(numb));
                th.Start();
                th.Join();
            }
        }

        private static void WorkerB(int number)
        {
            var numb = number - 1;

            if (numb >= 0)
            {
                Console.WriteLine(numb);
                ThreadPool.QueueUserWorkItem(cal => WorkerB(numb));
                semaphore.WaitOne();
            }
        }
    }
}
