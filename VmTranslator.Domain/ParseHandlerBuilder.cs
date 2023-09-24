using VmTranslator.Domain;
using VmTranslator.Domain.interfaces;

namespace VmTranslator.Domain
{
    public class ParseHandlerBuilder : IBuilder
    {
        #region fields
        private List<string> _vmCode;
        private readonly ParseHandler _parseHandler;
        #endregion

        public ParseHandlerBuilder(ParseHandler parseHandler)
        {
            _parseHandler = parseHandler;
        }

        #region Methods
        public void BuildAssemblyCode(List<string> vmCode)
        {
            _vmCode = vmCode;
        }

        public List<string> Build()
        {
            return _parseHandler.TranslateVmToAssembly(_vmCode);
        }
        #endregion
    }
}
