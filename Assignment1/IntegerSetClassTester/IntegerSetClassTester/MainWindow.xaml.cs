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
/// Program name: IntegerSetClassTester
/// Author: Xiaomeng Cao
/// Date: February 22,2 017
/// Course: CSE-483
///

namespace IntegerSetClassTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model _myModel;
        public MainWindow()
        {
            InitializeComponent();
            _myModel = new Model();
            this.DataContext = _myModel;
        }

        // function for update button
        private void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            // if one of two textboxes or both them are blank, promot user there is no input
            if (FirstSetInput_TextBox.Text == "" || SecondSetInput_TextBox.Text == "")
            {
                _myModel.Status = "Error, one of the inputs is not set";
                return;
            }

            // call update function to get union and intersection numbers
            _myModel.Update();
        }
    }
}
