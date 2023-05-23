using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace LHCommonFunctions.Source
{
    /***********************************************************************************************
    * 
    * This class represents an intel hex file
    * 
    **********************************************************************************************/
    public class Class_IntelHex
    {
        private List<DataBlock> LDataBlocks;                                        //This list contains data blocks (<StartAddress, Data>). A data block contains consecutive data. Empty spaces / skip in address will lead to multiple entries

        /***********************************************************************************************
        * 
        * Variables
        * 
        **********************************************************************************************/

        //A DataBlock contains of consecutive data in the hex file
        public struct DataBlock
        {
            public UInt32 u32StartAddress;
            public byte[] aData;
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

        //This function parses the IHex data and stores the data blocks in the LDataBlocks
        private void ParseData(String qsIHexContent)
        {
            LDataBlocks = new List<DataBlock>();
            String[] sLines = qsIHexContent.Split(Environment.NewLine);
            List<byte> LBytes = new List<byte>(); //List for accumulating consequtive bytes

            UInt32 u32AddressOffset = 0; //Address offset specified by an Extended Linear Address Record
            UInt32 u32AddressOfCurrentDataBlock = 0; //Address of the curretly editing data block which will be saved next in the data block list
            foreach (String sLine in sLines)
            {
                //Variables for parsing
                byte LineCharCnt;                                                                       //Counter for indexing the string
                byte LineNumOfBytes;                                                                    //Number of bytes in the line
                UInt32 u32LineAddress;                                                                  //The address
                byte LineRecordType;                                                                    //Type of the record

                if (String.IsNullOrEmpty(sLine))
                {
                    continue;
                }
                LineCharCnt = 0;
                if (sLine[LineCharCnt] != ':')
                {
                    throw new Exception();
                }

                LineNumOfBytes = Convert.ToByte(sLine.Substring(1, 2), 16);                         //Parse the number of bytes
                u32LineAddress = Convert.ToUInt32(sLine.Substring(3, 4), 16);                       //Parse the address
                LineRecordType = Convert.ToByte(sLine.Substring(7, 2), 16);                         //Parse the type


                switch (LineRecordType)
                {
                    case 00:
                        {
                            //Data Record 
                            if (LBytes.Count == 0)
                            {
                                u32AddressOfCurrentDataBlock = u32LineAddress;
                            }

                            for (byte ByteCnt = 0; ByteCnt < LineNumOfBytes; ByteCnt++)
                            {
                                LBytes.Add(Convert.ToByte(sLine.Substring(9 + 2 * ByteCnt, 2), 16));
                            }
                            break;
                        }
                    case 01:
                        {
                            //End of File Record
                            if (LBytes.Count > 0)
                            {
                                DataBlock DBInsert;
                                DBInsert.u32StartAddress = u32AddressOffset + u32AddressOfCurrentDataBlock;
                                DBInsert.aData = LBytes.ToArray();
                                LDataBlocks.Add(DBInsert);
                            }
                            break;
                        }
                    case 04:
                        {
                            //Extended Linear Address Record
                            if (LBytes.Count > 0)
                            {
                                DataBlock DBInsert;
                                DBInsert.u32StartAddress = u32AddressOffset + u32AddressOfCurrentDataBlock;
                                DBInsert.aData = LBytes.ToArray();
                                LDataBlocks.Add(DBInsert);
                                LBytes.Clear();

                            }
                            u32AddressOffset = (UInt32)(Convert.ToUInt16(sLine.Substring(9, 4), 16) << 16);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }


            }
        }

        //This function returns the data blocks of the hex file
        public List<DataBlock> GetDataBlocks()
        {
            return LDataBlocks;
        }

        //This gives you all bytes from the u32StartAddress to the u32EndAddress (u32EndAddress excluded)
        public byte[] GetBytes(UInt32 u32StartAddress, UInt32 u32EndAddress)
        {
            List<byte> aBytesToReturn = new List<byte>();

            foreach (DataBlock Block in LDataBlocks)
            {
                if (Block.u32StartAddress >= u32EndAddress)
                {
                    continue;
                }
                if (Block.u32StartAddress + Block.aData.Length >= u32EndAddress  )
                {
                    //We only want to copy data up until the end address
                    byte[] aBytesInRange = new byte[u32EndAddress - Block.u32StartAddress];
                    Array.Copy(Block.aData, aBytesInRange, aBytesInRange.Length);
                    aBytesToReturn.AddRange(aBytesInRange);
                }
                else
                {
                    aBytesToReturn.AddRange(Block.aData);
                }
            }
            return aBytesToReturn.ToArray();
        }
    }
}
