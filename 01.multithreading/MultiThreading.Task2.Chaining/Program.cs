/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        const int ArrayLength = 10;

        private static readonly Random Rand = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            Task.Run(() =>
            {
                var arr = new int[ArrayLength];

                for (var i = 0; i < ArrayLength; i++)
                    arr[i] = Rand.Next();

                Output(arr);
                return arr;
            })
                .ContinueWith(task =>
                {
                    for (var i = 0; i < ArrayLength; i++)
                        task.Result[i] = Rand.Next();

                    var arr = task.Result;
                    Output(arr);

                    return arr;
                })
                .ContinueWith(task =>
                {
                    var arr = task.Result;
                    Array.Sort(arr);
                    Output(arr);

                    return arr;
                })
                .ContinueWith(task =>
                {
                    var sum = 0;
                    double avg = 0;

                    foreach (var item in task.Result) sum += item;

                    avg = (double)sum / task.Result.Length;
                    Console.WriteLine(avg);
                })
                .Wait();

            Console.ReadLine();
        }

        private static void Output(int[] array)
        {
            var sb = new StringBuilder();

            foreach (var item in array)
            {
                sb.Append(item);
                sb.Append(" ");
            }

            Console.WriteLine(sb.ToString());
        }
    }
}
