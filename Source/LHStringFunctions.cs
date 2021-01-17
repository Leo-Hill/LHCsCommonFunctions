using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LHCommonFunctions.Source
{
    /***********************************************************************************************
    * 
    * This class provides functions for common string operations
    * 
    * *********************************************************************************************/
    public static class LHStringFunctions
    {
        //This function returns the extention of a file
        public static String sGetFileExtention(String qsFilePath)
        {
            String[] aSplitStrings = qsFilePath.Split('.');
            return aSplitStrings[aSplitStrings.Count() - 1];
        }
    
        //This function returns the filename of a file
        public static String sGetFileName(String qsFilePath)
        {
            String[] aSplitStrings = qsFilePath.Split('\\');
            return aSplitStrings[aSplitStrings.Count() - 1];
        }

        //This function inserts a string in an observablecollection. 
        public static void vInsertStringInObservableCollection(ObservableCollection<String> qsOC, String qsString, ListSortDirection qLSD)
        {
            if (qsOC.Count == 0)
            {
                qsOC.Add(qsString);                                                                 //Add the string if the osc is empty
                return;
            }
            int iIndex = 0;                                                                         //The index to iterate trough the oc
            int iCompareValue;                                                                      //The value to compare the String.CompareTo result
            if (qLSD == ListSortDirection.Ascending)
            {
                iCompareValue = -1;
            }
            else
            {
                iCompareValue = 1;
            }
            while ((iIndex<qsOC.Count)&&(iCompareValue != qsString.CompareTo(qsOC[iIndex])))
            {
                iIndex++;
            }
            qsOC.Insert(iIndex, qsString);
        }

        //This function calculates the pixelsize of a textblock and returns the size
        public static Size SZMeasureString(TextBlock qTextBlock)
        {
            FormattedText formattedText = new FormattedText(
                qTextBlock.Text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(qTextBlock.FontFamily, qTextBlock.FontStyle, qTextBlock.FontWeight, qTextBlock.FontStretch),
                qTextBlock.FontSize,
                Brushes.Black,
                new NumberSubstitution(), TextFormattingMode.Display);
            return new Size(formattedText.Width, formattedText.Height);
        }
    }
}
