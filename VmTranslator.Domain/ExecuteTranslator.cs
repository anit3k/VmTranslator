using VmTranslator.Domain.interfaces;

namespace VmTranslator.Domain
{
    public class ExecuteTranslator : IExecuteTranslator
    {
        private IFileHandler _fileHandler;
        private IBuilder _parseBuilder;

        public ExecuteTranslator(IFileHandler fileHandler, IBuilder parseBuilder)
        {
            _fileHandler = fileHandler;
            _parseBuilder = parseBuilder;
        }

        public void Run(string filePath)
        {
            try
            {
                List<string> linesReadFromFile = _fileHandler.ReadAllLines(filePath);

                List<string> lines = RemoveUnnecesaryLines(linesReadFromFile);

                _parseBuilder.BuildAssemblyCode(lines);
                var result = _parseBuilder.Build();

                _fileHandler.SaveAllLines(filePath, result);
            }
            catch (Exception msg)
            {
                throw new Exception(msg.Message);
            }
        }

        internal List<string> RemoveUnnecesaryLines(List<string> lines)
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
    }
}
