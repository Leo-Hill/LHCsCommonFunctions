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

namespace LHCommonFunctions {
    /***********************************************************************************************
    * 
    * 
    * 
    **********************************************************************************************/

    /// <summary>
    /// This class provides functions for common string operations
    /// </summary>
    public static class StringOperations {
        public const int CodepageIso8859_1 = 28591;                                             //Code-page of ISO 8859-1

        /// <summary>
        /// This function converts a byte array to a string. Any non ISO 8859-1 character will be converted to a hex number with format '\0x00'
        /// </summary>
        /// <param name="bytes">The byte array to convert</param>
        /// <returns>The byte array converted to a String</returns>
        public static String ConvertByteArrayToString(byte[] bytes) {
            String sReturnString = "";
            foreach (byte actByte in bytes) {
                if (actByte < ' ' || (actByte >= 0x7F && actByte <= 0x9F))                          //escape non ISO 8859-1 chars
                {
                    sReturnString += "\\0x" + actByte.ToString("X2");
                } else {
                    sReturnString += (char)actByte;
                }
            }
            return sReturnString;
        }

        /// <summary>
        /// This function converts a byte array to a string containing hex numbers. You can define a prefix for every hex number.
        /// </summary>
        /// <param name="bytes">The byte array to convert</param>
        /// <param name="prefix">The prefix inserted in front of every hex number e.g. "0x"</param>
        /// <returns></returns>
        public static String ConvertByteArrayToHexString(byte[] bytes, String prefix = "") {
            String returnString = "";
            foreach (byte actByte in bytes) {
                {
                    returnString += prefix + actByte.ToString("X2") + " ";
                }
            }
            return returnString;
        }

        /// <summary>
        /// This function returns the string representation of a uint32 IP
        /// </summary>
        /// <param name="ip_address">The IP in network format</param>
        /// <returns>The string representation of the IP</returns>
        public static String ConvertIpAddressToString(UInt32 ip_address) {
            String returnString = "";
            byte[] ipBytes = BitConverter.GetBytes(ip_address);
            returnString += ipBytes[0].ToString();
            returnString += ".";
            returnString += ipBytes[1].ToString();
            returnString += ".";
            returnString += ipBytes[2].ToString();
            returnString += ".";
            returnString += ipBytes[3].ToString();
            return returnString;
        }

        /// <summary>
        /// This function converts a String to its ISO 8859-1 byte representation. Hex numbers can be escaped by '\' (format \0x00). The null terminator is not included in the array.
        /// </summary>
        /// <param name="stringToConvert">The string to convert</param>
        /// <returns>A byte array containing the ISO 8859-1 byte representation of the passed string</returns>
        public static byte[] ConvertStringToByteArray(String stringToConvert) {
            List<byte> numbers = new List<byte>();
            int stringCnt = 0;
            while (stringCnt < stringToConvert.Length) {
                if ((stringToConvert[stringCnt] == '\\') && stringToConvert.Length > stringCnt + 4)               //Check for escape character
                {
                    if (stringToConvert[stringCnt + 1] == '0' && stringToConvert[stringCnt + 2] == 'x')           //Check for Hex number
                    {
                        numbers.Add((byte)int.Parse("" + stringToConvert[stringCnt + 3] + stringToConvert[stringCnt + 4], NumberStyles.HexNumber));
                        stringCnt += 5;
                    } else                                                                            //Usual char
                      {
                        numbers.Add((byte)stringToConvert[stringCnt]);
                        stringCnt++;
                    }
                } else                                                                                //Usual char
                  {
                    numbers.Add((byte)stringToConvert[stringCnt]);
                    stringCnt++;
                }
            }
            return numbers.ToArray();
        }

