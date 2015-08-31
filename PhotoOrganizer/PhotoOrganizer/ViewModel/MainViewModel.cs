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

        public Settings Setting
        {
            get { return setting; }
            private set { setting = value; }
        }

        internal MainViewModel(MainWindow control)
        {
            control.DataContext = this;

            workflowController = new WorkFlowController();
            workflowController.Initialize();
            setting = workflowController.Settings;
          
            ApplyCommand = new RelayCommand(ApplyCommand_Executed);
        }

        #region commands

        public ICommand ApplyCommand
        {
            get;
            private set;
        }

        private void ApplyCommand_Executed()
        {
            workflowController.SettingManager.SaveSetting(this.Setting);
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
