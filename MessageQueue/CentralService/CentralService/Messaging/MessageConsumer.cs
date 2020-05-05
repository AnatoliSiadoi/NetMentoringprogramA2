using System;
using System.Linq;
using System.Threading.Tasks;
using CentralService.Storage;
using MassTransit;
using Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CentralService.Messaging
{
    public class MessageConsumer : IConsumer<FileChunk>
    {
        private readonly ILogger<MessageConsumer> _logger;
        private readonly IFileStorage<FileChunk> _fileStorage;

        public MessageConsumer(ServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetService<ILogger<MessageConsumer>>();
            _fileStorage = serviceProvider.GetService<IFileStorage<FileChunk>>();
        }

        public Task Consume(ConsumeContext<FileChunk> context)
        {
            try
            {
                _fileStorage.SaveToStorage(
                    new FileChunk
                    {
                        FileName = context.Message.FileName,
                        Chunk = context.Message.Chunk.ToArray(),
                        Order = context.Message.Order,
                        Count = context.Message.Count
                    });
            }
            catch(Exception ex)
            {
                _logger.LogError($"Something went wrong exception: {ex}");
                throw;
            }

            return Task.CompletedTask;
        }
    }
}
