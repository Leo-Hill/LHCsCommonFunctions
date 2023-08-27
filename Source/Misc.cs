using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LHCommonFunctions {
    /// <summary>
    /// This class contains functions for common operations not categorized
    /// </summary>
    public static class Misc {
        /// <summary>
        /// This function centers the window in the screen
        /// </summary>
        /// <param name="window">The window to center</param>
        public static void CenterWindowOnScreen(Window window) {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = window.Width;
            double windowHeight = window.Height;
            window.Left = (screenWidth / 2) - (windowWidth / 2);
            window.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        /// <summary>
        /// This function returns the currently active window
        /// </summary>
        /// <returns></returns>
        public static Window GetActiveWindow() {
            return Application.Current.Windows.OfType<Window>().SingleOrDefault(window => window.IsActive);
        }

        /// <summary>
        /// This function returns the name of a control, which invoked am command 
        /// </summary>
        /// <param name="event_args">The arguments passed to the event</param>
        /// <returns></returns>
        public static String GetNameOfRoutedEventArgsSource(RoutedEventArgs event_args) {
            FrameworkElement frameworkElement = event_args.Source as FrameworkElement;
            return frameworkElement.Name;
        }

        /// <summary>
        /// This function inverts a ListSortDirection
        /// </summary>
        /// <param name="listSortDescription">The ListSortDirection to invert</param>
        /// <returns>The inverted ListSortDirection</returns>
        public static ListSortDirection InvertListSortDirection(ListSortDirection listSortDescription) {
            if (ListSortDirection.Ascending == listSortDescription) {
                return ListSortDirection.Descending;
            }
            return ListSortDirection.Ascending;
        }
    }
}
