using LHCommonFunctions.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Serialization;

namespace Functions
{
    [TestClass]
    public class Test_CalculationFunctions
    {
        [TestMethod]
        public void Crc32CaluclationWorksCorrectrly()
        {
            byte[] data = { 1, 2, 3, 4 };
            UInt32 result = LHCalculationFunctions.CalculateCrc(data);
            Assert.AreEqual((UInt32)0x793737CD, result);

            byte[] data2 = { 0xAA, 0xED, 0x1A, 0x45, 0x75, 0x2F, 0x73, 0x8A };
            result = LHCalculationFunctions.CalculateCrc(data2);
            Assert.AreEqual((UInt32)0x7723F97B, result);
        }

        [TestMethod]
        public void DoubleHasDecimalsWorksCorrectly()
        {
            Assert.IsTrue(LHCalculationFunctions.DoubleHasDecimals(1.234));
            Assert.IsFalse(LHCalculationFunctions.DoubleHasDecimals(1.0));

            Assert.IsFalse(LHCalculationFunctions.DoubleHasDecimals(1.00001, 4));
            Assert.IsTrue(LHCalculationFunctions.DoubleHasDecimals(1.00001, 5));
        }

        [TestMethod]
        public void CmToPtWorksCorrectly()
        {
            Assert.AreEqual(28.3465, LHCalculationFunctions.CmToPt(1),0.001);
            Assert.AreEqual(1927.5590, LHCalculationFunctions.CmToPt(68), 0.001);
        }
    }
}
