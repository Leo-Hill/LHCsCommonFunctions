using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHCommonFunctions.Source
{
    /***********************************************************************************************
    * 
    * This class provides functions for tracing and logging
    * 
    **********************************************************************************************/
    public static class LHTraceFunctions
    {
        //This function traces the qsText to the output window. if qbTimeStamp == true, it adds a TimeStamp in front of the text
        public static void vTraceLine(String qsText, bool qbTimeStamp = true)
        {
            if(true==qbTimeStamp)
            {
                Trace.Write(DateTime.Now.ToString("HH:mm:ss:fff"));
                Trace.Write("\t");
            }
            Trace.WriteLine(qsText);
        }

    }
}
