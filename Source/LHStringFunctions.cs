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
    **********************************************************************************************/
    public static class LHStringFunctions
    {
        //This function converts a byte array to a string. Any non ascii character will be converted to a hex number with format '\0x00'
        public static String sConvertByteArrayToString(byte [] qaByteArray)
        {
            String sReturnString = "";
            foreach(byte actByte in qaByteArray)
            {
                if(actByte<' '||actByte>'~')                                                        //actbyte smaller than space or bigger ~
                {
                    sReturnString += "\\0x" + actByte.ToString("X2");
                }
                else
                {
                    sReturnString += (char)actByte;
                }
            }
            return sReturnString;
        }

        //This function converts a String to its ASCII number representation. Hex numbers can be escaped by '\' (format \0x00). The null terminator is not included in the array.
        public static byte[] aConvertStringToByteArray(String qString)
        {
            List<byte> LNumbers = new List<byte>();
            int iStringCnt = 0;
            while (iStringCnt < qString.Length)
            {
                if ((qString[iStringCnt] == '\\')&& qString.Length > iStringCnt+4)                  //Check for escape character
                {
                    if (qString[iStringCnt + 1] == '0' && qString[iStringCnt + 2] == 'x')           //Check for Hex number
                    {
                        LNumbers.Add((byte)int.Parse("" + qString[iStringCnt + 3] + qString[iStringCnt + 4], NumberStyles.HexNumber));
                        iStringCnt += 5;
                    }
                    else                                                                            //Usual char
                    {
                        LNumbers.Add((byte)qString[iStringCnt]);
                        iStringCnt++;
                    }
                }
                else                                                                                //Usual char
                {
                    LNumbers.Add((byte)qString[iStringCnt]);
                    iStringCnt++;
                }
            }
            return LNumbers.ToArray();
        }

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

        //This function inserts a string in an observablecollection sorted. qLSD determines how the observablecollection is sorted
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
            while ((iIndex < qsOC.Count) && (iCompareValue != qsString.CompareTo(qsOC[iIndex])))
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
