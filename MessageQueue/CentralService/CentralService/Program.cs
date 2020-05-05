using CentralService.Messaging;
using CentralService.Storage;
using MassTransit;
using Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CentralService
{
    class Program
    {
        public static IConfigurationRoot configuration;
        public static ServiceProvider serviceProvider;

        static void Main(string[] args)
        {
            try
            {
                MainAsync(args).Wait();
            }
            catch
            {
            }
        }

        static async Task MainAsync(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            serviceProvider = serviceCollection.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<Program>>();
            try
            {
                logger.LogInformation("Service started");
                 await serviceProvider.GetService<IBusControl>().StartAsync();

                Console.WriteLine("Press 'q' to quit.");
                while (Console.Read() != 'q') ;
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
            serviceCollection.AddSingleton<IFileStorage<FileChunk>, LocalFolderStorage>();

            serviceCollection.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(sbc =>
                {
                    var host = sbc.Host(new Uri($"rabbitmq://{configuration["rabbitMQHost"]}"), h =>
                    {
                        h.Username($"{configuration["rabbitMQUsername"]}");
                        h.Password($"{configuration["rabbitMQPassword"]}");
                    });

                    sbc.ReceiveEndpoint($"{configuration["rabbitMQQueue"]}", ep =>
                    {
                        ep.Consumer(() => new MessageConsumer(serviceProvider));
                    });
                }));
            });
        }
    }
}
