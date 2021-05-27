using System;
using System.Collections.Generic;
using System.Linq;
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
        public static Object oCopyFromArrayReversed(byte[] qaSourceArray,int iSorceArrayStartIndex, Type qType)
        {
            if (BitConverter.IsLittleEndian)                                                        //Check if the running architecture is little endian.
            {
                if (qType == typeof(UInt32))
                {
                    UInt32 u32 = 0;
                    for (int iByteCnt = 0; iByteCnt < 4; iByteCnt++)
                    {
                        u32 += qaSourceArray[iSorceArrayStartIndex+iByteCnt];
                        if (iByteCnt < 3)
                        {
                            u32 = u32 << 8;
                        }
                    }
                    return u32;
                }
                else
                {
                    throw new NotImplementedException("Unknown DataType");
                }
            }
            else
            {
                throw new NotImplementedException("Big endian architecture not supported!");
            }
        }

        //This function copies a variable to a byte array. The byte order of the data in the array is little endian if the running architecture is litte endian
        public static void vCopyToArray(Object qObject, byte[] qaTargetArray, int qiTargetArrayStartIndex)
        {
            if (BitConverter.IsLittleEndian)                                                        //Check if the running architecture is little endian.
            {
                Type parameterType = qObject.GetType();
                if (parameterType == typeof(UInt16))
                {
                    Array.Copy(BitConverter.GetBytes((UInt16)qObject), 0, qaTargetArray, qiTargetArrayStartIndex, 2);
                }
                else if (parameterType == typeof(UInt32))
                {
                    Array.Copy(BitConverter.GetBytes((UInt32)qObject), 0, qaTargetArray, qiTargetArrayStartIndex, 4);
                }
                else if (parameterType == typeof(float))
                {
                    Array.Copy(BitConverter.GetBytes((float)qObject), 0, qaTargetArray, qiTargetArrayStartIndex, 4);
                }
                else if (parameterType == typeof(UInt64))
                {
                    Array.Copy(BitConverter.GetBytes((UInt64)qObject), 0, qaTargetArray, qiTargetArrayStartIndex, 8);
                }
                else if (parameterType == typeof(double))
                {
                    Array.Copy(BitConverter.GetBytes((double)qObject), 0, qaTargetArray, qiTargetArrayStartIndex, 4);
                }
                else if (parameterType == typeof(String))
                {
                    String sString = ((String)qObject);
                    Array.Copy(Encoding.GetEncoding(LHStringFunctions.I_CODEPAGE_ISO_8859_1).GetBytes(sString), 0, qaTargetArray, qiTargetArrayStartIndex, sString.Length);
                }
                else
                {
                    throw new NotImplementedException("Unknown DataType");
                }
            }
            else
            {
                throw new NotImplementedException("Big endian architecture not supported!");
            }
        }

        //This function copies a variable to a byte array. The byte order of the data in the array is big endian if the running architecture is litte endian
        public static void vCopyToArrayReversed(Object qObject, byte[] qaTargetArray, int qiTargetArrayStartIndex)
        {
            if (BitConverter.IsLittleEndian)                                                        //Check if the running architecture is little endian.
            {
                Type parameterType = qObject.GetType();
                if (parameterType == typeof(UInt32))
                {
                    UInt32 u32 = (uint)qObject;
                    byte[] au32 = new byte[4];
                    au32[0] = ((byte)((u32 & 0xFF000000) >> 24));
                    au32[1] = ((byte)((u32 & 0x00FF0000) >> 16));
                    au32[2] = ((byte)((u32 & 0x0000FF00) >> 8));
                    au32[3] = (byte)((u32 & 0x000000FF));

                    Array.Copy(au32, 0, qaTargetArray, qiTargetArrayStartIndex, 4);
                }
                else
                {
                    throw new NotImplementedException("Unknown DataType");
                }
            }
            else
            {
                throw new NotImplementedException("Big endian architecture not supported!");
            }
        }
    }
}
