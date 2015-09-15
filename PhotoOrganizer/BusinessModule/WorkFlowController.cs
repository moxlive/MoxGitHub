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

        public ISettingManager SettingManager { get; private set;}
        public Settings Settings { get; private set; }
        
        public WorkFlowController()
        {
            Settings = new Settings();
        }

        public void Initialize()
        {
            this.SettingManager = new SettingManager();
            this.SettingManager.ReadSetting(Settings);

            folderScanner = new FolderScanner();
            //folderScanner.InitVisitors(Settings);
            photoModifier = new PhotoModifier(Settings);
            fileWriter = new FileWriter(Settings);
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
