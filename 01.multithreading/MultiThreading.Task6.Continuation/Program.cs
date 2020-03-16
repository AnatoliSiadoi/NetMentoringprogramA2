﻿/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            var cts = new CancellationTokenSource();
            var ct = cts.Token;
            var i = 1;

            cts.Cancel();

            try
            {
                var mainTask = Task.Run(() =>
                {
                    if (i == 0) throw new ArgumentException();
                }, ct);

                var b = mainTask.ContinueWith(parent =>
                {
                    Console.WriteLine("Task was completed success");
                },
                    TaskContinuationOptions.OnlyOnRanToCompletion);

                var c = mainTask.ContinueWith(parent =>
                {
                    Console.WriteLine("Task was failure");
                },
                    TaskContinuationOptions.OnlyOnFaulted);

                var d = mainTask.ContinueWith(parent =>
                {
                    Console.WriteLine("Task was canceled");
                },
                    TaskContinuationOptions.DenyChildAttach | TaskContinuationOptions.OnlyOnCanceled);

                Task.WaitAll(mainTask, b, c, d);
            }
            catch (AggregateException e)
            {
                Console.WriteLine($"From exception: {e.Message}");
            }

            Console.ReadLine();
        }
    }
}
