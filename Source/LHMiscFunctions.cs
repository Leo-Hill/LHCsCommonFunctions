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
        //This function deletes all files in a directory and subdirectory recursive
        public static void vDeleteAllFilesInDirectory(String qsPath)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(qsPath);

            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                fileInfo.Delete();
            }

            foreach(DirectoryInfo subDirectoryInfo in directoryInfo.GetDirectories())
            {
                vDeleteAllFilesInDirectory(subDirectoryInfo.FullName);
            }
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
