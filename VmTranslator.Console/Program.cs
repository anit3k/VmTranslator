using VmTranslator.Console;

FileHandler fileHandler = new FileHandler();

Console.WriteLine("Looking in root of C:\\Files\\!");
Console.Write("Please enter the full name of the .vm file you want to translate: ");
string fileName = Console.ReadLine();

string filePath = $"C:\\Files\\{fileName}";

try
{
	List<string> lines = fileHandler.ReadAllLines(filePath);

	fileHandler.SaveAllLines(filePath, lines);
}
catch (Exception msg)
{
	Console.WriteLine(msg.Message);
}

Console.WriteLine("Program terminated");