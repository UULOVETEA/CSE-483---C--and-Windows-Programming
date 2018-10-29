using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// threading
using System.Threading;


namespace SimpleEvents
{
    partial class Model
    {
        public class SimpleEventArgs : EventArgs
        {
            public string message;
            public SimpleEventArgs(string m)
            {
                message = m;
            }
        }

        public class SimpleEventArgs2 : EventArgs
        {
            public string message2;
            public SimpleEventArgs2(string m2)
            {
                message2 = m2;
            }
        }

        public class SimpleEventArgs3 : EventArgs
        {
            public string message3;
            public SimpleEventArgs3(string m3)
            {
                message3 = m3;
            }
        }

        private Thread _subscriber1Thread;
        private bool _subscriber1ThreadIsRunning = false;
        private Thread _subscriber2Thread;
        private bool _subscriber2ThreadIsRunning = false;
        private Thread _subscriber3Thread;
        private bool _subscriber3ThreadIsRunning = false;

        public delegate void SimpleEventHandler(object sender, SimpleEventArgs e);
        public delegate void SimpleEventHandler2(object sender, SimpleEventArgs2 e);
        public delegate void SimpleEventHandler3(object sender, SimpleEventArgs3 e);

        public event SimpleEventHandler MyMessageEvent;
        public event SimpleEventHandler2 MyMessageEvent2;
        public event SimpleEventHandler3 MyMessageEvent3;
        public SimpleEventHandler handler;
        public SimpleEventHandler2 handler2;
        public SimpleEventHandler3 handler3;

        Random _randomNumber;

        public Model()
        {

            handler = MyMessageEvent;
            handler2 = MyMessageEvent2;

            _randomNumber = new Random();

            _subscriber1Thread = new Thread(new ThreadStart(Subscriber1ThreadFunction));
            _subscriber1ThreadIsRunning = true;
            _subscriber1Thread.Start();
            _subscriber2Thread = new Thread(new ThreadStart(Subscriber2ThreadFunction));
            _subscriber2ThreadIsRunning = true;
            _subscriber2Thread.Start();
            _subscriber3Thread = new Thread(new ThreadStart(Subscriber3ThreadFunction));
            _subscriber3ThreadIsRunning = true;
            _subscriber3Thread.Start();
        }

        public void Cleanup()
        {
            _subscriber1Thread.Abort();
            _subscriber2Thread.Abort();
            _subscriber3Thread.Abort();
        }

        public void SendButtonClicked()
        {
            if (handler != null)
                handler(this, new SimpleEventArgs("Initial Message: " + PublisherText));
        }

        void Subscriber1ThreadFunction()
        {
            handler += new SimpleEventHandler(Subscriber1Handler);

            try
            {
                while (_subscriber1ThreadIsRunning == true)
                {
                    Subscriber1Data = _randomNumber.Next(1,100).ToString();
                    Thread.Sleep(_randomNumber.Next(200,500));
                }
            }
            catch(System.Threading.ThreadAbortException)
            {
                Console.WriteLine("Thread 1 is aborted");
            }
        }

        async void Subscriber1Handler(object sender, SimpleEventArgs e)
        {
            await Task.Delay(1000);
            Subscriber1Text = "Subscriber 1: " + e.message;
            if (handler2 != null)
                handler2(this, new SimpleEventArgs2("T1Msg: " + e.message));
        }

        void Subscriber2ThreadFunction()
        {
            handler2 += new SimpleEventHandler2(Subscriber2Handler);

            try
            {
                while (_subscriber2ThreadIsRunning == true)
                {
                    Subscriber2Data = _randomNumber.Next(101,200).ToString();
                    Thread.Sleep(_randomNumber.Next(200, 500));
                }
            }
            catch(System.Threading.ThreadAbortException)
            {
                Console.WriteLine("Thread 2 is aborted");
            }
        }

        async void Subscriber2Handler(object sender, SimpleEventArgs2 e)
        {
            await Task.Delay(1000);
            Subscriber2Text = "Subscriber 2: " + e.message2;
            if (handler3 != null)
                handler3(this, new SimpleEventArgs3("T2Msg: " + e.message2));
        }

        void Subscriber3ThreadFunction()
        {
            handler3 += new SimpleEventHandler3(Subscriber3Handler);

            try
            {
                while (_subscriber3ThreadIsRunning == true)
                {
                    Subscriber3Data = _randomNumber.Next(201,300).ToString();
                    Thread.Sleep(_randomNumber.Next(200, 500));
                }
            }
            catch(System.Threading.ThreadAbortException)
            {
                Console.WriteLine("Thread 3 is aborted");
            }
        }

        async void Subscriber3Handler(object sender, SimpleEventArgs3 e)
        {
            await Task.Delay(1000);
            Subscriber3Text = "Subscriber 3: " + e.message3;
        }

    }
}
