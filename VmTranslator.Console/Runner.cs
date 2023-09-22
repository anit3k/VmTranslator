using Autofac;
using VmTranslator.Domain.interfaces;

namespace VmTranslator.ConsoleApplication
{
    public class Runner
    {
        private readonly IContainer _container;
        private readonly string _inputFolder;

        public Runner(IContainer container)
        {
            _container = container;
            _inputFolder = Path.Combine(Directory.GetCurrentDirectory(), "InputFiles");
        }

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


        private void DisplayAvailableVmFiles(string[] vmFiles)
        {
            Console.WriteLine("Available .vm files in the 'InputFiles' folder:");
            for (int i = 0; i < vmFiles.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(vmFiles[i])}");
            }
        }

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

        private void TranslateVmFile(string filePath)
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                var translator = scope.Resolve<IExecuteTranslator>();
                translator.Run(filePath);
            }
        }

        private void DisplayTranslationCompleteMessage(string filePath)
        {
            Console.Clear();
            Console.WriteLine($"Translation of '{Path.GetFileName(filePath)}' is complete.");
        }

        private void WaitForUserInputToContinue()
        {
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }        
    }
}
