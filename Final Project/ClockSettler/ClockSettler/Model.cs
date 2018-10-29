using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Sockets
using System.Net.Sockets;
using System.Net;

// debug
using System.Diagnostics;

// threading
using System.Threading;

// byte data serialization
using System.Runtime.Serialization.Formatters.Binary;

// memory streams
using System.IO;

using TimeDataDLL;


namespace ClockSettler
{
    partial class Model
    {
        public bool isAlarm;

        // make GameData serializeable. Otherwise, we can't send it over 
        // a byte stream (e.g. socket)
        [Serializable]
        public struct StructTimeData
        {
            public bool isAlarmTime;
            public int hour, minute, second;

            public StructTimeData(int h, int m, int s, bool a)
            {
                hour = h;
                minute = m;
                second = s;
                isAlarmTime = a;
            }
        }

        // this is the UDP socket that will be used to communicate
        // over the network
        UdpClient _dataSocket;

        // some data that keeps track of ports and addresses
        private static UInt32 _remotePort = 50001;
        private static String _remoteIPAddress = "127.0.0.1";

        // this is the thread that will run in the background
        // waiting for incomming data
        //private Thread _receiveDataThread;

        public Model()
        {
            _dataSocket = new UdpClient((int)_remotePort);
        }

        public void CleanUp()
        {
            // if we don't close the socket and abort the thread, 
            // the applicatoin will not close properly
            if (_dataSocket != null) _dataSocket.Close();
            //if (_receiveDataThread != null) _receiveDataThread.Abort();
        }

        /// <summary>
        /// Function to send set time
        /// </summary>
        public void SendSetTime()
        {
            
            isAlarm = false;

            StructTimeData timeData;

            // formatter used for serialization of data
            BinaryFormatter formatter = new BinaryFormatter();

            // stream needed for serialization
            MemoryStream stream = new MemoryStream();

            // Byte array needed to send data over a socket
            Byte[] sendBytes;

            // check to make sure boxes have something in them to send
            if (Hours == "" || Minutes == "" || Seconds == "")
            {
                Status = DateTime.Now + " Empty boxes! Try again.\n";
                return;
            }

            // we make sure that the data in the boxes is in the correct format
            try
            {
                timeData.hour = int.Parse(Hours);
                timeData.minute = int.Parse(Minutes);
                timeData.second = int.Parse(Seconds);
                timeData.isAlarmTime = isAlarm;

                if (timeData.hour < 0 || timeData.hour > 24)
                {
                    Status = DateTime.Now + " hour is invalid time! Try again.\n";
                    return;
                }
                if (timeData.minute < 0 || timeData.minute > 60)
                {
                    Status = DateTime.Now + " minute is invalid time! Try again.\n";
                    return;
                }
                if (timeData.second < 0 || timeData.second > 60)
                {
                    Status = DateTime.Now + " second is invalid time! Try again.\n";
                    return;
                }
            }
            catch (System.Exception)
            {
                // we get here if the format of teh data in the boxes was incorrect. Most likely the boxes we assumed
                // had integers in them had characters as well
                Status = DateTime.Now + " Data not in correct format! Try again.\n";
                return;
            }

            // serialize the gameData structure to a stream
            formatter.Serialize(stream, timeData);

            // retrieve a Byte array from the stream
            sendBytes = stream.ToArray();

            // send the serialized data
            IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);
            try
            {
                _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);
            }
            catch (SocketException)
            {
                Status = DateTime.Now + ":" + " ERROR: Alarm update not sent!\n";
                return;
            }

            Status =  DateTime.Now + ":" + " Alram update sent successfully.\n";
        }

        /// <summary>
        /// Function to send set time with alarm set
        /// </summary>
        public void SendAlarmTime()
        {
            isAlarm = true;

            StructTimeData timeData;

            // formatter used for serialization of data
            BinaryFormatter formatter = new BinaryFormatter();

            // stream needed for serialization
            MemoryStream stream = new MemoryStream();

            // Byte array needed to send data over a socket
            Byte[] sendBytes;

            // check to make sure boxes have something in them to send
            if (Hours == "" || Minutes == "" || Seconds == "")
            {
                Status = DateTime.Now + " Empty boxes! Try again.\n";
                return;
            }

            // we make sure that the data in the boxes is in the correct format
            try
            {
                timeData.hour = int.Parse(Hours);
                timeData.minute = int.Parse(Minutes);
                timeData.second = int.Parse(Seconds);
                timeData.isAlarmTime = isAlarm;

                if (timeData.hour < 0 || timeData.hour > 24)
                {
                    Status = DateTime.Now + " hour is invalid time! Try again.\n";
                    return;
                }
                if (timeData.minute < 0 || timeData.minute > 60)
                {
                    Status = DateTime.Now + " minute is invalid time! Try again.\n";
                    return;
                }
                if (timeData.second < 0 || timeData.second > 60)
                {
                    Status = DateTime.Now + " second is invalid time! Try again.\n";
                    return;
                }
            }
            catch (System.Exception)
            {
                // we get here if the format of teh data in the boxes was incorrect. Most likely the boxes we assumed
                // had integers in them had characters as well
                Status = DateTime.Now + " Data not in correct format! Try again.\n";
                return;
            }

            // serialize the gameData structure to a stream
            formatter.Serialize(stream, timeData);

            // retrieve a Byte array from the stream
            sendBytes = stream.ToArray();

            // send the serialized data
            IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);
            try
            {
                _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);
            }
            catch (SocketException)
            {
                Status = DateTime.Now + ":" + " ERROR: Alarm update not sent!\n";
                return;
            }

            Status = DateTime.Now + ":" + " Alram update sent successfully.\n";
        }

        /// <summary>
        /// Function to send current time
        /// </summary>
        public void SendNOWTime()
        {
            Hours = DateTime.Now.ToString("HH");
            Minutes = DateTime.Now.ToString("mm");
            Seconds = DateTime.Now.ToString("ss");
            isAlarm = false;

            StructTimeData timeData;

            // formatter used for serialization of data
            BinaryFormatter formatter = new BinaryFormatter();

            // stream needed for serialization
            MemoryStream stream = new MemoryStream();

            // Byte array needed to send data over a socket
            Byte[] sendBytes;

            timeData.hour = int.Parse(Hours);
            timeData.minute = int.Parse(Minutes);
            timeData.second = int.Parse(Seconds);
            timeData.isAlarmTime = isAlarm;


            // serialize the gameData structure to a stream
            formatter.Serialize(stream, timeData);

            // retrieve a Byte array from the stream
            sendBytes = stream.ToArray();

            // send the serialized data
            IPEndPoint remoteHost = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);
            try
            {
                _dataSocket.Send(sendBytes, sendBytes.Length, remoteHost);
            }
            catch (SocketException)
            {
                Status = DateTime.Now + ":" + " ERROR: Alarm update not sent!\n";
                return;
            }

            Status = DateTime.Now + ":" + " Alram update sent successfully.\n";
        }
    }
}
