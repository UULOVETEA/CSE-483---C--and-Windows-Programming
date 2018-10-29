using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;

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

///
/// Program Name: Tic Tac Toe-Network
/// Author: Xiaomeng Cao
/// Date: April 29, 2017
/// Course: CSE-483
///

namespace TicTacToe_Network
{
    partial class Model
    {
        public ObservableCollection<Tile> TileCollection;
        private static UInt32 _numTiles = 9;
        int count = 0;
        int index = 0;
        bool XMove, OMove;
        bool isReceive = false;

        // make GameData serializeable. Otherwise, we can't send it over 
        // a byte stream (e.g. socket)
        [Serializable]
        struct GameData
        {
            public int data;
            public String message;

            public GameData(int p, string s)
            {
                data = p;
                message = s;
            }
        }

        // this is the UDP socket that will be used to communicate
        // over the network
        UdpClient _dataSocket;

        // some data that keeps track of ports and addresses
        private static UInt32 _localPort;
        private static String _localIPAddress;
        private static UInt32 _remotePort;
        private static String _remoteIPAddress;

        // this is the thread that will run in the background
        // waiting for incomming data
        private Thread _receiveDataThread;

        // this thread is used to synchronize the startup of 
        // two UDP peers
        private Thread _synchWithOtherPlayerThread;

        public Model()
        {
            isReceive = false;

            TileCollection = new ObservableCollection<Tile>();

            // initial all buttons
            for (int i = 0; i < _numTiles; i++)
            {
                TileCollection.Add(new Tile()
                {
                    TileBrush = Brushes.Black,
                    TileLabel = "",
                    TileName = i.ToString(),
                    TileBackground = Brushes.LightGray
                });
            }

            // this disables the Send button initially
            PlayEnabled = false;
            RestartEnabled = false;

            // initialize the help test
            Status = "Select Socket Setup button to being.";
        }

        public void UserSelection(String buttonSelected)
        {   
            // if game is over, display an message when the user click buton         
            if (Status == "We have a winner!" || Status == "Game Over. Restart to play again.")
            {
                Status = "Game Over. Restart to play again.";
                LabelColor = Brushes.Red;
                return;
            }
            if (Status == "We have a tie!" || Status == "Game Over. Restart to play again.")
            {
                Status = "Game Over. Restart to play again.";
                LabelColor = Brushes.Red;
                return;
            }

            // if not X turn, display an message
            if (XMove == false && OMove == true && isReceive == false)
            {
                Status = "Not your Turn. Please wait O's action";
                LabelColor = Brushes.Red;
                return;
            }
            // else if not O turn
            else if (XMove == false && OMove == false && isReceive == false)
            {
                Status = "Not your Turn. Please wait X's action";
                LabelColor = Brushes.Red;
                return;
            }

            // get the number which button the user clicked
            index = int.Parse(buttonSelected);
            // increase count in order to determing is X turn or O turn
            count++;

            // if the button has already clicked
            if (TileCollection[index].isSet)
            {
                // print out the error message
                Status = "Error, button already clicked";
                // change the label color to red
                LabelColor = Brushes.Red;
                // decrease count to make any player will not have two chances
                count--;
                return;
            }

            // if count is odd
            if (count % 2 != 0)
            {
                // place X on the button
                TileCollection[index].TileLabel = "X";
                // make the X to red
                TileCollection[index].TileBrush = Brushes.Red;
                // disable the button
                TileCollection[index].isSet = true;
                // clear the status label
                Status = "";
                // change the label color to white
                LabelColor = Brushes.White;
            }
            // else if count is even
            else
            {
                TileCollection[index].TileLabel = "O";
                TileCollection[index].TileBrush = Brushes.Blue;
                TileCollection[index].isSet = true;
                Status = "";
                LabelColor = Brushes.White;
            }

            // call function CheckWinner to check if any player win
            CheckWinner();
        }

