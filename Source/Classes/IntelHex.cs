using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace LHCommonFunctions.Classes {

    /// <summary>
    /// This class can be used to parse and retrieve informations about an INTEL hex file
    /// </summary>
    public class IntelHex {
        /// <summary>
        /// A DataBlock contains of consecutive data in the hex file
        /// </summary>
        public struct DataBlock {
            public UInt32 StartAddress;
            public byte[] Data;
        }

        private List<DataBlock> _dataBlocks;                                        //This list contains data blocks (<StartAddress, Data>). A data block contains consecutive data. Empty spaces / skip in address will lead to multiple entries.
        private int _payloadSize = 0; //Size of the firmware data

        /// <summary>
        /// Constructor will parse the passed hex file.
        /// </summary>
        /// <param name="filePath">Absolute path to the hex file</param>
        public IntelHex(String filePath) {
            StreamReader reader = new StreamReader(filePath);
            String hexFileContent = reader.ReadToEnd();
            reader.Close();
            ParseData(hexFileContent);
        }

        /// <summary>
        ///  This function parses the hex data and stores the data blocks.
        /// </summary>
        /// <param name="hexContent"></param>
        /// <exception cref="Exception"></exception>
        private void ParseData(String hexContent) {
            _payloadSize = 0;
            _dataBlocks = new List<DataBlock>();
            String[] lines = hexContent.Split(Environment.NewLine);
            List<byte> bytes = new List<byte>(); //List for accumulating consecutive bytes

            UInt32 addressOffset = 0; //Address offset specified by an Extended Linear Address Record
            UInt32 addressOfCurrentDataBlock = 0; //Address of the currently editing data block which will be saved next in the data block list
            foreach (String line in lines) {
                //Variables for parsing
                byte lineCharCnt;                                                                       //Counter for indexing the string
                byte lineNumOfBytes;                                                                    //Number of bytes in the line
                UInt32 lineAddress;                                                                  //The address
                byte lineRecordType;                                                                    //Type of the record

                if (String.IsNullOrEmpty(line)) {
                    continue;
                }
                lineCharCnt = 0;
                if (line[lineCharCnt] != ':') {
                    throw new Exception();
                }

                lineNumOfBytes = Convert.ToByte(line.Substring(1, 2), 16);                         //Parse the number of bytes
                lineAddress = Convert.ToUInt32(line.Substring(3, 4), 16);                       //Parse the address
                lineRecordType = Convert.ToByte(line.Substring(7, 2), 16);                         //Parse the type


                switch (lineRecordType) {
                    case 00: {
                            //Data Record 
                            if (bytes.Count == 0) {
                                addressOfCurrentDataBlock = lineAddress;
                            }

                            for (byte ByteCnt = 0; ByteCnt < lineNumOfBytes; ByteCnt++) {
                                bytes.Add(Convert.ToByte(line.Substring(9 + 2 * ByteCnt, 2), 16));
                            }
                            break;
                        }
                    case 01: {
                            //End of File Record
                            if (bytes.Count > 0) {
                                DataBlock insertBlock;
                                insertBlock.StartAddress = addressOffset + addressOfCurrentDataBlock;
                                insertBlock.Data = bytes.ToArray();
                                _dataBlocks.Add(insertBlock);
                                _payloadSize += bytes.Count;
                                bytes.Clear();
                            }
                            break;
                        }
                    case 04: {
                            //Extended Linear Address Record
                            if (bytes.Count > 0) {
                                //Complete the currently editing DataBlock
                                DataBlock insertBlock;
                                insertBlock.StartAddress = addressOffset + addressOfCurrentDataBlock;
                                insertBlock.Data = bytes.ToArray();
                                _dataBlocks.Add(insertBlock);
                                _payloadSize += bytes.Count;
                                bytes.Clear();
                            }
                            addressOffset = (UInt32)(Convert.ToUInt16(line.Substring(9, 4), 16) << 16);
                            break;
                        }
                    default: {
                            break;
                        }
                }


            }
        }

        /// <summary>
        /// This function returns the data blocks of the hex file
        /// </summary>
        /// <returns></returns>
        public List<DataBlock> GetDataBlocks() {
            return _dataBlocks;
        }

        /// <summary>
        /// This gives you all bytes from the start address to the end address (end address excluded).
        /// </summary>
        /// <param name="startAddress">Start address of the data range</param>
        /// <param name="endAddress">End address of the data range (excluded)</param>
        /// <returns></returns>
        public byte[] GetBytes(UInt32 startAddress, UInt32 endAddress) {
            List<byte> bytesToReturn = new List<byte>();

            foreach (DataBlock Block in _dataBlocks) {
                if (Block.StartAddress >= endAddress) {
                    continue;
                }
                if (Block.StartAddress + Block.Data.Length >= endAddress) {
                    //We only want to copy data up until the end address
                    byte[] aBytesInRange = new byte[endAddress - Block.StartAddress];
                    Array.Copy(Block.Data, aBytesInRange, aBytesInRange.Length);
                    bytesToReturn.AddRange(aBytesInRange);
                } else {
                    bytesToReturn.AddRange(Block.Data);
                }
            }
            return bytesToReturn.ToArray();
        }

        /// <summary>
        /// This gives you the size of the firmware data
        /// </summary>
        /// <returns></returns>
        public int GetPayloadSize() {
            return _payloadSize;
        }
    }
}
