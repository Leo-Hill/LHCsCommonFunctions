using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace LHCommonFunctions.Classes {

    //IVCs

    //This converter converts a boolean values to a visibility inverted
    public class BooleanNotToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (true == (bool)value) {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    //This converter converts a boolean a visibility. In contrast to the default WPF converter, it will return hidden.
    public class IVCBooleanToVisibilityHiddenConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (true == (bool)value) {
                return Visibility.Visible;
            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    //This converter converts a boolean to a 'x' if it is true
    public class BooleanToStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (true == (bool)value) {
                return "x";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    //This converter converts a date-time to text
    public class DateTimeToStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            DateTime dateTime = (DateTime)value;                                                    //Input value is Datetime
            return dateTime.ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return Binding.DoNothing;
        }
    }

    //This converter converts a filter-string to a visibility depending on the passed parameter name
    public class FilterStringToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            String sFilter = (String)value;
            sFilter = sFilter.ToLower();

            String sViewName = (String)parameter;
            sViewName = sViewName.ToLower();

            String[] asFilters = sFilter.Split(' ');

            if (String.IsNullOrEmpty(sFilter)) {
                return Visibility.Visible;
            }
            if (String.IsNullOrEmpty(sViewName)) {
                return Visibility.Visible;
            }
            foreach (String act_filter in asFilters) {
                if (false == String.IsNullOrEmpty(act_filter) && false == sViewName.Contains(act_filter)) {
                    return Visibility.Collapsed;
                }
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return "";
        }
    }

    //This converter converts a boolean values to a visibility inverted
    public class InverseBooleanConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            if (true == (bool)value) {
                return false;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    //This converter converts an routedcommand's keyboard shortcut to a text
    public class RoutedCommandToInputGestureTextConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            RoutedCommand routedCommand = value as RoutedCommand;
            if (routedCommand != null) {
                InputGestureCollection inputGestureCollection = routedCommand.InputGestures;
                if ((inputGestureCollection != null) && (inputGestureCollection.Count >= 1)) {
                    for (int i = 0; i < inputGestureCollection.Count; i++)                          //Search for the first key gesture
                    {
                        KeyGesture keyGesture = ((IList)inputGestureCollection)[i] as KeyGesture;
                        if (keyGesture != null) {
                            return " (" + keyGesture.GetDisplayStringForCulture(CultureInfo.CurrentCulture) + ")";
                        }
                    }
                }
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return Binding.DoNothing;
        }
    }


    //MVCs

    //This converter converts multiple boolean values to a visibility (logic and)
    public class BooleanAndToVisibilityConverter : IMultiValueConverter {
        public object Convert(object[] qInputValues, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            foreach (object o in qInputValues) {
                if (o.GetType() != typeof(bool)) {
                    return Visibility.Collapsed;
                } else {
                    if (false == (bool)o) {
                        return Visibility.Visible;
                    }
                }
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    //This converter converts multiple boolean values to a visibility (logic or)
    public class BooleanOrToVisibilityConverter : IMultiValueConverter {
        public object Convert(object[] qInputValues, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            foreach (object o in qInputValues) {
                if (o.GetType() != typeof(bool)) {
                    return Visibility.Collapsed;
                } else {
                    if (true == (bool)o) {
                        return Visibility.Visible;
                    }
                }
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
