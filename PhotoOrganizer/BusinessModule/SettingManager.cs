using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace PhotoOrganizer.BusinessModule
{
    public class SettingManager
    {
        private const string settingFilePath = @"PhotoOrganizerSettings.xml";
        private const string attSearchPath = @"//SettingNode[@SettingName='{0}']";
        private const string settingNodeName = @"SettingNode";
        private const string settingNodeAttName = "@SettingName";

        public void SaveSetting(string settingName, string value)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root;
                if (File.Exists(settingFilePath))
                {
                    doc.Load(settingFilePath);
                }
                else
                {
                    doc.LoadXml("<Settings></Settings>");
                }

                root = doc.DocumentElement;
                CreateNewElement(settingName, value, doc, root);
                doc.Save(settingFilePath);
            }
            catch (Exception ex)
            { 
            }
        }

        public string ReadSettingString(string settingName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(settingFilePath);
            XmlElement root = doc.DocumentElement;

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("", "");
            string xPathString = string.Format(attSearchPath, settingName);            
            XmlNode selected = root.SelectSingleNode(xPathString, nsmgr);
            return selected.InnerText;
        }

        private static void CreateNewElement(string settingName, string value, XmlDocument doc, XmlNode parent)
        {
            XmlElement element = doc.CreateElement(settingNodeName);
            element.SetAttribute(settingNodeAttName, settingName);
            element.InnerText = value;
            parent.AppendChild(element);
        }
       
    }
}
