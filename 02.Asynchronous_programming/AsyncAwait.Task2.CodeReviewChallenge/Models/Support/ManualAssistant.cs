using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CloudServices.Interfaces;

namespace AsyncAwait.Task2.CodeReviewChallenge.Models.Support
{
    public class ManualAssistant : IAssistant
    {
        private readonly ISupportService _supportService;

        public ManualAssistant(ISupportService supportService)
        {
            _supportService = supportService ?? throw new ArgumentNullException(nameof(supportService));
        }

        public async Task<string> RequestAssistanceAsync(string requestInfo)
        {
            try
            {
                /*var task = Task.Run(async () => */await _supportService.RegisterSupportRequestAsync(requestInfo).ConfigureAwait(false);
                //Console.WriteLine(task.Status); // this is for debugging purposes
                await Task.Delay(5000); // this is just to be sure that the request is registered
                return await _supportService.GetSupportInfoAsync(requestInfo)
                    .ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                return $"Failed to register assistance request. Please try later. {ex.Message}";
            }
        }
    }
}
