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

namespace SimpleEvents
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Model _model;
        public MainWindow()
        {

            _model = new Model();
            this.DataContext = _model;
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
        }

        private void Send_Button_Click(object sender, RoutedEventArgs e)
        {
            _model.SendButtonClicked();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _model.Cleanup();
        }
    }
}
