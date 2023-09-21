using VmTranslator.Console;

namespace VmTranslator.Test
{
    internal class FileHandlerTest
    {
        private FileHandler _fileHandler;
        [SetUp]
        public void SetUp() 
        {
            _fileHandler = new FileHandler();
        }

        [Test]
        public void ReadAllLines_ValidFile_ReturnsList()
        {
            // Arrange
            string filePath = "valid_vm_file";
            File.WriteAllLines($"{filePath}.vm", new[] { "line1", "line2", "line3" });

            // Act
            List<string> lines = _fileHandler.ReadAllLines(filePath);

            // Assert
            Assert.AreEqual(3, lines.Count);
            Assert.AreEqual("line1", lines[0]);
            Assert.AreEqual("line2", lines[1]);
            Assert.AreEqual("line3", lines[2]);
        }

        [Test]
        public void SaveAllLines_ValidFile_SavesFile()
        {
            // Arrange
            string filePath = "valid_asm_file";
            List<string> linesToSave = new List<string> { "asmLine1", "asmLine2" };

            // Act
            _fileHandler.SaveAllLines(filePath, linesToSave);

            // Assert
            Assert.IsTrue(File.Exists($"{filePath}.asm"));
            string[] savedLines = File.ReadAllLines($"{filePath}.asm");
            Assert.AreEqual(2, savedLines.Length);
            Assert.AreEqual("asmLine1", savedLines[0]);
            Assert.AreEqual("asmLine2", savedLines[1]);
        }

        [TearDown]
        public void Cleanup()
        {
            // Clean up any created files after each test
            File.Delete("valid_vm_file.vm");
            File.Delete("valid_asm_file.asm");
        }
    }
}
