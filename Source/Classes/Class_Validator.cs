using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LHCommonFunctions.Source
{


    /***********************************************************************************************
    * 
    * This class provides a function to validate bound input data and all children
    * 
    **********************************************************************************************/
    public static class Class_Validator
    {
        public static bool IsValid(DependencyObject obj)
        {
            // The dependency object is valid if it has no errors and all of its children (that are dependency objects) are error-free.
            return !Validation.GetHasError(obj) &&LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().All(IsValid);
        }
    }
}
