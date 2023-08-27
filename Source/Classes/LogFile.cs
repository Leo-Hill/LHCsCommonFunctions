using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHCommonFunctions.Classes {

    /// <summary>
    /// This class provides functions to create and write to a log-file
    /// </summary>
    public class LogFile {
        private readonly String _timeFormat = "dd.MM.yyyy HH:mm:ss";

        private List<String> _bufferedLogMessages;                                                         //List for buffering log messages and writing them out later
        private  String _filePath;                              //The absolute path of the log-file
        private StreamWriter _streamWriter;                      //Stream for writing to the file

        /// <summary>
        /// Constructor
        /// Will create the passed directory if it doesn't exist.
        /// </summary>
        /// <param name="fileName">The name of the log-file</param>
        /// <param name="folderPath">Path where the log-file should be created.</param>
        public LogFile(String fileName, String folderPath) {
            _bufferedLogMessages = new List<String>();
            _filePath = folderPath + "/" + DateTime.Now.ToString("yyyyMMdd_HHmmss_") + fileName + ".txt";

            if (false == Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }
        }

        /// <summary>
        /// This gives you the filePath of the log-file.
        /// </summary>
        /// <returns>The filePath of the log-file</returns>
        public String GetFilePath() {
            return _filePath;
        }

        /// <summary>
        /// This function appends the passed message to the log file.
        /// </summary>
        /// <param name="message">The message to append</param>
        /// <param name="traceToDebugWindow">True, if you want the message to be traced in the debug window</param>
        public void Log(String message, bool traceToDebugWindow = true) {
            _streamWriter = new StreamWriter(_filePath, true);
            _streamWriter.WriteLine(DateTime.Now.ToString(_timeFormat) + "\t" + message);
            _streamWriter.Close();
            if (traceToDebugWindow) {
                Trace.vTraceLine(DateTime.Now.ToString(_timeFormat) + "\t" + message);
            }
        }

        /// <summary>
        /// This function buffers the passes message. 
        /// It can be used in order to prevent continuously opening and closing the log-file.
        /// Call WriteOutBuffer in order to write out the buffered messages.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="traceToDebugWindow"></param>
        public void LogToBuffer(String message, bool traceToDebugWindow = true) {
            _bufferedLogMessages.Add(DateTime.Now.ToString(_timeFormat) + "\t" + message);
            if (traceToDebugWindow) {
                Trace.vTraceLine(DateTime.Now.ToString(_timeFormat) + "\t" + message);    //Trace to the output window
            }
        }

        /// <summary>
        /// This function writes out all buffered messages to the log-file
        /// </summary>
        public void WriteOutBuffer() {
            _streamWriter = new StreamWriter(_filePath, true);
            foreach (String sMessage in _bufferedLogMessages) {
                _streamWriter.WriteLine(sMessage);
            }
            _streamWriter.Close();
            _bufferedLogMessages.Clear();
        }

    }
}