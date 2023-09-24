namespace VmTranslator.Domain
{
    /// <summary>
    /// Interface for Builder
    /// </summary>
    public interface IBuilder
    {
        public void BuildAssemblyCode(List<string> vmCode);
        public List<string> Build();
    }
}
