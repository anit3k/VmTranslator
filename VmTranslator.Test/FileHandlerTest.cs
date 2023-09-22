using VmTranslator.Domain;

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
        [TestCase("AddSimple.vm")]
        [TestCase("valid_vm_file.vm")]
        public void ReadAllLines_ValidFile_ReturnsList(string path)
        {
            // Arrange
            File.WriteAllLines($"{path}", new[] { "push constant 66", "@sp", "add" });

            // Act
            List<string> lines = _fileHandler.ReadAllLines(path);

            // Assert
            Assert.AreEqual(3, lines.Count);
            Assert.AreEqual("push constant 66", lines[0]);
            Assert.AreEqual("@sp", lines[1]);
            Assert.AreEqual("add", lines[2]);
        }

        [Test]
        [TestCase("AddSimple.txt")]
        [TestCase("valid_vm_file.bin")]
        public void ReadAllLines_NotValidFile_ThrowsException(string path)
        {
            // Arrange: Create the test file with the specified name
            File.WriteAllText(path, "This is not a valid VM file content");

            // Act & Assert: Use the ExpectedException attribute to specify the expected exception
            Assert.Throws<IOException>(() => _fileHandler.ReadAllLines(path), "File is not found or the file is not of type .vm");
        }

        [Test]
        [TestCase("AddSimple.vm")]
        [TestCase("valid_vm_file.vm")]
        public void SaveAllLines_ValidFile_SavesFile(string path)
        {
            // Arrange
            List<string> linesToSave = new List<string> { "asmLine1", "asmLine2" };

            // Act
            _fileHandler.SaveAllLines(path, linesToSave);

            // Calculate the expected ASM file path based on the VM file path
            string expectedAsmPath = Path.ChangeExtension(path, "asm");

            // Assert
            Assert.IsTrue(File.Exists(expectedAsmPath));
            string[] savedLines = File.ReadAllLines(expectedAsmPath);
            Assert.AreEqual(2, savedLines.Length);
            Assert.AreEqual("asmLine1", savedLines[0]);
            Assert.AreEqual("asmLine2", savedLines[1]);
        }


        [TearDown]
        public void Cleanup()
        {
            // Clean up any created files after each test
            File.Delete("valid_vm_file.vm");
            File.Delete("AddSimple.vm");
            File.Delete("valid_asm_file.asm");
        }
    }
}
