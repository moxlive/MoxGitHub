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
        FileWriter fileWriter;
        PhotoModifier photoModifier;

        public ISettingManager SettingManager { get; private set;}
        public Settings Settings { get; private set; }
        public ManualPictureCombiner ManualPictureCombiner { get; private set; }
        public FolderScanner FolderScanner { get; private set; }
        public IMessageDispatcher MessageDispatcher { get; private set; }

        public WorkFlowController()
        {
            Settings = new Settings();
        }

        public void Initialize(MessageProcessor messageHandler)
        {
            //ViewModel require Settings and ManualPictureCombiner
            this.SettingManager = new SettingManager();
            SettingManager.ReadSettings(Settings);

            MessageDispatcher = new MessageDispatcher();
            MessageDispatcher.MessageHandler += messageHandler;

            photoModifier = new PhotoModifier(Settings);
            fileWriter = new FileWriter(Settings, MessageDispatcher);
            ManualPictureCombiner = new ManualPictureCombiner(photoModifier, fileWriter, MessageDispatcher);   
            FolderScanner = new FolderScanner(MessageDispatcher);
            FolderScanner.InitVisitors(Settings);
            FolderScanner.NewPhotoGoupHandler = HandlePictureGroup;

        }

        private void HandlePictureGroup(PhotoGroup photoGroup)
        {
            using (Bitmap newPic = photoModifier.CombinePicture(photoGroup))
            {
                fileWriter.SavePic(photoGroup, newPic);
            }
        }
        
    }

    
}
