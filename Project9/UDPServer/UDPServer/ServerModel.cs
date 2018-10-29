using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Sockets
using System.Net.Sockets;
using System.Net;

// Threads
using System.Threading;

// Message Box in Console app
// Don't forget to add Reference to System.Windows.Forms
using System.Windows.Forms;
using System.ComponentModel;

namespace UDPServer
{
    class Model : INotifyPropertyChanged
    {
        // define our property change event handler, part of data binding
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        private string _rMessage;
        public string RMessage
        {
            get { return _rMessage; }
            set
            {
                _rMessage = value;
                OnPropertyChanged("RMessage");
            }
        }
        // some data that keeps track of ports and addresses
        private int _localPort = 5000;
        private string _localIPAddress = "127.0.0.1";

        // this is the thread that will run in the background
        // waiting for incomming data
        private Thread _receiveDataThread;

        // this is the UDP socket that will be used to communicate
        // over the network
        private UdpClient _dataSocket;

        public void Main()
        {
            RMessage = "UDP Server is Running!!\n";//, "UDP Server");
            _dataSocket = new UdpClient(_localPort);
            StartThread();
        }

        private void ReceiveThreadFunction()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(_localIPAddress), (int)_localPort);
            while (true)
            {
                try
                {
                    // wait for data
                    // this is a blocking call
                    Byte[] receiveData = _dataSocket.Receive(ref endPoint);

                    // convert byte array to a string
                    RMessage = DateTime.Now + ": " + System.Text.Encoding.Default.GetString(receiveData) + "\n";//, "UDP Server");
                }
                catch (SocketException ex)
                {
                    // got here because either the Receive failed, or more
                    // or more likely the socket was destroyed by 
                    // exiting from the JoystickPositionWindow form
                    RMessage = ex.ToString();//, "UDP Server");
                    return;
                }
            }
        }

        public void StartThread()
        {
            // start the thread to listen for data from other UDP peer
            ThreadStart threadFunction = new ThreadStart(ReceiveThreadFunction);
            _receiveDataThread = new Thread(threadFunction);
            _receiveDataThread.Start();
        }
    }
}
