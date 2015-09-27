using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using PhotoOrganizer.BusinessModule.Common;
using PhotoOrganizer.BusinessModule.Log;

namespace PhotoOrganizer.BusinessModule
{
    public class SettingManager : ISettingManager
    {
        private const string settingFilePath = @"PhotoOrganizerSettings.xml";
        private const string attSearchPath = @"//SettingNode[@SettingName='{0}']";
        private const string settingNodeName = @"SettingNode";
        private const string settingNodeAttName = @"SettingName";

        ILogger log;
        public SettingManager()
        {
            log = FileLogger.CreateLogger("SettingManager");
        }

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
                log.LogException(string.Format("Save Setting {0}={1} error.", settingName, value), ex);
            }
        }

        public void SaveSetting(Settings setting)
        {
            SaveSetting(Settings.ScanBasePathStr, setting.ScanBasePath);
            SaveSetting(Settings.OverviewFolderBasePathStr, setting.OverviewFolderBasePath);
            SaveSetting(Settings.FrontPictureNameStr, setting.FrontPictureName);
            SaveSetting(Settings.BackPictureNameStr, setting.BackPictureName);
            SaveSetting(Settings.FrontPictureRotateStr, setting.FrontPictureRotate);
            SaveSetting(Settings.BackPictureRotateStr, setting.BackPictureRotate);
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
                log.LogInfo(string.Format("Read Setting {0}={1}", settingName, selected.InnerText));   
                return selected.InnerText;
            }
            catch(Exception ex)
            {
                log.LogException(string.Format("Read Setting {0} error.", settingName), ex);     
            }

             return "";            
        }

        public void ReadSetting(Settings setting)
        {
            setting.ScanBasePath = ReadSettingString(Settings.ScanBasePathStr);
            setting.OverviewFolderBasePath = ReadSettingString(Settings.OverviewFolderBasePathStr);
            setting.FrontPictureName = ReadSettingString(Settings.FrontPictureNameStr);
            setting.BackPictureName = ReadSettingString(Settings.BackPictureNameStr);
            setting.FrontPictureRotate = ReadSettingString(Settings.FrontPictureRotateStr);
            setting.BackPictureRotate = ReadSettingString(Settings.BackPictureRotateStr);

        }

        private static void CreateOrUpdateElement(string settingName, string value, XmlDocument doc, XmlNode parent)
        {
            var existingElement = parent.SelectSingleNode(string.Format(attSearchPath, settingName));

            if (existingElement != null)
            {
                if (existingElement.InnerText != value)
                {
                    existingElement.InnerText = value;
                }
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
