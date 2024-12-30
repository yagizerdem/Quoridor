using Quoridor.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Observabels
{
    internal class Player : INotifyPropertyChanged
    {
        private int _row;
        private int _col;
        private Color _playerColor;

        public event PropertyChangedEventHandler PropertyChanged;

        public Color playerColor
        {
            get => _playerColor;
            set
            {
                if (_playerColor != value)
                {
                    _playerColor = value;
                    OnPropertyChanged(nameof(playerColor));
                }
            }
        }

        public int row
        {
            get => _row;
            set
            {
                if (_row != value)
                {
                    _row = value;
                    OnPropertyChanged(nameof(row));
                }
            }
        }

        public int col
        {
            get => _col;
            set
            {
                if (_col != value)
                {
                    _col = value;
                    OnPropertyChanged(nameof(col));
                }
            }
        }

        public char symbol => this.playerColor == Color.Blue ? 'B' : 'R';
        public Player(Color playerColor, int row, int col)
        {
            this.row = row;
            this.col = col;
            this.playerColor = playerColor;
        }

        public Player(Color playerColor)
        {
            this.playerColor = playerColor;
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
