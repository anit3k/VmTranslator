namespace VmTranslator.Console
{
    public class ParseHandlerBuilder : IBuilder
    {
        #region fields
        private List<string> _vmCode;
        #endregion

        #region Methods
        public void BuildAssemblyCode(List<string> vmCode)
        {
            _vmCode = vmCode;
        }
        public List<string> Build()
        {
            var handler = new ParseHandler(_vmCode);
            return handler.TranslateVmToAssembly();
        }
        #endregion
    }
}
