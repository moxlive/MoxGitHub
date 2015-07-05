using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PhotoOrganizer.BusinessModule.Photos;
using System.Drawing.Imaging;
using System.Drawing;

namespace PhotoOrganizer.BusinessModule
{
    public class FileWriter
    {

        string outputDir = @"D:\Programs\Github\PhotoOrganizer\TestFolder\";
            

        public bool SavePic(PhotoGroup group, Bitmap newPic)
        {
            string outputFolder = ForceFolderStracture(group);
            newPic.Save(outputFolder + group.CustomSeqNum + ".jpg");
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
    }
}
