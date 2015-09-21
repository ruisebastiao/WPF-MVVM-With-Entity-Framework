using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.ServiceModel;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VH.Model.Utilities;


namespace VH.Model
{
    public interface IVHObject : INotifyPropertyChanged
    {
        #region Properties
        string UniqueId { get; set; }
        bool PropertyChangeEventDisabled { get; set; }

        DirtyState DirtyState { get; set; }
        bool IsDirty { get; }
       // Status PSStatus { get; set; }
        object ClientTag { get; set; }
        #endregion

        #region Methods
        bool SerializeToFile();
        bool SerializeToFile(string outputDirectory);
        string SerializeToXml();
        MemoryStream SerializeToMemoryStream();

        T CreateAClone<T>();
        void MakeClean();

        void NotifyPropertyChanged(string propertyName);
        #endregion
    }

    [Serializable]
    [DataContract]
    public abstract class VHObject : IVHObject
    {
        #region Fields
        [DataMember]
        private string _uniqueId = String.Empty;

        // Do NOT mark this for WCF serialization - bool is False by default.
        protected bool _propertyChangeEventDisabled = false;
        // Do NOT mark this for WCF serialization
        protected object _entityLocker = new object();

        protected object _tag = null;
        private DirtyState _dirtyState;

        #endregion

        #region Properties
        [NotMapped]
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this._uniqueId = value; }
        }
        [NotMapped]
        public virtual bool PropertyChangeEventDisabled
        {
            get { return this._propertyChangeEventDisabled; }
            set { this._propertyChangeEventDisabled = value; }
        }
        [NotMapped]
        protected bool HasPropertyChangeListener
        {
            get { return (this.PropertyChanged != null); }
        }
       
        [NotMapped]
        public virtual DirtyState DirtyState
        {
            get { return this._dirtyState; }
            set
            {
                bool valueChanged = !(this._dirtyState == value);
                this._dirtyState = value;
                if (valueChanged)
                {
                    this.NotifyPropertyChanged("DirtyState");
                    this.NotifyPropertyChanged("IsPendingAdd");
                    this.NotifyPropertyChanged("IsPendingDelete");
                    this.NotifyPropertyChanged("IsDirty");
                }
            }
        }
        [NotMapped]
        public virtual object ClientTag
        {
            get { return _tag; }
            set
            {
                _tag = value;
                this.NotifyPropertyChanged("ClientTag");
            }
        }

        public bool SerializeToFile()
        {
            throw new NotImplementedException();
        }

        public bool SerializeToFile(string outputDirectory)
        {
            throw new NotImplementedException();
        }

        public string SerializeToXml()
        {
            throw new NotImplementedException();
        }

        public MemoryStream SerializeToMemoryStream()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Constructor
        protected VHObject() : this(false) { }
        protected VHObject(bool propertyChangeEventDisabled)
            : base()
        {
            this._uniqueId = Helper.GetUniqueId();
            this._propertyChangeEventDisabled = propertyChangeEventDisabled;
        }
        #endregion

        #region Overrides
        protected virtual bool Equals(VHObject obj)
        {
            if (obj != null)
                return (String.Equals(this.UniqueId, obj.UniqueId));
            else
                return base.Equals(obj);
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj as VHObject);
        }
        public override int GetHashCode()
        {
            if (String.IsNullOrEmpty(this.UniqueId))
                return Int32.MinValue;
            else
                return this.UniqueId.GetHashCode();
        }
        #endregion

        #region IPSObject Members
        //public bool SerializeToFile()
        //{
        //    return PSObjectExtensions.SerializeToXmlFile(this);
        //}
        //public bool SerializeToFile(string outputDirectory)
        //{
        //    string fileName = this.GetType().GetObjectName();

        //    return PSObjectExtensions.SerializeToXmlFile(this, fileName, outputDirectory);
        //}
        //public virtual string SerializeToXml()
        //{
        //    return PSObjectExtensions.SerializeToString(this);
        //}
        //public virtual MemoryStream SerializeToMemoryStream()
        //{
        //    return PSObjectExtensions.SerializeToMemoryStream(this);
        //}

        public virtual T CreateAClone<T>()
        {
            T clonedObject = default(T);

            string objectContent = Serializer.SerializeToString(this);
            clonedObject = Serializer.DeserializeFromString<T>(objectContent);

            return clonedObject;
        }
        public virtual void MakeClean()
        {
            this._dirtyState = DirtyState.UnChanged;
        }
        #endregion

        #region Protected Methods
        protected virtual bool SetProperty<PT>(string propertyName, ref PT propertyField, PT newValue)
        {
            // Make the actually property change now
            PT oldValue = propertyField;
            propertyField = newValue;

            bool valuesDifferent = !Object.Equals(oldValue, newValue);

            // If different and either _propertyChangeEventDisabled is NOT disabled or there is a PropertyChanged listener
            if (valuesDifferent && (!this.PropertyChangeEventDisabled || this.HasPropertyChangeListener))
            {
                // If the owner of this property is not dirty make it dirty
                if (!this.PropertyChangeEventDisabled && (this.DirtyState == DirtyState.UnChanged))
                    this.DirtyState = DirtyState.PendingAddChange;

                // Call any additional OnPropertyChanged Listeners now
                this.OnPropertyChanged(propertyName, oldValue, propertyField);
            }

            return valuesDifferent;
        }
        #endregion

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.OnPropertyChanged(propertyName);
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs propertyChangedEventArgs = new PropertyChangedEventArgs(propertyName);
            this.OnPropertyChanged(propertyChangedEventArgs);
        }
        protected virtual void OnPropertyChanged(string propertyName, object oldValue, object newValue)
        {
            PropertyNotificationEventArgs propertyNotificationEventArgs = new PropertyNotificationEventArgs(propertyName, oldValue, newValue);
            this.OnPropertyChanged(propertyNotificationEventArgs);
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs propertyChangedEventArgs)
        {
            // Run any additional PropertyChanged event
            PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
            if (propertyChangedEventHandler != null)
                propertyChangedEventHandler(this, propertyChangedEventArgs);
        }
        #endregion

        #region IValidateToNavigate Members
        public virtual bool IsDirty
        {
            get { return (this.DirtyState != DirtyState.UnChanged); }
        }

       // public Status PSStatus { get; set; }

        #endregion

        #region Factory Members
        public static VHObject Create(string name)
        {
            Type type = Type.GetType(name, false, true);
            if (type == null)
                return null;
            return Activator.CreateInstance(type) as VHObject;
        }

        public static IVHEntity CreateEntity(string name, int? id)
        {
            Type type = Type.GetType(name, false, true);
            if (type == null)
                return null;
            var entity = Activator.CreateInstance(type) as IVHEntity;
            if (entity == null)
                return null;
            entity.ID = id;
            return entity;
        }
        #endregion
    }
}