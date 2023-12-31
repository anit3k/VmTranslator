﻿using VmTranslator.Domain;

namespace VmTranslator.Test
{
    internal class ParseHandlerTest
    {
        private ParseHandler _parseHandler;

        [Test]
        public void Should_Translate_VM_To_ASM()
        {

            // Arrange
            var vmCode = Basic_Test(); // Get the VM code from your method

            // Initialize the ParseHandler for each test case
            _parseHandler = new ParseHandler(vmCode);

            var expectedAssemblyCode = Basic_Test_Result(); // Expected ASM code
            //var parseHandler = new ParseHandler(vmCode); // Initialize ParseHandler with VM code

            // Act
            List<string> assemblyCode = _parseHandler.TranslateVmToAssembly();

            // Assert
            CollectionAssert.AreEqual(expectedAssemblyCode, assemblyCode);
        }

        [Test]
        public void Should_Not_Translate_Invalid_VM_Commands()
        {
            // Arrange
            var invalidVmCode = new List<string>
            {
                "push invalid_segment 42",
                "pop unsupported_segment 10"
            };

            // Initialize the ParseHandler for the test case
            _parseHandler = new ParseHandler(invalidVmCode);

            // Act and Assert
            Assert.Throws<ArgumentException>(() => _parseHandler.TranslateVmToAssembly());
        }

        private List<string> Basic_Test()
        {
            return new List<string>
            {
                "push constant 10",
                "pop local 0",
                "push constant 21",
                "push constant 22",
                "pop argument 2",
                "pop argument 1",
                "push constant 36",
                "pop this 6",
                "push constant 42",
                "push constant 45",
                "pop that 5",
                "pop that 2",
                "push constant 510",
                "pop temp 6",
                "push local 0",
                "push that 5",
                "add",
                "push argument 1",
                "sub",
                "push this 6",
                "push this 6",
                "add",
                "sub",
                "push temp 6",
                "add"
            };
        }

        private List<string> Basic_Test_Result()
        {
            return new List<string>
            {
                "@10",
                "D=A",
                "@SP",
                "A=M",
                "M=D",
                "@SP",
                "M=M+1",
                "@0",
                "D=A",
                "@LCL",
                "D=M+D",
                "@R13",
                "M=D",
                "@SP",
                "AM=M-1",
                "D=M",
                "@R13",
                "A=M",
                "M=D",
                "@21",
                "D=A",
                "@SP",
                "A=M",
                "M=D",
                "@SP",
                "M=M+1",
                "@22",
                "D=A",
                "@SP",
                "A=M",
                "M=D",
                "@SP",
                "M=M+1",
                "@2",
                "D=A",
                "@ARG",
                "D=M+D",
                "@R13",
                "M=D",
                "@SP",
                "AM=M-1",
                "D=M",
                "@R13",
                "A=M",
                "M=D",
                "@1",
                "D=A",
                "@ARG",
                "D=M+D",
                "@R13",
                "M=D",
                "@SP",
                "AM=M-1",
                "D=M",
                "@R13",
                "A=M",
                "M=D",
                "@36",
                "D=A",
                "@SP",
                "A=M",
                "M=D",
                "@SP",
                "M=M+1",
                "@6",
                "D=A",
                "@THIS",
                "D=M+D",
                "@R13",
                "M=D",
                "@SP",
                "AM=M-1",
                "D=M",
                "@R13",
                "A=M",
                "M=D",
                "@42",
                "D=A",
                "@SP",
                "A=M",
                "M=D",
                "@SP",
                "M=M+1",
                "@45",
                "D=A",
                "@SP",
                "A=M",
                "M=D",
                "@SP",
                "M=M+1",
                "@5",
                "D=A",
                "@THAT",
                "D=M+D",
                "@R13",
                "M=D",
                "@SP",
                "AM=M-1",
                "D=M",
                "@R13",
                "A=M",
                "M=D",
                "@2",
                "D=A",
                "@THAT",
                "D=M+D",
                "@R13",
                "M=D",
                "@SP",
                "AM=M-1",
                "D=M",
                "@R13",
                "A=M",
                "M=D",
                "@510",
                "D=A",
                "@SP",
                "A=M",
                "M=D",
                "@SP",
                "M=M+1",
                "@6",
                "D=A",
                "@R5",
                "D=M+D",
                "@R13",
                "M=D",
                "@SP",
                "AM=M-1",
                "D=M",
                "@R13",
                "A=M",
                "M=D",
                "@0",
                "D=A",
                "@LCL",
                "A=M+D",
                "D=M",
                "@SP",
                "A=M",
                "M=D",
                "@SP",
                "M=M+1",
                "@5",
                "D=A",
                "@THAT",
                "A=M+D",
                "D=M",
                "@SP",
                "A=M",
                "M=D",
                "@SP",
                "M=M+1",
                "@SP",
                "AM=M-1",
                "D=M",
                "A=A-1",
                "M=M+D",
                "@1",
                "D=A",
                "@ARG",
                "A=M+D",
                "D=M",
                "@SP",
                "A=M",
                "M=D",
                "@SP",
                "M=M+1",
                "@SP",
                "AM=M-1",
                "D=M",
                "A=A-1",
                "M=M-D",
                "@6",
                "D=A",
                "@THIS",
                "A=M+D",
                "D=M",
                "@SP",
                "A=M",
                "M=D",
                "@SP",
                "M=M+1",
                "@6",
                "D=A",
                "@THIS",
                "A=M+D",
                "D=M",
                "@SP",
                "A=M",
                "M=D",
                "@SP",
                "M=M+1",
                "@SP",
                "AM=M-1",
                "D=M",
                "A=A-1",
                "M=M+D",
                "@SP",
                "AM=M-1",
                "D=M",
                "A=A-1",
                "M=M-D",
                "@11",
                "D=M",
                "@SP",
                "A=M",
                "M=D",
                "@SP",
                "M=M+1",
                "@SP",
                "AM=M-1",
                "D=M",
                "A=A-1",
                "M=M+D"
            };
        }
    }
}
