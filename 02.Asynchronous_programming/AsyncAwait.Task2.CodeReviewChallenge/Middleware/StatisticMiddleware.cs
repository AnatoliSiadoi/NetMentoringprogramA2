using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncAwait.Task2.CodeReviewChallenge.Headers;
using CloudServices.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AsyncAwait.Task2.CodeReviewChallenge.Middleware
{
    public class StatisticMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly IStatisticService _statisticService;

        public StatisticMiddleware(RequestDelegate next, IStatisticService statisticService)
        {
            _next = next;
            _statisticService = statisticService ?? throw new ArgumentNullException(nameof(statisticService));
        }

        public async Task InvokeAsync(HttpContext context)
        {   
            string path = context.Request.Path;

            var task = await _statisticService.RegisterVisitAsync(path)
                .ContinueWith(async (t) =>
                {
                    var res = await _statisticService.GetVisitsCountAsync(path);
                    context.Response.Headers.Add(CustomHttpHeaders.TotalPageVisits, res.ToString());
                }, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ConfigureAwait(false);

            Console.WriteLine(task.Status); // just for debugging purposes

            await Task.Delay(3000); // without this the statistic counter does not work
            await _next(context);
        }
    }
}
