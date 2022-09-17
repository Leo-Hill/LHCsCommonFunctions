using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LHCommonFunctions.Source
{
    /***********************************************************************************************
    * 
    * This class provides functions for interacting with xml files
    * 
    **********************************************************************************************/
    public static class LHXmlFunctions
    {
        private const String DICT_XML_ROOT_ELEMENT_NAME = "root";
        private const String DICT_XML_ITEM_ELEMENT_NAME = "item";
        private const String DICT_XML_ID_TAG = "id";

        //This function converts a sorted dictionary to a xml file and exports the file
        public static void vExportDictionaryToXml(SortedDictionary<String, String> qSDExport, String qsPath, String qsFilename)
        {
            XDocument XDExport = new XDocument();                                                   //Document to export
            XElement XERoot = new XElement(DICT_XML_ROOT_ELEMENT_NAME);
            foreach (String sKey in qSDExport.Keys)
            {
                XElement XEInsert = new XElement(DICT_XML_ITEM_ELEMENT_NAME, qSDExport[sKey]);
                XEInsert.SetAttributeValue(DICT_XML_ID_TAG, sKey);
                XERoot.Add(XEInsert);
            }
            XDExport.Add(XERoot);
            XDExport.Save(qsPath + "\\" + qsFilename);                                              //Save the file 

        }

        //This function imports a xml file and converts it to a sorted dictionary
        public static SortedDictionary<String, String> SDImportXmlToDictionary(String qsPath, String qsFilename)
        {
            XDocument XDImport = XDocument.Load(qsPath + "\\" + qsFilename);                        //Load the xml file
            SortedDictionary<String, String> SDReturn = new SortedDictionary<String, String>();
            foreach (XElement actElement in XDImport.Descendants(DICT_XML_ITEM_ELEMENT_NAME))                           //Loop through xml childs 
            {
                String sID = actElement.Attribute(DICT_XML_ID_TAG).Value;
                String sValue = actElement.Value;
                SDReturn.Add(sID, sValue);
            }

            return SDReturn;
        }
    }
}
