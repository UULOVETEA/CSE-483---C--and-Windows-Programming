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

// WPF Timer
using System.Windows.Threading;

// Sound Player
using System.Media;

///
/// Program Name: BrickBreaker
/// Author: Xiaomeng Cao
/// Date: March 29, 2017
/// Course: CSE-483
///

namespace BrickBreaker
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
        private TimerQueueTimer.WaitOrTimerDelegate _ballTimerCallbackDelegate;
        private TimerQueueTimer.WaitOrTimerDelegate _paddleTimerCallbackDelegate;
        private TimerQueueTimer _ballHiResTimer;
        private TimerQueueTimer _paddleHiResTimer;
        private double _ballXMove = 0.2;
        private double _ballYMove = 0.2;
        System.Drawing.Rectangle _ballRectangle;
        System.Drawing.Rectangle _paddleRectangle;
        bool _movepaddleLeft = false;
        bool _movepaddleRight = false;
        private bool _moveBall = false;

        // Bricks
        public ObservableCollection<Brick> BrickCollection;
        int _numBricks = 75;
        int numBricksHit = 0;
        Rectangle[] _brickRectangles = new Rectangle[75];
        // note that the brick hight, number of brick columns and rows
        // must match our window demensions.
        double _brickHeight = 20;
        double _brickWidth = 50;

        // Color
        System.Windows.Media.Brush FillColorRed;
        System.Windows.Media.Brush FillColorOrange;
        System.Windows.Media.Brush FillColorYellow;
        System.Windows.Media.Brush FillColorCyan;
        System.Windows.Media.Brush FillColorLime;

        // Sound effect
        SoundPlayer HitBrick = new SoundPlayer(BrickBreaker.Properties.Resources.Concrete_break);
        SoundPlayer HitPaddle = new SoundPlayer(BrickBreaker.Properties.Resources.Paddle_Ball);
        SoundPlayer GameOver = new SoundPlayer(BrickBreaker.Properties.Resources.PacMan_Game_Over);

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

        private double _elapsedTime;
        public double ElapsedTime
        {
            get { return _elapsedTime; }
            set
            {
                _elapsedTime = value;
                OnPropertyChanged("ElapsedTime");
            }
        }

        private int _score;
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                OnPropertyChanged("Score");
            }
        }

        private System.Windows.Visibility _gameWin;
        public System.Windows.Visibility GameWin
        {
            get { return _gameWin; }
            set
            {
                _gameWin = value;
                OnPropertyChanged("GameWin");
            }
        }

        /// <summary>
        /// Model constructor
        /// </summary>
        /// <returns></returns>
        public Model()
        {
            SolidColorBrush mySolidColorBrushRed = new SolidColorBrush();         
            SolidColorBrush mySolidColorBrushOrange = new SolidColorBrush();
            SolidColorBrush mySolidColorBrushYellow = new SolidColorBrush();
            SolidColorBrush mySolidColorBrushCyan = new SolidColorBrush();
            SolidColorBrush mySolidColorBrushLime = new SolidColorBrush();

            mySolidColorBrushRed.Color = System.Windows.Media.Color.FromRgb(255, 0, 0);
            mySolidColorBrushOrange.Color = System.Windows.Media.Color.FromRgb(255, 128, 0);
            mySolidColorBrushYellow.Color = System.Windows.Media.Color.FromRgb(255, 255, 0);
            mySolidColorBrushCyan.Color = System.Windows.Media.Color.FromRgb(0, 255, 255);
            mySolidColorBrushLime.Color = System.Windows.Media.Color.FromRgb(0, 255, 0);
            
            FillColorRed = mySolidColorBrushRed;
            FillColorOrange = mySolidColorBrushOrange;
            FillColorYellow = mySolidColorBrushYellow;
            FillColorCyan = mySolidColorBrushCyan;
            FillColorLime = mySolidColorBrushLime;
        }

        // Function to build bricks on the windows
        public void BuildBricks()
        {
            BrickCollection = new ObservableCollection<Brick>();

            // Build 5 x 15 bricks, make each row has different color
            for (int brick = 0; brick < _numBricks; brick++)
            {
                if (brick / 15 == 0)
                {
                    BrickCollection.Add(new Brick()
                    {
                        BrickFill = FillColorRed,
                        BrickHeight = _brickHeight,
                        BrickWidth = _brickWidth,
                        BrickVisible = System.Windows.Visibility.Visible,
                        BrickName = brick.ToString(),
                    });
                }
                if (brick / 15 == 1)
                {
                    BrickCollection.Add(new Brick()
                    {
                        BrickFill = FillColorOrange,
                        BrickHeight = _brickHeight,
                        BrickWidth = _brickWidth,
                        BrickVisible = System.Windows.Visibility.Visible,
                        BrickName = brick.ToString(),
                    });
                }
                if (brick / 15 == 2)
                {
                    BrickCollection.Add(new Brick()
                    {
                        BrickFill = FillColorYellow,
                        BrickHeight = _brickHeight,
                        BrickWidth = _brickWidth,
                        BrickVisible = System.Windows.Visibility.Visible,
                        BrickName = brick.ToString(),
                    });
                }
                if (brick / 15 == 3)
                {
                    BrickCollection.Add(new Brick()
                    {
                        BrickFill = FillColorCyan,
                        BrickHeight = _brickHeight,
                        BrickWidth = _brickWidth,
                        BrickVisible = System.Windows.Visibility.Visible,
                        BrickName = brick.ToString(),
                    });
                }
                if (brick / 15 == 4)
                {
                    BrickCollection.Add(new Brick()
                    {
                        BrickFill = FillColorLime,
                        BrickHeight = _brickHeight,
                        BrickWidth = _brickWidth,
                        BrickVisible = System.Windows.Visibility.Visible,
                        BrickName = brick.ToString(),
                    });
                }

                BrickCollection[brick].BrickCanvasLeft = (brick % 15) * _brickWidth;
                BrickCollection[brick].BrickCanvasTop = (brick / 15) * _brickHeight;
            }
        }

        public void InitModel()
        {
            GameWin = Visibility.Hidden;
            BuildBricks();
            
            //Start_Click();
            // this delegate is needed for the multi media timer defined 
            // in the TimerQueueTimer class.
            _ballTimerCallbackDelegate = new TimerQueueTimer.WaitOrTimerDelegate(BallMMTimerCallback);
            _paddleTimerCallbackDelegate = new TimerQueueTimer.WaitOrTimerDelegate(paddleMMTimerCallback);

            // create our multi-media timers
            _ballHiResTimer = new TimerQueueTimer();
            try
            {
                // create a Multi Media Hi Res timer.
                _ballHiResTimer.Create(1, 1, _ballTimerCallbackDelegate);
            }
            catch (QueueTimerException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Failed to create Ball timer. Error from GetLastError = {0}", ex.Error);
            }

            _paddleHiResTimer = new TimerQueueTimer();
            try
            {
                // create a Multi Media Hi Res timer.
                _paddleHiResTimer.Create(2, 2, _paddleTimerCallbackDelegate);
            }
            catch (QueueTimerException ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Failed to create paddle timer. Error from GetLastError = {0}", ex.Error);
            }
            
            UpdateRects();
        }

        public void CleanUp()
        {
            _ballHiResTimer.Delete();
            _paddleHiResTimer.Delete();

            _moveBall = false;
           
            // Hidde and delete all bricks
            for (int brick = 0; brick < _numBricks; brick++)
            {
                BrickCollection[brick].BrickVisible = Visibility.Hidden;
                BrickCollection[brick].BrickRectangle = Rectangle.Empty;
            }

            numBricksHit = 0;   // Reset numBricksHit to 0
            Score = 0;          // Reset Score to 0
            StopTimer();        // Stop the timer
            seconds = 0;        // Reset seconds to 0
            ElapsedTime = 0;    // Reset ElapsedTime to 0
        }

        // Function to set the position of ball of paddle
        public void SetStartPosition()
        {
            BallHeight = 20;
            BallWidth = 20;
            paddleWidth = 120;
            paddleHeight = 10;

            ballCanvasLeft = _windowWidth / 2 - BallWidth / 2;
            ballCanvasTop = _windowHeight / 2;

            _moveBall = false;

            // Make the ball is visible
            BallVisible = Visibility.Visible;

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

        private void BallMMTimerCallback(IntPtr pWhat, bool success)
        {
            if (!_moveBall)
                return;
            
            // Call function CheckCollision to determine the ball hit bricks or not
            CheckCollision();

            // start executing callback. this ensures we are synched correctly
            // if the form is abruptly closed
            // if this function returns false, we should exit the callback immediately
            // this means we did not get the mutex, and the timer is being deleted.
            if (!_ballHiResTimer.ExecutingCallback())
            {
                Console.WriteLine("Aborting timer callback.");
                return;
            }
            
            ballCanvasLeft += _ballXMove;
            ballCanvasTop += _ballYMove;

            // check to see if ball has it the left or right side of the drawing element
            if ((ballCanvasLeft + BallWidth >= _windowWidth) ||
                (ballCanvasLeft <= 0))
                _ballXMove = -_ballXMove;

            // check to see if ball has it the top of the drawing element
            if (ballCanvasTop <= 0)
                _ballYMove = -_ballYMove;

            if (ballCanvasTop + BallWidth >= _windowHeight)
            {
                // we hit bottom. stop moving the ball
                _moveBall = false;

                // play game over sound effect
                GameOver.Play();

                // stop the timer
                StopTimer();

                // hidde the ball
                BallVisible = Visibility.Hidden;
            }

            // see if we hit the paddle
            _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)BallWidth, (int)BallHeight);
            if (_ballRectangle.IntersectsWith(_paddleRectangle))
            {
                // Play hit paddle sound effect
                HitPaddle.Play();

                // hit paddle. reverse direction in Y direction
                _ballYMove = -_ballYMove;

                // move the ball away from the paddle so we don't intersect next time around and
                // get stick in a loop where the ball is bouncing repeatedly on the paddle
                ballCanvasTop += 2 * _ballYMove;

                // add move the ball in X some small random value so that ball is not traveling in the same 
                // pattern
                ballCanvasLeft += _randomNumber.Next(5);
            }

            // done in callback. OK to delete timer
            _ballHiResTimer.DoneExecutingCallback();
        }

        private void paddleMMTimerCallback(IntPtr pWhat, bool success)
        {
            // start executing callback. this ensures we are synched correctly
            // if the form is abruptly closed
            // if this function returns false, we should exit the callback immediately
            // this means we did not get the mutex, and the timer is being deleted.
            if (!_paddleHiResTimer.ExecutingCallback())
            {
                Console.WriteLine("Aborting timer callback.");
                return;
            }

            if (_movepaddleLeft && paddleCanvasLeft > 0)
                paddleCanvasLeft -= 2;
            else if (_movepaddleRight && paddleCanvasLeft < _windowWidth - paddleWidth)
                paddleCanvasLeft += 2;

            _paddleRectangle = new System.Drawing.Rectangle((int)paddleCanvasLeft, (int)paddleCanvasTop, (int)paddleWidth, (int)paddleHeight);

            // done in callback. OK to delete timer
            _paddleHiResTimer.DoneExecutingCallback();
        }

        private void UpdateRects()
        {
            //_ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)ballWidth, (int)ballHeight);
            for (int brick = 0; brick < _numBricks; brick++)
                BrickCollection[brick].BrickRectangle = new System.Drawing.Rectangle((int)BrickCollection[brick].BrickCanvasLeft,
                    (int)BrickCollection[brick].BrickCanvasTop, (int)BrickCollection[brick].BrickWidth, (int)BrickCollection[brick].BrickHeight);
        }

        enum InterectSide { NONE, LEFT, RIGHT, TOP, BOTTOM };
        private InterectSide IntersectsAt(Rectangle brick, Rectangle ball)
        {
            if (brick.IntersectsWith(ball) == false)
                return InterectSide.NONE;

            Rectangle r = Rectangle.Intersect(brick, ball);

            _ballRectangle = new System.Drawing.Rectangle((int)ballCanvasLeft, (int)ballCanvasTop, (int)BallWidth, (int)BallHeight);

            if (_ballRectangle.IntersectsWith(r))
            {
                // hit paddle. reverse direction in Y direction
                _ballYMove = -_ballYMove;

                // move the ball away from the paddle so we don't intersect next time around and
                // get stick in a loop where the ball is bouncing repeatedly on the paddle
                ballCanvasTop += 2 * _ballYMove;

                // add move the ball in X some small random value so that ball is not traveling in the same 
                // pattern
                ballCanvasLeft += _randomNumber.Next(5);
            }

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

        private void CheckCollision()
        {
            for (int brick = 0; brick < _numBricks; brick++)
            {
                InterectSide whichSide = IntersectsAt(BrickCollection[brick].BrickRectangle, _ballRectangle);
                switch (whichSide)
                {
                    case InterectSide.NONE:
                        break;

                    case InterectSide.TOP:
                        // Hidde the brick that is hit
                        BrickCollection[brick].BrickVisible = Visibility.Hidden;
                        // Delete the brick that is hit
                        BrickCollection[brick].BrickRectangle = Rectangle.Empty;
                        // Play the hit brick sound effect
                        HitBrick.Play();
                        Score += 1;
                        numBricksHit++;
                        // Call function CheckWin to determine user win the game or not
                        CheckWin();
                        break;

                    case InterectSide.BOTTOM:
                        BrickCollection[brick].BrickVisible = Visibility.Hidden;
                        BrickCollection[brick].BrickRectangle = Rectangle.Empty;
                        HitBrick.Play();
                        Score += 1;
                        numBricksHit++;
                        CheckWin();
                        break;

                    case InterectSide.LEFT:
                        BrickCollection[brick].BrickVisible = Visibility.Hidden;
                        BrickCollection[brick].BrickRectangle = Rectangle.Empty;
                        HitBrick.Play();
                        Score += 1;
                        numBricksHit++;
                        CheckWin();
                        break;

                    case InterectSide.RIGHT:
                        BrickCollection[brick].BrickVisible = Visibility.Hidden;
                        BrickCollection[brick].BrickRectangle = Rectangle.Empty;
                        HitBrick.Play();
                        Score += 1;
                        numBricksHit++;
                        CheckWin();
                        break;
                }
            }
        }

        // Function to check the user win the game or not
        private void CheckWin()
        {
            // if user hit the number of bricks is equal to total number of bricks
            if (numBricksHit == _numBricks)
            {
                // Stop the timer
                StopTimer();
                // Stop move the ball
                MoveBall = !MoveBall;
                // Hidde the ball
                BallVisible = Visibility.Hidden;
                // Display a message that win the game
                GameWin = Visibility.Visible;
            }
        }

        #region Timer function
        DispatcherTimer timer;
        int seconds = 0;
        bool timerRunning = false;
        public void StartTimer(bool startStop)
        {
            if (startStop == true)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timer.Start();
                timerRunning = true;
            }
            else if (timerRunning == true && startStop == false)
            {
                timer.Stop();
            }
        }

        public void StopTimer()
        {
            StartTimer(false);
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            seconds++;
            ElapsedTime = seconds;
        }
        #endregion
    }
}
