using Autofac;
using VmTranslator.ConsoleApplication;

IContainer container = AutofacConfig.Configure();

var app = new Runner(container);
app.ExecuteProgram();