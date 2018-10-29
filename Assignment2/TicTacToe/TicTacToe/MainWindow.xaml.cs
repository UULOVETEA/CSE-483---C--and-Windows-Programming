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

///
/// Program Name: TicTacToe
/// Author: Xiaomeng Cao
/// Date: March 9, 2017
/// Course: CSE-483
///

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Model _model;

        public MainWindow()
        {
            InitializeComponent();

            this.ResizeMode = ResizeMode.NoResize;
            _model = new Model();
            this.DataContext = _model;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedButton = e.OriginalSource as FrameworkElement;
            if (selectedButton != null)
            {
                var currentTile = selectedButton.DataContext as Tile;
                _model.UserSelection(currentTile.TileName);
            }
        }

        private void Restart_Button_Click(object sender, RoutedEventArgs e)
        {
            _model.Clear();
        }

        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            MyItemsControl.ItemsSource = _model.TileCollection;
        }
    }
}
