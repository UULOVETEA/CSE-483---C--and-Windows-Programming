/////////////////////////////////////////////////////////////////////////////
// EventShapes.cs - Class Project for CSE 483                                   //
// ver 1.2                                                                 //                                           
// ----------------------------------------------------------------------- //
// Language:    C#, ver 6.0, Visual Studio 2015                            //
// Platform:    BootCamp on 2010 MacbookPro, Core i5, Windows 7 SP1        //
// Application: CSE681 Project2, 2015                                      //
// Author:      Kyle Nucera, 462 Link+, Syracuse University                //
//              (908) 906-9311, kjnucera@syr.edu                           //
/////////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 *
 *
 */
/*
 * Maintenance:
 * ------------
 * Required Files: 
 *
 *
 * Build Process:  devenv EventShapes.sln /Rebuild debug
 *                 Run from Developer Command Prompt
 *                 To find: search for developer
 *
 * Maintenance History:
 * --------------------
 * ver 1.2 : 20 Feb 16
 * - Added a Model Class
 * - Added Databindings
 * - Added button to change existing shape
 * ver 1.1 : 04 Feb 16
 * - Added Clear
 * - Added Random Color
 * - Added statuses
 * ver 1.0 : 02 Feb 16
 * - first release
 *
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// added for brushes
using System.Windows.Media;
// added for shapes
using System.Windows.Shapes;
// for observable collections
using System.Collections.ObjectModel;

//several classes dealing with shapes
using System.Windows;

namespace EventShapes
{
    public partial class Model
    {
        // Selecting a shape type
        public enum SelectedShape
        { None, Circle, Rectangle, Square, Triangle }

        public class ChangeColorEventArgs : EventArgs
        {
            public Brush brush;
            public ChangeColorEventArgs(Brush b)
            {
                brush = b;
            }
        }

        private UInt32 _drawingAreaHeight = 100;
        private UInt32 _drawingAreaWidth = 100;

        // provide an observable collections for shapes
        public ObservableCollection<MyShape> RectCollection;
        public ObservableCollection<MyShape> CircleCollection;
        public ObservableCollection<MyShape> SquareCollection;
        public ObservableCollection<MyShape> TriangleCollection;


        // Event delegates and handlers
        public delegate void ChangeColorEventHandler(object sender, ChangeColorEventArgs e);
        public event ChangeColorEventHandler ChangeRectangleColorEvent;
        public event ChangeColorEventHandler ChangeSquareColorEvent;
        public event ChangeColorEventHandler ChangeCircleColorEvent;
        public event ChangeColorEventHandler ChangeTriangleColorEvent;
        public ChangeColorEventHandler RectangleColorHandler;
        public ChangeColorEventHandler SquareColorHandler;
        public ChangeColorEventHandler CircleColorHandler;
        public ChangeColorEventHandler TriangleColorHandler;



        public void InitModel(UInt32 height, UInt32 width)
        {

            _drawingAreaHeight = height;
            _drawingAreaWidth = width;

            // create handlers for color change events
            RectangleColorHandler = ChangeRectangleColorEvent;
            SquareColorHandler = ChangeSquareColorEvent;
            CircleColorHandler = ChangeCircleColorEvent;
            TriangleColorHandler = ChangeTriangleColorEvent;

            // create observable collections
            RectCollection = new ObservableCollection<MyShape>();
            SquareCollection = new ObservableCollection<MyShape>();
            CircleCollection = new ObservableCollection<MyShape>();
            TriangleCollection = new ObservableCollection<MyShape>();
        }

        public void DrawShape(double x, double y)
        {
            MyShape renderShape = new MyShape();
            renderShape.Fill = CurrentBrush;
            renderShape.CanvasLeft = x;
            renderShape.CanvasTop = y;

            switch (CurrentShape)
            {
                case SelectedShape.Circle:
                    renderShape.Height = CircleRadious;
                    renderShape.Width = CircleRadious;
                    renderShape.Name = "Circle";
                    renderShape.CanvasLeft -= CircleRadious / 2;
                    renderShape.CanvasTop -= CircleRadious / 2;
                    CircleColorHandler += new ChangeColorEventHandler(renderShape.ChangeColorHandler);
                    CircleCollection.Add(renderShape);
                    break;
                case SelectedShape.Rectangle:
                    renderShape.Height = RectangleHeight;
                    renderShape.Width = RectangleWidth;
                    renderShape.Name = "Rectangle";
                    renderShape.CanvasLeft -= RectangleWidth / 2;
                    renderShape.CanvasTop -= RectangleHeight / 2;
                    RectangleColorHandler += new ChangeColorEventHandler(renderShape.ChangeColorHandler);
                    RectCollection.Add(renderShape);
                    break;
                case SelectedShape.Square:
                    renderShape.Height = SquareWidth;
                    renderShape.Width = SquareWidth;
                    renderShape.Name = "Square";
                    renderShape.CanvasLeft -= SquareWidth / 2;
                    renderShape.CanvasTop -= SquareWidth / 2;
                    SquareColorHandler += new ChangeColorEventHandler(renderShape.ChangeColorHandler);
                    SquareCollection.Add(renderShape);
                    break;
                case SelectedShape.Triangle:
                    renderShape.MyPointCollection = new PointCollection();
                    Point p1 = new Point(TriangleWidth / 2, 0);
                    Point p2 = new Point(0, TriangleHeight);
                    Point p3 = new Point(TriangleHeight, TriangleWidth);
                    renderShape.MyPointCollection.Add(p1);
                    renderShape.MyPointCollection.Add(p2);
                    renderShape.MyPointCollection.Add(p3);
                    renderShape.Name = "Triangle";
                    renderShape.CanvasLeft -= TriangleWidth / 2;
                    renderShape.CanvasTop -= TriangleHeight / 2;
                    TriangleColorHandler += new ChangeColorEventHandler(renderShape.ChangeColorHandler);
                    TriangleCollection.Add(renderShape);
                    break;
                default:
                    renderShape.Height = RectangleHeight;
                    renderShape.Width = RectangleWidth;
                    renderShape.Name = "Rectangle";
                    RectangleColorHandler += new ChangeColorEventHandler(renderShape.ChangeColorHandler);
                    RectCollection.Add(renderShape);
                    break;
            }
        }

        public void ClearAllShapes()
        {
            CircleCollection.Clear();
            RectCollection.Clear();
            SquareCollection.Clear();
            TriangleCollection.Clear();
        }

        public void ChangeShapeColor()
        {

            // send events for color changes
            // NOTE: we MUST check for null handlers. If no entity has registered
            // to recieve an event, these will be null
            switch (CurrentShape)
            {
                case SelectedShape.Rectangle:
                    if (RectangleColorHandler != null)
                        RectangleColorHandler(this, new ChangeColorEventArgs(CurrentBrush));
                    break;
                case SelectedShape.Square:
                    if (SquareColorHandler != null)
                        SquareColorHandler(this, new ChangeColorEventArgs(CurrentBrush));
                    break;
                case SelectedShape.Circle:
                    if (CircleColorHandler != null)
                        CircleColorHandler(this, new ChangeColorEventArgs(CurrentBrush));
                    break;
                case SelectedShape.Triangle:
                    if (TriangleColorHandler != null)
                        TriangleColorHandler(this, new ChangeColorEventArgs(CurrentBrush));
                    break;
            }
        }

        // TODO: Note that in the move handlers below the model has to know the type of UI element on which 
        // the shapes are being drawn (i.e. a Canvas). This tightly couples the
        // model to the view, which is undesirable
        public void RectangleMoveHandler(object sender, double X, double Y)
        {
            System.Windows.Controls.Canvas.SetTop((Rectangle)sender, Y - (double)(RectangleHeight / 2));
            System.Windows.Controls.Canvas.SetLeft((Rectangle)sender, X - (double)(RectangleWidth / 2));
        }

        public void SquareMoveHandler(object sender, double X, double Y)
        {
            System.Windows.Controls.Canvas.SetTop((Rectangle)sender, Y - (double)(SquareWidth / 2));
            System.Windows.Controls.Canvas.SetLeft((Rectangle)sender, X - (double)(SquareWidth / 2));
        }

        public void CircleMoveHandler(object sender, double X, double Y)
        {
            System.Windows.Controls.Canvas.SetTop((Ellipse)sender, Y - (double)(CircleRadious / 2));
            System.Windows.Controls.Canvas.SetLeft((Ellipse)sender, X - (double)(CircleRadious / 2));
        }

        public void TriangleMoveHandler(object sender, double X, double Y)
        {
            System.Windows.Controls.Canvas.SetTop((Polygon)sender, Y - (double)(TriangleHeight / 2));
            System.Windows.Controls.Canvas.SetLeft((Polygon)sender, X - (double)(TriangleWidth / 2));
        }
    }
}
