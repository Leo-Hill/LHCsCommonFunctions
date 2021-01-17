using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LHCommonFunctions.Source
{
    /***********************************************************************************************
    * 
    * This class provides functions for common operations
    * 
    * *********************************************************************************************/
    public static class LHMiscFunctions
    {
        //This function inverts a ListSortDirection
        public static ListSortDirection LSDInvertListSortDirection(ListSortDirection qListSortDescription)
        {
            if(ListSortDirection.Ascending==qListSortDescription)
            {
                return ListSortDirection.Descending;
            }
            return ListSortDirection.Ascending;
        }
    }
}
