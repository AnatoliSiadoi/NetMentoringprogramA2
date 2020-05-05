using InputService.FileHandler;
using InputService.FileWatcher;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace InputService
{
    class Program
    {
        public static IConfigurationRoot configuration;

        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<Program>>();
            try
            {
                logger.LogInformation("Service started");
                serviceProvider.GetService<IFileWatcher>().Start();
                serviceProvider.GetService<IBusControl>().Start();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong exception: {ex}");
                throw ex;
            }
            finally
            {
                serviceProvider.GetService<IBusControl>().Stop();
                logger.LogInformation("Service stopped");
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging(log => log.AddConsole());

            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);

            serviceCollection.AddTransient<IFileWatcher, Watcher>();
            serviceCollection.AddTransient<IFileHandler, Handler>();

            serviceCollection.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(sbc =>
                {
                    var host = sbc.Host(new Uri($"rabbitmq://{configuration["rabbitMQHost"]}"), h =>
                    {
                        h.Username($"{configuration["rabbitMQUsername"]}");
                        h.Password($"{configuration["rabbitMQPassword"]}");
                    });

                    sbc.ConfigureEndpoints(provider);
                }));
            });
        }
    }
}
