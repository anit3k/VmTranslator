using Autofac;
using VmTranslator.Domain.interfaces;

namespace VmTranslator.ConsoleApplication
{
    /// <summary>
    /// Handles the execution of the UI.
    /// </summary>
    public class Runner
    {
        #region Fields
        private readonly IContainer _container;
        private readonly string _inputFolder;
        #endregion

        #region Constructor
        public Runner(IContainer container)
        {
            _container = container;
            _inputFolder = Path.Combine(Directory.GetCurrentDirectory(), "InputFiles");
        }
        #endregion


        #region Methods

        /// <summary>
        /// Runs the while loop, as long as the user needs to translate all desired files.
        /// </summary>
        public void ExecuteProgram()
        {
            while (true)
            {
                Console.Clear();
                string[] vmFiles = Directory.GetFiles(_inputFolder, "*.vm");

                if (vmFiles.Length == 0)
                {
                    Console.WriteLine($"No .vm files found in the '{_inputFolder}' folder.");
                    return;
                }

                DisplayAvailableVmFiles(vmFiles);

                if (TryGetUserSelection(out int selectedFileIndex, vmFiles.Length))
                {
                    if (selectedFileIndex == 0)
                    {
                        Console.Clear();
                        break; // Quit the program
                    }

                    string selectedFilePath = vmFiles[selectedFileIndex - 1];
                    TranslateVmFile(selectedFilePath);

                    DisplayTranslationCompleteMessage(selectedFilePath);
                    WaitForUserInputToContinue();
                }
                else
                {
                    Console.WriteLine("Invalid input. Please select a valid file number or 'q' to quit.");
                }
            }

            Console.WriteLine("Program terminated");
        }

        /// <summary>
        /// Diplays all the VM files in the input folder for the users, to select from.
        /// </summary>
        /// <param name="vmFiles">The list of files to translate from.</param>
        private void DisplayAvailableVmFiles(string[] vmFiles)
        {
            Console.WriteLine("Available .vm files in the 'InputFiles' folder:");
            for (int i = 0; i < vmFiles.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(vmFiles[i])}");
            }
        }

        /// <summary>
        /// Read the key input of the user, if it matches a file number, run translation for selected fil.
        /// </summary>
        /// <param name="selectedFileIndex">The file index of the selected file</param>
        /// <param name="maxIndex">The total amount of files to select</param>
        /// <returns>As long as user chooses valid options, return true, else keep waiting for a correct key</returns>
        private bool TryGetUserSelection(out int selectedFileIndex, int maxIndex)
        {
            Console.Write("Please enter the number of the file you want to translate (or 'q' to quit): ");

            ConsoleKeyInfo info = Console.ReadKey();
            string userInput = Convert.ToString(info.KeyChar).ToLower();

            if (userInput == "q")
            {
                selectedFileIndex = 0;
                return true;
            }

            if (int.TryParse(userInput, out selectedFileIndex) && selectedFileIndex >= 1 && selectedFileIndex <= maxIndex)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Runs the translation for the selected file, using Dependency Injection.
        /// </summary>
        /// <param name="filePath">The file path for the selected file to translate.</param>
        private void TranslateVmFile(string filePath)
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                var translator = scope.Resolve<IExecuteTranslator>();
                translator.Run(filePath);
            }
        }

        /// <summary>
        /// Give user information that translation has been handled correctly.
        /// </summary>
        /// <param name="filePath">The filepath to say has been translated.</param>
        private void DisplayTranslationCompleteMessage(string filePath)
        {
            Console.Clear();
            Console.WriteLine($"Translation of '{Path.GetFileName(filePath)}' is complete.");
        }

        /// <summary>
        /// Wait's for a user input to go back to the while loop.
        /// </summary>
        private void WaitForUserInputToContinue()
        {
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }        
        #endregion
    }
}
