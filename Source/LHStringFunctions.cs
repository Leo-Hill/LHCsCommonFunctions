using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHCommonFunctions.Source
{
    /***********************************************************************************************
    * 
    * This class provides functions for common string operations
    * 
    * *********************************************************************************************/
    public static class LHStringFunctions
    {
        //This function returns the extention of a file
        public static String sGetFileExtention(String qsFilePath)
        {
            String[] aSplitStrings = qsFilePath.Split('.');
            return aSplitStrings[aSplitStrings.Count() - 1];
        }
    }
}
