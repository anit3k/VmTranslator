namespace VmTranslator.Domain.interfaces
{
    /// <summary>
    /// Interface for FileHandler
    /// </summary>
    public interface IFileHandler
    {
        public List<string> ReadAllLines(string path);
        public void SaveAllLines(string path, List<string> result);
    }
}
