using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using PhotoOrganizer.BusinessModule.Common;

namespace PhotoOrganizer.BusinessModule
{
    public class SettingManager : ISettingManager
    {
        private const string settingFilePath = @"PhotoOrganizerSettings.xml";
        private const string attSearchPath = @"//SettingNode[@SettingName='{0}']";
        private const string settingNodeName = @"SettingNode";
        private const string settingNodeAttName = @"SettingName";

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
                CreateOrUpdateElement(settingName, value, doc, root);
                doc.Save(settingFilePath);
            }
            catch (Exception ex)
            { 
            }
        }

        public void SaveSetting(Settings setting)
        {
            SaveSetting(Settings.ScanBasePathStr, setting.ScanBasePath);
            SaveSetting(Settings.OverviewFolderBasePathStr, setting.OverviewFolderBasePath);
            SaveSetting(Settings.FrontPictureNameStr, setting.FrontPictureName);
            SaveSetting(Settings.BackPictureNameStr, setting.BackPictureName);
        }

        public string ReadSettingString(string settingName)
        {
            try
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
            catch
            {
               
            }

             return "";            
        }

        public void ReadSetting(Settings setting)
        {
            setting.ScanBasePath = ReadSettingString(Settings.ScanBasePathStr);
            setting.OverviewFolderBasePath = ReadSettingString(Settings.OverviewFolderBasePathStr);
            setting.FrontPictureName = ReadSettingString(Settings.FrontPictureNameStr);
            setting.BackPictureName = ReadSettingString(Settings.BackPictureNameStr);

        }

        private static void CreateOrUpdateElement(string settingName, string value, XmlDocument doc, XmlNode parent)
        {
            var existingElement = parent.SelectSingleNode(string.Format(attSearchPath, settingName));

            if (existingElement != null && existingElement.InnerText != value)
            {
                existingElement.InnerText = value;
            }
            else
            {
                XmlElement element = doc.CreateElement(settingNodeName);
                element.SetAttribute(settingNodeAttName, settingName);
                element.InnerText = value;
                parent.AppendChild(element);
            }
        }
       
    }
}
