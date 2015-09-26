using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace PhotoOrganizer.BusinessModule.Common
{
    public class Settings : INotifyPropertyChanged
    {
        public const string ScanBasePathStr = "ScanBasePath";
        public const string OverviewFolderBasePathStr = "OverviewFolderBasePath";
        public const string FrontPictureNameStr = "FrontPictureName";
        public const string BackPictureNameStr = "BackPictureName";
        public const string FrontPictureRotateStr = "FrontPictureRotate";
        public const string BackPictureRotateStr = "BackPictureRotate";


        private string overviewFolderBasePath;
        private string scanBasePath;
        private string frontPictureName;
        private string backPictureName;
        private string frontPictureRotate;
        private string backPictureRotate;

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

        public string FrontPictureRotate
        {
            get
            {
                return this.frontPictureRotate;
            }

            set
            {
                if (value != this.frontPictureRotate)
                {
                    this.frontPictureRotate = value;
                    OnPropertyChanged("FrontPictureRotate");
                }
            }
        }

        public string BackPictureRotate
        {
            get
            {
                return this.backPictureRotate;
            }

            set
            {
                if (value != this.backPictureRotate)
                {
                    this.backPictureRotate = value;
                    OnPropertyChanged("BackPictureRotate");
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