        // function to check if any player win the game or we have a draw
        void CheckWinner()
        {
            // if any player has the same Xs or Os on the specific row
            if (TileCollection[0].TileLabel != "" && TileCollection[0].TileLabel == TileCollection[1].TileLabel && TileCollection[1].TileLabel == TileCollection[2].TileLabel)
            {
                MarkWinner(0, 1, 2);
            }
            else if (TileCollection[3].TileLabel != "" && TileCollection[3].TileLabel == TileCollection[4].TileLabel && TileCollection[4].TileLabel == TileCollection[5].TileLabel)
            {
                MarkWinner(3, 4, 5);
            }
            else if (TileCollection[6].TileLabel != "" && TileCollection[6].TileLabel == TileCollection[7].TileLabel && TileCollection[7].TileLabel == TileCollection[8].TileLabel)
            {
                MarkWinner(6, 7, 8);
            }
            else if (TileCollection[0].TileLabel != "" && TileCollection[0].TileLabel == TileCollection[4].TileLabel && TileCollection[4].TileLabel == TileCollection[8].TileLabel)
            {
                MarkWinner(0, 4, 8);
            }
            else if (TileCollection[2].TileLabel != "" && TileCollection[2].TileLabel == TileCollection[4].TileLabel && TileCollection[4].TileLabel == TileCollection[6].TileLabel)
            {
                MarkWinner(2, 4, 6);
            }
            else if (TileCollection[0].TileLabel != "" && TileCollection[0].TileLabel == TileCollection[3].TileLabel && TileCollection[3].TileLabel == TileCollection[6].TileLabel)
            {
                MarkWinner(0, 3, 6);
            }
            else if (TileCollection[1].TileLabel != "" && TileCollection[1].TileLabel == TileCollection[4].TileLabel && TileCollection[4].TileLabel == TileCollection[7].TileLabel)
            {
                MarkWinner(1, 4, 7);
            }
            else if (TileCollection[2].TileLabel != "" && TileCollection[2].TileLabel == TileCollection[5].TileLabel && TileCollection[5].TileLabel == TileCollection[8].TileLabel)
            {
                MarkWinner(2, 5, 8);
            }
            // else if no one win the game
            else if (TileCollection[0].isSet == true && TileCollection[1].isSet == true && TileCollection[2].isSet == true
                && TileCollection[3].isSet == true && TileCollection[4].isSet == true && TileCollection[5].isSet == true
                && TileCollection[6].isSet == true && TileCollection[7].isSet == true && TileCollection[8].isSet == true)
            {
                Status = "We have a tie!";
                LabelColor = Brushes.Orange;
            }
        }

        // function to get the specific number and highlight the winning row
        void MarkWinner(int a, int b, int c)
        {
            // highlight the specific row
            TileCollection[a].TileBackground = Brushes.Yellow;
            TileCollection[b].TileBackground = Brushes.Yellow;
            TileCollection[c].TileBackground = Brushes.Yellow;
            // print out winning message
            Status = "You Win!";
            // change the label color to lime
            LabelColor = Brushes.Lime;
            // make all buttons disable
            for (int x = 0; x < _numTiles; x++)
            {
                TileCollection[x].isSet = true;
            }
        }

        // fucntion to clear contents of the buttons and label
        public void Clear()
        {
            // clear the all buttons and make them to original
            for (int x = 0; x < _numTiles; x++)
            {
                TileCollection[x].TileBackground = Brushes.LightGray;
                TileCollection[x].TileBrush = Brushes.Black;
                TileCollection[x].TileLabel = "";
                // enable all buttons
                TileCollection[x].isSet = false;
            }

            // reset the count to 0
            count = 0;
            // clear the status label
            Status = "";
            // change the label color to white
            LabelColor = Brushes.White;

            isReceive = false;

            // Determine game order based on the port number
            if (_localPort < _remotePort)
            {
                Status = "You are X, go first";
                XMove = true;
                OMove = false;
            }
            else
            {
                Status = "You are O, wait for X's action";
                XMove = false;
                OMove = false;
            }
        }

