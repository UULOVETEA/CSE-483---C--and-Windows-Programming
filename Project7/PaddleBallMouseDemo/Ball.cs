using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// INotifyPropertyChanged
using System.ComponentModel;

// Brushes
using System.Windows.Media;


namespace PaddleBallMouseDemo
{
    public partial class Model : INotifyPropertyChanged
    {
        private double _ballCanvasTop;
        public double ballCanvasTop
        {
            get { return _ballCanvasTop; }
            set
            {
                _ballCanvasTop = value;
                OnPropertyChanged("ballCanvasTop");
            }
        }

        private double _ballCanvasLeft;
        public double ballCanvasLeft
        {
            get { return _ballCanvasLeft; }
            set
            {
                _ballCanvasLeft = value;
                OnPropertyChanged("ballCanvasLeft");
            }
        }

        private double _ballHeight;
        public double ballHeight
        {
            get { return _ballHeight; }
            set
            {
                _ballHeight = value;
                OnPropertyChanged("ballHeight");
            }
        }

        private double _ballWidth;
        public double ballWidth
        {
            get { return _ballWidth; }
            set
            {
                _ballWidth = value;
                OnPropertyChanged("ballWidth");
            }
        }
    }
}
