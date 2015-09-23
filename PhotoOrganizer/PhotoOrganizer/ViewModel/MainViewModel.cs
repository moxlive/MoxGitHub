using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoOrganizer.BusinessModule.Common;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using PhotoOrganizer.BusinessModule;
using Microsoft.TeamFoundation.MVVM;

namespace PhotoOrganizer.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private WorkFlowController workflowController;
        private Settings setting;
        private ManualPictureCombiner manualPictureCombiner;

        public Settings Setting
        {
            get { return setting; }
            private set { setting = value; }
        }

        public ManualPictureCombiner ManualPictureCombiner
        {
            get { return manualPictureCombiner; }
            private set { manualPictureCombiner = value; }
        }

        internal MainViewModel(MainWindow control)
        {
            control.DataContext = this;

            workflowController = new WorkFlowController();
            workflowController.Initialize();
            setting = workflowController.Settings;
            manualPictureCombiner = workflowController.ManualPictureCombiner;

            ApplyCommand = new RelayCommand(ApplyCommand_Executed);
            FullScanCommand = new RelayCommand(FullScanCommand_Executed);

        }

        #region commands

        public ICommand ApplyCommand
        {
            get;
            private set;
        }

        public ICommand FullScanCommand
        {
            get;
            private set;
        }
      
        private void ApplyCommand_Executed()
        {
            workflowController.SettingManager.SaveSetting(this.Setting);
        }

        private void FullScanCommand_Executed()
        {
            workflowController.FolderScanner.FullScan();
        }
             
        #endregion commands

        #region helpers
        public event PropertyChangedEventHandler PropertyChanged;
  
        private void OnPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(
                    this, new PropertyChangedEventArgs(propName));
        }

        #endregion helpers
    }

}
