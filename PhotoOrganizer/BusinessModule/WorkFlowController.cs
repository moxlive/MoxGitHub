using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoOrganizer.BusinessModule.Photos;
using System.Drawing.Imaging;
using System.Drawing;


namespace PhotoOrganizer.BusinessModule
{
    public class WorkFlowController
    {
        FolderScanner folderScanner;
        FileWriter fileWriter;
        PhotoModifier photoModifier;

        public WorkFlowController()
        {
            folderScanner = new FolderScanner();
            folderScanner.InitVisitors();

            photoModifier = new PhotoModifier();

            fileWriter = new FileWriter();
        }

        public void StartScan()
        {
            IList<PhotoGroup> groups = folderScanner.FindNewPhotoGroups();

          

            foreach (PhotoGroup group in groups)
            {
                Bitmap newPic = photoModifier.CombinePicture(group);
               
                fileWriter.SavePic(group, newPic);
            }

        }

     
    }

    
}
