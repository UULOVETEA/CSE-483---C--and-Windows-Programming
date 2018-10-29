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

// hi res timer
using PrecisionTimers;

// Rectangle
// Must update References manually
using System.Drawing;

// INotifyPropertyChanged
using System.ComponentModel;

using System.Threading;


namespace BouncingBall
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

        private static UInt32 _numBalls = 1;
        private UInt32[] _buttonPresses = new UInt32[_numBalls];
        Random _randomNumber = new Random();
        //private TimerQueueTimer.WaitOrTimerDelegate _ballTimerCallbackDelegate;
        //private TimerQueueTimer.WaitOrTimerDelegate _paddleTimerCallbackDelegate;
        //private TimerQueueTimer _ballHiResTimer;
        //private TimerQueueTimer _paddleHiResTimer;
        private double _ballXMove = 1;
        private double _ballYMove = 1;
        System.Drawing.Rectangle _ballRectangle;
        System.Drawing.Rectangle _paddleRectangle;
        bool _movepaddleLeft = false;
        bool _movepaddleRight = false;
        private bool _moveBall = false;
        private bool _isThreadBallRunning = true;

        private Thread _threadA = null;

        public bool MoveBall
        {
            get { return _moveBall; }
            set { _moveBall = value; }
        }

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
            // this delegate is needed for the multi media timer defined 
            // in the TimerQueueTimer class.
            //_ballTimerCallbackDelegate = new TimerQueueTimer.WaitOrTimerDelegate(BallMMTimerCallback);
            //_paddleTimerCallbackDelegate = new TimerQueueTimer.WaitOrTimerDelegate(paddleMMTimerCallback);

            //// create our multi-media timers
            //_ballHiResTimer = new TimerQueueTimer();
            //try
            //{
            //    // create a Multi Media Hi Res timer.
            //    _ballHiResTimer.Create(1, 1, _ballTimerCallbackDelegate);
            //}
            //catch (QueueTimerException ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //    Console.WriteLine("Failed to create Ball timer. Error from GetLastError = {0}", ex.Error);
            //}

            //_paddleHiResTimer = new TimerQueueTimer();
            //try
            //{
            //    // create a Multi Media Hi Res timer.
            //    _paddleHiResTimer.Create(2, 2, _paddleTimerCallbackDelegate);
            //}
            //catch (QueueTimerException ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //    Console.WriteLine("Failed to create paddle timer. Error from GetLastError = {0}", ex.Error);
            //}
            _threadA = new Thread(new ThreadStart(ThreadBallFunction));
            _threadA.Start();
        }

        public void CleanUp()
        {
            _isThreadBallRunning = false;
            if (_threadA != null)
            {
                _threadA.Join();
            }
        }


        public void SetStartPosition()
        {
            
            BallHeight = 50;
            BallWidth = 50;
            paddleWidth = 120;
            paddleHeight = 10;

            ballCanvasLeft = _windowWidth/2 - BallWidth/2;
            ballCanvasTop = _windowHeight/5;
           
            _moveBall = false;

            paddleCanvasLeft = _windowWidth / 2 - paddleWidth / 2;
            paddleCanvasTop = _windowHeight - paddleHeight;
            _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);
        }

        public void MoveLeft(bool move)
        {
            _movepaddleLeft = move;
        }

        public void MoveRight(bool move)
        {
            _movepaddleRight = move;
        }


        private void BallMMTimerCallback()
        {

            if (!_moveBall)
                return;

            // start executing callback. this ensures we are synched correctly
            // if the form is abruptly closed
            // if this function returns false, we should exit the callback immediately
            // this means we did not get the mutex, and the timer is being deleted.
            //if (!_ballHiResTimer.ExecutingCallback())
            //{
            //    Console.WriteLine("Aborting timer callback.");
            //    return;
            //}

            ballCanvasLeft += _ballXMove;
            ballCanvasTop += _ballYMove;

            // check to see if ball has it the left or right side of the drawing element
            if ((ballCanvasLeft + BallWidth >= _windowWidth) ||
                (ballCanvasLeft <= 0))
                _ballXMove = -_ballXMove;


            // check to see if ball has it the top of the drawing element
            if ( ballCanvasTop <= 0) 
                _ballYMove = -_ballYMove;

            if (ballCanvasTop + BallWidth >= _windowHeight)
            {
                // we hit bottom. stop moving the ball
                _moveBall = false;
            }

            // see if we hit the paddle
            _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)BallWidth, (int)BallHeight);
            if (_ballRectangle.IntersectsWith(_paddleRectangle))
            {
                // hit paddle. reverse direction in Y direction
                _ballYMove = -_ballYMove;

                // move the ball away from the paddle so we don't intersect next time around and
                // get stick in a loop where the ball is bouncing repeatedly on the paddle
                ballCanvasTop += 2*_ballYMove;

                // add move the ball in X some small random value so that ball is not traveling in the same 
                // pattern
                ballCanvasLeft += _randomNumber.Next(5);
            }

             // done in callback. OK to delete timer
            //_ballHiResTimer.DoneExecutingCallback();
        }

        private void paddleMMTimerCallback()
        {

            // start executing callback. this ensures we are synched correctly
            // if the form is abruptly closed
            // if this function returns false, we should exit the callback immediately
            // this means we did not get the mutex, and the timer is being deleted.
            //if (!_paddleHiResTimer.ExecutingCallback())
            //{
            //    Console.WriteLine("Aborting timer callback.");
            //    return;
            //}

            if (_movepaddleLeft && paddleCanvasLeft > 0)
                paddleCanvasLeft -= 3;
            else if (_movepaddleRight && paddleCanvasLeft < _windowWidth - paddleWidth)
                paddleCanvasLeft += 3;
            
            _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);


            // done in callback. OK to delete timer
            //_paddleHiResTimer.DoneExecutingCallback();
        }

        void ThreadBallFunction()
        {
            try
            {
                while (_isThreadBallRunning)
                {
                    Thread.Sleep(2);
                    if (!MoveBall) continue;
                    BallMMTimerCallback();
                    paddleMMTimerCallback();

                    // below are all the things this thread needs to do
                    //ThreadBallData = _randomNumber.Next().ToString();
                }
            }
            catch (ThreadAbortException)
            {
                Debug.Write("Thread Ball Aborted\n");
            }
        }
    }
}
