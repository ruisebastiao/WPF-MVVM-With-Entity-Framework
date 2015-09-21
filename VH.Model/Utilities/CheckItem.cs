using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VH.Model.Properties;

namespace VH.Model.Utilities
{
    public class CheckItem : INotifyPropertyChanged
    {
        private string _itemName;
        private bool _isChecked;

        public string ItemName
        {
            get { return _itemName; }
            set
            {
                _itemName = value;
                this.OnPropertyChanged("ItemName");
            }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                this.OnPropertyChanged("IsChecked");
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }

    public class CheckItemCollection : ObservableCollection<CheckItem>
    {
        
    }
}