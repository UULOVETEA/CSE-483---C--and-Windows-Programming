using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

///
/// Program name: IntegerSetClassTester
/// Author: Xiaomeng Cao
/// Date: February 22,2 017
/// Course: CSE-483
///

namespace IntegerSetClassTester
{
    class Model : INotifyPropertyChanged
    {
        // define our property change event handler, part of data binding
        public event PropertyChangedEventHandler PropertyChanged;

        // creat the instance of IntegerClass for union, intersection, set1, and set2
        IntegerSet union;
        IntegerSet intersection;
        IntegerSet set1;
        IntegerSet set2;

        public Model()
        {
            union = new IntegerSet();
            intersection = new IntegerSet();            
        }

        // data binding for first set textbox
        private string _firstSet;
        public string FirstSet
        {
            get { return _firstSet; }
            set
            {
                set1 = new IntegerSet();
                _firstSet = value;
                OnPropertyChanged("FirstSet");

                try
                {
                    // get the numbers without comma
                    string[] firstSetNumber = FirstSet.Split(',');

                    foreach (string stuff in firstSetNumber)
                    {
                        try
                        {                           
                            int number;
                            // convert each string to int, and store into number
                            number = Convert.ToInt32(stuff);
                            // insert each number to set1
                            set1.InsertElement(number);
                            Status = "Set Entered Correctly";
                        }
                        catch (OverflowException)
                        {
                            Status = "One value is not even within converting range";
                        }
                        catch (FormatException)
                        {
                            Status = "You entered an unrecognizable character for converting";
                        }
                        catch (Exception)
                        {
                            Status = "Number is out of range of the Set";
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Status = "You didn't enter any values";
                }
            }
        }

        // data binding for second set textbox
        private string _secondSet;
        public string SecondSet
        {
            get { return _secondSet; }
            set
            {
                set2 = new IntegerSet();
                _secondSet = value;
                OnPropertyChanged("SecondSet");

                try
                {                    
                    // get the numbers without comma
                    string[] secondSetNumber = SecondSet.Split(',');

                    foreach (string stuff in secondSetNumber)
                    {
                        try
                        {                            
                            int number;
                            // convert each string to int, and store into number
                            number = Convert.ToInt32(stuff);
                            // insert each number to set1
                            set2.InsertElement(number);
                            Status = "Set Entered Correctly";
                        }
                        catch (OverflowException)
                        {
                            Status = "One value is not even within converting range";
                        }
                        catch (FormatException)
                        {
                            Status = "You entered an unrecognizable character for converting";
                        }
                        catch (Exception)
                        {
                            Status = "Number is out of range of the Set";
                        }
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Status = "You didn't enter any values";
                }                               
            }
        }

        // data binding for union textbox
        private string _unionNumbers;
        public string UnionNumbers
        {
            get { return _unionNumbers; }
            set
            {
                _unionNumbers = value;
                OnPropertyChanged("UnionNumbers");
            }
        }

        // data binding for intersection textbox
        private string _intersectionNumbers;
        public string IntersectionNumbers
        {
            get { return _intersectionNumbers; }
            set
            {
                _intersectionNumbers = value;
                OnPropertyChanged("IntersectionNumbers");
            }
        }

        // data binding for status label
        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        // Function to find union and intersection between two integer set
        public void Update()
        {
            // find the union numbers between set1 and set2
            union = set1.Union(set2);
            // convert union numbers to string
            UnionNumbers = union.ToString();
            // find the intersection numbers between set1 and set2
            intersection = set2.Intersection(set1);
            // convert intersection numbers to string
            IntersectionNumbers = intersection.ToString();
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