        /// <summary>
        /// This function converts a IP-String to a uint32 IP 
        /// </summary>
        /// <param name="ip_address">The IP address to convert</param>
        /// <returns>The IP address as uint32</returns>
        public static UInt32 ConvertStringToIpAddress(String ip_address) {
            try {
                UInt32 returnIpAddress = 0;
                String[] splitStrings = ip_address.Split('.');

                returnIpAddress += (UInt32)(byte.Parse(splitStrings[3]));
                returnIpAddress = returnIpAddress << 8;
                returnIpAddress += (UInt32)(byte.Parse(splitStrings[2]));
                returnIpAddress = returnIpAddress << 8;
                returnIpAddress += (UInt32)(byte.Parse(splitStrings[1]));
                returnIpAddress = returnIpAddress << 8;
                returnIpAddress += (UInt32)(byte.Parse(splitStrings[0]));
                return returnIpAddress;
            } catch {
                return 0;
            }
        }


        /// <summary>
        /// This function returns the extension of a file
        /// </summary>
        /// <param name="filePath">The absolute path of the file</param>
        /// <returns></returns>
        public static String GetFileExtension(String filePath) {
            String[] splitStrings = filePath.Split('.');
            return splitStrings[splitStrings.Count() - 1];
        }

        /// <summary>
        /// This function returns the filename of a file without its extension
        /// </summary>
        /// <param name="filePath">The absolute path of the file</param>
        /// <returns></returns>
        public static String GetFileName(String filePath) {
            String[] splitStrings = filePath.Split('\\');
            return splitStrings[splitStrings.Count() - 1];
        }

        /// <summary>
        /// This function inserts a string in sorted ObservableCollection.
        /// </summary>
        /// <param name="observableCollection">The ObservableCollection to insert the String to</param>
        /// <param name="insert_string">The String to insert</param>
        /// <param name="sort_direction">Indicates if the list is sorted ascending or descending</param>
        public static void vInsertStringInObservableCollection(ObservableCollection<String> observableCollection, String insert_string, ListSortDirection sort_direction) {
            if (observableCollection.Count == 0) {
                observableCollection.Add(insert_string);                                                                 //Add the string if the osc is empty
                return;
            }
            int iIndex = 0;                                                                         //The index to iterate trough the oc
            int iCompareValue;                                                                      //The value to compare the String.CompareTo result
            if (sort_direction == ListSortDirection.Ascending) {
                iCompareValue = -1;
            } else {
                iCompareValue = 1;
            }
            while ((iIndex < observableCollection.Count) && (iCompareValue != insert_string.CompareTo(observableCollection[iIndex]))) {
                iIndex++;
            }
            observableCollection.Insert(iIndex, insert_string);
        }

        /// <summary>
        /// This function calculates the pixel-size of a text-block and returns the size
        /// </summary>
        /// <param name="textBlock">The TextBlock to evaluate</param>
        /// <returns>The Size of the TextBlock</returns>
        public static System.Windows.Size GetTextBlockSize(TextBlock textBlock) {
            FormattedText formattedText = new FormattedText(
                textBlock.Text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch),
                textBlock.FontSize,
                Brushes.Black,
                VisualTreeHelper.GetDpi(textBlock).PixelsPerDip);
            return new System.Windows.Size(formattedText.Width, formattedText.Height);
        }

        /// <summary>
        /// This function converts a timespan to a string "HH:mm:SS". HH is in total hours -> can be > 23
        /// </summary>
        /// <param name="timeSpan">The TimeSpan to convert</param>
        /// <returns>The timespan in string format</returns>
        public static String TimeSpanToString(TimeSpan timeSpan) {
            String returnString;
            returnString = (timeSpan.Days * 24 + timeSpan.Hours).ToString("00");
            returnString += ":";
            returnString += timeSpan.Minutes.ToString("00");
            returnString += ":";
            returnString += timeSpan.Seconds.ToString("00");
            return returnString;
        }

        /// <summary>
        /// This function converts a timespan to a string "HH'h' mm'm' SS's'". HH is in total hours -> can be > 23
        /// </summary>
        /// <param name="timeSpan">The TimeSpan to convert</param>
        /// <returns>The timespan in string format</returns>
        public static String TimeSpanToStringWithChars(TimeSpan timeSpan) {
            String returnString;
            returnString = (timeSpan.Days * 24 + timeSpan.Hours).ToString("00");
            returnString += "h ";
            returnString += timeSpan.Minutes.ToString("00");
            returnString += "m ";
            returnString += timeSpan.Seconds.ToString("00");
            returnString += "s ";
            return returnString;
        }

    }
}
