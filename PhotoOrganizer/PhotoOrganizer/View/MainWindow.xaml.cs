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

namespace PhotoOrganizer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
    
        MainViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            vm = new MainViewModel(this);
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
