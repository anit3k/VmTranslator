using VmTranslator.Console;

FileHandler fileHandler = new FileHandler();
ParseHandler parseHandler = new ParseHandler();

Console.WriteLine("Please enter name of the file you wish to translate.");

string fileName = Console.ReadLine();

string filePath = $"C:\\Files\\{fileName}";

try
{
	List<string> lines = fileHandler.ReadAllLines(filePath);

    List<string> CleanedLines = new List<string>();

    foreach (String line in lines)
    {
        if (!line.StartsWith("//") && !string.IsNullOrWhiteSpace(line))
        {
            line.Trim();              // Update line with trimmed string
            line.Replace(" ", "");    // Update line with spaces removed
            CleanedLines.Add(line);
        }
    }

	fileHandler.SaveAllLines(filePath, CleanedLines);
}
catch (Exception msg)
{
	Console.WriteLine(msg.Message);
}

Console.WriteLine("Program terminated");