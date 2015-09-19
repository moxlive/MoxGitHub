using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Drawing;
using Microsoft.TeamFoundation.MVVM;

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

        public ManualPictureCombiner(PhotoModifier pm, FileWriter fw)
        {
            photoModifier = pm;
            fileWriter = fw;
            ChooseFrontPicCommand = new RelayCommand(() => { ChoosenFrontPicPath = UpdateChoosenFilePath() ?? ChoosenFrontPicPath; });
            ChooseBackPicCommand = new RelayCommand(() => { ChoosenBackPicPath = UpdateChoosenFilePath() ?? ChoosenBackPicPath; });
            ChooseOutputPicCommand = new RelayCommand(() => { ChoosenOutputPicPath = UpdateChoosenFilePath() ?? ChoosenOutputPicPath; });
            CombinePicCommand = new RelayCommand(() => {
                Bitmap newPic = photoModifier.CombinePicture(ChoosenFrontPicPath, ChoosenBackPicPath);
                if (newPic != null)
                {
                    fileWriter.SavePic(ChoosenOutputPicPath, newPic);
                }
            });
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
