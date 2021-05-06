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
        //This function calculates the CRC checksum of an u32 array
        public static UInt32 u32CalculateCrc(UInt32[] qaData, int qiSize)
        {
            UInt32 u32ActCrcValue = 0xFFFFFFFF;                                                     //Initial CRC value
            UInt32 u32CrcPoly = 0x4C11DB7;                                                          //Crc polynom
            UInt32 u32ActBinaryIndex;                                                               //Current bit of the crc value processing
            for (int iByteCnt = 0; iByteCnt < qiSize; iByteCnt++)
            {
                u32ActCrcValue = u32ActCrcValue ^ qaData[iByteCnt];
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
