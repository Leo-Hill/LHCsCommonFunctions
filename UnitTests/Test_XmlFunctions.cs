using LHCommonFunctions.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace Functions
{
    [TestClass]
    public class Test_XmlFunctions
    {

        private String _testFilePath = System.Environment.CurrentDirectory + "\\Temp";

        [TestCleanup()]
        public void Cleanup()
        {
            if (Directory.Exists(_testFilePath))
            {
                Directory.Delete(_testFilePath, true);
            }
        }

        [TestMethod]
        public void CreateXmlSettingsFileWorksCorrectly()
        {
            LHXmlFunctions.CreateXmlSettingsFile(_testFilePath + "\\TestFile.xml");

            StreamReader streamReader = new StreamReader(_testFilePath + "\\TestFile.xml");
            String[] xmlStrings = streamReader.ReadToEnd().Split("\r\n");
            streamReader.Close();
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>", xmlStrings[0]);
            Assert.AreEqual("<rootElement />", xmlStrings[1]);
        }

        [TestMethod]
        public void ExportDictionaryToXmlWorksCorrectly()
        {
            SortedDictionary<String, String> export = new SortedDictionary<String, String>();
            export.Add("Test1", "Value1");
            export.Add("Test2", "Value2");
            export.Add("Test3", "Value3");

            LHXmlFunctions.ExportDictionaryToXml(export, _testFilePath + "\\TestFile.xml");

            StreamReader streamReader = new StreamReader(_testFilePath + "\\TestFile.xml");
            String[] xmlStrings = streamReader.ReadToEnd().Split("\r\n");
            streamReader.Close();
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?>", xmlStrings[0]);
            Assert.AreEqual("<rootElement>", xmlStrings[1]);
            Assert.AreEqual("  <item id=\"Test1\">Value1</item>", xmlStrings[2]);
            Assert.AreEqual("  <item id=\"Test2\">Value2</item>", xmlStrings[3]);
            Assert.AreEqual("  <item id=\"Test3\">Value3</item>", xmlStrings[4]);
        }

        [TestMethod]
        public void ImportDictionaryToXmlWorksCorrectly()
        {
            SortedDictionary<String, String> dict = LHXmlFunctions.ImportXmlToDictionary("..\\..\\..\\Resources\\Assets\\TestXmlFile.xml");
            Assert.AreEqual("Value1", dict["Test1"]);
            Assert.AreEqual("Value2", dict["Test2"]);
            Assert.AreEqual("Value3", dict["Test3"]);
        }
    }
}
