using Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CentralService.Storage
{
    public class LocalFolderStorage : IFileStorage<FileChunk>
    {
        private readonly object objLock = new object();
        private readonly ILogger<LocalFolderStorage> _logger;
        private readonly IConfigurationRoot _config;
        private Dictionary<string, List<FileChunk>> inMemoryStorage = new Dictionary<string, List<FileChunk>>();

        public LocalFolderStorage(ILogger<LocalFolderStorage> logger, IConfigurationRoot config)
        {
            _logger = logger;
            _config = config;
        }

        public void SaveToStorage(FileChunk item)
        {
            lock (objLock)
            {
                if (inMemoryStorage.ContainsKey(item.FileName))
                {
                    inMemoryStorage[item.FileName].Add(item);
                }
                else
                {
                    var initial = new List<FileChunk> { item };
                    inMemoryStorage.TryAdd(item.FileName, initial);
                }

                if (inMemoryStorage[item.FileName].Count == item.Count)
                {
                    var localFileName = Path.Combine($"{_config["pathDirectory"]}", item.FileName);
                    var result = inMemoryStorage[item.FileName].OrderBy(x => x.Order);
                    foreach (var chunk in result)
                    {
                        var body = chunk.Chunk.ToArray();

                        using (var fileStream = new FileStream(localFileName, FileMode.Append, FileAccess.Write))
                        {
                            fileStream.Write(body, 0, body.Length);
                        };
                    }

                    _logger.LogInformation($"File saved by path : {_config["pathDirectory"]}");
                }
            }
        }
    }
}
