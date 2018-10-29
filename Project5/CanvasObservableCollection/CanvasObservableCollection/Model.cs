using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// INotifyPropertyChanged
using System.ComponentModel;

// observable collections
using System.Collections.ObjectModel;

// Brush
using System.Windows.Media;


namespace CanvasObservableCollection
{
    public partial class Model : INotifyPropertyChanged
    {
        // provide an observable collections for shapes
        public ObservableCollection<MyShape> RectCollection;
        public ObservableCollection<MyShape> ElliCollection;

        public event PropertyChangedEventHandler PropertyChanged;
        public void InitModel(UInt32 height, UInt32 width)
        {

            _drawingAreaHeight = height;
            _drawingAreaWidth = width;

            // place them manually at the top of the item collection in the view
            RectCollection = new ObservableCollection<MyShape>();
            ElliCollection = new ObservableCollection<MyShape>();

            ResetRectangles();
            ResetEllipses();
        }
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private Random randomNumber = new Random(Guid.NewGuid().GetHashCode());
        private UInt32 _drawingAreaHeight = 100;
        private UInt32 _drawingAreaWidth = 100;

        // this property is bound to our view
        private UInt32 _numRects = 100;
        public UInt32 NumRects
        {
            get { return _numRects; }
            set
            {
                _numRects = value;
                OnPropertyChanged("NumRects");
            }
        }

        private UInt32 _numEllis = 100;
        public UInt32 NumEllis
        {
            get { return _numEllis; }
            set
            {
                _numEllis = value;
                OnPropertyChanged("NumEllis");
            }
        }

        private void SetRandomShapes(MyShape shape)
        {

            shape.Height = randomNumber.Next(10, 50);
            shape.Width = shape.Height;
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            // Describes the brush's color using RGB values. 
            // Each value has a range of 0-255.

            mySolidColorBrush.Color = Color.FromArgb(255, (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255));
            shape.Fill = mySolidColorBrush;
            shape.CanvasTop = randomNumber.Next(0, (int)(_drawingAreaHeight - shape.Height));
            shape.CanvasLeft = randomNumber.Next(0, (int)(_drawingAreaWidth - shape.Width));
        }

        public void ResetRectangles()
        {
            RectCollection.Clear();

            for (int rects = 0; rects < _numRects; rects++)
            {
                MyShape temp = new MyShape();
                SetRandomShapes(temp);
                RectCollection.Add(temp);
            }
        }

        public void ResetEllipses()
        {
            ElliCollection.Clear();

            for (int ellis = 0; ellis < _numEllis; ellis++)
            {
                MyShape temp2 = new MyShape();
                SetRandomShapes(temp2);
                ElliCollection.Add(temp2);
            }
        }

    }

}
