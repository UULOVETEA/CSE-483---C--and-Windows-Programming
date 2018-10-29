using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// added for INotifyPropertyChanged
using System.ComponentModel;
using System.Windows.Media;

namespace SimpleDataBindingCalculator
{
    class Model : INotifyPropertyChanged
    {
        // define our property change event handler, part of data binding
        public event PropertyChangedEventHandler PropertyChanged;

        // define our own type for calcualtor operations
        public enum CurrentOperation { NONE, OPERATION_ADD, OPERATION_SUBTRACT, OPERATION_MULTIPLY, OPERATION_DIVIDE };

        // property for the current calculator operation
        private CurrentOperation _currentCalculatorOperation;
        public CurrentOperation CurrentCalculatorOperation
        {
            get { return _currentCalculatorOperation; }
            set
            {
                _currentCalculatorOperation = value;
                OnPropertyChanged("CurrentCalculatorOperation");
            }
        }

        private double _firstNumber;
        public double FirstNumber
        {
            get { return _firstNumber; }
            set
            {
                _firstNumber = value;
                OnPropertyChanged("FirstNumber");
            }
        }

        private double _secondNumber;
        public double SecondNumber
        {
            get { return _secondNumber; }
            set
            {
                _secondNumber = value;
                OnPropertyChanged("SecondNumber");
            }
        }

        private double _result;
        public double Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged("Result");
            }
        }

        public void DoCalculation()
        {
            switch (_currentCalculatorOperation)
            {
                case CurrentOperation.OPERATION_ADD:
                    Result = FirstNumber + SecondNumber;
                    break;

                case CurrentOperation.OPERATION_SUBTRACT:
                    Result = FirstNumber - SecondNumber;
                    break;

                case CurrentOperation.OPERATION_MULTIPLY:
                    Result = FirstNumber * SecondNumber;
                    break;

                case CurrentOperation.OPERATION_DIVIDE:
                    Result = FirstNumber / SecondNumber;
                    break;
            }
        }

        // implements method for data binding to any and all properties
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
