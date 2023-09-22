namespace VmTranslator.Domain
{
    public interface IBuilder
    {
        public void BuildAssemblyCode(List<string> vmCode);
        public List<string> Build();
    }
}
