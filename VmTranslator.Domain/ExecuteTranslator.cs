using VmTranslator.Domain.interfaces;

namespace VmTranslator.Domain
{
    /// <summary>
    /// Entry point of the domain layer, and executes the translator.
    /// </summary>
    public class ExecuteTranslator : IExecuteTranslator
    {
        #region Fields
        private IFileHandler _fileHandler;
        private IBuilder _parseBuilder;
        #endregion

        #region Constructor
        public ExecuteTranslator(IFileHandler fileHandler, IBuilder parseBuilder)
        {
            _fileHandler = fileHandler;
            _parseBuilder = parseBuilder;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes the translation of the selected file.
        /// </summary>
        /// <param name="filePath">The path to the file we wish to translate, selected from the program class.</param>
        /// <exception cref="Exception">Error message if error occurs during execution.</exception>
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

        /// <summary>
        /// Removes unneccesary lines like comments, and empty lines, and trims the start and end of lines for whitespace.
        /// </summary>
        /// <param name="lines">The lines to clean</param>
        /// <returns>Returns raw vm code, with removed comments, empty lines.</returns>
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
        #endregion
    }
}
