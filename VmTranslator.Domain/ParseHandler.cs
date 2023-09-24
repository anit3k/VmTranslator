using System.IO;

namespace VmTranslator.Domain
{
    /// <summary>
    /// A class that is in charge of the translations.
    /// </summary>
    public class ParseHandler
    {
        #region fields
        private List<string> _vmCode;
        private readonly List<string> _assemblyCode;
        private int _labelCounter = 0;

        private const string _constantSegment = "constant";
        private const string _localSegment = "local";
        private const string _argumentSegment = "argument";
        private const string _thisSegment = "this";
        private const string _thatSegment = "that";
        private const string _tempSegment = "temp";
        #endregion

        #region Constructor
        public ParseHandler()
        {
            _assemblyCode = new List<string>();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Translates the lines received by calling the "TranslateCurrentLineToAsmCommands" method for each line.
        /// </summary>
        /// <param name="vmCode">All the lines gotten from the file we wish to translate.</param>
        /// <returns>Returns a list of strings containing all the new code as assembly.</returns>
        public List<string> TranslateVmToAssembly(List<string> vmCode)
        {
            _vmCode = vmCode;

            foreach (var line in _vmCode)
            {
                TranslateCurrentLineToAsmCommands(line);
            }

            return _assemblyCode;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Splits all the lines into parts on the spaces, so we know each space seperates a command.
        /// Then we know part 0 is the command, so we look at that index, and run it through a switch case, of all possible commands, and call the method to handle that command.
        /// </summary>
        /// <param name="line">The line we want to look at to identify the command.</param>
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

        /// <summary>
        /// Add the predefined assemlby code lines to the result, if the method is called.
        /// Handles the push command, and translates it to assembly, by looking at the parts of the code.
        /// We know part 1, is segments. And part 2 is index's.
        /// We then run the parts for the segment through the switch case to know how to handle the segment code, and give back the respective code.
        /// </summary>
        /// <param name="parts">The parts contains the different commands, like segments and index's and commands, for the passed line.</param>
        private void TranslatePush(string[] parts)
        {
            string segment = parts[1];
            int index = int.Parse(parts[2]);

            switch (segment)
            {
                case _constantSegment:
                    Emit(
                        $"@{index}",
                        "D=A"
                    );
                    PushDToStack();
                    break;
                case _localSegment:
                case _argumentSegment:
                case _thisSegment:
                case _thatSegment:
                    Emit(
                        $"@{index}",
                        "D=A",
                        $"@{SegmentToMemory(segment, parts)}",
                        "A=M+D",
                        "D=M"
                    );
                    PushDToStack();
                    break;
                case _tempSegment:
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

        /// <summary>
        /// Add the predefined assemlby code lines to the result, if the method is called.
        /// </summary>
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

        /// <summary>
        /// Add the predefined assemlby code lines to the result, if the method is called.
        /// </summary>
        /// <param name="parts">Contains the commands, segments, and indexs for the parsed line.</param>
        private void TranslatePop(string[] parts)
        {
            string segment = parts[1];
            int index = int.Parse(parts[2]);

            Emit(
                $"@{index}",
                "D=A",
                $"@{SegmentToMemory(segment, parts)}",
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

        /// <summary>
        /// Handles the Segment code, by looking at the segment passed on, and adding the needed assembly code for the segment to the result list.
        /// </summary>
        /// <param name="segment">The segment we are checking for type.</param>
        /// <param name="parts">Looks for the parts like "this" and "that" for the pointer to be able to understand.</param>
        /// <returns>Every case returns a string that get's added to the result line.</returns>
        /// <exception cref="ArgumentException">Throws an exception if it's not part of the segment switch, since it's not correct syntax otherwise.</exception>
        private string SegmentToMemory(string segment, string[] parts)
        {
            switch (segment)
            {
                case _localSegment:
                    return "LCL";
                case _argumentSegment:
                    return "ARG";
                case _thisSegment:
                    return "THIS";
                case _thatSegment:
                    return "THAT";
                case _tempSegment:
                    return "R5";
                case "pointer":
                    // "pointer 0" corresponds to "THIS"
                    // "pointer 1" corresponds to "THAT"
                    return (2 + int.Parse(parts[2])).ToString();
                case "static":
                    return $"STATIC_{parts[2]}";
                default:
                    throw new ArgumentException($"Unknown segment: {segment}");
            }
        }

        /// <summary>
        /// Add the predefined assemlby code lines to the result, if the method is called.
        /// </summary>
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

        /// <summary>
        /// Add the predefined assemlby code lines to the result, if the method is called.
        /// </summary>
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

        /// <summary>
        /// Add the predefined assemlby code lines to the result, if the method is called.
        /// </summary>
        private void TranslateNeg()
        {
            Emit(
                "@SP",
                "A=M-1", // Load the address of the top value
                "M=-M" // Negate the value at the top of the stack
            );
        }

        /// <summary>
        /// Handles the EQUAL keyword, and adds the correct assembly code to the result, based on the jump conditions passed in.
        /// </summary>
        /// <param name="jumpCondition">The jump conditions we need to check up against.</param>
        private void TranslateComparison(string jumpCondition)
        {
            string trueLabel = "EQUAL";
            string endLabel = "END_EQUAL";

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

        /// <summary>
        /// Add the predefined assemlby code lines to the result, if the method is called.
        /// </summary>
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

        /// <summary>
        /// Add the predefined assemlby code lines to the result, if the method is called.
        /// </summary>
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

        /// <summary>
        /// Add the predefined assemlby code lines to the result, if the method is called.
        /// </summary>
        private void TranslateNot()
        {
            Emit(
                "@SP",
                "A=M-1", // Load the address of the top value
                "M=!M" // Perform bitwise NOT operation and store the result
            );
        }

        /// <summary>
        /// Adds the instructions in the assembly version to the _assemblyCode variable, which we use as our result.
        /// </summary>
        /// <param name="asmInstructions">The instructions are the lines we write when we call "Emit" that we wish to add to the _assemblyCode variable.</param>
        private void Emit(params string[] asmInstructions)
        {
            _assemblyCode.AddRange(asmInstructions);
        }
        #endregion
    }
}
