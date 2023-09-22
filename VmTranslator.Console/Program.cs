using Autofac;
using VmTranslator.Console;
using VmTranslator.Domain.interfaces;


IContainer container = AutofacConfig.Configure();

Console.WriteLine("Looking in root of C:\\Files\\!");
Console.Write("Please enter the full name of the .vm file you want to translate: ");
string fileName = Console.ReadLine();

using (var scope = container.BeginLifetimeScope())
{
    var translator = scope.Resolve<IExecuteTranslator>();
    translator.Run(fileName);
}

Console.WriteLine("Program terminated");