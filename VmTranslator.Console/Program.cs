using VmTranslator.Domain;
using VmTranslator.Domain.interfaces;

IFileHandler fileHandler = new FileHandler();
IBuilder parseBuilder = new ParseHandlerBuilder();

IExecuteTranslator executeTranslator = new ExecuteTranslator(fileHandler, parseBuilder);

Console.WriteLine("Looking in root of C:\\Files\\!");
Console.Write("Please enter the full name of the .vm file you want to translate: ");
string fileName = Console.ReadLine();

executeTranslator.Run(fileName);

Console.WriteLine("Program terminated");