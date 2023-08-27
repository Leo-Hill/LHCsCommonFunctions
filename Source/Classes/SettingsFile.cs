using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LHCommonFunctions.Classes {
    /// <summary>
    /// This class is used to load and save settings for an application.
    /// Internally the settings are represented by a SortedDictionary and can be imported/exported from/as a XML file. 
    /// </summary>
    public class SettingsFile {

        //Assume we have a SortedDictionary with following content:
        //("Language", "de")
        //("Style", "Light")
        //...
        //With the element names defined below, the XML file will have the following format
        //<?xml version="1.0" encoding="utf-8"?>
        //<root>
        //  <item id="Language">de</item >
        //  <item id="Style">Light</item>
        //  <item ...
        //</root>
        private const string _settingsFileRootElementName = "root"; //This will be the root element for the XML file
        private const string _settingsFileItemName = "item"; //This will be item-name for elements in the XML file
        private const string _settingsFileIdTag = "id"; //This tag is used as ID for the stored values in the XML file

        private SortedDictionary<String, String> _settings;                                  //Dictionary containing the current settings <ID,Value>
        private String _filePath; //The absolute file path of the settings file


        /// <summary>
        /// Constructor will check if the settings XML file is already existing on the disk. 
        /// If the file exists, it will load the settings. If not, it will save the defaultSettings.
        /// </summary>
        /// <param name="filePath">The absolute path of the XML file.</param>
        /// <param name="defaultSettings">SortedDictionary containing default settings. Will be saved in case the XML file does not exist.</param>
        public SettingsFile(String filePath, SortedDictionary<Object, Object> defaultSettings) {
            _filePath = filePath;

            FileInfo fileInfo = new FileInfo(_filePath);
            if (!fileInfo.Exists) {
                if (!fileInfo.Directory.Exists) {
                    System.IO.Directory.CreateDirectory(fileInfo.DirectoryName);
                }
                _settings = new SortedDictionary<String, String>();
                foreach (KeyValuePair<Object, Object> pair in defaultSettings) {
                    _settings.Add(pair.Key.ToString(), pair.Value.ToString());
                }
                SaveSettings();
            } else {
                LoadSettings();
            }
        }

        /// <summary>
        /// This gives you the value stored in the settings.
        /// The function will checks if the value can be parsed to the desired type and if not it returns 0 (default enum value)
        /// </summary>
        /// <param name="key">The key of the setting you want to request</param>
        /// <param name="value_type">The type of the value you want to request</param>
        /// <returns></returns>
        public Object GetValue(Object key, Type value_type) {
            String key_string = key.ToString();
            if (!_settings.ContainsKey(key_string)) {
                return null;
            }
            Object value;
            Enum.TryParse(value_type, _settings[key_string], out value);
            if (value == null) {
                return 0;
            }
            return value;
        }

        /// <summary>
        /// This function will load the settings from the XML file. If the XML file doesn't exist, it will just return.
        /// </summary>
        public void LoadSettings() {
            if (!System.IO.File.Exists(_filePath)) {
                return;
            }
            //Load the settings file into the dictionary
            _settings = new SortedDictionary<String, String>();
            XDocument xmlDocument = XDocument.Load(_filePath);
            foreach (XElement actElement in xmlDocument.Descendants(_settingsFileItemName)) {
                String id = actElement.Attribute(_settingsFileIdTag).Value;
                String value = actElement.Value;
                _settings.Add(id, value);
            }
        }

        /// <summary>
        /// This function will export the current settings to the XML file.
        /// </summary>
        public void SaveSettings() {
            XDocument xmlDocument = new XDocument();
            XElement rootElement = new XElement(_settingsFileRootElementName);
            foreach (String key in _settings.Keys) {
                XElement item = new XElement(_settingsFileItemName, _settings[key]);
                item.SetAttributeValue(_settingsFileIdTag, key);
                rootElement.Add(item);
            }
            xmlDocument.Add(rootElement);
            xmlDocument.Save(_filePath);
        }

        /// <summary>
        /// This will set the passed value to the settings dictionary.
        /// The file will be saved after the value was set.
        /// In case the passed value is equal to the value in the stored settings, the function simply returns.
        /// </summary>
        /// <param name="key">The name of the setting to set</param>
        /// <param name="value">The value of the setting to set</param>
        public void SetValue(Object key, Object value) {
            String key_string = key.ToString();
            String value_string = value.ToString();

            if (_settings.ContainsKey(key_string) && _settings[key_string] == value_string) {
                //Value does not have to be updated.
                return;
            }
            _settings[key.ToString()] = value.ToString();
            SaveSettings();
        }

    }
}
