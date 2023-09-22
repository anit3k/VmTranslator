using VmTranslator.Domain.interfaces;

namespace VmTranslator.Domain
{
    /// <summary>
    /// Reads data from a vm file, and saves it to a asm file.
    /// </summary>
    public class FileHandler : IFileHandler
    {
        #region Methods
        /// <summary>
        /// Reads all the lines from a given file.
        /// </summary>
        /// <param name="path">The path of the file we want to read</param>
        /// <returns>Returns a list of all the lines from the file.</returns>
        /// <exception cref="IOException">Throws an error message if the file fails to read.</exception>
        public List<string> ReadAllLines(string path)
        {
            try
            {
                var test = !path.Contains(".vm");

                if (!path.Contains(".vm") || !File.Exists(path))
                {
                    throw new FileNotFoundException("File is not found or the file is not of type .vm", path);
                }
                return File.ReadAllLines($"{path}").ToList();
            }
            catch (Exception msg)
            {
                throw new IOException(msg.Message);
            }
        }

        /// <summary>
        /// Saves the formatted and translated vm file to a new .asm file.
        /// </summary>
        /// <param name="path">The path we want to save the file at.</param>
        /// <param name="result">The list containing all the translated lines.</param>
        /// <exception cref="IOException">Throws an error if the save fails.</exception>
        public void SaveAllLines(string path, List<string> result)
        {
            try
            {
                string asmFilePath = path.Remove(path.Length - 2, 2);
                if (!File.Exists($"{asmFilePath}asm"))
                {
                    File.WriteAllLines($"{asmFilePath}asm", result);
                }
                else
                {
                    File.Delete($"{asmFilePath}asm");
                    File.WriteAllLines($"{asmFilePath}asm", result);
                }
            }
            catch (Exception msg)
            {
                throw new IOException(msg.Message);
            }
        }
        #endregion
    }
}
