using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    * *********************************************************************************************/
    public static class LHMiscFunctions
    {

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
