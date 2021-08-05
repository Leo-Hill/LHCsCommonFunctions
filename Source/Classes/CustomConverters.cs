using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace LHCommonFunctions.Source
{
    //This converter converts an routedcommand's keyboard shortcut to a text
    public class IVCRoutedCommandToInputGestureText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            RoutedCommand routedCommand = value as RoutedCommand;
            if (routedCommand != null)
            {
                InputGestureCollection inputGestureCollection = routedCommand.InputGestures;
                if ((inputGestureCollection != null) && (inputGestureCollection.Count >= 1))
                {
                    for (int i = 0; i < inputGestureCollection.Count; i++)                          //Search for the first key gesture
                    {
                        KeyGesture keyGesture = ((IList)inputGestureCollection)[i] as KeyGesture;
                        if (keyGesture != null)
                        {
                            return " (" + keyGesture.GetDisplayStringForCulture(CultureInfo.CurrentCulture) + ")";
                        }
                    }
                }
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    //This converter converts a datetime to text
    public class IVCDateTimeToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateTime = (DateTime)value;                                                    //Input value is Datetime
            return dateTime.ToShortDateString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    //This converter converts multiple boolean values to a visibility (logic and)
    public class MVCBooleanAndToVisibility : IMultiValueConverter
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
                    if (false == (bool)o)
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