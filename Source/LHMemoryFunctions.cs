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
        //This function copies a variable to a byte array. The byte order of the data in the array is little endian
        public static void vCopyToArrayLittleEndian(Object qObject, byte[] qaTargetArray, int qiTargetArrayStartIndex)
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
            }
            else
            {
                throw new NotImplementedException("Big endian architecture not supported!");
            }
        }
    }
}
