namespace VmTranslator.Domain
{
    /// <summary>
    /// Concrete implementation of the builder pattern.
    /// </summary>
    public class ParseHandlerBuilder : IBuilder
    {
        #region fields
        private List<string> _vmCode;
        private readonly ParseHandler _parseHandler;
        #endregion

        #region Constructor
        public ParseHandlerBuilder(ParseHandler parseHandler)
        {
            _parseHandler = parseHandler;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Maps the VM code to the private field.
        /// </summary>
        /// <param name="vmCode">The VM code to map to the private field.</param>
        public void BuildAssemblyCode(List<string> vmCode)
        {
            _vmCode = vmCode;
        }

        /// <summary>
        /// Sends the VM code to the ParseHandler.
        /// </summary>
        /// <returns>Returns a List of strings, which is our entire ASM code.</returns>
        public List<string> Build()
        {
            return _parseHandler.TranslateVmToAssembly(_vmCode);
        }
        #endregion
    }
}
