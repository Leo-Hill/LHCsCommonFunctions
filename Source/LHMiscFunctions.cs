using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LHCommonFunctions.Source
{
    /***********************************************************************************************
    * 
    * This class provides functions for common operations
    * 
    **********************************************************************************************/
    public static class LHMiscFunctions
    {
        //This function centers the qWindow in the screen
        public static void vCenterWindowOnScreen(Window qWindow)
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = qWindow.Width;
            double windowHeight = qWindow.Height;
            qWindow.Left = (screenWidth / 2) - (windowWidth / 2);
            qWindow.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        //This function deletes all files in a directory and subdirectory recursive
        public static void vDeleteAllFilesInDirectory(String qsPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(qsPath);

            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                fileInfo.Delete();
            }

            foreach (DirectoryInfo subDirectoryInfo in directoryInfo.GetDirectories())
            {
                vDeleteAllFilesInDirectory(subDirectoryInfo.FullName);
            }
        }

        //This function checks if a file is locked/opened
        public static bool bFileIsLocked(String qFilePath)
        {
            FileInfo fileInfo = new FileInfo(qFilePath);
            try
            {
                using (FileStream stream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            return false;                                                                           //File is not locked
        }

        //This function returns the currently active window
        public static Window GetActiveWindow()
        {
            return Application.Current.Windows.OfType<Window>().SingleOrDefault(window => window.IsActive);
        }

        //This funcionreturns the name of a control, which invokded am command 
        public static String SGetNameOfRoutedEventArgsSource(RoutedEventArgs e)
        {
            FrameworkElement frameworkElement = e.Source as FrameworkElement;
            return frameworkElement.Name;
        }

        //This function inverts a ListSortDirection
        public static ListSortDirection LSDInvertListSortDirection(ListSortDirection qListSortDescription)
        {
            if (ListSortDirection.Ascending == qListSortDescription)
            {
                return ListSortDirection.Descending;
            }
            return ListSortDirection.Ascending;
        }
    }
}
