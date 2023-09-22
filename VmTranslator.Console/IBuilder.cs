namespace VmTranslator.Console
{
    public interface IBuilder
    {
        void BuildAssemblyCode(List<string> vmCode);
        List<string> Build();
    }
}
