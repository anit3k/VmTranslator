using Autofac;
using VmTranslator.Domain;
using VmTranslator.Domain.interfaces;

namespace VmTranslator.ConsoleApplication
{
    /// <summary>
    /// The handling of Dependency Injection
    /// </summary>
    public class AutofacConfig
    {
        /// <summary>
        /// Setup of the Dependencies to use in the application.
        /// This method is static to ensure that the IContainer/Dependencies is available throughout the application.
        /// </summary>
        /// <returns>Returns the IContainer that contains the dependencies that's needed throughout the application.</returns>
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
