namespace VmTranslator.Console
{
    public class ParseHandlerBuilder : IBuilder
    {
        private List<string> _vmCode;

        public void BuildAssemblyCode(List<string> vmCode)
        {
            _vmCode = vmCode;
        }
        public List<string> Build()
        {
            var handler = new ParseHandler(_vmCode);
            return handler.TranslateVmToAssembly();
        }
    }
}
