namespace VmTranslator.Console
{
    public class ParseHandler
    {
        private List<string> _vmCode;
        public ParseHandler(List<string> vmCode)
        {

            _vmCode = vmCode;
        }
        public List<string> TranslateVmToAssembly()
        {
            var assemblyCode = new List<string>();

            foreach (var line in _vmCode)
            {
                string[] parts = line.Split(' ');

                switch (parts[0])
                {
                    case "push":
                        assemblyCode.AddRange(TranslatePush(parts));
                        break;
                    case "pop":
                        assemblyCode.AddRange(TranslatePop(parts));
                        break;
                    case "add":
                        assemblyCode.AddRange(TranslateAdd());
                        break;
                    case "sub":
                        assemblyCode.AddRange(TranslateSub());
                        break;
                    case "neg":
                        assemblyCode.AddRange(TranslateNeg());
                        break;
                    case "eq":
                        assemblyCode.AddRange(TranslateEq());
                        break;
                    case "gt":
                        assemblyCode.AddRange(TranslateGt());
                        break;
                    case "lt":
                        assemblyCode.AddRange(TranslateLt());
                        break;
                    case "and":
                        assemblyCode.AddRange(TranslateAnd());
                        break;
                    case "or":
                        assemblyCode.AddRange(TranslateOr());
                        break;
                    case "not":
                        assemblyCode.AddRange(TranslateNot());
                        break;
                    default:
                        // Handle unrecognized commands
                        break;
                }

            }

            return assemblyCode;
        }

        private List<string> TranslatePush(string[] parts)
        {
            var assemblyCode = new List<string>();

            string segment = parts[1];
            int index = int.Parse(parts[2]); // Assuming the index is the third part

            switch (segment)
            {
                case "constant":
                    // Pushing a constant value onto the stack
                    assemblyCode.Add($"@{index}");
                    assemblyCode.Add("D=A");
                    assemblyCode.Add("@SP");
                    assemblyCode.Add("A=M");
                    assemblyCode.Add("M=D"); // SP points to the current top of the stack
                    assemblyCode.Add("@SP");
                    assemblyCode.Add("M=M+1"); // Increment SP
                    break;

                case "local":
                case "argument":
                case "this":
                case "that":
                    // Pushing from a memory segment
                    assemblyCode.Add($"@{index}");
                    assemblyCode.Add("D=A");
                    assemblyCode.Add($"@{segmentToMemory(segment)}");
                    assemblyCode.Add("A=M+D"); // Calculate the address to read from
                    assemblyCode.Add("D=M"); // Load the value at the address into D
                    assemblyCode.Add("@SP");
                    assemblyCode.Add("A=M");
                    assemblyCode.Add("M=D"); // SP points to the current top of the stack
                    assemblyCode.Add("@SP");
                    assemblyCode.Add("M=M+1"); // Increment SP
                    break;

                case "temp":
                    // Pushing from the temp segment
                    assemblyCode.Add($"@{index + 5}"); // Temp segments start at R5
                    assemblyCode.Add("D=M"); // Load the value at the address into D
                    assemblyCode.Add("@SP");
                    assemblyCode.Add("A=M");
                    assemblyCode.Add("M=D"); // SP points to the current top of the stack
                    assemblyCode.Add("@SP");
                    assemblyCode.Add("M=M+1"); // Increment SP
                    break;

                default:
                    // Handle other segments if needed
                    break;
            }

            return assemblyCode;
        }

        private List<string> TranslatePop(string[] parts)
        {
            var assemblyCode = new List<string>();

            string segment = parts[1];
            int index = int.Parse(parts[2]);

            // Calculate the target address
            assemblyCode.Add($"@{index}");
            assemblyCode.Add("D=A");

            if (segment == "local")
            {
                assemblyCode.Add("@LCL");
                assemblyCode.Add("A=M+D");
            }
            else if (segment == "argument")
            {
                assemblyCode.Add("@ARG");
                assemblyCode.Add("A=M+D");
            }
            else if (segment == "this")
            {
                assemblyCode.Add("@THIS");
                assemblyCode.Add("A=M+D");
            }
            else if (segment == "that")
            {
                assemblyCode.Add("@THAT");
                assemblyCode.Add("A=M+D");
            }

            // Store the address in R13 temporarily
            assemblyCode.Add("@R13");
            assemblyCode.Add("M=D");

            // Pop the top value from the stack and store it in the target address
            assemblyCode.Add("@SP");
            assemblyCode.Add("AM=M-1");
            assemblyCode.Add("D=M");
            assemblyCode.Add("@R13");
            assemblyCode.Add("A=M");
            assemblyCode.Add("M=D");

            return assemblyCode;
        }

