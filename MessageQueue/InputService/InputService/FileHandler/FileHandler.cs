using MassTransit;
using Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace InputService.FileHandler
{
    public class Handler : IFileHandler
    {
        private readonly ILogger<Handler> _logger;
        private readonly IConfigurationRoot _config;
        private readonly IBusControl _bus;

        public Handler(ILogger<Handler> logger, IConfigurationRoot config, IBusControl bus)
        {
            _logger = logger;
            _config = config;
            _bus = bus;
        }

        public void Process(string filePath)
        {
            _logger.LogInformation("Start processing file.");
            var chunkSize = int.Parse(_config["chunkSize"]);
            var name = Path.GetFileName(filePath);
            var file = File.ReadAllBytes(filePath);
            var order = 0;
            var skip = 0;

            var count = Math.Ceiling((double)file.Length / chunkSize);
            while (skip < file.Length)
            {
                var buffer = file.Skip(skip).Take(chunkSize).ToArray();
                _bus.Publish(new FileChunk { FileName = name, Chunk = buffer , Order = order++,  Count = count });
                skip += chunkSize;
            }
            _logger.LogInformation("Finished processing file.");
        }
    }
}