        /// <summary>
        /// this method is called to set this UDP peer's local port and address
        /// </summary>
        /// <param name="port"></param>
        /// <param name="ipAddress"></param>
        public void SetLocalNetworkSettings(UInt32 port, String ipAddress)
        {
            _localPort = port;
            _localIPAddress = ipAddress;
        }

        /// <summary>
        /// this method is called to set the remote UDP peer's port and address
        /// </summary>
        /// <param name="port"></param>
        /// <param name="ipAddress"></param>
        public void SetRemoteNetworkSettings(UInt32 port, String ipAddress)
        {
            _remotePort = port;
            _remoteIPAddress = ipAddress;
        }

        /// <summary>
        /// initialize the necessary data, and start the synchronization
        /// thread to wait for the other peer to join
        /// </summary>
        /// <returns></returns>
        public bool InitModel()
        {
            try
            {
                // set up generic UDP socket and bind to local port
                //
                _dataSocket = new UdpClient((int)_localPort);
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                return false;
            }

            ThreadStart threadFunction;
            threadFunction = new ThreadStart(SynchWithOtherPlayer);
            _synchWithOtherPlayerThread = new Thread(threadFunction);
            StatusTextBox = StatusTextBox + DateTime.Now + ":" + " Waiting for other UDP peer to join.\n";
            _synchWithOtherPlayerThread.Start();

            // reset help text
            Status = "";

            return true;
        }

        /// <summary>
        /// called to send some data to the other UDP peer
        /// </summary>
        public void SendMessage()
        {
            if (XMove == false && OMove == true)
            {
                Status = "Not your Turn. Please wait player O";
                return;
            }
            else if (XMove == false && OMove == false)
            {
                Status = "Not your Turn. Please wait player X";
                return;
            }

            // data structure used to communicate data with the other player
            GameData gameData;

            // formatter used for serialization of data
            BinaryFormatter formatter = new BinaryFormatter();

            // stream needed for serialization
            MemoryStream stream = new MemoryStream();

            // Byte array needed to send data over a socket
            Byte[] sendBytes;

            // we make sure that the data in the boxes is in the correct format
            try
            {
                gameData.data = index;
                gameData.message = Status;
            }
            catch (System.Exception)
            {
                // we get here if the format of teh data in the boxes was incorrect. Most likely the boxes we assumed
                // had integers in them had characters as well
                StatusTextBox = StatusTextBox + DateTime.Now + " Data not in correct format! Try again.\n";
                return;
            }

            // serialize the gameData structure to a stream
            formatter.Serialize(stream, gameData);

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
                StatusTextBox = StatusTextBox + DateTime.Now + ":" + " ERROR: Message not sent!\n";
                return;
            }
            StatusTextBox = StatusTextBox + DateTime.Now + ":" + " Message sent successfully.\n";

            // make change for boolean variable to control game order
            XMove = !XMove;
            OMove = !OMove;
        }

        /// <summary>
        /// called when the view is closing to ensure we clean up our socket
        /// if we don't, the application may hang on exit
        /// </summary>
        public void Model_Cleanup()
        {
            // important. Close socket or application will not exit correctly.
            if (_dataSocket != null) _dataSocket.Close();
            if (_receiveDataThread != null) _receiveDataThread.Abort();
        }

        // this is the thread that waits for incoming messages
        private void ReceiveThreadFunction()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    // wait for data
                    Byte[] receiveData = _dataSocket.Receive(ref endPoint);

                    // check to see if this is synchronization data 
                    // ignore it. we should not recieve any sychronization
                    // data here, because synchronization data should have 
                    // been consumed by the SynchWithOtherPlayer thread. but, 
                    // it is possible to get 1 last synchronization byte, which we
                    // want to ignore
                    if (receiveData.Length < 2)
                        continue;

                    // process and display data
                    GameData gameData;
                    BinaryFormatter formatter = new BinaryFormatter();
                    MemoryStream stream = new MemoryStream();

