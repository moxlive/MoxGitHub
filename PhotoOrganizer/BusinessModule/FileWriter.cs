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
using PhotoOrganizer.BusinessModule.Log;

namespace PhotoOrganizer.BusinessModule
{
    public class FileWriter
    {
        string outputDir = @"D:\Programs\Github\PhotoOrganizer\TestFolder\";
        Settings setting;
        ILogger log;

        public FileWriter(Settings settings)
        {
            log = FileLogger.CreateLogger("FileWriter");
            this.setting = settings;
            LoadSettings();
        }

        public bool SavePic(string outputPath, Bitmap newPic)
        {
            log.LogInfo(string.Format("Saving {0}.", outputPath));
               
            try
            {
                ForceFolderStracture(Path.GetDirectoryName(outputPath));
                newPic.Save(outputPath, ImageFormat.Jpeg);
            }
            catch (Exception e)
            {
                log.LogException(string.Format("Save {0} exception.", outputPath), e);
                return false;
            }
            finally
            {
                newPic.Dispose();
            }

            log.LogInfo(string.Format("Save {0} successfully.", outputPath));
            
            return true;
        }

        public bool SavePic(PhotoGroup group, Bitmap newPic)
        {
            if (newPic == null)
            {
                return false;
            }
            string outputPath = BuildPath(group);
            return SavePic(outputPath, newPic);
        }

        private void ForceFolderStracture(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        private string BuildPath(PhotoGroup group)
        {
            string outputPath = string.Format("{0}\\{1}\\{2}\\{3}\\", outputDir, "Scan Overview", group.Date.ToString("yyyyMMdd"), group.CustomSeqNum);
            outputPath += group.ScanSeqNum + ".jpg";
            return outputPath;
        }

        private void LoadSettings()
        {
            outputDir = setting.OverviewFolderBasePath;

        }
    }
}
