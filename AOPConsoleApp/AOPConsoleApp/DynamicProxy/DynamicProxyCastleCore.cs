using Castle.DynamicProxy;
using log4net;
using Newtonsoft.Json;
using System;

namespace AOPConsoleApp.DynamicProxy
{
    public class DynamicProxyCastleCoreInterceptor : IInterceptor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void Intercept(IInvocation invocation)
        {
            string parameters;
            try
            {
                parameters = JsonConvert.SerializeObject(invocation.Arguments);
            }
            catch (JsonSerializationException ex)
            {
                parameters = "Not serializable";
            }

            log.Info($"Start method {invocation.TargetType}.{invocation.Method.Name} at {DateTime.Now} with parameters {parameters}");

            try
            {
                invocation.Proceed();

                log.Info($"Return value of m = {invocation.ReturnValue}");
            }
            catch (Exception ex)
            {
                log.Error($"Method {invocation.TargetType}.{invocation.Method.Name} throw the next exception: {ex.Message}");
                throw;
            }
        }
    }

}
