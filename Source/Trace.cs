using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHCommonFunctions {
    /// <summary>l
    /// This class provides functions for tracing and logging
    /// </summary>
    public static class Trace {
        //

        /// <summary>
        /// This function prints text to the output window. Optionally it adds a TimeStamp in front of the text
        /// </summary>
        /// <param name="text">The text to print</param>
        /// <param name="timeStamp">Indicates if a timestamp should be added in front of the text</param>
        public static void vTraceLine(String text, bool timeStamp = true) {
            if (true == timeStamp) {
                System.Diagnostics.Trace.Write(DateTime.Now.ToString("HH:mm:ss:fff"));
                System.Diagnostics.Trace.Write("\t");
            }
            System.Diagnostics.Trace.WriteLine(text);
        }

    }
}
