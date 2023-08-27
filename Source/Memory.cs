using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHCommonFunctions {
    /// <summary>
    /// This class provides functions for memory operations
    /// </summary>
    public static class Memory {

        /// <summary>
        /// This copies function from an array in reversed order and returns it as user-defined data-type
        /// </summary>
        /// <param name="sourceArray">The array to copy the data from</param>
        /// <param name="sorceArrayStartIndex">The start index of the data to copy in the array</param>
        /// <param name="destinationType">The data-type to return</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Object CopyFromArrayReversed(byte[] sourceArray, int sorceArrayStartIndex, Type destinationType) {
            if (!BitConverter.IsLittleEndian) {
                throw new NotImplementedException("Big endian architecture not supported!");
            }
            if (destinationType == typeof(UInt32)) {
                UInt32 u32 = 0;
                for (int iByteCnt = 0; iByteCnt < 4; iByteCnt++) {
                    u32 += sourceArray[sorceArrayStartIndex + iByteCnt];
                    if (iByteCnt < 3) {
                        u32 = u32 << 8;
                    }
                }
                return u32;
            } else if (destinationType == typeof(UInt64)) {
                UInt64 u64 = 0;
                for (int iByteCnt = 0; iByteCnt < 8; iByteCnt++) {
                    u64 += sourceArray[sorceArrayStartIndex + iByteCnt];
                    if (iByteCnt < 7) {
                        u64 = u64 << 8;
                    }
                }
                return u64;
            } else {
                throw new NotImplementedException("Unknown DataType");
            }
        }

        /// <summary>
        /// This function copies a variable to a byte array
        /// </summary>
        /// <param name="source">The data to copy</param>
        /// <param name="destination">The destination array to copy the data to</param>
        /// <param name="targetArrayStartIndex">The start index of the destination array to copy the data to</param>
        /// <exception cref="NotImplementedException"></exception>
        public static void CopyToArray(Object source, byte[] destination, int targetArrayStartIndex) {
            if (!BitConverter.IsLittleEndian) {
                throw new NotImplementedException("Big endian architecture not supported!");
            }
            Type parameterType = source.GetType();
            if (parameterType == typeof(UInt16)) {
                Array.Copy(BitConverter.GetBytes((UInt16)source), 0, destination, targetArrayStartIndex, 2);
            } else if (parameterType == typeof(UInt32)) {
                Array.Copy(BitConverter.GetBytes((UInt32)source), 0, destination, targetArrayStartIndex, 4);
            } else if (parameterType == typeof(float)) {
                Array.Copy(BitConverter.GetBytes((float)source), 0, destination, targetArrayStartIndex, 4);
            } else if (parameterType == typeof(Int64)) {
                Array.Copy(BitConverter.GetBytes((Int64)source), 0, destination, targetArrayStartIndex, 8);
            } else if (parameterType == typeof(UInt64)) {
                Array.Copy(BitConverter.GetBytes((UInt64)source), 0, destination, targetArrayStartIndex, 8);
            } else if (parameterType == typeof(double)) {
                Array.Copy(BitConverter.GetBytes((double)source), 0, destination, targetArrayStartIndex, 4);
            } else if (parameterType == typeof(String)) {
                String sString = ((String)source);
                Array.Copy(Encoding.GetEncoding(StringOperations.CodepageIso8859_1).GetBytes(sString), 0, destination, targetArrayStartIndex, sString.Length);
            } else {
                throw new NotImplementedException("Unknown DataType");
            }
        }

        /// <summary>
        /// This function copies a variable to a byte array in reversed byte order
        /// </summary>
        /// <param name="source">The data to copy</param>
        /// <param name="destination">The destination array to copy the data to</param>
        /// <param name="targetArrayStartIndex">The start index of the destination array to copy the data to</param>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public static void vCopyToArrayReversed(Object source, byte[] destination, int targetArrayStartIndex) {
            if (!BitConverter.IsLittleEndian) {
                throw new NotImplementedException("Big endian architecture not supported!");
            }
            Type parameterType = source.GetType();
            if (parameterType == typeof(UInt32)) {
                UInt32 u32 = (uint)source;
                byte[] au32 = new byte[4];
                au32[0] = ((byte)((u32 & 0xFF000000) >> 24));
                au32[1] = ((byte)((u32 & 0x00FF0000) >> 16));
                au32[2] = ((byte)((u32 & 0x0000FF00) >> 8));
                au32[3] = (byte)((u32 & 0x000000FF));

                Array.Copy(au32, 0, destination, targetArrayStartIndex, 4);
            } else {
                throw new NotImplementedException("Unknown DataType");
            }
        }

        /// <summary>
        /// This function returns you the passes object in reverse byte order
        /// </summary>
        /// <param name="data">The object you want to have in reversed byte order</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Object oReverseEdianess(Object data) {
            if (data.GetType() == typeof(UInt32)) {
                UInt32 u32 = (UInt32)data;
                byte[] aData = new byte[4];
                CopyToArray(u32, aData, 0);
                u32 = (UInt32)CopyFromArrayReversed(aData, 0, typeof(UInt32));
                return u32;
            } else {
                throw new NotImplementedException();
            }
        }
    }
}
