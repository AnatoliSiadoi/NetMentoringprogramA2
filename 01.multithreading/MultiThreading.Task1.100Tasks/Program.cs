/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    class Program
    {
        const int TaskAmount = 100;
        const int MaxIterationsCount = 1000;

        private static readonly List<Task> Tasks = new List<Task>(TaskAmount);

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. Multi threading V1.");
            Console.WriteLine("1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.");
            Console.WriteLine("Each Task should iterate from 1 to 1000 and print into the console the following string:");
            Console.WriteLine("“Task #0 – {iteration number}”.");
            Console.WriteLine();
            
            HundredTasks();

            Console.ReadLine();
        }

        private static void HundredTasks()
        {
            for(var i = 0; i < TaskAmount; i++)
            {
                int identifier = i;
                Tasks.Add(new Task(() => Worker(identifier)));
            }

            Tasks.ForEach(t => t.Start());

            Task.WhenAll(Tasks).ContinueWith(task => Console.WriteLine("Done")).Wait();
        }

        private static void Output(int taskNumber, int iterationNumber)
        {
            Console.WriteLine($"Task #{taskNumber} – {iterationNumber}");
        }

        private static void Worker(int identifier)
        {
            for (var j = 1; j < MaxIterationsCount; j++)
                Output(identifier, j);
        }
    }
}
