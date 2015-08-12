using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoOrganizer.BusinessModule.Photos;
using System.Drawing.Imaging;
using System.Drawing;
using PhotoOrganizer.BusinessModule.Common;



namespace PhotoOrganizer.BusinessModule
{
    public class WorkFlowController
    {
        FolderScanner folderScanner;
        FileWriter fileWriter;
        PhotoModifier photoModifier;
        ISettingManager settingManager;

        public WorkFlowController()
        {
          
        }

        public void Initialize()
        {
            settingManager = new SettingManager();
            folderScanner = new FolderScanner();
            folderScanner.InitVisitors(settingManager);
            photoModifier = new PhotoModifier(settingManager);
            fileWriter = new FileWriter(settingManager);
        }

        public void StartScan()
        {
            SettingManager settingManager = new SettingManager();
            //settingManager.SaveSetting(Constants.OverviewFolderBasePath, @"D:\Programs\Github\PhotoOrganizer\TestFolder\");
            //settingManager.SaveSetting(Constants.ScanBasePath, @"D:\Programs\Github\PhotoOrganizer\TestFolder\");
            //settingManager.SaveSetting(Constants.FrontPictureName, @"089.jpg");
            //settingManager.SaveSetting(Constants.BackPictureName, @"090.jpg");

            IList<PhotoGroup> groups = folderScanner.FindNewPhotoGroups();

            foreach (PhotoGroup group in groups)
            {
                Bitmap newPic = photoModifier.CombinePicture(group);

                fileWriter.SavePic(group, newPic);
            }

        }

     
    }

    
}
