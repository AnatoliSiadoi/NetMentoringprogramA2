using log4net;
using Newtonsoft.Json;
using PostSharp.Aspects;
using System;

namespace AOPConsoleApp.CodeRewriting
{
    [Serializable]
    public class CodeRewritingPostSharp : OnMethodBoundaryAspect
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override void OnEntry(MethodExecutionArgs args)
        {
            string parameters;
            try
            {
                parameters = JsonConvert.SerializeObject(args.Arguments);
            }
            catch (JsonSerializationException ex)
            {
                parameters = "Not serializable";
            }

            log.Info($"Start method {args.Instance.GetType()}.{args.Method.Name} at {DateTime.Now} with parameters {parameters}");

            args.FlowBehavior = FlowBehavior.Default;
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            log.Info($"Return value of method {args.Instance.GetType()}.{args.Method.Name} = {args.ReturnValue}");
        }

        public override void OnException(MethodExecutionArgs args)
        {
            log.Error($"Method {args.Instance.GetType()}.{args.Method.Name} throw the next exception: {args.Exception.Message}");
            base.OnException(args);
        }
    }

}
