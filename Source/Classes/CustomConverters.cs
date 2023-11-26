using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace LHCommonFunctions.Source
{

    //IVCs

    //This converter converts a boolean values to a visibility inverted
    public class IVCBooleanNotToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (true == (bool)value)
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //This converter converts a boolean a visibility. In contrast to the default WPF converter, it will return hidden.
    public class IVCBooleanToVisibilityHidden : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (true == (bool)value)
            {
                return Visibility.Visible;
            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //This converter converts a boolean to a 'x' if it is true
    public class IVCBooleanToX : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (true == (bool)value)
            {
                return "x";
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //This converter converts a datetime to text
    public class IVCDateTimeToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateTime = (DateTime)value;                                                    //Input value is Datetime
            return dateTime.ToString("dd.MM.yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //This converter converts a filter-string to a visibility depending on the passed parameter name
    public class IVCFilterStringToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String sFilter = (String)value;
            sFilter = sFilter.ToLower();

            String sViewName = (String)parameter;
            sViewName = sViewName.ToLower();

            String[] asFilters = sFilter.Split(' ');

            if (String.IsNullOrEmpty(sFilter))
            {
                return Visibility.Visible;
            }
            if (String.IsNullOrEmpty(sViewName))
            {
                return Visibility.Visible;
            }
            foreach (String act_filter in asFilters)
            {
                if (false == String.IsNullOrEmpty(act_filter) && false == sViewName.Contains(act_filter))
                {
                    return Visibility.Collapsed;
                }
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //This converter converts a boolean values to a visibility inverted
    public class IVCInverseBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (true == (bool)value)
            {
                return false;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //This converter converts an routedcommand's keyboard shortcut to a text
    public class IVCRoutedCommandToInputGestureText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RoutedCommand routedCommand = value as RoutedCommand;
            if (routedCommand == null)
            {
                return Binding.DoNothing;
            }
            InputGestureCollection inputGestureCollection = routedCommand.InputGestures;
            if (inputGestureCollection == null || inputGestureCollection.Count == 0)
            {
                return Binding.DoNothing;
            }
            for (int i = 0; i < inputGestureCollection.Count; i++)                                  //Search for the first key gesture
            {
                KeyGesture keyGesture = ((IList)inputGestureCollection)[i] as KeyGesture;
                if (keyGesture != null)
                {
                    return "(" + keyGesture.GetDisplayStringForCulture(CultureInfo.CurrentCulture) + ")";
                }
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    //MVCs

    //This converter converts multiple boolean values to a visibility (logic and)
    public class MVCBooleanAndToVisibility : IMultiValueConverter
    {
        public object Convert(object[] qInputValues, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            foreach (object o in qInputValues)
            {
                if (o.GetType() != typeof(bool))
                {
                    continue;
                }
                else if (false == (bool)o)
                {
                    return Visibility.Collapsed;
                }
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    //This converter converts multiple boolean values to a visibility (logic or)
    public class MVCBooleanOrToVisibility : IMultiValueConverter
    {
        public object Convert(object[] qInputValues, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            foreach (object o in qInputValues)
            {
                if (o.GetType() != typeof(bool))
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    if (true == (bool)o)
                    {
                        return Visibility.Visible;
                    }
                }
            }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
