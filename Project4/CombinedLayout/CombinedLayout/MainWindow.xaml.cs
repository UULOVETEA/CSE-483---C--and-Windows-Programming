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

namespace CombinedLayout
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random randomNumber = new Random(Guid.NewGuid().GetHashCode());

        private enum enumShapeType { ELLIPSE, RECTANGLE };

        public MainWindow()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
        }

        private void SetRandomShapes(Canvas e, enumShapeType t, int number)
        {
            double canvasHeight = e.ActualHeight;
            double canvasWidth = e.ActualWidth;
            Shape shape = new Ellipse();

            for (int count = 0; count < number; count++)
            {
                switch (t)
                {
                    case enumShapeType.ELLIPSE:
                        shape = new Ellipse();
                        break;

                    case enumShapeType.RECTANGLE:
                        shape = new Rectangle();
                        break;
                }
                shape.Height = randomNumber.Next(10, 25);
                shape.Width = shape.Height;
                SolidColorBrush mySolidColorBrush = new SolidColorBrush();

                // Describes the brush's color using RDB values.
                // Each value has a range of 0-255.

                mySolidColorBrush.Color = Color.FromArgb(255, (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255), (byte)randomNumber.Next(0, 255));
                shape.Fill = mySolidColorBrush;
                e.Children.Add(shape);
                Canvas.SetLeft(shape, randomNumber.Next(0, (int)(canvasWidth - shape.Width)));
                Canvas.SetTop(shape, randomNumber.Next(0, (int)(canvasHeight - shape.Height)));
                Canvas.SetRight(shape, randomNumber.Next(0, (int)(canvasHeight - shape.Height)));
                Canvas.SetBottom(shape, randomNumber.Next(0, (int)(canvasHeight - shape.Height)));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TopLeft.Children.Clear();
            TopRight.Children.Clear();
            BottomLeft.Children.Clear();
            BottomRight.Children.Clear();
            SetRandomShapes(TopLeft, enumShapeType.ELLIPSE, 25);
            SetRandomShapes(TopRight, enumShapeType.RECTANGLE, 25);
            SetRandomShapes(BottomLeft, enumShapeType.ELLIPSE, 125);
            SetRandomShapes(BottomRight, enumShapeType.RECTANGLE, 125);
        }

        private void ResetAllButton_Click(object sender, RoutedEventArgs e)
        {
            TopLeft.Children.Clear();
            TopRight.Children.Clear();
            BottomLeft.Children.Clear();
            BottomRight.Children.Clear();
            SetRandomShapes(TopLeft, enumShapeType.ELLIPSE, 20);
            SetRandomShapes(TopRight, enumShapeType.RECTANGLE, 20);
            SetRandomShapes(BottomLeft, enumShapeType.ELLIPSE, 60);
            SetRandomShapes(BottomRight, enumShapeType.RECTANGLE, 60);
        }

        private void ResetTopLetButton_Click(object sender, RoutedEventArgs e)
        {
            TopLeft.Children.Clear();
            SetRandomShapes(TopLeft, enumShapeType.ELLIPSE, 25);
        }

        private void ResetTopRightutton_Click(object sender, RoutedEventArgs e)
        {
            TopRight.Children.Clear();
            SetRandomShapes(TopRight, enumShapeType.RECTANGLE, 25);
        }

        private void ResetButtomLeftButton_Click(object sender, RoutedEventArgs e)
        {
            BottomLeft.Children.Clear();
            SetRandomShapes(BottomLeft, enumShapeType.ELLIPSE, 125);
        }

        private void ResetButtonRightButton_Click(object sender, RoutedEventArgs e)
        {
            BottomRight.Children.Clear();
            SetRandomShapes(BottomRight, enumShapeType.RECTANGLE, 125);
        }
    }
}
