using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;

///
/// Program Name: Tic Tac Toe-Network
/// Author: Xiaomeng Cao
/// Date: April 29, 2017
/// Course: CSE-483
///

namespace TicTacToe_Network
{
    partial class Model : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private String _status = "";
        public String Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        private Brush _labelColor;
        public Brush LabelColor
        {
            get { return _labelColor; }
            set
            {
                _labelColor = value;
                OnPropertyChanged("LabelColor");
            }
        }

        private String _meBox;
        public String MeBox
        {
            get { return _meBox; }
            set
            {
                _meBox = value;
                OnPropertyChanged("MeBox");
            }
        }

        private String _myFriendBox;
        public String MyFriendBox
        {
            get { return _myFriendBox; }
            set
            {
                _myFriendBox = value;
                OnPropertyChanged("MyFriendBox");
            }

        }
        private String _statusTextBox;
        public String StatusTextBox
        {
            get { return _statusTextBox; }
            set
            {
                _statusTextBox = value;
                OnPropertyChanged("StatusTextBox");
            }
        }

        private String _data1;
        public String Data1
        {
            get { return _data1; }
            set
            {
                _data1 = value;
                OnPropertyChanged("Data1");
            }
        }

        private String _data2;
        public String Data2
        {
            get { return _data2; }
            set
            {
                _data2 = value;
                OnPropertyChanged("Data2");
            }
        }

        private bool _playEnabled;
        public bool PlayEnabled
        {
            get { return _playEnabled; }
            set
            {
                _playEnabled = value;
                OnPropertyChanged("PlayEnabled");
            }
        }

        private bool _restartEnabled;
        public bool RestartEnabled
        {
            get { return _restartEnabled; }
            set
            {
                _restartEnabled = value;
                OnPropertyChanged("RestartEnabled");
            }
        }
    }
}
