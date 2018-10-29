using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// added for INotifyPropertyChanged
using System.ComponentModel;

// added for brushes
using System.Windows.Media;

namespace EventShapes
{
    public partial class Model : INotifyPropertyChanged
    {
        // define our property chage event handler, part of data binding
        public event PropertyChangedEventHandler PropertyChanged;

        // Draw a shape
        public double CircleRadious { get; set; } = 40;
        public double RectangleHeight { get; set; } = 40;
        public double RectangleWidth { get; set; } = 80;
        public double SquareWidth { get; set; } = 40;
        public double TriangleHeight { get; set; } = 40;
        public double TriangleWidth { get; set; } = 40;

        // property for the selected shape
        private string _shapeText;
        public string ShapeText
        {
            get { return _shapeText; }
            set
            {
                _shapeText = value;
                OnPropertyChanged("ShapeText");
            }
        }
        // for editing the existing shapes
        private string _systemShape;
        public string SystemShape
        {
            get { return _systemShape; }
            set
            {
                _systemShape = value;
                OnPropertyChanged("SystemShape");
            }
        }
        // property for the current calculator operation
        private SelectedShape _currentShape;
        public SelectedShape CurrentShape
        {
            get { return _currentShape; }
            set
            {
                _currentShape = value;
                OnPropertyChanged("CurrentShape");
            }
        }
        // Set/get random brush colors
        private Brush _currentBrush = Brushes.Chocolate;
        public Brush CurrentBrush
        {
            get { return _currentBrush; }
            set
            {
                _currentBrush = value;
                OnPropertyChanged("CurrentBrush");
            }
        }

        // For binding
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }
    }
}
