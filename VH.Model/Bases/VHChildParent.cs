using System.Runtime.Serialization;

namespace VH.Model
{
    public interface IVHChildParent<TParent> : IVHEntity
        where TParent : IVHEntity
    {
        #region Properties
        TParent ParentEntity { get; set; }
        #endregion

        #region Methods
        void UpdateParent(TParent parentEntity);
        void UpdateParent(TParent parentEntity, bool retainDirtyState);

        void SetToNew(bool setParentEntityToNew);
        #endregion
    }

    [DataContract(Name="VHChildParent{0}{1}")]
    public class VHChildParent<T, TParent> : VHEntity<T>, IVHChildParent<TParent>
        where T : VHEntity<T>
        where TParent : IVHEntity
    {
        #region Fields
        [DataMember]
        protected TParent _parentEntity = default(TParent);
        #endregion

        #region Properties
        public TParent ParentEntity
        {
            get { return (this._parentEntity); }
            set { this.SetProperty("ParentEntity", ref this._parentEntity, value); }
        }
        #endregion

        #region Constructors
        public VHChildParent() : this(default(TParent)) { }
        public VHChildParent(TParent parentEntity)
        {
            this._parentEntity = parentEntity;
        }
        public VHChildParent(bool propertyChangeEventDisabled) : base(propertyChangeEventDisabled) { }
        #endregion

        #region Overrides
        public override void SetToNew()
        {
            this.SetToNew(false);
        }
        #endregion

        #region Public Methods
        public void UpdateParent(TParent parentEntity)
        {
            this.UpdateParent(parentEntity, true);
        }
        public void UpdateParent(TParent parentEntity, bool retainDirtyState)
        {
            if (retainDirtyState)
                this._parentEntity = parentEntity;
            else
                this.ParentEntity = parentEntity;
        }

        public virtual void SetToNew(bool setParentEntityToNew)
        {
            base.SetToNew();

            if ((setParentEntityToNew) && (this.ParentEntity != null))
                this.ParentEntity.SetToNew();
        }
        #endregion
    }
}