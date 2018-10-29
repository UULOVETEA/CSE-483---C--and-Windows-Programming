using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace UDPClient
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

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        // some data that keeps track of ports and addresses
        private UInt32 _remotePort = 5000;
        private String _remoteIPAddress = "127.0.0.1";

        // this is the UDP socket that will be used to communicate
        // over the network
        private UdpClient _dataSocket;

        public void Main()
        {
            try
            {
                // set up generic UDP socket and bind to local port
                //
                _dataSocket = new UdpClient();
            }
            catch (Exception ex)
            {
                Status = ex.ToString();
                return;
            }

            //SendMessage();
        }

        public void SendMessage()
        {
            IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);
            String text;
            text = Message.ToString();
            Byte[] sendBytes = Encoding.ASCII.GetBytes(text);

            try
            {
                _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);
                Status += DateTime.Now + ": " + ":Message Sent Successfully";
            }
            catch (SocketException ex)
            {
                Status = ex.ToString();
                return;
            }
        }
    }
}
