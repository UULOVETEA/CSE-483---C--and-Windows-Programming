using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

///
/// Program Name: TicTacToe
/// Author: Xiaomeng Cao
/// Date: March 9, 2017
/// Course: CSE-483
///

namespace TicTacToe
{
    class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableCollection<Tile> TileCollection;
        private static UInt32 _numTiles = 9;
        int count = 0;

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

        public Model()
        {
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
            Status = "We have a winner!";
            // change the label color to lime
            LabelColor = Brushes.Lime;
            // make all buttons disable
            for (int x = 0; x < _numTiles; x++)
            {
                TileCollection[x].isSet = true;
            }
        }

        public void UserSelection(String buttonSelected)
        {
            // get the number which button the user clicked
            int index = int.Parse(buttonSelected);
            // increase count in order to determing is X turn or O turn
            count++;

            if (Status == "We have a winner!" || Status == "Game Over. Restart to play again.")
            {
                Status = "Game Over. Restart to play again.";
                return;
            }
            if (Status == "We have a tie!" || Status == "Game Over. Restart to play again.")
            {
                Status = "Game Over. Restart to play again.";
                return;
            }

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
            if (count%2 != 0)
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
            // else count is even
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
        }
    }
}
