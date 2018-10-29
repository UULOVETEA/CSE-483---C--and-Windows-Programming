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

namespace SimpleViewCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Status_Label.Foreground = Brushes.Black;
            Status_Label.Content = "Calculator Initialized";
        }

        bool addButtonClicked = false;
        bool subButtonClicked = false;
        bool TimesButtonClicked = false;
        bool DivisionButtonClicked = false;

        private void Equal_Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                int firstNumber = 0;
                int secondNumber = 0;
                int result = 0;

                firstNumber = int.Parse(FirstNumber_TextBox.Text);
                secondNumber = int.Parse(SecondNumber_TextBox.Text);

                if (addButtonClicked)
                {
                    result = firstNumber + secondNumber;
                    Result_TextBox.Text = result.ToString();
                }
                else if (subButtonClicked)
                {
                    result = firstNumber - secondNumber;
                    Result_TextBox.Text = result.ToString();
                }
                else if (TimesButtonClicked)
                {
                    result = firstNumber * secondNumber;
                    Result_TextBox.Text = result.ToString();
                }
                else if (DivisionButtonClicked)
                {
                    result = firstNumber / secondNumber;
                    Result_TextBox.Text = result.ToString();
                }
            }
            catch (System.ArgumentException)
            {
                Status_Label.Foreground = Brushes.Red;
                Status_Label.Content = "System.ArgumentNullException occurred. \n";
            }
            catch (System.FormatException)
            {
                Status_Label.Foreground = Brushes.Red;
                Status_Label.Content = "System.FormatException occurred. \n";
            }
            catch (System.OverflowException)
            {
                Status_Label.Foreground = Brushes.Red;
                Status_Label.Content = "System.OverflowException occurred. \n";
            }
            catch (System.DivideByZeroException)
            {
                Status_Label.Foreground = Brushes.Red;
                Status_Label.Content = "System.DivideByZeroException occurred. \n";
            }
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            addButtonClicked = true;
            subButtonClicked = false;
            TimesButtonClicked = false;
            DivisionButtonClicked = false;
            CurrentOperation_TextBox.Text = "ADD";
        }

        private void Subtraction_Button_Click(object sender, RoutedEventArgs e)
        {
            addButtonClicked = false;
            subButtonClicked = true;
            TimesButtonClicked = false;
            DivisionButtonClicked = false;
            CurrentOperation_TextBox.Text = "SUB";
        }

        private void Times_Button_Click(object sender, RoutedEventArgs e)
        {
            addButtonClicked = false;
            subButtonClicked = false;
            TimesButtonClicked = true;
            DivisionButtonClicked = false;
            CurrentOperation_TextBox.Text = "TIMES";
        }

        private void Divison_Button_Click(object sender, RoutedEventArgs e)
        {
            addButtonClicked = false;
            subButtonClicked = false;
            TimesButtonClicked = false;
            DivisionButtonClicked = true;
            CurrentOperation_TextBox.Text = "DIVISION";
        }
    }
}
