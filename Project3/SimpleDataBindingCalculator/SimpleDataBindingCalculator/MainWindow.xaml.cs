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

namespace SimpleDataBindingCalculator
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

        private void Operation_Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Button bt = (System.Windows.Controls.Button)sender;
            if (bt.Name.Contains("Add")) _myModel.CurrentCalculatorOperation = Model.CurrentOperation.OPERATION_ADD;
            if (bt.Name.Contains("Subtract")) _myModel.CurrentCalculatorOperation = Model.CurrentOperation.OPERATION_SUBTRACT;
            if (bt.Name.Contains("Multiply")) _myModel.CurrentCalculatorOperation = Model.CurrentOperation.OPERATION_MULTIPLY;
            if (bt.Name.Contains("Divide")) _myModel.CurrentCalculatorOperation = Model.CurrentOperation.OPERATION_DIVIDE;

            Status_Label.Content = _myModel.CurrentCalculatorOperation;
        }

        private void Equal_Button_Click(object sender, RoutedEventArgs e)
        {
            _myModel.DoCalculation();
        }
    }
}
