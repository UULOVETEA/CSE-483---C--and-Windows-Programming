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

namespace CanvasObservableCollection
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
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            this.DataContext = _model;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _model.InitModel((UInt32)TopCanvas.ActualHeight, (UInt32)TopCanvas.ActualWidth);

            // create observable collections. 
            MyRectangles.ItemsSource = _model.RectCollection;
            MyEllipses.ItemsSource = _model.ElliCollection;
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            _model.ResetRectangles();
            _model.ResetEllipses();
        }
    }
}
