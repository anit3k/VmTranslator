namespace VmTranslator.Console
{
    public class ParseHandler
    {
        public List<string> TranslateVmToAssembly(List<string> vmCode)
        {
            var assemblyCode = new List<string>();

            foreach (var line in vmCode)
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
            int index = int.Parse(parts[2]); // Assuming the index is the third part

            switch (segment)
            {
                case "local":
                case "argument":
                case "this":
                case "that":
                    // Popping to a memory segment
                    assemblyCode.Add($"@{index}");
                    assemblyCode.Add("D=A");
                    assemblyCode.Add($"@{segmentToMemory(segment)}");
                    assemblyCode.Add("D=M+D"); // Calculate the address to write to
                    assemblyCode.Add("@R13"); // Use R13 as a temporary register to store the address
                    assemblyCode.Add("M=D"); // Store the address in R13
                    assemblyCode.Add("@SP");
                    assemblyCode.Add("M=M-1"); // Decrement SP
                    assemblyCode.Add("A=M");
                    assemblyCode.Add("D=M"); // Load the value at the top of the stack into D
                    assemblyCode.Add("@R13"); // Load the address from R13
                    assemblyCode.Add("A=M"); // Go to the calculated address
                    assemblyCode.Add("M=D"); // Store the value in memory
                    break;

                case "temp":
                    // Popping to the temp segment
                    assemblyCode.Add("@SP");
                    assemblyCode.Add("M=M-1"); // Decrement SP
                    assemblyCode.Add("A=M");
                    assemblyCode.Add("D=M"); // Load the value at the top of the stack into D
                    assemblyCode.Add($"@{index + 5}"); // Temp segments start at R5
                    assemblyCode.Add("M=D"); // Store the value in the temp segment
                    break;

                default:
                    // Handle other segments if needed
                    break;
            }

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

            // Implement translation logic for sub command

            return assemblyCode;
        }

        private List<string> TranslateNeg()
        {
            var assemblyCode = new List<string>();

            // Implement translation logic for sub command

            return assemblyCode;
        }
        private List<string> TranslateEq()
        {
            var assemblyCode = new List<string>();

            // Implement translation logic for sub command

            return assemblyCode;
        }

        private List<string> TranslateGt()
        {
            var assemblyCode = new List<string>();

            // Implement translation logic for sub command

            return assemblyCode;
        }

        private List<string> TranslateLt()
        {
            var assemblyCode = new List<string>();

            // Implement translation logic for sub command

            return assemblyCode;
        }

        private List<string> TranslateAnd()
        {
            var assemblyCode = new List<string>();

            // Implement translation logic for sub command

            return assemblyCode;
        }

        private List<string> TranslateOr()
        {
            var assemblyCode = new List<string>();

            // Implement translation logic for sub command

            return assemblyCode;
        }

        private List<string> TranslateNot()
        {
            var assemblyCode = new List<string>();

            // Implement translation logic for sub command

            return assemblyCode;
        }        
    }
}
