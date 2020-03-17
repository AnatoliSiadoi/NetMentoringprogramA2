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

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            try
            {
                var mainTaskA = Task.Run(() =>
                {
                    Console.WriteLine($"Main task A thread : {Thread.CurrentThread.ManagedThreadId}");
                    //throw new Exception();
                }).ContinueWith(t => { Console.WriteLine("Task A was completed success."); });

                var mainTaskB = Task.Run(() =>
                {
                    Console.WriteLine($"Main task B thread : {Thread.CurrentThread.ManagedThreadId}");
                    throw new Exception();
                }).ContinueWith(t => { Console.WriteLine("Task B was completed success."); }, TaskContinuationOptions.NotOnRanToCompletion);

                var mainTaskC = Task.Run(() =>
                {
                    Console.WriteLine($"Main task C thread : {Thread.CurrentThread.ManagedThreadId}");
                    throw new Exception();
                }).ContinueWith(t => { Console.WriteLine($"Task C thread : {Thread.CurrentThread.ManagedThreadId}"); }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

                var mainTaskD = Task.Run(() =>
                {
                    Console.WriteLine($"Main task D thread : {Thread.CurrentThread.ManagedThreadId}");
                }, token).ContinueWith(t => { Console.WriteLine($"Task D thread : {Thread.CurrentThread.ManagedThreadId}"); }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);

                tokenSource.Cancel();

                Task.WaitAll(mainTaskA, mainTaskB, mainTaskC, mainTaskD);
            }
            catch (AggregateException e)
            {
                Console.WriteLine($"From exception: {e.Message}");
            }

            Console.ReadLine();
        }
    }
}
