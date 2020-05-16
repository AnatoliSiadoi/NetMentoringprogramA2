using AOPConsoleApp.DAL;
using AOPConsoleApp.DynamicProxy;
using Autofac;
using Castle.DynamicProxy;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;

namespace AOPConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigureLogging();
            ConfigureDI().Resolve<DummyUI>().Start();
        }

        private static void ConfigureLogging()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        private static IContainer ConfigureDI()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DummyUI>();

            var dynamicProxyInstance = new ProxyGenerator().CreateInterfaceProxyWithTarget<IDummyRepository>(
                new DummyRepository(),
                new DynamicProxyCastleCoreInterceptor());

            builder.RegisterInstance(dynamicProxyInstance).As<IDummyRepository>();
            builder.RegisterType<DummyService>().As<IDummyService>();

            return builder.Build();
        }
    }
}
