namespace VmTranslator.Console
{
    public class ParseHandler
    {
        #region fields
        private readonly List<string> _vmCode;
        private readonly List<string> _assemblyCode;
        private int _labelCounter = 0;

        private const string ConstantSegment = "constant";
        private const string LocalSegment = "local";
        private const string ArgumentSegment = "argument";
        private const string ThisSegment = "this";
        private const string ThatSegment = "that";
        private const string TempSegment = "temp";
        #endregion

        #region Constructor
        public ParseHandler(List<string> vmCode)
        {
            _vmCode = vmCode;
            _assemblyCode = new List<string>();
        }
        #endregion

        #region Public Methods
        public List<string> TranslateVmToAssembly()
        {
            foreach (var line in _vmCode)
            {
                TranslateCurrentLineToAsmCommands(line);
            }

            return _assemblyCode;
        }
        #endregion

        #region Private Methods
        private void TranslateCurrentLineToAsmCommands(string line)
        {
            string[] parts = line.Split(' ');
            string command = parts[0];

            switch (command)
            {
                case "push":
                    TranslatePush(parts);
                    break;
                case "pop":
                    TranslatePop(parts);
                    break;
                case "add":
                    TranslateAdd();
                    break;
                case "sub":
                    TranslateSub();
                    break;
                case "neg":
                    TranslateNeg();
                    break;
                case "eq":
                    TranslateComparison("JEQ");
                    break;
                case "gt":
                    TranslateComparison("JGT");
                    break;
                case "lt":
                    TranslateComparison("JLT");
                    break;
                case "and":
                    TranslateAnd();
                    break;
                case "or":
                    TranslateOr();
                    break;
                case "not":
                    TranslateNot();
                    break;
                default:
                    // Handle unrecognized commands
                    break;
            }
        }

        private void TranslatePush(string[] parts)
        {
            string segment = parts[1];
            int index = int.Parse(parts[2]);

            switch (segment)
            {
                case ConstantSegment:
                    Emit(
                        $"@{index}",
                        "D=A"
                    );
                    PushDToStack();
                    break;
                case LocalSegment:
                case ArgumentSegment:
                case ThisSegment:
                case ThatSegment:
                    Emit(
                        $"@{index}",
                        "D=A",
                        $"@{SegmentToMemory(segment)}",
                        "A=M+D",
                        "D=M"
                    );
                    PushDToStack();
                    break;
                case TempSegment:
                    Emit(
                        $"@{5 + index}", // Temp segments start at R5
                        "D=M"
                    );
                    PushDToStack();
                    break;
                default:
                    // Handle other segments if needed
                    break;
            }
        }
        private void PushDToStack()
        {
            Emit(
                "@SP",
                "A=M",
                "M=D", // SP points to the current top of the stack
                "@SP",
                "M=M+1" // Increment SP
            );
        }

        private void TranslatePop(string[] parts)
        {
            string segment = parts[1];
            int index = int.Parse(parts[2]);

            Emit(
                $"@{index}",
                "D=A",
                $"@{SegmentToMemory(segment)}",
                "D=M+D",
                "@R13",
                "M=D", // Store the target address in R13
                "@SP",
                "AM=M-1", // Decrement SP and load the address of the top value
                "D=M", // Load the top value
                "@R13",
                "A=M",
                "M=D" // Store the value at the target address
            );
        }

        private string SegmentToMemory(string segment)
        {
            switch (segment)
            {
                case LocalSegment:
                    return "LCL";
                case ArgumentSegment:
                    return "ARG";
                case ThisSegment:
                    return "THIS";
                case ThatSegment:
                    return "THAT";
                default:
                    throw new ArgumentException($"Unknown segment: {segment}");
            }
        }

        private void TranslateAdd()
        {
            Emit(
                "@SP",
                "AM=M-1", // Decrement SP and load the address of the second value
                "D=M", // Load the second value
                "A=A-1", // Move back to the address of the first value
                "M=M+D" // Add the second value to the first value
            );
        }


        private void TranslateSub()
        {
            Emit(
                "@SP",
                "AM=M-1", // Decrement SP and load the address of the second value
                "D=M", // Load the second value into D
                "A=A-1", // Move back to the address of the first value
                "M=M-D" // Subtract the second value from the first value and store the result
            );
        }

        private void TranslateNeg()
        {
            Emit(
                "@SP",
                "A=M-1", // Load the address of the top value
                "M=-M" // Negate the value at the top of the stack
            );
        }

        private void TranslateComparison(string jumpCondition)
        {
            string trueLabel = $"TRUE{_labelCounter}";
            string endLabel = $"END{_labelCounter}";
            _labelCounter++;

            Emit(
                "@SP",
                "AM=M-1", // Decrement SP and load the address of the second value
                "D=M", // Load the second value
                "A=A-1", // Move back to the address of the first value
                "D=M-D", // Compare the values (subtract the second from the first)
                $"@{trueLabel}",
                $"D;{jumpCondition}", // Jump if the condition is met
                "@SP",
                "A=M-1", // Move to the top of the stack
                "M=0", // False
                $"@{endLabel}",
                "0;JMP", // Unconditional jump to end
                $"({trueLabel})",
                "@SP",
                "A=M-1", // Move to the top of the stack
                "M=-1", // True
                $"({endLabel})"
            );
        }

        private void TranslateAnd()
        {
            Emit(
                "@SP",
                "AM=M-1", // Decrement SP and load the address of the second value
                "D=M", // Load the second value into D
                "A=A-1", // Move back to the address of the first value
                "M=M&D" // Perform bitwise AND operation and store the result
            );
        }

        private void TranslateOr()
        {
            Emit(
                "@SP",
                "AM=M-1", // Decrement SP and load the address of the second value
                "D=M", // Load the second value into D
                "A=A-1", // Move back to the address of the first value
                "M=M|D" // Perform bitwise OR operation and store the result
            );
        }

        private void TranslateNot()
        {
            Emit(
                "@SP",
                "A=M-1", // Load the address of the top value
                "M=!M" // Perform bitwise NOT operation and store the result
            );
        }

        private void Emit(params string[] asmInstructions)
        {
            _assemblyCode.AddRange(asmInstructions);
        }
        #endregion
    }
}
