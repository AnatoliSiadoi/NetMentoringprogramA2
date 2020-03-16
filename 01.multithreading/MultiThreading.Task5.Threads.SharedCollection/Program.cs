/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private const int NumberCount = 10;
        private static readonly List<int> sharedCollection = new List<int>();
        private static readonly AutoResetEvent readyToWriteEvent = new AutoResetEvent(true);
        private static readonly AutoResetEvent readyToPrintEvent = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            var cts = new CancellationTokenSource();
            var ct = cts.Token;

            var writer = Task.Run(() =>
                {
                    var i = 0;
                    while (i <= NumberCount)
                    {
                        readyToWriteEvent.WaitOne();
                        sharedCollection.Add(i++);
                        readyToPrintEvent.Set();
                    }

                    cts.Cancel();
                }, ct);

            var printer = Task.Run(() =>
                {
                    while (true)
                    {
                        readyToPrintEvent.WaitOne();
                        var sb = new StringBuilder();
                        foreach (var item in sharedCollection)
                        {
                            sb.Append(item);
                            sb.Append(" ");
                        }

                        Console.WriteLine(sb.ToString());
                        readyToWriteEvent.Set();
                    }
                }, ct);

            Task.WaitAll(writer, printer);
            Console.ReadLine();
        }
    }
}
