using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegerSetConsole
{
    public class IntegerSet
    {
        private bool[] _set;

        public bool[] Set
        {
            get { return _set; }
            set { _set = value; }
        }

        /// <summary>
        /// The constructor for the IntegerSet class
        /// </summary>
        public IntegerSet()
        {
            Console.Write("\n IntegerSet Constructed");
            _set = new bool[101];
            for (int i = 0; i < 101; i++)
            {
                _set[i] = false;
            }
        }

        public IntegerSet(bool[] val)
            :this()
        {
            Console.Write("\n Integer Constructed with bool[]");
            int size = Math.Min(val.Length, 101);
            for (int i = 0; i < size; i++)
            {
                _set[i] = val[i];
            }
        }

        public IntegerSet Union(IntegerSet otherSet)
        {
            IntegerSet resultSet = new IntegerSet();
            int counter = 0;

            foreach (bool value in otherSet.Set)
            {
                if (value)
                    resultSet.Set[counter] = true;
                else if (_set[counter])
                    resultSet.Set[counter] = true;
                counter++;
            }

            return resultSet;
        }

        public IntegerSet Intersection(IntegerSet otherSet)
        {
            IntegerSet resultSet = new IntegerSet();
            int counter;

            for (counter = 0; counter < 101; counter++)
            {
                resultSet.Set[counter] = true;
            }

            counter = 0;
            foreach (bool value in otherSet.Set)
            {
                if (!value)
                    resultSet.Set[counter] = false;
                else if (!_set[counter])
                    resultSet.Set[counter] = false;
                counter++;
            }

            return resultSet;
        }

        public void InsertElement(int k)
        {
            try
            {
                _set[k] = true;
            }
            catch (Exception)
            {
                Console.WriteLine("The number" + k + "is an invalid number.\n Try another one");
            }
        }

        public void DeleteElement(int k)
        {
            try
            {
                _set[k] = false;
            }
            catch (Exception)
            {
                Console.WriteLine("The number" + k + "is an invalid number.\n Try another one");
            }
        }

        public override string ToString()
        {
            string resultString = String.Empty;

            for (int i = 0; i < _set.Length; i++)
            {
                if(_set[i])
                {
                    resultString = resultString + i + ", ";
                }
            }

            return resultString;
        }

        public bool IsEqualTo(IntegerSet otherSet)
        {
            bool result = true;
            int counter = 0;

            foreach (bool value in otherSet.Set)
            {
                if (value != _set[counter])
                    result = false;
                counter++;
            }

            return result;
        }

        public void Clear()
        {
            for (int i = 0; i < _set.Length; i++)
            {
                _set[i] = false;
            }
        }
    }
}
