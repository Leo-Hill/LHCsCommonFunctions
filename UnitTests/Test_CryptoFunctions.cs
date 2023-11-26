using LHCommonFunctions.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Functions
{
    [TestClass]
    public class Test_CryptoFunctions
    {
        private static String _testFilePath = System.Environment.CurrentDirectory + "\\Temp";

        [ClassCleanup]
        public static void Cleanup()
        {
            Directory.Delete(_testFilePath, true);
        }

        [TestMethod]
        public void AesFileEncryptionDecryptionRoundtrip()
        {
            String originalFile = "..\\..\\..\\Resources\\Assets\\TestHexFile.hex";
            String encryptedFile = _testFilePath + "\\Encrypted";
            String decryptedFile = _testFilePath + "\\Decrypted";

            String password = "Sepp";
            String salt = "Werner";
            int numOfIterations = 35;

            Directory.CreateDirectory(_testFilePath);

            LHCryptoFunctions.AesEncrypt(originalFile, encryptedFile, password, salt, numOfIterations);
            LHCryptoFunctions.AesDecrypt(encryptedFile, decryptedFile, password, salt, numOfIterations);

            StreamReader originalFileReader = new StreamReader(originalFile);
            String originalContent = originalFileReader.ReadToEnd();
            originalFileReader.Close();

            StreamReader decryptedFileReader = new StreamReader(decryptedFile);
            String decryptedContent = decryptedFileReader.ReadToEnd();
            decryptedFileReader.Close();

            Assert.AreEqual(originalContent, decryptedContent);
        }
    }
}
