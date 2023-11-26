using LHCommonFunctions.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Classes
{
    [TestClass]
    public class Test_LogFile
    {
        private String _testFilePath = System.Environment.CurrentDirectory + "\\Temp";
 
        [TestCleanup()]
        public void Cleanup()
        {
            Directory.Delete(_testFilePath, true);
        }

        [TestMethod]
        public void LoggingWorks()
        {
            Class_LogFile _logFile = new Class_LogFile("TestLog", _testFilePath);
            _logFile.Log("TestMessage", false);
            StreamReader streamReader = new StreamReader(_logFile.FileName);
            String[] content = streamReader.ReadToEnd().Split(Environment.NewLine);
            String[] lineElements = content[0].Split("\t");
            streamReader.Close();
            File.Delete(_logFile.FileName);


            DateTime dateTimeLogMessage = DateTime.ParseExact(lineElements[0], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            DateTime dateTimeNow = DateTime.Now;
            TimeSpan dateTimeDifference = dateTimeLogMessage - dateTimeNow;

            Assert.AreEqual("TestMessage", lineElements[1]);
            Assert.IsTrue(dateTimeDifference.TotalMilliseconds < 5000);

        }

        [TestMethod]
        public void LoggingToBufferWorks()
        {
            Class_LogFile _logFile = new("TestLogToBuffer", _testFilePath);
            _logFile.LogToBuffer("TestMessage", false);
            _logFile.WriteOutBuffer();
            StreamReader streamReader = new StreamReader(_logFile.FileName);
            String[] content = streamReader.ReadToEnd().Split(Environment.NewLine);
            String[] lineElements = content[0].Split("\t");
            streamReader.Close();
            File.Delete(_logFile.FileName);


            DateTime dateTimeLogMessage = DateTime.ParseExact(lineElements[0], "dd.MM.yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            DateTime dateTimeNow = DateTime.Now;
            TimeSpan dateTimeDifference = dateTimeLogMessage - dateTimeNow;

            Assert.IsTrue(dateTimeDifference.TotalMilliseconds < 5000);
            Assert.AreEqual("TestMessage", lineElements[1]);
        }


    }
}
