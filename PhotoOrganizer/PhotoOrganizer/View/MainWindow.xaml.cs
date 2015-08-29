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
using PhotoOrganizer.Common;

namespace PhotoOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Settings setting = new Settings();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WorkFlowController controller = new WorkFlowController();
            controller.Initialize();
            //controller.StartScan();
            //TestBinding();
            this.DataContext = setting;
        }
      
        /// <summary>
        /// only for testing
        /// </summary>
        private void TestBinding()
        {
            //Binding b2 = new Binding();
            //b2.Source = setting;
            //b2.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            //b2.Path = new PropertyPath("ScanBasePath");
            ////b2.NotifyOnSourceUpdated = true;
            //b2.Mode = BindingMode.TwoWay;
            //this.ScanPathTextBox.SetBinding(TextBox.TextProperty, b2);

            Binding b = new Binding();
            b.Source = setting;
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            b.Path = new PropertyPath("OverviewFolderBasePath");
            b.Mode = BindingMode.TwoWay;
            //b.NotifyOnSourceUpdated = true;
            this.OutputPathTextBox.SetBinding(TextBox.TextProperty, b);
                     
        }
    }
}
