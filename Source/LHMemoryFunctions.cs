// Ignore Spelling: Endianess

using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace LHCommonFunctions.Source
{
    /***********************************************************************************************
    * 
    * This class provides functions for common memory operations
    * 
    **********************************************************************************************/
    public static class LHMemoryFunctions
    {
        /// <summary>
        /// This function copies an object from an array.
        /// The byte order of the data in the source array is expected to be big endian.
        /// </summary>
        /// <param name="sourceArray">The array to copy from</param>
        /// <param name="sourceArrayIndex">The index of the source array from where the data is copied from</param>
        /// <param name="targetType">The datatype of the object you want to copy</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Object CopyFromBigEndianArray(byte[] sourceArray, int sourceArrayIndex, Type targetType)
        {
            if (!BitConverter.IsLittleEndian)                                                        //Check if the running architecture is little endian.
            {
                throw new NotImplementedException("Big endian architecture not supported!");
            }
            else
            {
                int targetSize = Marshal.SizeOf(targetType); //Size of the target byte in size
                byte[] sourceBytes = new byte[targetSize];
                Array.Copy(sourceArray, sourceArrayIndex, sourceBytes, 0, targetSize);
                Array.Reverse(sourceBytes);

                if (targetType == typeof(UInt16))
                {
                    return BitConverter.ToUInt16(sourceBytes, 0);
                }
                else if (targetType == typeof(Int16))
                {
                    return BitConverter.ToInt16(sourceBytes, 0);
                }
                else if (targetType == typeof(UInt32))
                {
                    return BitConverter.ToUInt32(sourceBytes, 0);
                }
                else if (targetType == typeof(Int32))
                {
                    return BitConverter.ToInt32(sourceBytes, 0);
                }
                else if (targetType == typeof(UInt64))
                {
                    return BitConverter.ToUInt64(sourceBytes, 0);
                }
                else if (targetType == typeof(Int64))
                {
                    return BitConverter.ToInt64(sourceBytes, 0);
                }
                else
                {
                    throw new NotImplementedException("Unknown DataType");
                }
            }
        }

        /// <summary>
        /// This function copies a variable to a byte array.
        /// The byte order of the data in the array is little endian
        /// </summary>
        /// <param name="sourceObject">The object to copy to the array</param>
        /// <param name="targetArray">The array to copy the object to</param>
        /// <param name="targetArrayIndex">The index of the target array to copy the object to</param>
        /// <exception cref="NotImplementedException"></exception>
        public static void CopyToArrayLittleEndian(Object sourceObject, byte[] targetArray, int targetArrayIndex)
        {
            if (!BitConverter.IsLittleEndian)
            {
                throw new NotImplementedException("Big endian architecture not supported!");
            }
            else
            {
                Type parameterType = sourceObject.GetType();
                if (parameterType == typeof(UInt16))
                {
                    Array.Copy(BitConverter.GetBytes((UInt16)sourceObject), 0, targetArray, targetArrayIndex, 2);
                }
                else if (parameterType == typeof(Int16))
                {
                    Array.Copy(BitConverter.GetBytes((Int16)sourceObject), 0, targetArray, targetArrayIndex, 2);
                }
                else if (parameterType == typeof(UInt32))
                {
                    Array.Copy(BitConverter.GetBytes((UInt32)sourceObject), 0, targetArray, targetArrayIndex, 4);
                }
                else if (parameterType == typeof(Int32))
                {
                    Array.Copy(BitConverter.GetBytes((Int32)sourceObject), 0, targetArray, targetArrayIndex, 4);
                }
                else if (parameterType == typeof(float))
                {
                    Array.Copy(BitConverter.GetBytes((float)sourceObject), 0, targetArray, targetArrayIndex, 4);
                }
                else if (parameterType == typeof(Int64))
                {
                    Array.Copy(BitConverter.GetBytes((Int64)sourceObject), 0, targetArray, targetArrayIndex, 8);
                }
                else if (parameterType == typeof(UInt64))
                {
                    Array.Copy(BitConverter.GetBytes((UInt64)sourceObject), 0, targetArray, targetArrayIndex, 8);
                }
                else if (parameterType == typeof(double))
                {
                    Array.Copy(BitConverter.GetBytes((double)sourceObject), 0, targetArray, targetArrayIndex, 4);
                }
                else
                {
                    throw new NotImplementedException("Unknown DataType");
                }
            }
        }

        /// <summary>
        /// This function copies a variable to a byte array.
        /// The byte order of the data in the array is big endian
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="TargetArray"></param>
        /// <param name="TargetArrayIndex"></param>
        /// <exception cref="NotImplementedException"></exception>
        public static void CopyToArrayBigEndian(Object sourceObject, byte[] targetArray, int targetArrayIndex)
        {
            if (!BitConverter.IsLittleEndian)                                                        //Check if the running architecture is little endian.
            {
                throw new NotImplementedException("Big endian architecture not supported!");
            }
            else
            {
                int sourceObjectSize = Marshal.SizeOf(sourceObject); //Size of the source object in
                CopyToArrayLittleEndian(sourceObject, targetArray, targetArrayIndex);
                Array.Reverse(targetArray, targetArrayIndex, sourceObjectSize);
            }
        }

        /// <summary>
        /// This function copies a string to a byte array.
        /// </summary>
        /// <param name="sourceString">The string to copy</param>
        /// <param name="targetArray">The array to copy the string to</param>
        /// <param name="targetArrayIndex">The index of the array where to copy the string to</param>
        /// <param name="copyNullTerminator">Specify if you want to copy a null terminator at the end of the string</param>
        /// <exception cref="Exception"></exception>
        public static void CopyStringToArray(String sourceString, byte[] targetArray, int targetArrayIndex, bool copyNullTerminator)
        {
            int minArrayLength = targetArrayIndex + sourceString.Length;
            if (copyNullTerminator)
            {
                minArrayLength += 1;
            }
            if (targetArray.Length < minArrayLength)
            {
                throw new Exception("Target array not big enough!");
            }
            Array.Copy(Encoding.GetEncoding(LHStringFunctions.I_CODEPAGE_ISO_8859_1).GetBytes(sourceString), 0, targetArray, targetArrayIndex, sourceString.Length);
            if (copyNullTerminator)
            {
                targetArray[targetArrayIndex + sourceString.Length ] = 0;
            }
        }

        /// <summary>
        /// This function reverses the endianess of the passed object
        /// </summary>
        /// <param name="sourceObject">The object to reverse the endianess</param>
        /// <returns>The object in reversed endianess</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Object ReverseEndianess(Object sourceObject)
        {
            if (sourceObject.GetType() == typeof(UInt32))
            {
                UInt32 u32 = (UInt32)sourceObject;
                byte[] data = new byte[4];
                CopyToArrayLittleEndian(u32, data, 0);
                u32 = (UInt32)CopyFromBigEndianArray(data, 0, typeof(UInt32));
                return u32;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
