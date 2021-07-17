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
    * This class provides fu´nctions to create and write to a logfile
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
        List<String> LsBufferedLogMessages;                                                         //List for storing log messages and writing them out later
        String sFileName;                                                                           //Name of the file


        //Objects
        StreamWriter streamWriter;                                                                  //Stream for writing to the file


        /***********************************************************************************************
        * 
        * Constructor
        * 
        **********************************************************************************************/

        public Class_LogFile(String qsLogName, String qsFolderPath)
        {
            //Initialize variables
            //Primitive
            LsBufferedLogMessages = new List<String>();
            sFileName = qsFolderPath +"/"+ DateTime.Now.ToString("yyyyMMdd_HHmmss_") + qsLogName + ".txt";    //Build the filename

            //Check if the target directory exists
            if(false==Directory.Exists(qsFolderPath))
            {
                Directory.CreateDirectory(qsFolderPath);
            }

        }

        /***********************************************************************************************
        * 
        * Functions
        * 
        **********************************************************************************************/

        //This function writes the qsMessage to the log file
        public void vLog(String qsMessage, bool qbTraceToDebugWindow = true)
        {
            streamWriter = new StreamWriter(sFileName, true);
            streamWriter.WriteLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "\t" + qsMessage);
            if (qbTraceToDebugWindow)
            {
                LHTraceFunctions.vTraceLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "\t" + qsMessage);    //Trace to the output window
            }
            streamWriter.Close();
        }

        //This function stores the qsMessage to the LsBufferedLogMessages
        public void vLogToBuffer(String qsMessage, bool qbTraceToDebugWindow = true)
        {
            LsBufferedLogMessages.Add(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "\t" + qsMessage);
            if (qbTraceToDebugWindow)
            {
                LHTraceFunctions.vTraceLine(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "\t" + qsMessage);    //Trace to the output window
            }
        }

        //This function writes out all buffered messages from the LsBufferedLogMessages and clears the list
        public void vWriteOutBuffer()
        {
            streamWriter = new StreamWriter(sFileName, true);
            foreach (String sMessage in LsBufferedLogMessages)
            {
                streamWriter.WriteLine(sMessage);
            }
            streamWriter.Close();
            LsBufferedLogMessages.Clear();
        }

    }
}
