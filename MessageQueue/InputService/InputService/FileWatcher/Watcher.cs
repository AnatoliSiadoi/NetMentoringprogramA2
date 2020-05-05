using InputService.FileHandler;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace InputService.FileWatcher
{
    public class Watcher : IFileWatcher
    {
        private readonly ILogger<Watcher> _logger;
        private readonly IConfigurationRoot _config;
        private readonly IFileHandler _fileHandler;

        public Watcher(ILogger<Watcher> logger, IConfigurationRoot config, IBusControl bus, IFileHandler fileHandler)
        {
            _logger = logger;
            _config = config;
            _fileHandler = fileHandler;
        }

        public void Start()
        {
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = _config["pathDirectory"];
                _logger.LogInformation($"Start scanning directory: {watcher.Path}");

                watcher.NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

                watcher.Filter = $"*{_config["fileExtension"]}";
                watcher.Created += OnChanged;

                watcher.EnableRaisingEvents = true;

                Console.WriteLine("Press 'q' to quit.");
                while (Console.Read() != 'q') ;
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            _logger.LogInformation($"File: {e.FullPath} {e.ChangeType}");
            _fileHandler.Process(e.FullPath);
        }
    }
}
