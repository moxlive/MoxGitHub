using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PhotoOrganizer.BusinessModule;
using PhotoOrganizer.BusinessModule.Common;
using PhotoOrganizer.ViewModel;
using System.Windows.Forms;
using Hardcodet.Wpf.TaskbarNotification;
using log4net;

namespace PhotoOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
    
        MainViewModel vm;
        TaskbarIcon taskbarIcon;

        public MainWindow()
        {
            InitializeComponent();          
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            vm = new MainViewModel(this);
            taskbarIcon = (TaskbarIcon)FindResource("NotifyIcon");
            taskbarIcon.TrayMouseDoubleClick += NotifyIcon_MouseDoubleClick;
        }

        private void NotifyIcon_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
     
            this.WindowState = System.Windows.WindowState.Normal;
            this.ShowInTaskbar = true;
            taskbarIcon.Visibility = System.Windows.Visibility.Hidden;
            this.Show();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized)
            {
                taskbarIcon.Visibility = System.Windows.Visibility.Visible;
                taskbarIcon.ShowBalloonTip("3DBean", "I'm here", BalloonIcon.Info);
                this.ShowInTaskbar = false;
            }
        }
        /// <summary>
        /// only for testing
        /// </summary>
        //private void TestBinding()
        //{
        //    Binding b = new Binding();
        //    b.Source = setting;
        //    b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        //    b.Path = new PropertyPath("OverviewFolderBasePath");
        //    b.Mode = BindingMode.TwoWay;
        //    this.OutputPathTextBox.SetBinding(TextBox.TextProperty, b);                     
        //}
    }
}
