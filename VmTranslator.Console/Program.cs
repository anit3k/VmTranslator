// We have added a class diagram as a png that lays at:
// https://github.com/anit3k/VmTranslator/blob/master/VmTranslatorClassDiagram.svg

using Autofac;
using VmTranslator.ConsoleApplication;

IContainer container = AutofacConfig.Configure();

var app = new Runner(container);
app.ExecuteProgram();