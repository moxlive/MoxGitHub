using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PhotoOrganizer.Common
{
    public class Settings : INotifyPropertyChanged
    {
        private string overviewFolderBasePath;
        private string scanBasePath;
        private string frontPictureName;
        private string backPictureName;

        #region properties
        public string ScanBasePath
        {
            get
            {
                return this.scanBasePath;
            }

            set
            {
                if (value != this.scanBasePath)
                {
                    this.scanBasePath = value;
                    OnPropertyChanged("ScanBasePath");
                }
            }
        }

        public string OverviewFolderBasePath
        {
            get
            {
                return this.overviewFolderBasePath;
            }

            set
            {
                if (value != this.overviewFolderBasePath)
                {
                    this.overviewFolderBasePath = value;
                    OnPropertyChanged("OverviewFolderBasePath");   
                }
            }
        }

        public string FrontPictureName
        {
            get
            {
                return this.frontPictureName;
            }

            set
            {
                if (value != this.frontPictureName)
                {
                    this.frontPictureName = value;
                    OnPropertyChanged("FrontPictureName");
                }
            }
        }

        public string BackPictureName
        {
            get
            {
                return this.backPictureName;
            }

            set
            {
                if (value != this.backPictureName)
                {
                    this.backPictureName = value;
                    OnPropertyChanged("BackPictureName");
                }
            }
        }

        #endregion properties

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(
                    this, new PropertyChangedEventArgs(propName));
        }


    }
}
