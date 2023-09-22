using VmTranslator.Console;

FileHandler _fileHandler = new FileHandler();
IBuilder _builder = new ParseHandlerBuilder();

Console.WriteLine("Looking in root of C:\\Files\\!");
Console.Write("Please enter the full name of the .vm file you want to translate: ");
string fileName = Console.ReadLine();

string filePath = $"C:\\Files\\{fileName}";

try
{
    List<string> linesReadFromFile = _fileHandler.ReadAllLines(filePath);

    List<string> lines = RemoveUnnecesaryLines(linesReadFromFile);

    _builder.BuildAssemblyCode(lines);
    var result = _builder.Build();

    _fileHandler.SaveAllLines(filePath, result);
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