using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHCommonFunctions.Source
{
    /***********************************************************************************************
    * 
    * This class provides functions for common calculations
    * 
    **********************************************************************************************/
    public static class LHCalculationFunctions
    {
        //This function calculates the CRC32 checksum of a byte array
        public static UInt32 u32CalculateCrc(byte[] qaData)
        {
            if (0 != (qaData.Length % 4))                                                           //Check if the array can be divided by 4 in order to seperate the bytes to U32s
            {
                throw new Exception("CRC32 exception. Array can non be converted!");
            }
            UInt32 u32ActCrcValue = 0xFFFFFFFF;                                                     //Initial CRC value
            UInt32 u32CrcPoly = 0x4C11DB7;                                                          //Crc polynom
            UInt32 u32ActBinaryIndex;                                                               //Current bit of the crc value processing
            UInt32 u32ActInputData;
            for (int iByteCnt = 0; iByteCnt < qaData.Length; iByteCnt+=4)
            {
                u32ActInputData = BitConverter.ToUInt32(qaData, iByteCnt);                          //Convert 4 bytes to 1 u32
                u32ActCrcValue = u32ActCrcValue ^ u32ActInputData;
                u32ActBinaryIndex = 0;
                while (u32ActBinaryIndex < 32)
                {
                    if ((u32ActCrcValue & (1 << 31)) != 0)
                    {
                        u32ActCrcValue = (u32ActCrcValue << 1) ^ u32CrcPoly;
                    }
                    else
                    {
                        u32ActCrcValue = u32ActCrcValue << 1;
                    }
                    u32ActBinaryIndex++;
                }
            }
            return u32ActCrcValue;
        }
    }
}
