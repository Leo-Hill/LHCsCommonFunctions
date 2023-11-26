using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
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
        /***********************************************************************************************
        * 
        * Constants
        * 
        **********************************************************************************************/
        public const int I_CODEPAGE_ISO_8859_1 = 28591;                                             //Codepage of ISO 8859-1

        /***********************************************************************************************
        * 
        * Functions
        * 
        **********************************************************************************************/
        /// <summary>
        /// This function converts a byte array to a string. Any non ISO 8859-1 character will be converted to a hex number with format '\0x00'
        /// </summary>
        /// <param name="byteArray">The array to convert</param>
        /// <returns>The array converted to a string</returns>
        public static String ConvertByteArrayToString(byte[] byteArray)
        {
            String returnString = "";
            foreach (byte actByte in byteArray)
            {
                if (actByte < ' ' || (actByte >= 0x7F && actByte <= 0x9F))                          //escape non ISO 8859-1 chars
                {
                    returnString += "\\0x" + actByte.ToString("X2");
                }
                else
                {
                    returnString += (char)actByte;
                }
            }
            return returnString;
        }

        /// <summary>
        /// This function gives you a string representing the content of the byteArray as hex numbers. 
        /// You can define a prefix for every hex number (e.g. "0x)
        /// The bytes will be separated by a space character.
        /// </summary>
        /// <param name="byteArray">The byte array converted to a string representing the hex numbers</param>
        /// <param name="prefix">Prefix put in front of every hex number</param>
        /// <returns>A string containing all bytes of the byteArray as hex numbers</returns>
        public static String ConvertByteArrayToHexString(byte[] byteArray, String prefix = "")
        {
            String sReturnString = "";
            foreach (byte actByte in byteArray)
            {
                {
                    sReturnString += prefix + actByte.ToString("X2") + " ";
                }
            }
            return sReturnString.Substring(0, sReturnString.Length - 1);
        }

        /// <summary>
        /// This function returns the string representation of a uint32 IP
        /// </summary>
        /// <param name="ipAddress">IP address to convert (LSB first)</param>
        /// <returns> the string representation of a uint32 IP</returns>
        public static String ConvertIpAddressToString(UInt32 ipAddress)
        {
            String retrunString = "";
            byte[] ipBytes = BitConverter.GetBytes(ipAddress);
            retrunString += ipBytes[0].ToString();
            retrunString += ".";
            retrunString += ipBytes[1].ToString();
            retrunString += ".";
            retrunString += ipBytes[2].ToString();
            retrunString += ".";
            retrunString += ipBytes[3].ToString();
            return retrunString;
        }

        /// <summary>
        /// This function converts a String to its ISO 8859-1 number representation.
        /// Hex numbers can be escaped by '\' (format \0x00). The null terminator is not included in the array.
        /// </summary>
        /// <param name="inputSting">The string to convert</param>
        /// <param name="copyNullTerminator">Specify if the null terminator should be copied to the array</param>
        /// <returns>A byte array containing the byte representation of the string</returns>
        public static byte[] ConvertStringToByteArray(String inputSting, bool copyNullTerminator)
        {
            Regex hexNumberRegex = new Regex("\\\\0[xX][0-9a-fA-F]{2}");
            MatchCollection hexNumberMatches = hexNumberRegex.Matches(inputSting);
            List<int> hexMatchesIndexes = new List<int>();

            List<byte> byteList = new List<byte>();

            foreach (Match match in hexNumberMatches)
            {
                hexMatchesIndexes.Add(match.Index);
            }

            int charCnt = 0;
            while (charCnt < inputSting.Length)
            {
                if (hexMatchesIndexes.Contains(charCnt))
                {
                    byteList.Add((byte)int.Parse("" + inputSting[charCnt + 3] + inputSting[charCnt + 4], NumberStyles.HexNumber));
                    charCnt += 5;
                }
                else
                {
                    byteList.Add((byte)inputSting[charCnt]);
                    charCnt++;
                }
            }
            if (copyNullTerminator)
            {
                byteList.Add(0x00);
            }
            return byteList.ToArray();
        }

        /// <summary>
        /// This function converts a IP-String to a uint32 IP.
        /// The uint32 will be in LSB first format.
        /// </summary>
        /// <param name="ipString">The IP address in string format</param>
        /// <returns>The IP in uint32 format.</returns>
        public static UInt32 ConvertStringToIpAddress(String ipString)
        {
            try
            {
                UInt32 u32ReturnIP = 0;
                String[] aSplitStrings = ipString.Split('.');

                u32ReturnIP += (UInt32)(byte.Parse(aSplitStrings[3]));
                u32ReturnIP = u32ReturnIP << 8;
                u32ReturnIP += (UInt32)(byte.Parse(aSplitStrings[2]));
                u32ReturnIP = u32ReturnIP << 8;
                u32ReturnIP += (UInt32)(byte.Parse(aSplitStrings[1]));
                u32ReturnIP = u32ReturnIP << 8;
                u32ReturnIP += (UInt32)(byte.Parse(aSplitStrings[0]));

                return u32ReturnIP;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// This function returns the extension of a file
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        /// <returns>The extension of the file</returns>
        public static String GetFileExtension(String filePath)
        {
            String[] splitStrings = filePath.Split('.');
            return splitStrings[splitStrings.Count() - 1];
        }

        /// <summary>
        /// This function returns the filename of a file.
        /// </summary>
        /// <param name="filePath">The path of the file</param>
        /// <returns>The name of the file</returns>
        public static String GetFileName(String filePath)
        {
            String[] splitStrings = filePath.Split('\\');
            return splitStrings[splitStrings.Count() - 1];
        }

        /// <summary>
        /// This gives you the parent folder of the specified file/folder path
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static String GetParentFolder(String filePath)
        {
            String[] splitStrings = filePath.Split('\\');
            String parent = "";

            for (int folderCnt = 0; folderCnt < splitStrings.Length - 1; folderCnt++)
            {
                parent += splitStrings[folderCnt];

                if (folderCnt < splitStrings.Length - 2)
                {
                    parent += "\\";
                }
            }
            return parent;
        }

        /// <summary>
        /// This function inserts a string into a sorted ObservableCollection sorted.
        /// </summary>
        /// <param name="collection">The collection to insert the string to</param>
        /// <param name="insertString">The string to insert</param>
        /// <param name="sortDirection">The sorting direction of the collection</param>
        public static void InsertStringInSortedObservableCollection(ObservableCollection<String> collection, String insertString, ListSortDirection sortDirection)
        {
            if (collection.Count == 0)
            {
                collection.Add(insertString);                                                                 //Add the string if the osc is empty
                return;
            }
            int iIndex = 0;                                                                         //The index to iterate trough the oc
            int iCompareValue;                                                                      //The value to compare the String.CompareTo result
            if (sortDirection == ListSortDirection.Ascending)
            {
                iCompareValue = -1;
            }
            else
            {
                iCompareValue = 1;
            }
            while ((iIndex < collection.Count) && (iCompareValue != insertString.CompareTo(collection[iIndex])))
            {
                iIndex++;
            }
            collection.Insert(iIndex, insertString);
        }

        //This function calculates the pixelsize of a textblock and returns the size
        public static System.Windows.Size MeasureString(TextBlock qTextBlock)
        {
            FormattedText formattedText = new FormattedText(
                qTextBlock.Text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(qTextBlock.FontFamily, qTextBlock.FontStyle, qTextBlock.FontWeight, qTextBlock.FontStretch),
                qTextBlock.FontSize,
                Brushes.Black,
                VisualTreeHelper.GetDpi(qTextBlock).PixelsPerDip);
            return new System.Windows.Size(formattedText.Width, formattedText.Height);
        }

        /// <summary>
        /// This function removes invalid chars in a file name
        /// </summary>
        /// <param name="fileName">The name to analyze</param>
        /// <returns>The fileName with valid chars only</returns>
        public static String RemoveInvalidCharactersInFileName(String fileName)
        {
            char[] acInvalidChars = Path.GetInvalidFileNameChars();                                 //Get invalid filename chars

            foreach (char c in acInvalidChars)
            {
                if (fileName.Contains(c))
                {
                    fileName = fileName.Replace(c.ToString(), "");
                }
            }
            return fileName;
        }

        /// <summary>
        /// This function converts a timespan to a string "HH:mm:ss".
        /// Note: HH are absolute hours, so it can be > 23
        /// </summary>
        /// <param name="timeSpan">The timespan to convert</param>
        /// <returns>The timeSpan converted to string</returns>
        public static String TimeSpanToString(TimeSpan timeSpan)
        {
            String sReturnString;
            sReturnString = (timeSpan.Days * 24 + timeSpan.Hours).ToString("00");
            sReturnString += ":";
            sReturnString += timeSpan.Minutes.ToString("00");
            sReturnString += ":";
            sReturnString += timeSpan.Seconds.ToString("00");
            return sReturnString;
        }

        /// <summary>
        /// This function converts a timespan to a string "HH'h' mm'm' SS's'". 
        /// Note: HH are absolute hours, so it can be > 23
        /// </summary>
        /// <param name="timeSpan">The timespan to convert</param>
        /// <returns>The timeSpan converted to string</returns>
        public static String TimeSpanToStringWithChars(TimeSpan timeSpan)
        {
            String sReturnString;
            sReturnString = (timeSpan.Days * 24 + timeSpan.Hours).ToString("00");
            sReturnString += "h ";
            sReturnString += timeSpan.Minutes.ToString("00");
            sReturnString += "m ";
            sReturnString += timeSpan.Seconds.ToString("00");
            sReturnString += "s";
            return sReturnString;
        }

    }
}
