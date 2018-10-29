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

namespace PaddleDemo
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
        System.Drawing.Rectangle _ballRectangle;
        System.Drawing.Rectangle _paddleRectangle;
        bool _movepaddleLeft = false;
        bool _movepaddleRight = false;
        uint _paddleMoveSize = 10;


#if THREADING_TIMER
        // .NET Threading.Timer
        System.Threading.Timer _paddleTimer;
        // Create a delegate that invokes methods for the timer.
        TimerCallback _paddleTimerCB;
#else
        // .NET Timers.Timer
        System.Timers.Timer _paddleTimer;
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
        }

        public void InitModel()
        {
#if THREADING_TIMER
            // Create an inferred delegate that invokes methods for the timer.
            _paddleTimerCB = paddleTimerCallback;
            // Create a timer that signals the delegate to invoke 
            _paddleTimer = new System.Threading.Timer(_paddleTimerCB, null, 5, 5);
#else
            _paddleTimer = new System.Timers.Timer(5);
            _paddleTimer.Elapsed += new ElapsedEventHandler(paddleTimerHandler);
            _paddleTimer.Start();

#endif

            //how far does the paddle move (pixels)
           _paddleMoveSize = 5;
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
            ballCanvasTop = _windowHeight / 2 - ballHeight / 2;

            paddleCanvasLeft = _windowWidth / 2 - paddleWidth / 2;
            paddleCanvasTop = _windowHeight - paddleHeight;
            _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);
            _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)ballWidth, (int)ballHeight);
        }

        internal void ProcessMouseDrag(uint x, uint y)
        {
            ballCanvasLeft = x - ballWidth / 2;
            ballCanvasTop = y - ballHeight / 2;
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
            _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)ballWidth, (int)ballHeight);
        }
    }
}
