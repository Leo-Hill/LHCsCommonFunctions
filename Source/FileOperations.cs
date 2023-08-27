using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHCommonFunctions {
    /// <summary>
    /// This class contains helper functions for files  
    /// </summary>
    public static class FileOperations {
    
        /// <summary>
        /// This function deletes all files in a directory and subdirectory recursive
        /// </summary>
        /// <param name="directory_path">The directory whose content should be deleted</param>
        public static void DeleteAllFilesInDirectory(String directory_path) {
            DirectoryInfo directoryInfo = new DirectoryInfo(directory_path);

            foreach (FileInfo fileInfo in directoryInfo.GetFiles()) {
                fileInfo.Delete();
            }

            foreach (DirectoryInfo subDirectoryInfo in directoryInfo.GetDirectories()) {
                DeleteAllFilesInDirectory(subDirectoryInfo.FullName);
            }
        }

        /// <summary>
        /// This function checks if a file is locked
        /// </summary>
        /// <param name="file_path">The absolute path to the file to check</param>
        /// <returns>True if the file is locked, false if not</returns>
        public static bool FileIsLocked(String file_path) {
            FileInfo fileInfo = new FileInfo(file_path);
            try {
                using (FileStream stream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.None)) {
                    stream.Close();
                }
            } catch (IOException) {
                //the file is unavailable because it is:
                // - still being written to
                // - being processed by another thread
                // - does not exist
                return true;
            }
            return false;                                                                           //File is not locked
        }

        /// <summary>
        /// This function removes invalid chars in a filename
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <returns>The filename without the invalid characters</returns>
        public static String ReplaceInvalidCharactersInFileName(String fileName) {
            char[] invalidChars = Path.GetInvalidFileNameChars();                                 //Get invalid filename chars
            foreach (char c in invalidChars) {
                if (fileName.Contains(c)) {
                    fileName = fileName.Remove(c);
                }
            }
            return fileName;
        }
    }
}