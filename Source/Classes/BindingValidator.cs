using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LHCommonFunctions.Classes {
    /// <summary>
    /// This class provides a function to validate bound input data and all children
    /// </summary>
    public static class BindingValidator {

        /// <summary>
        /// The dependency object is valid if it has no errors and all of its children (that are dependency objects) are error-free.
        /// </summary>
        /// <param name="obj">The object to analyze</param>
        /// <returns></returns>
        public static bool IsValid(DependencyObject obj) {
            return !Validation.GetHasError(obj) && LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>().All(IsValid);
        }
    }
}
