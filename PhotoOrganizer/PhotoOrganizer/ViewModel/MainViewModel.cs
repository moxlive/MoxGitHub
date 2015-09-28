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
        private string statusMessage;
        private MainWindow view;
        private IMessageDispatcher messageDispacher;

        #region Binding
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

        public string StatusMessage
        {
            get { return statusMessage; }
            private set 
            { 
                statusMessage = value;
                OnPropertyChanged("StatusMessage");
            }
        }

        #endregion

        internal MainViewModel(MainWindow control)
        {
            view = control;
            control.DataContext = this;
            workflowController = new WorkFlowController();
            workflowController.Initialize(PopulateMessage);
            setting = workflowController.Settings;
            manualPictureCombiner = workflowController.ManualPictureCombiner;

            ApplySettingCommand = new RelayCommand(ApplySettingCommand_Executed);
            FullScanCommand = new RelayCommand(FullScanCommand_Executed, CanDoFullScan);
        }

        public void PopulateMessage(string message)
        {  
            if (Application.Current.Dispatcher.CheckAccess())
            {
                UpdateUI(message);
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(
                    new Action(() => { UpdateUI(message); }), 
                    null);
            }
        }

        private void UpdateUI(string message)
        {
            if (view.WindowState == WindowState.Minimized)
            {
                view.PopTaskBarMessage(message);
            }
            else
            {
                StatusMessage = message;
            }
        }

        #region commands

        public ICommand ApplySettingCommand
        {
            get;
            private set;
        }

        public ICommand FullScanCommand
        {
            get;
            private set;
        }

        public ICommand CombinePictureCommand
        {
            get;
            private set;
        }
        
        private bool enableFullScanCommand = true;

        private void ApplySettingCommand_Executed()
        {            
            workflowController.SettingManager.SaveSettings(this.Setting);
        }

        private void FullScanCommand_Executed(object state)
        {
            enableFullScanCommand = false;
            workflowController.FolderScanner.FullScan(() => { enableFullScanCommand = true; });            
        }

        private bool CanDoFullScan(object state)
        {
            return enableFullScanCommand;
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
