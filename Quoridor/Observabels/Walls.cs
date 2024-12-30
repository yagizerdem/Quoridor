using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quoridor.Observabels
{
    internal class Walls : INotifyPropertyChanged
    {
        private readonly HashSet<string> _hashSet = new HashSet<string>();

        public event PropertyChangedEventHandler? PropertyChanged;

        // Add an item and trigger PropertyChanged
        public bool Add(string item)
        {
            bool added = _hashSet.Add(item);
            if (added)
            {
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged("ItemAdded");
            }
            return added;
        }

        // Remove an item and trigger PropertyChanged
        public bool Remove(string item)
        {
            bool removed = _hashSet.Remove(item);
            if (removed)
            {
                OnPropertyChanged(nameof(Count));
                OnPropertyChanged("ItemRemoved");
            }
            return removed;
        }

        // Expose the Count property
        public int Count => _hashSet.Count;

        // Check if an item exists
        public bool Contains(string item) => _hashSet.Contains(item);

        public IEnumerable<string> Iterator()
        {
            foreach (string value in this._hashSet)
            {
                yield return value;
            }
        }

        // Raise the PropertyChanged event
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public HashSet<string> getHashSet ()=> this._hashSet;
    
        
    }
}
