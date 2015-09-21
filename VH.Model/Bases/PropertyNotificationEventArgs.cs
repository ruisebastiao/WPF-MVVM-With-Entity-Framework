using System.ComponentModel;

namespace VH.Model
{
    public class PropertyNotificationEventArgs : PropertyChangedEventArgs
    {
        #region Fields
        private object _oldValue = null;
        private object _newValue = null;
        #endregion

        #region Constructors
        public PropertyNotificationEventArgs(string propertyName)
            : this(propertyName, null, null) { }

        public PropertyNotificationEventArgs(string propertyName, object oldValue, object newValue)
            : base(propertyName)
        {
            this._oldValue = oldValue;
            this._newValue = newValue;
        }
        #endregion

        #region Properties
        public object NewValue
        {
            get { return this._newValue; }
        }
        public object OldValue
        {
            get { return this._oldValue; }
        }
        #endregion
    }
}
