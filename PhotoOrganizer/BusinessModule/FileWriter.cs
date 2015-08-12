using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PhotoOrganizer.BusinessModule.Photos;
using PhotoOrganizer.BusinessModule.Common;
using System.Drawing.Imaging;
using System.Drawing;

namespace PhotoOrganizer.BusinessModule
{
    public class FileWriter
    {

        string outputDir = @"D:\Programs\Github\PhotoOrganizer\TestFolder\";
        
        ISettingManager settingManager;

        public FileWriter(ISettingManager settingMgr)
        {
            settingManager = settingMgr;
            LoadSettings();
        }

        public bool SavePic(PhotoGroup group, Bitmap newPic)
        {
            try
            {
                if (newPic == null)
                {
                    return false;
                }
                string outputFolder = ForceFolderStracture(group);
                //todo : allow user to set it 
                newPic.Save(outputFolder + group.ScanSeqNum + ".jpg", ImageFormat.Jpeg);
            }
            catch
            {
            }
            finally
            {
                newPic.Dispose();
            }

            return true;
        }

        private string ForceFolderStracture(PhotoGroup group)
        {
            string outputFolder = string.Format("{0}\\{1}\\{2}\\{3}\\", outputDir, "Scan Overview", group.Date.ToString("yyyyMMdd"), group.CustomSeqNum);
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }
            return outputFolder;
        }

        private void LoadSettings()
        {
            outputDir = settingManager.ReadSettingString(Constants.OverviewFolderBasePath);

        }
    }
}
