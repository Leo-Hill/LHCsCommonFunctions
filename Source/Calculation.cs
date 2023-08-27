// Ignore Spelling: Crc

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHCommonFunctions {
    /// <summary>
    /// This class provides functions for common calculations
    /// </summary>
    public static class Calculation {
        /// <summary>
        /// This function calculates the CRC32 checksum of a byte array
        /// </summary>
        /// <param name="data">The data for calculating the CRC32 checksum for</param>
        /// <returns></returns>
        /// <exception cref="Exception">In case the data is not aligned to uin32, we cannot calculate CRC32</exception>
        public static UInt32 CalculateCrc(byte[] data) {
            if (0 != (data.Length % 4))                                                           //Check if the array can be divided by 4 in order to separate the bytes to U32s
            {
                throw new Exception("CRC32 exception. Array can non be converted!");
            }
            const UInt32 crcPoly = 0x4C11DB7;                                                          //Crc polynomial

            UInt32 actCrcValue = 0xFFFFFFFF;                                                     //The result of the CRC calculation.
            UInt32 actBinaryIndex;                                                               //Current bit of the crc value processing
            UInt32 actInputData;    //Current uint32 to process
            for (int iByteCnt = 0; iByteCnt < data.Length; iByteCnt += 4) {
                actInputData = BitConverter.ToUInt32(data, iByteCnt);                          //Convert 4 bytes to 1 u32
                actCrcValue ^= actInputData;
                actBinaryIndex = 0;
                while (actBinaryIndex < 32) {
                    if ((actCrcValue & (1 << 31)) != 0) {
                        actCrcValue = (actCrcValue << 1) ^ crcPoly;
                    } else {
                        actCrcValue <<= 1;
                    }
                    actBinaryIndex++;
                }
            }
            return actCrcValue;
        }

        /// <summary>
        /// This function checks, if the qDouble is an integer/has decimals
        /// </summary>
        /// <param name="double_number">The number to analyze</param>
        /// <param name="epsilon">The last digit to be evaluated as decimal place</param>
        /// <returns></returns>
        public static bool DoubleHasDecimals(double double_number, double epsilon = 0.0001) {
            if ((double_number % 1) < epsilon) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// This function calculates cm to points
        /// </summary>
        /// <param name="cm">The cm to convert</param>
        /// <param name="inch_per_point">Inch per points to use for the calculation</param>
        /// <returns></returns>
        public static double CmToPt(double cm, int inch_per_point = 72) {
            double dResult;
            dResult = (cm / 2.54) * inch_per_point;
            return dResult;
        }

    }
}
