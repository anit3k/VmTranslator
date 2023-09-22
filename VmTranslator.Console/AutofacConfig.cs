using Autofac;
using VmTranslator.Domain;
using VmTranslator.Domain.interfaces;

namespace VmTranslator.Console
{
    public class AutofacConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ExecuteTranslator>().As<IExecuteTranslator>();
            builder.RegisterType<FileHandler>().As<IFileHandler>();
            builder.RegisterType<ParseHandlerBuilder>().As<IBuilder>();
            builder.RegisterType<ParseHandler>().As<ParseHandler>();

            return builder.Build();
        }
    }
}
