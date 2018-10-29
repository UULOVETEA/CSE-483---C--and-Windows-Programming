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

// socket setup window
using SocketSetup;

///
/// Program Name: Tic Tac Toe-Network
/// Author: Xiaomeng Cao
/// Date: April 29, 2017
/// Course: CSE-483
///

namespace TicTacToe_Network
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
            // make it so the user cannot resize the window
            this.ResizeMode = ResizeMode.NoResize;
            // create an instance of our Model
            _model = new Model();
            //set data binding context to our model
            this.DataContext = _model;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedButton = e.OriginalSource as FrameworkElement;
            if (selectedButton != null)
            {
                var currentTile = selectedButton.DataContext as Tile;
                _model.UserSelection(currentTile.TileName);
                _model.SendMessage();
            }
        }

        private void Restart_Button_Click(object sender, RoutedEventArgs e)
        {
            _model.Clear();
        }

        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            // create an observable collection. this collection
            // contains the tiles the represent the Tic Tac Toe grid
            MyItemsControl.ItemsSource = _model.TileCollection;
        }

        private void SocketSetup_Button_Click(object sender, RoutedEventArgs e)
        {
            // call up socket setup windows to get setup data
            SocketSetupWindow socketSetupWindow = new SocketSetupWindow();
            socketSetupWindow.ShowDialog();

            // set title bar to be unique
            this.Title = this.Title + " " + socketSetupWindow.SocketData.LocalIPString + "@" + socketSetupWindow.SocketData.LocalPort.ToString();

            // update model with setup data
            _model.SetLocalNetworkSettings(socketSetupWindow.SocketData.LocalPort, socketSetupWindow.SocketData.LocalIPString);
            _model.SetRemoteNetworkSettings(socketSetupWindow.SocketData.RemotePort, socketSetupWindow.SocketData.RemoteIPString);

            // initialize model and get the ball rolling
            _model.InitModel();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _model.Model_Cleanup();
        }
    }
}
