using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace EventShapes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>   
    public partial class MainWindow : Window
    {
        private Model _model;
        public MainWindow()
        {
            _model = new EventShapes.Model();

            this.DataContext = _model;
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
        }
        private void CreateRectangle_Click(object sender, RoutedEventArgs e)
        {
            CreateRectangle.Background = Brushes.Yellow;
            CreateCircle.Background = Brushes.Gray;
            CreateSquare.Background = Brushes.Gray;
            CreateTriangle.Background = Brushes.Gray;
            _model.CurrentShape = Model.SelectedShape.Rectangle;
            _model.ShapeText = _model.CurrentShape.ToString();
            _model.SystemShape = "Rectangle";
        }

        private void CreateSquare_Click(object sender, RoutedEventArgs e)
        {
            CreateRectangle.Background = Brushes.Gray;
            CreateCircle.Background = Brushes.Gray;
            CreateSquare.Background = Brushes.Yellow;
            CreateTriangle.Background = Brushes.Gray;
            _model.CurrentShape = EventShapes.Model.SelectedShape.Square;
            _model.ShapeText = _model.CurrentShape.ToString();
            _model.SystemShape = "Square";
        }

        private void CreateCircle_Click(object sender, RoutedEventArgs e)
        {
            CreateRectangle.Background = Brushes.Gray;
            CreateCircle.Background = Brushes.Yellow;
            CreateSquare.Background = Brushes.Gray;
            CreateTriangle.Background = Brushes.Gray;
            _model.CurrentShape = EventShapes.Model.SelectedShape.Circle;
            _model.ShapeText = _model.CurrentShape.ToString();
            _model.SystemShape = "Ellipse";
        }
        private void ColorSelect_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDialog =
                       new System.Windows.Forms.ColorDialog();
            colorDialog.AllowFullOpen = true;
            colorDialog.ShowDialog();
            System.Windows.Media.Color col = new System.Windows.Media.Color();
            col.A = colorDialog.Color.A;
            col.B = colorDialog.Color.B;
            col.G = colorDialog.Color.G;
            col.R = colorDialog.Color.R;
            _model.CurrentBrush = new SolidColorBrush(col);
        }
        private void ClearCanvas_Click(object sender, RoutedEventArgs e)
        {
            _model.ClearAllShapes();
        }
        private void canvasArea_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _model.DrawShape(e.GetPosition(canvasArea).X, e.GetPosition(canvasArea).Y);
        }

        private void ChangeColor_Button_Click(object sender, RoutedEventArgs e)
        {
            _model.ChangeShapeColor();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _model.InitModel((UInt32)canvasArea.ActualHeight, (UInt32)canvasArea.ActualWidth);

            // create observable collections. 
            MyRectangles.ItemsSource = _model.RectCollection;
            MyCircles.ItemsSource = _model.CircleCollection;
            MySquares.ItemsSource = _model.SquareCollection;
            MyTriangle.ItemsSource = _model.TriangleCollection;
        }

        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                _model.RectangleMoveHandler(sender, e.GetPosition(canvasArea).X, e.GetPosition(canvasArea).Y);
            }
        }

        private void Square_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                _model.SquareMoveHandler(sender, e.GetPosition(canvasArea).X, e.GetPosition(canvasArea).Y);
            }
        }

        private void Circle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                _model.CircleMoveHandler(sender, e.GetPosition(canvasArea).X, e.GetPosition(canvasArea).Y);
            }
        }

        private void Triangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                _model.TriangleMoveHandler(sender, e.GetPosition(canvasArea).X, e.GetPosition(canvasArea).Y);
            }
        }

        private void CreateTriangle_Click(object sender, RoutedEventArgs e)
        {
            CreateRectangle.Background = Brushes.Gray;
            CreateCircle.Background = Brushes.Gray;
            CreateSquare.Background = Brushes.Gray;
            CreateTriangle.Background = Brushes.Yellow;
            _model.CurrentShape = EventShapes.Model.SelectedShape.Triangle;
            _model.ShapeText = _model.CurrentShape.ToString();
            _model.SystemShape = "Triangle";
        }
    }
}
