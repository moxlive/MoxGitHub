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
        Settings setting;
        ILogger log;
        IMessageDispatcher msgDispatcher;

        public FileWriter(Settings settings, IMessageDispatcher msg)
        {
            log = FileLogger.CreateLogger("FileWriter");
            msgDispatcher = msg;
            this.setting = settings;
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

            string msg = string.Format("Save {0} successfully.", outputPath);
            log.LogInfo(msg);
            msgDispatcher.PopulateMessage(msg);
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
            string outputPath = string.Format("{0}\\{1}\\{2}\\{3}\\", setting.OverviewFolderBasePath, "Scan Overview", group.Date.ToString("yyyyMMdd"), group.CustomSeqNum);
            outputPath += group.ScanSeqNum + ".jpg";
            return outputPath;
        }

    }
}
