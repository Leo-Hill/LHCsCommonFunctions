using LHCommonFunctions.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace Classes
{
    [TestClass]
    public class Test_IntelHex
    {

        private Class_IntelHex _intelHex;

        public Test_IntelHex()
        {
            Assembly? assembly = Assembly.GetExecutingAssembly();
            Stream? stream = assembly.GetManifestResourceStream("UnitTests.Resources.Assets.TestHexFile.hex");
            if(stream==null) {
                throw new Exception();
            }
            StreamReader textStreamReader = new StreamReader(stream);
            String content = textStreamReader.ReadToEnd();
            _intelHex = new Class_IntelHex(content);
        }

        [TestMethod]
        public void IntelHexReturnsCorrectNumberOfDataBlocks()
        {
            Assert.AreEqual(4, _intelHex.GetDataBlocks().Count);
            Assert.AreEqual(9, _intelHex.GetBytes(0x08004000, 0x08004008).Length);
        }

        [TestMethod]
        public void IntelHexReturnsCorrectContent()
        {
            Assert.AreEqual(80, _intelHex.GetBytes(0x08004000, 0xFFFFFFFF).Length);
            Assert.AreEqual(16, _intelHex.GetBytes(0x08004000, 0x08004010).Length);
        }

        [TestMethod]
        public void IntelHexReturnsCorrectPayloadSize()
        {
            Assert.AreEqual(80, _intelHex.GetPayloadSize());
        }
    }
}