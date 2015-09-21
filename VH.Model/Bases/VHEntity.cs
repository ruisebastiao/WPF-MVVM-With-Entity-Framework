using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using VH.Model.Utilities;

namespace VH.Model
{
    public interface IVHEntity : IVHObject
    {
        #region Properties
        int? ID { get; set; }
        DateTime LastChangeDateTime { get; }
        DateTime LastChangeDateTimeUTC { get; set; }
        string LastChangeUser { get; set; }
        DateTime CreationDateTime { get; }
        DateTime CreationDateTimeUTC { get; set; }
        string CreationUser { get; set; }
        Status Status { get; set; }
        bool BeingUsed { get; set; }
        string PopupData { get; set; }
        string PopupText { get; }

        bool IsPendingAdd { get; }
        bool IsPendingDelete { get; }
        bool IsValid { get; }
        AccessType AccessType { get; set; }
        bool IsReadOnly { get; }
        bool IsEditable { get; }
        #endregion

        #region Methods
        void PopulateObjectCommonData<T>(T targetObject) where T : IVHEntity;
        
        TModel CreateModel<TModel, TPrimary>()
            where TModel : IVHModel<TModel, TPrimary>, new()
            where TPrimary : IVHEntity;

        IVHEntity TrimToPSRid();
        void SetToNew();
        #endregion
    }

    [Serializable]
    [DataContract(Name="VHEntity{0}")]
    public abstract class VHEntity<T> : VHObject, IVHEntity, IDataErrorInfo  where T : VHEntity<T>
    {
        #region Fields
        [DataMember]
        protected int? _id = null;
        [DataMember]
        protected DateTime _lastChangeDateTime = DateTime.MinValue;
        [DataMember]
        protected string _lastChangeUser = String.Empty;
        [DataMember]
        protected DateTime _creationDateTime = DateTime.MinValue;
        [DataMember]
        protected string _creationUser = String.Empty;
        [DataMember]
        protected string _creationJob = String.Empty;
        [DataMember]
        protected Status _status ;
        [DataMember]
        protected bool _beingUsed = false;
        [DataMember]
        protected string _popupData = null;
        [DataMember]
        protected AccessType _accessType = AccessType.Default;

        private string _error;

        #endregion

        #region Properties
        [Column("ID")]
        public virtual int? ID
        {
            get { return this._id; }
            set
            {
                bool valueChanged = this.SetProperty("ID", ref this._id, value);
                if (valueChanged)
                {
                    this.NotifyPropertyChanged("IsValid");
                    this.NotifyPropertyChanged("IsPendingAdd");
                    this.NotifyPropertyChanged("IsPendingDelete");
                    this.NotifyPropertyChanged("IsDirty");
                }
            }
        }
        public virtual DateTime LastChangeDateTime
        {
            get { return _lastChangeDateTime.ToLocalTime(); }
        }
        [Column("LASTCHANGEDATETIMEUTC")]
        public virtual DateTime LastChangeDateTimeUTC
        {
            get { return _lastChangeDateTime; }
            set { this.SetProperty("LastChangeDateTimeUTC", ref this._lastChangeDateTime, value); }
        }
        [Column("LASTCHANGEUSER")]
        public virtual string LastChangeUser
        {
            get { return _lastChangeUser; }
            set { this.SetProperty("LastChangeUser", ref this._lastChangeUser, value ?? String.Empty); }
        }
        [NotMapped]
        public virtual DateTime CreationDateTime
        {
            get { return this._creationDateTime.ToLocalTime(); }
        }
        [Column("CREATIONDATETIMEUTC")]
        public virtual DateTime CreationDateTimeUTC
        {
            get { return this._creationDateTime; }
            set { this.SetProperty("CreationDateTimeUTC", ref this._creationDateTime, value); }
        }
        [Column("CREATIONUSER")]
        public virtual string CreationUser
        {
            get { return this._creationUser; }
            set { this.SetProperty("CreationUser", ref this._creationUser, value ?? String.Empty); }
        }

