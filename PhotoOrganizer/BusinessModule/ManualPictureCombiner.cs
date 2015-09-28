using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Drawing;
using Microsoft.TeamFoundation.MVVM;
using PhotoOrganizer.BusinessModule.Log;

namespace PhotoOrganizer.BusinessModule
{
    public class ManualPictureCombiner : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string choosenFrontPicPath;
        private string choosenBackPicPath;
        private string choosenOutputPicPath;
        private PhotoModifier photoModifier;
        private FileWriter fileWriter;
        private ILogger log;
        private IMessageDispatcher messageDispatcher;
        
        #region property

        public string ChoosenFrontPicPath
        {
            get
            {
                return choosenFrontPicPath;
            }

            set
            {
                choosenFrontPicPath = value;
                OnPropertyChanged("ChoosenFrontPicPath");
            }
        }

        public string ChoosenBackPicPath
        {
            get
            {
                return choosenBackPicPath;
            }

            set
            {
                choosenBackPicPath = value;
                OnPropertyChanged("ChoosenBackPicPath");
            }
        }

        public string ChoosenOutputPicPath
        {
            get
            {
                return choosenOutputPicPath;
            }

            set
            {
                choosenOutputPicPath = value;
                OnPropertyChanged("ChoosenOutputPicPath");
            }
        }

        public ICommand ChooseFrontPicCommand
        {
            get;
            private set;
        }

        public ICommand ChooseBackPicCommand
        {
            get;
            private set;
        }

        public ICommand ChooseOutputPicCommand
        {
            get;
            private set;
        }

        public ICommand CombinePicCommand

        {
            get;
            private set;
        }

        #endregion

        public ManualPictureCombiner(PhotoModifier pm, FileWriter fw, IMessageDispatcher md)
        {
            log = FileLogger.CreateLogger("ManualPictureCombiner");
            photoModifier = pm;
            fileWriter = fw;
            messageDispatcher = md;
            ChooseFrontPicCommand = new RelayCommand(() => { ChoosenFrontPicPath = UpdateChoosenFilePath() ?? ChoosenFrontPicPath; });
            ChooseBackPicCommand = new RelayCommand(() => { ChoosenBackPicPath = UpdateChoosenFilePath() ?? ChoosenBackPicPath; });
            ChooseOutputPicCommand = new RelayCommand(() => { ChoosenOutputPicPath = UpdateChoosenFilePath() ?? ChoosenOutputPicPath; });
            CombinePicCommand = new RelayCommand(CombinePicture);
        }

        private async void CombinePicture()
        {
            string msg = string.Format("Combine picture started. Front Picture={0}, Back Picture={1}, New Picture={2}",
                ChoosenFrontPicPath, ChoosenBackPicPath, ChoosenOutputPicPath);
            log.LogInfo(msg);
            messageDispatcher.PopulateMessage(msg);

            if (string.IsNullOrEmpty(ChoosenFrontPicPath)
                || string.IsNullOrEmpty(ChoosenBackPicPath)
                || string.IsNullOrEmpty(ChoosenOutputPicPath))
            {
                log.LogError("Picture path shall not be empty.");
                messageDispatcher.PopulateMessage("Picture path shall not be empty.");
                return;
            }

            await Task.Factory.StartNew(() => {
                try
                {
                    using (Bitmap newPic = photoModifier.CombinePicture(ChoosenFrontPicPath, ChoosenBackPicPath))
                    {
                        if (newPic != null)
                        {
                            fileWriter.SavePic(ChoosenOutputPicPath, newPic);
                        }
                    }

                    log.LogInfo("Combine picture completed.");
                    messageDispatcher.PopulateMessage("Combine picture completed.");
                }
                catch (Exception ex)
                {
                    log.LogException("Combine picture error.", ex);
                    messageDispatcher.PopulateMessage("Combine picture error.");
                }
            });

            //Force collect bitmap left in memory.
            GC.Collect();
        }

        private string UpdateChoosenFilePath()
        {
            var fileDialog = new System.Windows.Forms.OpenFileDialog();
            var result = fileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                return fileDialog.FileName;
            }

            return null;
        }

        private void OnPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(
                    this, new PropertyChangedEventArgs(propName));
        }

    }
}
