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

        Settings setting;

        public FileWriter(Settings settings)
        {
            this.setting = settings;
            LoadSettings();
        }

        public bool SavePic(string outputPath, Bitmap newPic)
        {
            try
            {
                ForceFolderStracture(Path.GetDirectoryName(outputPath));
                newPic.Save(outputPath, ImageFormat.Jpeg);
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

        public bool SavePic(PhotoGroup group, Bitmap newPic)
        {
            try
            {
                if (newPic == null)
                {
                    return false;
                }
                string outputPath = BuildPath(group);
                //todo : allow user to set it 
                SavePic(outputPath, newPic);
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
