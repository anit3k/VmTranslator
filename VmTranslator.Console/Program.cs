using VmTranslator.Console;

FileHandler fileHandler = new FileHandler();

Console.WriteLine("Please enter name of the file you wish to translate.");

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