        private string segmentToMemory(string segment)
        {
            switch (segment)
            {
                case "local":
                    return "LCL";
                case "argument":
                    return "ARG";
                case "this":
                    return "THIS";
                case "that":
                    return "THAT";
                default:
                    throw new ArgumentException($"Unknown segment: {segment}");
            }
        }

        private List<string> TranslateAdd()
        {
            var assemblyCode = new List<string>();

            // Pop the top two values from the stack
            assemblyCode.Add("@SP");
            assemblyCode.Add("AM=M-1"); // Decrement SP and load the address of the second value
            assemblyCode.Add("D=M"); // Load the second value into D
            assemblyCode.Add("A=A-1"); // Move back to the address of the first value
            assemblyCode.Add("M=M+D"); // Add the second value to the first value and store the result

            return assemblyCode;
        }


        private List<string> TranslateSub()
        {
            var assemblyCode = new List<string>();

            // Pop the top two values from the stack
            assemblyCode.Add("@SP");
            assemblyCode.Add("AM=M-1"); // Decrement SP and load the address of the second value
            assemblyCode.Add("D=M"); // Load the second value into D
            assemblyCode.Add("A=A-1"); // Move back to the address of the first value
            assemblyCode.Add("M=M-D"); // Subtract the second value from the first value and store the result

            return assemblyCode;
        }

        private List<string> TranslateNeg()
        {
            var assemblyCode = new List<string>();

            // Pop the top value from the stack
            assemblyCode.Add("@SP");
            assemblyCode.Add("A=M-1"); // Load the address of the top value
            assemblyCode.Add("M=-M"); // Negate the value at the top of the stack

            return assemblyCode;
        }

        private List<string> TranslateEq()
        {
            var assemblyCode = new List<string>();

            // Pop the top two values from the stack
            assemblyCode.Add("@SP");
            assemblyCode.Add("AM=M-1"); // Decrement SP and load the address of the second value
            assemblyCode.Add("D=M"); // Load the second value into D
            assemblyCode.Add("A=A-1"); // Move back to the address of the first value

            // Compare the two values and set the result (true or false) on the stack
            assemblyCode.Add("D=M-D"); // Compare the values (subtract the second from the first)
            assemblyCode.Add("@EQUAL"); // Label for equal (true)
            assemblyCode.Add("D;JEQ"); // If D is 0 (equal), jump to the EQUAL label
            assemblyCode.Add("D=0"); // If not equal, set D to 0 (false)
            assemblyCode.Add("@END_EQUAL");
            assemblyCode.Add("0;JMP"); // Unconditional jump to END_EQUAL
            assemblyCode.Add("(EQUAL)"); // Label for equal (true)
            assemblyCode.Add("D=-1"); // Set D to -1 (true)
            assemblyCode.Add("(END_EQUAL)");

            // Store the result (true or false) at the top of the stack
            assemblyCode.Add("@SP");
            assemblyCode.Add("A=M-1"); // Move to the top of the stack
            assemblyCode.Add("M=D"); // Store the result at the top of the stack

            return assemblyCode;
        }

