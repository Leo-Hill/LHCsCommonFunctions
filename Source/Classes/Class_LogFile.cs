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
    * This class provides functions to create and write to a logfile
    * 
    **********************************************************************************************/
    public class Class_LogFile
    {

        /***********************************************************************************************
        * 
        * Variables
        * 
        **********************************************************************************************/

        //Primitive
        private List<String> _bufferedLogMessages;                                                  //List for storing log messages and writing them out later
        private readonly String dateTimeFormat = "dd.MM.yyyy HH:mm:ss";                             //Format used for writing/ptinting messages
        public String FileName { get; private set; }                                                //Name of the file

        //Objects
        private StreamWriter streamWriter;                                                          //Stream for writing to the file


        /***********************************************************************************************
        * 
        * Constructor
        * 
        **********************************************************************************************/

        /// <summary>
        /// Constructor creates the file and necessary folders.
        /// </summary>
        /// <param name="fileName">Name of the file. ".txt" will be added</param>
        /// <param name="path">The folder where to save the file to</param>
        public Class_LogFile(String fileName, String path)
        {
            //Initialize variables
            //Primitive
            _bufferedLogMessages = new List<String>();
            FileName = path + "\\" + DateTime.Now.ToString("yyyyMMdd_HHmmss_") + fileName + ".txt";    //Build the filename

            //Check if the target directory exists
            if (false == Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

        }

        /***********************************************************************************************
        * 
        * Functions
        * 
        **********************************************************************************************/

        /// <summary>
        /// This function writes the message to the log file
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="traceToDebugWindow">Specify if you want to show the message in the debug window (true by default)</param>
        public void Log(String message, bool traceToDebugWindow = true)
        {
            streamWriter = new StreamWriter(FileName, true);
            streamWriter.WriteLine(DateTime.Now.ToString(dateTimeFormat) + "\t" + message);
            if (traceToDebugWindow)
            {
                LHTraceFunctions.vTraceLine(DateTime.Now.ToString(dateTimeFormat) + "\t" + message);    //Trace to the output window
            }
            streamWriter.Close();
        }

        /// <summary>
        /// This function can be used to cache messages in an internal buffer and write them out later.
        /// This might be useful if you want to log many messages in a short time and prevent opening/closing the file for every message.
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="traceToDebugWindow">Specify if you want to show the message in the debug window (true by default)</param>
        public void LogToBuffer(String message, bool traceToDebugWindow = true)
        {
            _bufferedLogMessages.Add(DateTime.Now.ToString(dateTimeFormat) + "\t" + message);
            if (traceToDebugWindow)
            {
                LHTraceFunctions.vTraceLine(DateTime.Now.ToString(dateTimeFormat) + "\t" + message);    //Trace to the output window
            }
        }

        /// <summary>
        /// This function writes out all buffered messages cached with LogToBuffer.
        /// </summary>
        public void WriteOutBuffer()
        {
            streamWriter = new StreamWriter(FileName, true);
            foreach (String sMessage in _bufferedLogMessages)
            {
                streamWriter.WriteLine(sMessage);
            }
            streamWriter.Close();
            _bufferedLogMessages.Clear();
        }

    }
}
