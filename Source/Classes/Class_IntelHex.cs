using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Shapes;

namespace LHCommonFunctions.Source
{
    /***********************************************************************************************
    * 
    * This class represents an intel hex file
    * 
    **********************************************************************************************/
    public class Class_IntelHex
    {
        private List<DataBlock> _dataBlockList;                                        //This list contains data blocks (<StartAddress, Data>). A data block contains consecutive data. Empty spaces / skip in address will lead to multiple entries

        /***********************************************************************************************
        * 
        * Variables
        * 
        **********************************************************************************************/

        /// <summary>
        /// A DataBlock contains bytes of consecutive data in the hex file
        /// </summary>
        public struct DataBlock
        {
            public UInt32 StartAddress;
            public byte[] DataBytes;
        }

        /// <summary>
        /// Contains information which can be parsed from one line
        /// </summary>
        private struct LineInformation
        {
            public byte numOfBytes;
            public UInt32 address;
            public RecordType recordType;
            public byte[] data;
        }

        /// <summary>
        /// Txpe describing the content of one record/line in the hex file
        /// </summary>
        private enum RecordType
        {
            Data = 0,
            EndOfFile = 1,
            ExtendedLinearAddress = 4,
            StartLinearAddress = 5
        }

        /***********************************************************************************************
        * 
        * Constructor
        * 
        **********************************************************************************************/
        public Class_IntelHex(String qsFileName)
        {
            StreamReader reader = new StreamReader(qsFileName);
            String sHexFileContent = reader.ReadToEnd();
            reader.Close();
            ParseData(sHexFileContent);
        }


        /***********************************************************************************************
        * 
        * Functions
        * 
        **********************************************************************************************/

        /// <summary>
        /// This function parses the IHex data and stores the data blocks in the data block list
        /// </summary>
        /// <param name="hexContent">The raw content of the hex file</param>
        /// <exception cref="Exception"></exception>
        private void ParseData(String hexContent)
        {
            _dataBlockList = new List<DataBlock>();
            String[] contentLines = hexContent.Split("\n");
            List<byte> consecutiveBytes = new List<byte>(); //List for accumulating consecutive bytes

            UInt32 currentAddressOffset = 0; //Address offset specified by the previous Extended Linear Address Record
            UInt32 startAddressOfCurrentBlock = 0; //Address of the currently editing data block 

            LineInformation currentLineInformation = new LineInformation();
            for (int lineCnt = 0; lineCnt < contentLines.Length; lineCnt++)
            {
                if (String.IsNullOrEmpty(contentLines[lineCnt]))
                {
                    continue;
                }
                LineInformation previousLineInformation = currentLineInformation;
                currentLineInformation = ParseLineInformation(contentLines[lineCnt]);

                switch (currentLineInformation.recordType)
                {
                    case RecordType.Data:
                        {
                            //Data Record 
                            if (consecutiveBytes.Count == 0)
                            {
                                startAddressOfCurrentBlock = currentLineInformation.address + currentAddressOffset;
                            }

                            if (previousLineInformation.recordType == 0 && previousLineInformation.data != null)
                            {
                                int dataGap = ((int)(currentLineInformation.address - (previousLineInformation.address + previousLineInformation.data.Length)));

                                if (dataGap > 0)
                                {
                                    //There is a gap in the hex file -> we complete the current data block and create a new one, since the data is not consecutive anymore
                                    InsertNewBlock(consecutiveBytes, startAddressOfCurrentBlock);
                                    consecutiveBytes.Clear();
                                    startAddressOfCurrentBlock = currentLineInformation.address + currentAddressOffset;
                                }
                            }

                            consecutiveBytes.AddRange(currentLineInformation.data);

                            break;
                        }
                    case RecordType.EndOfFile:
                        {
                            //End of File Record
                            InsertNewBlock(consecutiveBytes, startAddressOfCurrentBlock);
                            consecutiveBytes.Clear();

                            break;
                        }
                    case RecordType.ExtendedLinearAddress:
                        {
                            //Extended Linear Address Record
                            InsertNewBlock(consecutiveBytes, startAddressOfCurrentBlock);
                            consecutiveBytes.Clear();

                            currentAddressOffset = (UInt16)LHMemoryFunctions.oCopyFromArrayReversed(currentLineInformation.data, 0, typeof(UInt16));
                            currentAddressOffset = currentAddressOffset << 16;

                            break;
                        }
                    case RecordType.StartLinearAddress:
                        {
                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }


            }
        }

        private LineInformation ParseLineInformation(String line)
        {
            LineInformation lineInformation = new LineInformation();

            if (line[0] != ':')
            {
                throw new Exception();
            }

            lineInformation.numOfBytes = Convert.ToByte(line.Substring(1, 2), 16);                         //Parse the number of bytes
            lineInformation.address = Convert.ToUInt32(line.Substring(3, 4), 16);                       //Parse the address
            lineInformation.recordType = (RecordType)Convert.ToByte(line.Substring(7, 2), 16);                         //Parse the type

            lineInformation.data = new byte[lineInformation.numOfBytes];
            for (byte dataCnt = 0; dataCnt < lineInformation.numOfBytes; dataCnt++)
            {
                lineInformation.data[dataCnt] = Convert.ToByte(line.Substring(9 + 2 * dataCnt, 2), 16);
            }
            return lineInformation;

        }

        private void InsertNewBlock(List<byte> content, UInt32 startAddress)
        {
            if (content.Count == 0)
            {
                return;
            }
            DataBlock block = new DataBlock();
            block.StartAddress = startAddress;
            block.DataBytes = content.ToArray();
            _dataBlockList.Add(block);
        }

        //This function returns the data blocks of the hex file
        public List<DataBlock> GetDataBlocks()
        {
            return _dataBlockList;
        }

        //This gives you all bytes from the u32StartAddress to the u32EndAddress
        public byte[] GetBytes(UInt32 u32StartAddress, UInt32 u32EndAddress)
        {
            List<byte> aBytesToReturn = new List<byte>();

            foreach (DataBlock Block in _dataBlockList)
            {
                if (Block.StartAddress > u32EndAddress)
                {
                    continue;
                }
                if (Block.StartAddress + Block.DataBytes.Length > u32EndAddress)
                {
                    //We only want to copy data up until the end address
                    byte[] aBytesInRange = new byte[u32EndAddress - Block.StartAddress];
                    Array.Copy(Block.DataBytes, aBytesInRange, aBytesInRange.Length);
                    aBytesToReturn.AddRange(aBytesInRange);
                }
                else
                {
                    aBytesToReturn.AddRange(Block.DataBytes);
                }
            }
            return aBytesToReturn.ToArray();
        }

        //This gives you the size of the payload
        public int iGetPayloadSize()
        {
            int payloadSize = 0;
            foreach (DataBlock block in _dataBlockList)
            {
                payloadSize += block.DataBytes.Length;
            }
            return payloadSize;
        }
    }
}
