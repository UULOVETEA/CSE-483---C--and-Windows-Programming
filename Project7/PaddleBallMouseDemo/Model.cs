using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// observable collections
using System.Collections.ObjectModel;

// debug output
using System.Diagnostics;

// timer, sleep
using System.Threading;

using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

// Rectangle
// Must update References manually
using System.Drawing;

// INotifyPropertyChanged
using System.ComponentModel;

// Threading.Timer
using System.Windows.Threading;

// Timer.Timer
using System.Timers;

namespace PaddleBallMouseDemo
{
    public partial class Model : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        Random _randomNumber = new Random();
        System.Drawing.Rectangle _paddleRectangle;
        System.Drawing.Rectangle _ballRectangle;
        bool _movepaddleLeft = false;
        bool _movepaddleRight = false;
        uint _paddleMoveSize = 10;
        System.Windows.Media.Brush RedColor;
        System.Windows.Media.Brush BlueColor;


#if THREADING_TIMER
        // .NET Threading.Timer
        System.Threading.Timer _paddleTimer;
        // Create a delegate that invokes methods for the timer.
        TimerCallback _paddleTimerCB;
#else
        // .NET Timers.Timer
        //System.Timers.Timer _paddleTimer;
#endif

        private double _windowHeight = 100;
        public double WindowHeight
        {
            get { return _windowHeight; }
            set { _windowHeight = value; }
        }

        private double _windowWidth = 100;
        public double WindowWidth
        {
            get { return _windowWidth; }
            set { _windowWidth = value; }
        }

        /// <summary>
        /// Model constructor
        /// </summary>
        /// <returns></returns>
        public Model()
        {
            SolidColorBrush mySolidColorBrushRed = new SolidColorBrush();
            SolidColorBrush mySolidColorBrushBlue = new SolidColorBrush();

            mySolidColorBrushRed.Color = System.Windows.Media.Color.FromRgb(255, 0, 0);
            RedColor = mySolidColorBrushRed;
            mySolidColorBrushBlue.Color = System.Windows.Media.Color.FromRgb(0, 0, 255);
            BlueColor = mySolidColorBrushBlue;
        }

        public void InitModel()
        {
#if THREADING_TIMER
            // Create an inferred delegate that invokes methods for the timer.
            _paddleTimerCB = paddleTimerCallback;
            // Create a timer that signals the delegate to invoke 
            _paddleTimer = new System.Threading.Timer(_paddleTimerCB, null, 5,5);
#else
            //_paddleTimer = new System.Timers.Timer(5);
            //_paddleTimer.Elapsed += new ElapsedEventHandler(paddleTimerHandler);
            //_paddleTimer.Start();

#endif

            // how far does the paddle move (pixels)
            _paddleMoveSize = 5;

            PaddleFill = RedColor;
        }

        public void CleanUp()
        {
        }


        public void SetStartPosition()
        {            
            paddleWidth = 120;
            paddleHeight = 50;

            ballWidth = 50;
            ballHeight = 50;

            ballCanvasLeft = _windowWidth / 2 - ballWidth / 2;
            ballCanvasTop = _windowHeight / 3 - ballHeight / 3;

            UpdateBallRect();


            paddleCanvasLeft = _windowWidth / 2 - paddleWidth / 2;
            paddleCanvasTop = _windowHeight / 2 - paddleHeight / 2;

            _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);
        }

        public void ProcessMouseClick(uint x, uint y)
        {
            ballCanvasLeft = x - ballWidth/2;
            ballCanvasTop = y - ballHeight/2;
            UpdateBallRect();
        }

        public void ProcessMouseDrag(uint x, uint y)
        {
            System.Windows.Point p = new System.Windows.Point();
            p.X = (double)x;
            p.Y = (double)y;
            ballCanvasLeft = x - ballWidth / 2;
            ballCanvasTop = y - ballHeight / 2;
            UpdateBallRect();

            check();
        }

        void check()
        {
            InterectSide side = IntersectsAt(_paddleRectangle, _ballRectangle);

            if(PaddleFill == RedColor)
            {
                switch (side)
                {
                    case InterectSide.NONE:
                        break;
                    case InterectSide.TOP:
                        paddleCanvasTop += 10;
                        _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);
                        break;
                    case InterectSide.BOTTOM:
                        paddleCanvasTop -= 10;
                        _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);
                        break;
                    case InterectSide.RIGHT:
                        paddleCanvasLeft -= 10;
                        _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);
                        break;
                    case InterectSide.LEFT:
                        paddleCanvasLeft += 10;
                        _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);
                        break;
                }
            }
            
        }

        private bool IsPointinRectangle(System.Windows.Point p, Rectangle r)
        {
            bool flag = false;
            if (p.X > r.X && p.X < r.X + r.Width && p.Y > r.Y && p.Y < r.Y + r.Height)
            {
                flag = true;
            }
            return flag;

        }

        private void UpdateBallRect()
        {
            _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)ballWidth, (int)ballHeight);
        }



        public void MoveLeft(bool move)
        {
            _movepaddleLeft = move;
        }

        public void MoveRight(bool move)
        {
            _movepaddleRight = move;
        }

#if THREADING_TIMER
        private void paddleTimerCallback(Object stateInfo)
#else
        private void paddleTimerHandler(object source, ElapsedEventArgs e)
#endif
        {
#if !THREADING_TIMER
            Console.WriteLine(e.SignalTime.ToString());
#endif
            if (_movepaddleLeft && paddleCanvasLeft > 0)
                paddleCanvasLeft -= _paddleMoveSize;
            else if (_movepaddleRight && paddleCanvasLeft < _windowWidth - paddleWidth)
                paddleCanvasLeft += _paddleMoveSize;
            
            _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);
        }

        enum InterectSide { NONE, LEFT, RIGHT, TOP, BOTTOM };
        private InterectSide IntersectsAt(Rectangle brick, Rectangle ball)
        {
            if (brick.IntersectsWith(ball) == false)
                return InterectSide.NONE;

            Rectangle r = Rectangle.Intersect(brick, ball);

            // did we hit the top of the brick
            if (ball.Top + ball.Height - 1 == r.Top &&
                r.Height == 1)
                return InterectSide.TOP;

            if (ball.Top == r.Top &&
                r.Height == 1)
                return InterectSide.BOTTOM;

            if (ball.Left == r.Left &&
                r.Width == 1)
                return InterectSide.RIGHT;

            if (ball.Left + ball.Width - 1 == r.Left &&
                r.Width == 1)
                return InterectSide.LEFT;

            return InterectSide.NONE;
        }

        public void ChangePaddleColor()
        {
            if (PaddleFill == BlueColor)
            {
                PaddleFill = RedColor;
                return;
            }

            if (PaddleFill == RedColor)
            {
                PaddleFill = BlueColor;
                return;
            }
        }
    }
}
