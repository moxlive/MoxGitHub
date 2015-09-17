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

            photoModifier = new PhotoModifier(Settings);
            fileWriter = new FileWriter(Settings);

            folderScanner = new FolderScanner();
            folderScanner.InitVisitors(Settings);
            folderScanner.NewPhotoGoupHandler = (photoGroup) =>
            {
                Bitmap newPic = photoModifier.CombinePicture(photoGroup);
                fileWriter.SavePic(photoGroup, newPic);
            };
        }
        
    }

    
}
