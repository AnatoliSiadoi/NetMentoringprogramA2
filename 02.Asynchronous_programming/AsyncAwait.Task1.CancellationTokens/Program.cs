/*
 * Изучите код данного приложения для расчета суммы целых чисел от 0 до N, а затем
 * измените код приложения таким образом, чтобы выполнялись следующие требования:
 * 1. Расчет должен производиться асинхронно.
 * 2. N задается пользователем из консоли. Пользователь вправе внести новую границу в процессе вычислений,
 * что должно привести к перезапуску расчета.
 * 3. При перезапуске расчета приложение должно продолжить работу без каких-либо сбоев.
 */

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens
{
    class Program
    {
        /// <summary>
        /// The Main method should not be changed at all.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("Mentoring program L2. Async/await.V1. Task 1");
            Console.WriteLine("Calculating the sum of integers from 0 to N.");
            Console.WriteLine("Use 'q' key to exit...");
            Console.WriteLine();
            Console.WriteLine("Enter N: ");

            string input = Console.ReadLine();
            while (input.Trim().ToUpper() != "Q")
            {
                if (int.TryParse(input, out int n))
                {
                    CalculateSum(n);
                }
                else
                {
                    Console.WriteLine($"Invalid integer: '{input}'. Please try again.");
                    Console.WriteLine("Enter N: ");
                }
                input = Console.ReadLine();
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }

        private static void CalculateSum(int n)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            // todo: make calculation asynchronous

            var t = new Task(() =>
            {
                try
                {
                    long sum = Calculator.Calculate(n, token);
                    Console.WriteLine($"Sum for {n} = {sum}.");
                    Console.WriteLine();
                }
                catch (OperationCanceledException e)
                {
                    Console.WriteLine($"Sum for {n} cancelled...");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                finally
                {
                    Console.WriteLine($"Finally for {n}");
                }
            });
            t.Start();

            Console.WriteLine($"The task for {n} started... Enter N to cancel the request:");
            Console.WriteLine("Enter N: ");

            if (t.Status != TaskStatus.RanToCompletion)
            {
                var input = Console.ReadLine();

                if (int.TryParse(input, out int newN))
                {
                    if (t.Status == TaskStatus.Running)
                    {
                        cts.Cancel();
                    }
                    CalculateSum(newN);
                }
            }
            // todo: add code to process cancellation and uncomment this line    
            // Console.WriteLine($"Sum for {n} cancelled...");
        }
    }
}