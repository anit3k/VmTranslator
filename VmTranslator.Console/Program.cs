using VmTranslator.Console;

FileHandler fileHandler = new FileHandler();
ParseHandler parseHandler = new ParseHandler();

Console.WriteLine("Looking in root of C:\\Files\\!");
Console.Write("Please enter the full name of the .vm file you want to translate: ");
string fileName = Console.ReadLine();

string filePath = $"C:\\Files\\{fileName}";

try
{
    List<string> linesReadFromFile = fileHandler.ReadAllLines(filePath);

    List<string> lines = RemoveUnnecesaryLines(linesReadFromFile);

    fileHandler.SaveAllLines(filePath, lines);
}
catch (Exception msg)
{
    Console.WriteLine(msg.Message);
}

Console.WriteLine("Program terminated");

List<string> RemoveUnnecesaryLines(List<string> lines)
{
    List<string> cleanedLines = new List<string>();

    foreach (String line in lines)
    {
        if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("//"))
        {
            line.Trim();
            cleanedLines.Add(line);
        }
    }

    return cleanedLines;
}