        private List<string> TranslateGt()
        {
            var assemblyCode = new List<string>();

            // Pop the top two values from the stack
            assemblyCode.Add("@SP");
            assemblyCode.Add("AM=M-1"); // Decrement SP and load the address of the second value
            assemblyCode.Add("D=M"); // Load the second value into D
            assemblyCode.Add("A=A-1"); // Move back to the address of the first value

            // Compare the two values and set the result (true or false) on the stack
            assemblyCode.Add("D=M-D"); // Compare the values (subtract the second from the first)
            assemblyCode.Add("@GREATER_THAN"); // Label for greater than (true)
            assemblyCode.Add("D;JGT"); // If D is greater than 0 (greater than), jump to the GREATER_THAN label
            assemblyCode.Add("D=0"); // If not greater than, set D to 0 (false)
            assemblyCode.Add("@END_GREATER_THAN");
            assemblyCode.Add("0;JMP"); // Unconditional jump to END_GREATER_THAN
            assemblyCode.Add("(GREATER_THAN)"); // Label for greater than (true)
            assemblyCode.Add("D=-1"); // Set D to -1 (true)
            assemblyCode.Add("(END_GREATER_THAN)");

            // Store the result (true or false) at the top of the stack
            assemblyCode.Add("@SP");
            assemblyCode.Add("A=M-1"); // Move to the top of the stack
            assemblyCode.Add("M=D"); // Store the result at the top of the stack

            return assemblyCode;
        }

        private List<string> TranslateLt()
        {
            var assemblyCode = new List<string>();

            // Pop the top two values from the stack
            assemblyCode.Add("@SP");
            assemblyCode.Add("AM=M-1"); // Decrement SP and load the address of the second value
            assemblyCode.Add("D=M"); // Load the second value into D
            assemblyCode.Add("A=A-1"); // Move back to the address of the first value

            // Compare the two values and set the result (true or false) on the stack
            assemblyCode.Add("D=M-D"); // Compare the values (subtract the second from the first)
            assemblyCode.Add("@LESS_THAN"); // Label for less than (true)
            assemblyCode.Add("D;JLT"); // If D is less than 0 (less than), jump to the LESS_THAN label
            assemblyCode.Add("D=0"); // If not less than, set D to 0 (false)
            assemblyCode.Add("@END_LESS_THAN");
            assemblyCode.Add("0;JMP"); // Unconditional jump to END_LESS_THAN
            assemblyCode.Add("(LESS_THAN)"); // Label for less than (true)
            assemblyCode.Add("D=-1"); // Set D to -1 (true)
            assemblyCode.Add("(END_LESS_THAN)");

            // Store the result (true or false) at the top of the stack
            assemblyCode.Add("@SP");
            assemblyCode.Add("A=M-1"); // Move to the top of the stack
            assemblyCode.Add("M=D"); // Store the result at the top of the stack

            return assemblyCode;
        }

        private List<string> TranslateAnd()
        {
            var assemblyCode = new List<string>();

            // Pop the top two values from the stack
            assemblyCode.Add("@SP");
            assemblyCode.Add("AM=M-1"); // Decrement SP and load the address of the second value
            assemblyCode.Add("D=M"); // Load the second value into D
            assemblyCode.Add("A=A-1"); // Move back to the address of the first value

            // Perform the AND operation and store the result at the top of the stack
            assemblyCode.Add("M=M&D"); // Perform bitwise AND operation and store the result

            return assemblyCode;
        }

        private List<string> TranslateOr()
        {
            var assemblyCode = new List<string>();

            // Pop the top two values from the stack
            assemblyCode.Add("@SP");
            assemblyCode.Add("AM=M-1"); // Decrement SP and load the address of the second value
            assemblyCode.Add("D=M"); // Load the second value into D
            assemblyCode.Add("A=A-1"); // Move back to the address of the first value

            // Perform the OR operation and store the result at the top of the stack
            assemblyCode.Add("M=M|D"); // Perform bitwise OR operation and store the result

            return assemblyCode;
        }

        private List<string> TranslateNot()
        {
            var assemblyCode = new List<string>();

            // Access the top value on the stack
            assemblyCode.Add("@SP");
            assemblyCode.Add("A=M-1"); // Load the address of the top value

            // Perform the NOT operation and store the result at the top of the stack
            assemblyCode.Add("M=!M"); // Perform bitwise NOT operation and store the result

            return assemblyCode;
        }
    }
}
