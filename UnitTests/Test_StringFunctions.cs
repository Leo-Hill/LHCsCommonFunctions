using LHCommonFunctions.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Functions
{
    [TestClass]
    public class Test_StringFunctions
    {
        [TestMethod]
        public void ConvertByteArrayToStringWorksCorrectly()
        {
            byte[] input = { 0x21, 0x46, 0x67 };
            String output = LHStringFunctions.ConvertByteArrayToString(input);
            Assert.AreEqual("!Fg", output);
        }

        [TestMethod]
        public void ConvertByteArrayToStringIgnoresBytesOutOfRange()
        {
            byte[] input = { 0x02, 0x21, 0x46, 0x9F, 0x67, 0xFF };
            String output = LHStringFunctions.ConvertByteArrayToString(input);
            Assert.AreEqual("\\0x02!F\\0x9Fgÿ", output);
        }

        [TestMethod]
        public void ConvertByteArrayToHexStringWorksCorrectly()
        {
            byte[] input = { 0x11, 0x22, 0x33, 0x44, 0xAA, 0xFF };
            String output = LHStringFunctions.ConvertByteArrayToHexString(input);
            Assert.AreEqual("11 22 33 44 AA FF", output);
        }

        [TestMethod]
        public void ConvertByteArrayToHexStringWorksCorrectlyWithPrefix()
        {
            byte[] input = { 0x11, 0x22, 0x33, 0x44, 0xAA, 0xFF };
            String output = LHStringFunctions.ConvertByteArrayToHexString(input, "0x");
            Assert.AreEqual("0x11 0x22 0x33 0x44 0xAA 0xFF", output);
        }

        [TestMethod]
        public void ConvertIpAddressToStringWorksCorrectly()
        {
            UInt32 ip = 0xFFFFFFFF;
            Assert.AreEqual("255.255.255.255", LHStringFunctions.ConvertIpAddressToString(ip));

            ip = 0x0100A8C0;
            Assert.AreEqual("192.168.0.1", LHStringFunctions.ConvertIpAddressToString(ip));
        }

        [TestMethod]
        public void ConvertStringToByteArrayConvertsNormalStringCorrectly()
        {
            String input = "!Fg";
            byte[] expected = { 0x21, 0x46, 0x67 };

            byte[] result = LHStringFunctions.ConvertStringToByteArray(input, false);
            for (int byteCnt = 0; byteCnt < expected.Length; byteCnt++)
            {
                Assert.AreEqual(expected[byteCnt], result[byteCnt]);
            }
        }

        [TestMethod]
        public void ConvertStringToByteArrayAddsNullTerminatorCorrectly()
        {
            String input = "!Fg";
            byte[] expected = { 0x21, 0x46, 0x67, 0x00 };

            byte[] result = LHStringFunctions.ConvertStringToByteArray(input, true);
            for (int byteCnt = 0; byteCnt < expected.Length; byteCnt++)
            {
                Assert.AreEqual(expected[byteCnt], result[byteCnt]);
            }
        }

        [TestMethod]
        public void ConvertStringToByteArrayConvertsStringWithHexNumbersCorrectly()
        {
            String input = "Sepp\\0x1A\\0xAx";
            byte[] expected = { 0x53, 0x65, 0x70, 0x70, 0x1A, 0x5C, 0x30, 0x78, 0x41, 0x78 };

            byte[] result = LHStringFunctions.ConvertStringToByteArray(input, false);
            for (int byteCnt = 0; byteCnt < expected.Length; byteCnt++)
            {
                Assert.AreEqual(expected[byteCnt], result[byteCnt]);
            }
        }

        [TestMethod]
        public void ConvertStringToIpAddressWorksCorrectly()
        {
            String ip = "1.2.3.4";
            Assert.AreEqual((UInt32)0x04030201, LHStringFunctions.ConvertStringToIpAddress(ip));

            ip = "192.168.0.1";
            Assert.AreEqual((UInt32)0x0100A8C0, LHStringFunctions.ConvertStringToIpAddress(ip));
        }

        [TestMethod]
        public void GetFileExtensionWorksCorrectly()
        {
            String path = "Sepp\\Test\\Path\\file.exe";
            Assert.AreEqual("exe", LHStringFunctions.GetFileExtension(path));
        }

        [TestMethod]
        public void GetFileNameWorksCorrectly()
        {
            String path = "Sepp\\Test\\Path\\file.exe";
            Assert.AreEqual("file.exe", LHStringFunctions.GetFileName(path));
        }

        [TestMethod]
        public void GetParentFolderWorksCorrectly()
        {
            String input = "Sepp\\Hans\\Sepp\\Schorsch.txt";
            String result=LHStringFunctions.GetParentFolder(input);
            Assert.AreEqual("Sepp\\Hans\\Sepp", result);
        }

        [TestMethod]
        public void InsertStringInAscendingSortedObservableCollectionWorksCorrectly()
        {
            ObservableCollection<String> collection = new ObservableCollection<String>();
            collection.Add("AAA");
            collection.Add("BBB");
            collection.Add("DDD");
            collection.Add("ZZZ");
            LHStringFunctions.InsertStringInSortedObservableCollection(collection, "CCC", ListSortDirection.Ascending);

            Assert.AreEqual("CCC", collection[2]);
        }

        [TestMethod]
        public void InsertStringInDescendingSortedObservableCollectionWorksCorrectly()
        {
            ObservableCollection<String> collection = new ObservableCollection<String>();
            collection.Add("ZZZ");
            collection.Add("DDD");
            collection.Add("BBB");
            collection.Add("AAA");
            LHStringFunctions.InsertStringInSortedObservableCollection(collection, "CCC", ListSortDirection.Descending);

            Assert.AreEqual("CCC", collection[2]);
        }

        [TestMethod]
        public void RemoveInvalidCharactersInFilePathWorksCorrectly()
        {
            String input = "<Se|?pp>.exe";
            Assert.AreEqual("Sepp.exe", LHStringFunctions.RemoveInvalidCharactersInFileName(input));
        }

        [TestMethod]
        public void TimeSpanToStringWorksCorrectly()
        {
            TimeSpan timeSpan = new TimeSpan(5, 7, 23, 44, 2);
            Assert.AreEqual("127:23:44", LHStringFunctions.TimeSpanToString(timeSpan));
        }

        [TestMethod]
        public void TimeSpanToStringWithCharsWorksCorrectly()
        {
            TimeSpan timeSpan = new TimeSpan(5, 7, 23, 44, 2);
            Assert.AreEqual("127h 23m 44s", LHStringFunctions.TimeSpanToStringWithChars(timeSpan));
        }
    }
}
