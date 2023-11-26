using LHCommonFunctions.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions
{
    [TestClass]
    public class Test_MemoryFunctions
    {
        [TestMethod]
        public void CopyFromBigEndianArrayCopiesUint16Correctly()
        {
            byte[] data = new byte[] { 0xFF, 0x12, 0x34 };
            UInt16 result = (UInt16)LHMemoryFunctions.CopyFromBigEndianArray(data, 1, typeof(UInt16));
            Assert.AreEqual((UInt16)0x1234, result);
        }

        [TestMethod]
        public void CopyFromBigEndianArrayCopiesInt16Correctly()
        {
            byte[] data = new byte[] { 0xFF, 0xF2, 0x34 };
            Int16 result = (Int16)LHMemoryFunctions.CopyFromBigEndianArray(data, 1, typeof(Int16));
            Assert.AreEqual((Int16)(-0xDCC), result);
        }

        [TestMethod]
        public void CopyFromBigEndianArrayCopiesUint32Correctly()
        {
            byte[] data = new byte[] { 0xFF, 0x12, 0x34, 0x56, 0x78 };
            UInt32 result = (UInt32)LHMemoryFunctions.CopyFromBigEndianArray(data, 1, typeof(UInt32));
            Assert.AreEqual((UInt32)0x12345678, result);
        }

        [TestMethod]
        public void CopyFromBigEndianArrayCopiesInt32Correctly()
        {
            byte[] data = new byte[] { 0xFF, 0xF2, 0x34, 0x56, 0x78 };
            Int32 result = (Int32)LHMemoryFunctions.CopyFromBigEndianArray(data, 1, typeof(Int32));
            Assert.AreEqual((Int32)(-0xDCBA988), result);
        }

        [TestMethod]
        public void CopyFromBigEndianArrayCopiesUint64Correctly()
        {
            byte[] data = new byte[] { 0xFF, 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
            UInt64 result = (UInt64)LHMemoryFunctions.CopyFromBigEndianArray(data, 1, typeof(UInt64));
            Assert.AreEqual((UInt64)0x123456789ABCDEF0, result);
        }

        [TestMethod]
        public void CopyFromBigEndianArrayCopiesInt64Correctly()
        {
            byte[] data = new byte[] { 0xFF, 0xF2, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
            Int64 result = (Int64)LHMemoryFunctions.CopyFromBigEndianArray(data, 1, typeof(Int64));
            Assert.AreEqual((Int64)(-0xDCBA98765432110), result);
        }

        [TestMethod]
        public void CopyDataToArrayLittleEndianCopiesUint16Correctly()
        {
            UInt16 value = 0xF234;
            byte[] data = new byte[12];
            LHMemoryFunctions.CopyToArrayLittleEndian(value, data, 4);
            Assert.AreEqual(data[4], 0x34);
            Assert.AreEqual(data[5], 0xF2);
        }

        [TestMethod]
        public void CopyDataToArrayLittleEndianCopiesInt16Correctly()
        {
            Int16 value = 0x1234;
            byte[] data = new byte[12];
            LHMemoryFunctions.CopyToArrayLittleEndian(value, data, 4);
            Assert.AreEqual(data[4], 0x34);
            Assert.AreEqual(data[5], 0x12);
        }

        [TestMethod]
        public void CopyDataToArrayLittleEndianCopiesUint32Correctly()
        {
            UInt32 value = 0xF2345678;
            byte[] data = new byte[12];
            LHMemoryFunctions.CopyToArrayLittleEndian(value, data, 4);
            Assert.AreEqual(data[4], 0x78);
            Assert.AreEqual(data[5], 0x56);
            Assert.AreEqual(data[6], 0x34);
            Assert.AreEqual(data[7], 0xF2);
        }

        [TestMethod]
        public void CopyDataToArrayLittleEndianCopiesInt32Correctly()
        {
            Int32 value = 0x12345678;
            byte[] data = new byte[12];
            LHMemoryFunctions.CopyToArrayLittleEndian(value, data, 4);
            Assert.AreEqual(data[4], 0x78);
            Assert.AreEqual(data[5], 0x56);
            Assert.AreEqual(data[6], 0x34);
            Assert.AreEqual(data[7], 0x12);
        }

        [TestMethod]
        public void CopyDataToArrayLittleEndianCopiesUint64Correctly()
        {
            UInt64 value = 0xF23456789ABCDEF0;
            byte[] data = new byte[12];
            LHMemoryFunctions.CopyToArrayLittleEndian(value, data, 4);
            Assert.AreEqual(data[4], 0xF0);
            Assert.AreEqual(data[5], 0xDE);
            Assert.AreEqual(data[6], 0xBC);
            Assert.AreEqual(data[7], 0x9A);
            Assert.AreEqual(data[8], 0x78);
            Assert.AreEqual(data[9], 0x56);
            Assert.AreEqual(data[10], 0x34);
            Assert.AreEqual(data[11], 0xF2);
        }

        [TestMethod]
        public void CopyDataToArrayLittleEndianCopiesInt64Correctly()
        {
            UInt64 value = 0x123456789ABCDEF0;
            byte[] data = new byte[12];
            LHMemoryFunctions.CopyToArrayLittleEndian(value, data, 4);
            Assert.AreEqual(data[4], 0xF0);
            Assert.AreEqual(data[5], 0xDE);
            Assert.AreEqual(data[6], 0xBC);
            Assert.AreEqual(data[7], 0x9A);
            Assert.AreEqual(data[8], 0x78);
            Assert.AreEqual(data[9], 0x56);
            Assert.AreEqual(data[10], 0x34);
            Assert.AreEqual(data[11], 0x12);
        }

        [TestMethod]
        public void CopyDataToArrayBigEndianCopiesUint16Correctly()
        {
            UInt16 value = 0xF234;
            byte[] data = new byte[12];
            LHMemoryFunctions.CopyToArrayBigEndian(value, data, 4);
            Assert.AreEqual(data[4], 0xF2);
            Assert.AreEqual(data[5], 0x34);
        }

        [TestMethod]
        public void CopyDataToArrayBigEndianCopiesInt16Correctly()
        {
            Int16 value = 0x1234;
            byte[] data = new byte[12];
            LHMemoryFunctions.CopyToArrayBigEndian(value, data, 4);
            Assert.AreEqual(data[4], 0x12);
            Assert.AreEqual(data[5], 0x34);
        }

        [TestMethod]
        public void CopyDataToArrayBigEndianCopiesUint32Correctly()
        {
            UInt32 value = 0xF2345678;
            byte[] data = new byte[12];
            LHMemoryFunctions.CopyToArrayBigEndian(value, data, 4);
            Assert.AreEqual(data[4], 0xF2);
            Assert.AreEqual(data[5], 0x34);
            Assert.AreEqual(data[6], 0x56);
            Assert.AreEqual(data[7], 0x78);
        }

        [TestMethod]
        public void CopyDataToArrayBigEndianCopiesInt32Correctly()
        {
            Int32 value = 0x12345678;
            byte[] data = new byte[12];
            LHMemoryFunctions.CopyToArrayBigEndian(value, data, 4);
            Assert.AreEqual(data[4], 0x12);
            Assert.AreEqual(data[5], 0x34);
            Assert.AreEqual(data[6], 0x56);
            Assert.AreEqual(data[7], 0x78);
        }

        [TestMethod]
        public void CopyDataToArrayBigEndianCopiesUint64Correctly()
        {
            UInt64 value = 0xF23456789ABCDEF0;
            byte[] data = new byte[12];
            LHMemoryFunctions.CopyToArrayBigEndian(value, data, 4);
            Assert.AreEqual(data[4], 0xF2);
            Assert.AreEqual(data[5], 0x34);
            Assert.AreEqual(data[6], 0x56);
            Assert.AreEqual(data[7], 0x78);
            Assert.AreEqual(data[8], 0x9A);
            Assert.AreEqual(data[9], 0xBC);
            Assert.AreEqual(data[10], 0xDE);
            Assert.AreEqual(data[11], 0xF0);
        }

        [TestMethod]
        public void CopyDataToArrayBigEndianCopiesInt64Correctly()
        {
            UInt64 value = 0x123456789ABCDEF0;
            byte[] data = new byte[12];
            LHMemoryFunctions.CopyToArrayBigEndian(value, data, 4);
            Assert.AreEqual(data[4], 0x12);
            Assert.AreEqual(data[5], 0x34);
            Assert.AreEqual(data[6], 0x56);
            Assert.AreEqual(data[7], 0x78);
            Assert.AreEqual(data[8], 0x9A);
            Assert.AreEqual(data[9], 0xBC);
            Assert.AreEqual(data[10], 0xDE);
            Assert.AreEqual(data[11], 0xF0);
        }


        [TestMethod]
        public void CopyStringToArrayCopiesStringCorrectly()
        {
            String value = "ABC";
            byte[] data = new byte[12];
            data[7] = 0xFF;
            LHMemoryFunctions.CopyStringToArray(value, data, 4, true);
            Assert.AreEqual(data[4], 0x41);
            Assert.AreEqual(data[5], 0x42);
            Assert.AreEqual(data[6], 0x43);
            Assert.AreEqual(data[7], 0x00);
        }

        [TestMethod]
        public void CopyStringToArrayCopiesStringCorrectlyWithoutNullTerminator()
        {
            String value = "ABC";
            byte[] data = new byte[12];
            data[7] = 0xFF;
            LHMemoryFunctions.CopyStringToArray(value, data, 4, false);
            Assert.AreEqual(data[4], 0x41);
            Assert.AreEqual(data[5], 0x42);
            Assert.AreEqual(data[6], 0x43);
            Assert.AreEqual(data[7], 0xFF);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void CopyStringToArrayThrowsExceptionIfTargetArrayNotLongEnough()
        {
            String value = "Hallo";
            byte[] data = new byte[10];
            LHMemoryFunctions.CopyStringToArray(value, data, 5, true);
        }

        [TestMethod]
        public void ReverseEndianessWorksForUint32()
        {
            UInt32 value = 0xF2345678;
            UInt32 result = (UInt32)LHMemoryFunctions.ReverseEndianess(value);
            Assert.AreEqual((UInt32)0x785634F2, result);
        }
    }
}
