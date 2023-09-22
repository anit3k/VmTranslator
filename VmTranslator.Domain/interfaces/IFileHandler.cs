namespace VmTranslator.Domain.interfaces
{
    public interface IFileHandler
    {
        public List<string> ReadAllLines(string path);
        public void SaveAllLines(string path, List<string> result);
    }
}
