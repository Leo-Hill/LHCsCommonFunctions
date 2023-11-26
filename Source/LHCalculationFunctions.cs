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
        /// <summary>
        /// This function calculates the CRC32 checksum of a byte array according to following setup 
        /// XorOut: 0x00000000
        /// Input reflected: false
        /// Output reflected false
        /// </summary>
        /// <param name="input_data">Data for calculating the CRC32</param>
        /// <returns>The CRC32 checksum for the data</returns>
        /// <exception cref="Exception"></exception>
        public static UInt32 CalculateCrc(byte[] input_data)
        {
            UInt32 actCrcValue = 0xFFFFFFFF;                                                        //Initial CRC value
            UInt32 crcPoly = 0x4C11DB7;                                                             //Crc polynomial
            UInt32 actInputData;
            for (int byteCnt = 0; byteCnt < input_data.Length; byteCnt++)
            {
                actInputData = (UInt32)(input_data[byteCnt] << 24);
                actCrcValue = actCrcValue ^ actInputData;
                for (int binary_index = 0; binary_index < 8; binary_index++)
                {
                    if ((actCrcValue & 0x80000000) != 0)
                    {
                        actCrcValue = (actCrcValue << 1) ^ crcPoly;
                    }
                    else
                    {
                        actCrcValue = actCrcValue << 1;
                    }
                }
            }
            return actCrcValue;
        }

        /// <summary>
        /// This function checks, if the double value is an integer/has decimals
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <param name="num_of_decimal_places_to_analyze">This defines the threshold of detecting decimals.</param>
        /// <returns></returns>
        public static bool DoubleHasDecimals(double value, int num_of_decimal_places_to_analyze = 4)
        {
            double epsilon = Math.Pow(10, -num_of_decimal_places_to_analyze);
            return (!((value % 1) < epsilon));
        }

        /// <summary>
        /// This function calculates cm to points
        /// </summary>
        /// <param name="cm"></param>
        /// <param name="inchPerPoint"></param>
        /// <returns></returns>
        public static double CmToPt(double cm, int inchPerPoint = 72)
        {
            return (cm / 2.54) * inchPerPoint;
        }

    }
}
