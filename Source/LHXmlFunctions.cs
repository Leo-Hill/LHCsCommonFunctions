using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LHCommonFunctions.Source
{
    /***********************************************************************************************
    * 
    * This class provides functions for interacting with XML files
    * 
    **********************************************************************************************/
    public static class LHXmlFunctions
    {
        private const String DICT_XML_ROOT_ELEMENT_NAME = "rootElement";
        private const String DICT_XML_ITEM_ELEMENT_NAME = "item";
        private const String DICT_XML_ID_TAG = "id";

        /// <summary>
        /// This function creates a new XML file with an empty "root" node 
        /// </summary>
        /// <param name="filePath">The file path of the file to save</param>
        public static void CreateXmlSettingsFile(String filePath)
        {
            XDocument xmlFile = new XDocument();                                                    //Document to export
            XElement rootElement = new XElement(DICT_XML_ROOT_ELEMENT_NAME);
            xmlFile.Add(rootElement);

            String parentFolder = LHStringFunctions.GetParentFolder(filePath);
            if (!Directory.Exists(parentFolder))
            {
                Directory.CreateDirectory(parentFolder);
            }

            xmlFile.Save(filePath);                                                                 //Save the file 
        }

        /// <summary>
        /// This function converts a sorted dictionary to a XML file and exports the file
        /// </summary>
        /// <param name="exportDictionary">The dictionary to export. Key:The ID in the XML Value:The value to save</param>
        /// <param name="filePath">The file path of the file to save</param>
        public static void ExportDictionaryToXml(SortedDictionary<String, String> exportDictionary, String filePath)
        {
            XDocument exportFile = new XDocument();                                                 //Document to export
            XElement rootElement = new XElement(DICT_XML_ROOT_ELEMENT_NAME);
            foreach (String sKey in exportDictionary.Keys)
            {
                XElement insertElement = new XElement(DICT_XML_ITEM_ELEMENT_NAME, exportDictionary[sKey]);
                insertElement.SetAttributeValue(DICT_XML_ID_TAG, sKey);
                rootElement.Add(insertElement);
            }
            exportFile.Add(rootElement);

            String parentFolder = LHStringFunctions.GetParentFolder(filePath);
            if (!Directory.Exists(parentFolder))
            {
                Directory.CreateDirectory(parentFolder);
            }

            exportFile.Save(filePath);                                                              //Save the file 
        }

        /// <summary>
        /// This function imports a XML file and converts it to a sorted dictionary
        /// </summary>
        /// <param name="filePath">The file path of the file to import</param>
        /// <returns></returns>
        public static SortedDictionary<String, String> ImportXmlToDictionary(String filePath)
        {
            XDocument importfile = XDocument.Load(filePath);                                        //Load the xml file
            SortedDictionary<String, String> returnDict = new SortedDictionary<String, String>();
            foreach (XElement actElement in importfile.Descendants(DICT_XML_ITEM_ELEMENT_NAME))     //Loop through xml childs 
            {
                String id = actElement.Attribute(DICT_XML_ID_TAG).Value;
                String value = actElement.Value;
                returnDict.Add(id, value);
            }
            return returnDict;
        }
    }
}
