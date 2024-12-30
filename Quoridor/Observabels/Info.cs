using Quoridor.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Observabels
{
    internal class Info : INotifyPropertyChanged
    {
        private string _message;
        private Color _turn;
        private int _redwallsLeft;
        private int _bluewallsLeft;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int redwallsLeft
        {
            get => _redwallsLeft;
            set
            {
                if (_redwallsLeft != value)
                {
                    _redwallsLeft = value;
                    OnPropertyChanged(nameof(redwallsLeft));
                }
            }
        }

        public int bluewallsLeft
        {
            get => _bluewallsLeft;
            set
            {
                if (_bluewallsLeft != value)
                {
                    _bluewallsLeft = value;
                    OnPropertyChanged(nameof(bluewallsLeft));
                }
            }
        }

        public Color turn
        {
            get => _turn;
            set
            {
                if (_turn != value)
                {
                    _turn = value;
                    OnPropertyChanged(nameof(turn));
                }
            }
        }

        public string message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged(nameof(message));
                }
            }
        }
    
        
        public void SwitchTurn()
        {
            this.turn = this.turn == Color.Blue ? Color.Red : Color.Blue;
        }
    }
}
