namespace VmTranslator.Console
{
    /// <summary>
    /// Reads data from a vm file, and saves it to a asm file.
    /// </summary>
    public class FileHandler
    {
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
                return File.ReadAllLines($"{path}.vm").ToList();
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
                if (!File.Exists($"{path}.asm"))
                {
                    File.WriteAllLines($"{path}.asm", result);
                }
                else
                {
                    File.Delete($"{path}.asm");
                    File.WriteAllLines($"{path}.asm", result);
                }
            }
            catch (Exception msg)
            {
                throw new IOException(msg.Message);
            }
        }
    }
}
