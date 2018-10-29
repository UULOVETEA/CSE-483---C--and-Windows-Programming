using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// INotifyPropertyChanged
using System.ComponentModel;


namespace SimpleEvents
{
    partial class Model : INotifyPropertyChanged
    {
        private string _publisherText;
        public string PublisherText
        {
            get { return _publisherText; }
            set
            {
                _publisherText = value;
                OnPropertyChanged("PublisherText");
            }
        }


        private string _subscriber1Text;
        public string Subscriber1Text
        {
            get { return _subscriber1Text; }
            set
            {
                _subscriber1Text = value;
                OnPropertyChanged("Subscriber1Text");
            }
        }

        private string _subscriber1Data;
        public string Subscriber1Data
        {
            get { return _subscriber1Data; }
            set
            {
                _subscriber1Data = value;
                OnPropertyChanged("Subscriber1Data");
            }
        }


        private string _subscriber2Text;
        public string Subscriber2Text
        {
            get { return _subscriber2Text; }
            set
            {
                _subscriber2Text = value;
                OnPropertyChanged("Subscriber2Text");
            }
        }

        private string _subscriber2Data;
        public string Subscriber2Data
        {
            get { return _subscriber2Data; }
            set
            {
                _subscriber2Data = value;
                OnPropertyChanged("Subscriber2Data");
            }
        }

        private string _subscriber3Text;
        public string Subscriber3Text
        {
            get { return _subscriber3Text; }
            set
            {
                _subscriber3Text = value;
                OnPropertyChanged("Subscriber3Text");
            }
        }

        private string _subscriber3Data;
        public string Subscriber3Data
        {
            get { return _subscriber3Data; }
            set
            {
                _subscriber3Data = value;
                OnPropertyChanged("Subscriber3Data");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