       [Column("STATUS")]
        public virtual Status Status
        {
            get { return this._status; }
            set { this.SetProperty("Status", ref this._status, value); }
        }
        [NotMapped]
        public virtual bool BeingUsed
        {
            get { return (this._beingUsed); }
            set
            {
                bool changed = !Object.Equals(this._beingUsed, value);
                this._beingUsed = value;
                if (changed)
                    this.NotifyPropertyChanged("BeingUsed");
            }
        }
        [NotMapped]
        public virtual string PopupData
        {
            get { return this._popupData; }
            set
            {
                bool changed = !Object.Equals(this._popupData, value);
                this._popupData = value;
                if (changed)
                    this.NotifyPropertyChanged("PopupData");
            }
        }
        [NotMapped]
        public virtual string PopupText
        {
            get
            {
                if (this._popupData != null)
                    return this._popupData;
                else
                    return this.ToString();
            }
        }
        [NotMapped]
        public virtual bool IsPendingAdd
        {
            get
            {
                return ((this.DirtyState == DirtyState.PendingAddChange) &&
                        (!this.ID.HasValue));
            }
        }
        [NotMapped]
        public virtual bool IsPendingDelete
        {
            get
            {
                return (this.DirtyState == DirtyState.PendingDelete);
            }
        }

        [NotMapped]
        public virtual bool IsValid
        {
            get { return (this.ID.HasValue); }
        }
        [NotMapped]
        public virtual AccessType AccessType
        {
            get { return this._accessType; }
            set
            {
                bool changed = !Object.Equals(this._accessType, value);
                this._accessType = value;
                if (changed)
                    this.NotifyPropertyChanged("AccessType");
            }
        }
        [NotMapped]
        public virtual bool IsReadOnly
        {
            get { return (this.AccessType == AccessType.ReadOnly); }
        }
        [NotMapped]
        public virtual bool IsEditable
        {
            get { return !this.IsReadOnly; }
        }

        [NotMapped]
        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                this.NotifyPropertyChanged("Error");
            }
        }

        #endregion

        #region Constructors
        protected VHEntity(bool propertyChangeEventDisabled) : base(propertyChangeEventDisabled) { }
        protected VHEntity() : this(false)
        {
            this.ApplyCurrentDateTime();
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Public Methods
        public  T Clone()
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(this, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
        public virtual void PopulateObjectCommonData<Target>(Target targetObject)
            where Target : IVHEntity
        {
            if (targetObject != null)
            {
                targetObject.PropertyChangeEventDisabled = true;
                {
                    targetObject.ID = this.ID;
                    targetObject.LastChangeDateTimeUTC = this.LastChangeDateTimeUTC;
                    targetObject.LastChangeUser = this.LastChangeUser;
                    targetObject.CreationDateTimeUTC = this.CreationDateTimeUTC;
                    targetObject.CreationUser = this.CreationUser;
                    targetObject.Status = this.Status;

                    targetObject.UniqueId = this.UniqueId;
                    targetObject.AccessType = this.AccessType;
                    targetObject.BeingUsed = this.BeingUsed;
                    targetObject.PopupData = this.PopupData;
                    targetObject.DirtyState = this.DirtyState;
                }
                targetObject.PropertyChangeEventDisabled = this._propertyChangeEventDisabled;
            }
        }
        public virtual T CreateAClone()
        {
            return base.CreateAClone<T>();
        }
        public virtual void SetToNew()
        {
            this.UniqueId = Helper.GetUniqueId();
            this.ID = null;
        }

        public TModel CreateModel<TModel, TPrimary>()
            where TModel : IVHModel<TModel, TPrimary>, new()
            where TPrimary : IVHEntity
        {
            // All TModel (e.g. CommonPersonModel) must have valid constructor (TPrimary primaryEntity) as Activator expectation            
            TModel result = (TModel)Activator.CreateInstance(typeof(TModel), new object[] { this });
            return result;
        }

        public virtual IVHEntity TrimToPSRid()
        {
            IVHEntity entity = (IVHEntity)Activator.CreateInstance<T>();
            entity.PropertyChangeEventDisabled = true;
            
            entity.ID = this.ID;
            entity.DirtyState = this.DirtyState;
            
            entity.PropertyChangeEventDisabled = this.PropertyChangeEventDisabled;

            return entity;
        }

        public virtual void ApplyCurrentDateTime()
        {
            this.LastChangeDateTimeUTC = DateTime.Now;
            this.CreationDateTimeUTC = DateTime.Now;
            this.CreationUser = "Admin";
            this.LastChangeUser = "Admin";
        }
        #endregion

        #region Overridden Methods
        public override bool Equals(object obj)
        {
            return this.Equals(obj as T);
        }
        public virtual bool Equals(T obj)
        {
            // If both have PSRid value then compare them
            if (this.IsValid && obj != null && obj.IsValid)
                return Object.Equals(this.ID, obj.ID);
            
            // Else have the base return base class equals
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            if (this.IsValid)
                return this.ID.Value;
            else
                return base.GetHashCode();
        }
        #endregion

        public virtual string this[string columnName]
        {
            get { return ""; }
        }
    }
}