                    // deserialize data back into our GameData structure
                    stream = new System.IO.MemoryStream(receiveData);
                    gameData = (GameData)formatter.Deserialize(stream);
                    // store data into local variable
                    String OpponentSelected = gameData.data.ToString();
                    String OpponentMessage = gameData.message.ToString();
                    // Set to true in order to pass data to synchronize game board
                    isReceive = true;
                    //Data2 = gameData.data2.ToString();
                    UserSelection(OpponentSelected);
                    if (OpponentMessage == "You Win!")
                    {
                        Status = "You Lost!";
                        LabelColor = Brushes.Gray;
                    }
                    // update status window
                    StatusTextBox = StatusTextBox + DateTime.Now + ":" + " New message received.\n";
                    // make change for boolean variable to control game order
                    isReceive = false;
                    XMove = !XMove;
                    OMove = !OMove;
                }
                catch (SocketException ex)
                {
                    // got here because either the Receive failed, or more
                    // or more likely the socket was destroyed by 
                    // exiting from the JoystickPositionWindow form
                    Console.WriteLine(ex.ToString());
                    return;
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
            }
        }

        /// <summary>
        /// this thread is used at initialization to synchronize with the other
        /// UDP peer
        /// </summary>
        private void SynchWithOtherPlayer()
        {
            // set up socket for sending synch byte to UDP peer
            // we can't use the same socket (i.e. _dataSocket) in the same thread context in this manner
            // so we need to set up a separate socket here
            Byte[] data = new Byte[1];
            IPEndPoint endPointSend = new IPEndPoint(IPAddress.Parse(_remoteIPAddress), (int)_remotePort);
            IPEndPoint endPointRecieve = new IPEndPoint(IPAddress.Any, 0);

            UdpClient synchSocket = new UdpClient((int)_localPort + 10);

            // set timeout of receive to 1 second
            _dataSocket.Client.ReceiveTimeout = 1000;

            while (true)
            {
                try
                {
                    synchSocket.Send(data, data.Length, endPointSend);
                    _dataSocket.Receive(ref endPointRecieve);

                    // got something, so break out of loop
                    break;
                }
                catch (SocketException ex)
                {
                    // we get an exception if there was a timeout
                    // if we timed out, we just go back and try again
                    if (ex.ErrorCode == (int)SocketError.TimedOut)
                    {
                        Debug.Write(ex.ToString());
                    }
                    else
                    {
                        // we did not time out, but got a really bad 
                        // error
                        synchSocket.Close();
                        StatusTextBox = StatusTextBox + "Socket exception occurred. Unable to sync with other UDP peer.\n";
                        StatusTextBox = StatusTextBox + ex.ToString();
                        return;
                    }
                }
                catch (System.ObjectDisposedException ex)
                {
                    // something bad happened. close the socket and return
                    Console.WriteLine(ex.ToString());
                    synchSocket.Close();
                    StatusTextBox = StatusTextBox + "Error occurred. Unable to sync with other UDP peer.\n";
                    return;
                }
            }

            // send synch byte
            synchSocket.Send(data, data.Length, endPointSend);

            // close the socket we used to send periodic requests to player 2
            synchSocket.Close();

            // reset the timeout for the dataSocket to infinite
            // _dataSocket will be used to recieve data from other UDP peer
            _dataSocket.Client.ReceiveTimeout = 0;

            // start the thread to listen for data from other UDP peer
            ThreadStart threadFunction = new ThreadStart(ReceiveThreadFunction);
            _receiveDataThread = new Thread(threadFunction);
            _receiveDataThread.Start();

            // got this far, so we received a response from player 2
            StatusTextBox = StatusTextBox + DateTime.Now + ":" + " Other UDP peer has joined the session.\n";
            PlayEnabled = true;
            RestartEnabled = true;

            // Determine game order based on the port number
            if (_localPort < _remotePort)
            {
                Status = "You are X, go first";
                XMove = true;
                OMove = false;
            }
            else
            {
                Status = "You are O, wait for X's action";
                XMove = false;
                OMove = false;
            }
        }
    }
}
