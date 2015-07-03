using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Vélobster.Util
{
    public class ObservableList<T> : ObservableCollection<T>
    {
        public void AddRange(IEnumerable<T> range)
        {
            CheckReentrancy();
            var startIndex = Count;
            foreach (var item in range)
                Items.Add(item);
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, range.ToList(), startIndex));
        }
    }
}
