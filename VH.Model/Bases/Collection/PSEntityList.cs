using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;
using VH.Model.Utilities;

namespace VH.Model
{
    public interface IVHEntityList<T> : IVHObject, IListPaging  where T : IVHEntity
    {
        #region Properties
        List<T> InternalList { get; set; }
        MTObservableCollection<T> ObservableList { get; set; }
        T ActiveItem { get; set; }
        bool ActiveItemIsDirty { get; }

        event EventHandler ActiveItemChanged;

        event EventHandler SelectedItemsChanged;
        MTObservableCollection<T> SelectedItems { get; set; }
        #endregion

        #region Methods (Mimics IList<T>)
        bool HasItem { get; }
        int Count { get; }
        T this[int index] { get; set; }
        bool IsInternalListDirty { get; }

        ReadOnlyCollection<T> AsReadOnly();
        T Find(Predicate<T> match);
        List<T> FindAll(Predicate<T> match);
        bool Append(VHEntityList<T> additionalList);
        void Clear();
        bool Remove(T item);
        bool Contains(T item);
        int IndexOf(T item);
        void Insert(int index, T item);
        void RemoveAt(int index);
        void Add(T item);
        void CopyTo(T[] array, int arrayIndex);
        IEnumerator<T> GetEnumerator();

        List<int> GetPSRids();
        T GetEntity(T entity);
        T GetEntityByPSRid(int psRid);
        TList GetDirtyEntities<TList>() where TList : IVHEntityList<T>, new();
        bool UpdateEntityInstance<IT>(IT newEntityInstance, bool ignoreDirtyFlag) where IT : T;
        #endregion Methods (Mimics IList<T>)

        #region Methods (Mimics IList<T>) for ObservableList
        bool HasItemInObservableList { get; }
        int CountFromObservableList { get; }
        bool IsObservableListDirty { get; }

        void ClearObservableList();
        bool RemoveFromObservableList(T item);
        bool ContainsInObservableList(T item);
        int IndexOfWithinObservableList(T item);
        void InsertIntoObservableList(int index, T item);
        void RemoveAtInObservableList(int index);
        void AddToObservableList(T item);
        void CopyToFromObservableList(T[] array, int arrayIndex);
        IEnumerator<T> GetEnumeratorFromObservableList();

        List<int> GetPSRidsFromObservableList();

        T GetEntityFromObservableList(T entity);
        T GetEntityByPSRidFromObservableList(int psRid);

        IVHEntityList<T> ReplicateFromObservableList();
        List<T> GetReplicatedInternalListFromObservableList();
        bool UpdateEntityInstanceWithinObservableList<IT>(IT newEntityInstance, bool ignoreDirtyFlag) where IT : T;
        TList GetDirtyEntitiesFromObservableList<TList>() where TList : IVHEntityList<T>, new();
        TList GetInvalidEntitiesFromObservableList<TList>() where TList : IVHEntityList<T>, new();

        bool UndoDeleteToObservableList(T entity);
        #endregion Methods (Mimics IList<T>) for ObservableList

        #region Methods
        void MakeObservableListClean();

        void ReplaceInternalListWithObservableList();
        void ReplaceObservableListWithInternalList();

        //TModelList CreateModelListFromInternalList<TModelList, TModel, TList>()
        //    where TModelList : IVHModelList<TModel, TList, T>, new()
        //    where TModel : IPSModel<TModel, T>, new()
        //    where TList : IPSEntityList<T>, new();
        //TModelList CreateModelListFromObservableList<TModelList, TModel, TList>()
        //    where TModelList : IPSModelList<TModel, TList, T>, new()
        //    where TModel : IPSModel<TModel, T>, new()
        //    where TList : IPSEntityList<T>, new();

        TList CreateACloneFromObservableList<TList>()
            where TList : IVHEntityList<T>, new();

        void SetToNewForInternalList();
        void SetToNewForObservableList();

        TList GetSelectedItems<TList>()
            where TList : IVHEntityList<T>, new();
        void ReplaceInstance(T oldValue, T newValue);

        #endregion
    }

    /// <remarks>
    /// Must use Serializable if NetDataContractSerializer is applied
    ///     in order to transfer internal members (e.g. private, protected,...) 
    ///     over the wire.
    /// Otherwise, DataContract/CollectionDataContract should be used
    ///     depends on the implementation of the class (implemented IList vs derived of List)
    /// </remarks>
    //[Serializable]
    [DataContract(Name = "VHEntityList{0}")]
    public class VHEntityList<T> : VHObject, IVHEntityList<T>  where T : IVHEntity
    {
        #region Fields
        [DataMember]
        protected int _currentPageNumber = 0;
        [DataMember]
        protected int _numberOfRemainingPage = 0;
        [DataMember]
        protected List<T> _internalList = new List<T>();

        // Do not mark this for WCF serialization - _internalList will hold the items
        protected MTObservableCollection<T> _observableList = null;
        // Do not mark this serialize this - _internalList will hold the items
        protected MTObservableCollection<T> _pendingDeletedItems = null;
        #endregion

        #region Properties
        public int CurrentPageNumber
        {
            get { return this._currentPageNumber; }
            set { this._currentPageNumber = value; }
        }
        public int NumberOfRemainingPage
        {
            get { return this._numberOfRemainingPage; }
            set { this._numberOfRemainingPage = value; }
        }
        public List<T> InternalList
        {
            get
            {
                this._internalList = this._internalList ?? new List<T>();
                return this._internalList;
            }
            set
            {
                if (value == null)
                    this._internalList.Clear();
                else
                    this._internalList = value;
            }
        }
        public bool IsPagingAvailable
        {
            get { return (this._numberOfRemainingPage > 0); }
        }
        public int NextPageNumber
        {
            get { return this._currentPageNumber + 1; }
        }
        public MTObservableCollection<T> ObservableList
        {
            get
            {
                if (this._observableList == null)
                    this.ObservableList = new MTObservableCollection<T>(this._internalList);

                return this._observableList;
            }
            set
            {
                if (this._observableList != null)
                    this._observableList.CollectionChanged -= (NotifyCollectionChangedEventHandler)this.OnObservableListCollectionChanged;

                this._observableList = value;

                if (this._observableList != null)
                    this._observableList.CollectionChanged += (NotifyCollectionChangedEventHandler)this.OnObservableListCollectionChanged;
            }
        }
        public MTObservableCollection<T> PendingDeletedItems
        {
            get
            {
                if (this._pendingDeletedItems == null)
                    this._pendingDeletedItems = new MTObservableCollection<T>();

                return this._pendingDeletedItems;
            }
        }

        private T _activeItem = default(T);
        public T ActiveItem
        {
            get { return this._activeItem; }
            set
            {
                this._activeItem = value;
                this.NotifyPropertyChanged("ActiveItem");
                this.NotifyPropertyChanged("ActiveItemIsDirty");
                this.RaiseActiveItemChanged();
            }
        }
        public bool ActiveItemIsDirty
        {
            get
            {
                bool isDirty = false;
                if (!isDirty && this.ActiveItem != null)
                    isDirty = this.ActiveItem.IsDirty;

                return isDirty;
            }
        }
        public event EventHandler ActiveItemChanged;

        protected virtual void OnActiveItemChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var eh = this.ActiveItemChanged;
            if (eh != null)
            {
                Dispatcher dispatcher = (from EventHandler nh in eh.GetInvocationList()
                                         let dpo = nh.Target as DispatcherObject
                                         where dpo != null
                                         select dpo.Dispatcher).FirstOrDefault();

                if (dispatcher != null && dispatcher.CheckAccess() == false)
                {
                    dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => this.OnActiveItemChanged(sender, e)));
                }
                else
                {
                    foreach (EventHandler nh in eh.GetInvocationList())
                        nh.Invoke(this, EventArgs.Empty);
                }
            }
        }
        protected virtual void RaiseActiveItemChanged()
        {
            this.OnActiveItemChanged(this, null);
        }

        private MTObservableCollection<T> _selectedItems = null;
        public MTObservableCollection<T> SelectedItems
        {
            get
            {
                if (this._selectedItems == null)
                    this.SelectedItems = new MTObservableCollection<T>();

                return this._selectedItems;
            }
            set
            {
                if (this._selectedItems != null)
                    this.SelectedItems.CollectionChanged -= this.OnSelectedItemsChanged;

                this._selectedItems = value;
                this.NotifyPropertyChanged("SelectedItems");

                if (this._selectedItems != null)
                    this.SelectedItems.CollectionChanged += this.OnSelectedItemsChanged;
            }
        }
        public event EventHandler SelectedItemsChanged;

        protected virtual void OnSelectedItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var eh = this.SelectedItemsChanged;
            if (eh != null)
            {
                Dispatcher dispatcher = (from EventHandler nh in eh.GetInvocationList()
                                         let dpo = nh.Target as DispatcherObject
                                         where dpo != null
                                         select dpo.Dispatcher).FirstOrDefault();

                if (dispatcher != null && dispatcher.CheckAccess() == false)
                {
                    dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => this.OnSelectedItemsChanged(sender, e)));
                }
                else
                {
                    foreach (EventHandler nh in eh.GetInvocationList())
                        nh.Invoke(this, EventArgs.Empty);
                }
            }
        }
        protected virtual void RaiseSelectedItemsChanged()
        {
            this.OnSelectedItemsChanged(this, null);
        }
        #endregion

        #region Constructors
        protected VHEntityList() : base() { }
        protected VHEntityList(List<T> internalList)
            : this()
        {
            if (internalList != null)
                this._internalList = internalList;
        }
        #endregion

        #region Methods (Mimics IList<T>)
        public virtual bool HasItem
        {
            get { return (this.Count > 0); }
        }
        public virtual int Count
        {
            get
            {
                int items = 0;
                if (this._internalList != null)
                    items += this._internalList.Count;

                return items;
            }
        }
        public virtual T this[int index]
        {
            get { return this.InternalList[index]; }
            set { this.InternalList[index] = value; }
        }
        public virtual bool IsInternalListDirty
        {
            get
            {
                bool isDirty = false;

                isDirty = (this.PendingDeletedItems.Count > 0);

                if (!isDirty)
                {
                    foreach (T item in this.InternalList)
                    {
                        if (item != null)
                        {
                            isDirty = item.IsDirty;
                            if (isDirty)
                                break;
                        }
                    }
                }

                return isDirty;
            }
        }
        public virtual bool HasValidItem
        {
            get
            {
                if (!this.HasItem)
                    return false;

                var entities = from item in this.InternalList
                               where item != null && (item.IsPendingAdd || (item.IsValid && !item.IsPendingDelete))
                               select item;

                return (entities.Count() > 0);
            }
        }

        public virtual ReadOnlyCollection<T> AsReadOnly()
        {
            return this.InternalList.AsReadOnly();
        }
        public virtual T Find(Predicate<T> match)
        {
            return this.InternalList.Find(match);
        }
        public virtual List<T> FindAll(Predicate<T> match)
        {
            return this.InternalList.FindAll(match);
        }
        public virtual bool Append(VHEntityList<T> additionalList)
        {
            bool appendedResult = false;

            if (additionalList.Count > 0)
            {
                this.InternalList.AddRange(additionalList.InternalList);
                appendedResult = true;
            }

            this._currentPageNumber = additionalList.CurrentPageNumber;
            this._numberOfRemainingPage = additionalList.NumberOfRemainingPage;

            return appendedResult;
        }
        public virtual void Clear()
        {
            this._currentPageNumber = 0;
            this._numberOfRemainingPage = 0;
            this._internalList.Clear();
            if (this._observableList != null)
                this._observableList.Clear();
            if (this._pendingDeletedItems != null)
                this._pendingDeletedItems.Clear();
        }
        public virtual bool Remove(T item)
        {
            bool result = true;
            int itemIndex = -1;

            itemIndex = this.IndexOf(item);
            if (itemIndex > -1)
                this.RemoveAt(itemIndex);

            return result;
        }
        public virtual bool Contains(T item)
        {
            return this.InternalList.Exists(internalItem => internalItem.Equals(item));
        }
        public virtual int IndexOf(T item)
        {
            int itemIndex = this.InternalList.IndexOf(item);

            if (itemIndex < 0)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (Object.Equals(this[i], item))
                    {
                        itemIndex = i;
                        break;
                    }
                }
            }

            return itemIndex;
        }
        public virtual void Insert(int index, T item)
        {
            this.InternalList.Insert(index, item);
        }
        public virtual void RemoveAt(int index)
        {
            this.InternalList.RemoveAt(index);
        }
        public virtual void Add(T item)
        {
            this.InternalList.Add(item);
        }
        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            this.InternalList.CopyTo(array, arrayIndex);
        }
        public virtual IEnumerator<T> GetEnumerator()
        {
            return this.InternalList.GetEnumerator();
        }

        public virtual List<int> GetPSRids()
        {
            List<int> results = new List<int>();

            if (this.HasItem)
            {
                IEnumerable<int> query = from entity in this._internalList
                                         where entity != null && entity.IsValid
                                         select entity.ID.Value;

                if (query.Count<int>() > 0)
                    results.AddRange(query);
            }

            return results;
        }
        public virtual string[] GetPSRidsAsArrayOfStrings()
        {
            if (this.HasItem)
            {
                var query = from entity in this._internalList
                            where entity != null && entity.IsValid
                            select entity.ID.Value.ToString();

                return query.ToArray();
            }

            return new string[0];
        }

        public virtual T GetEntity(T entity, T defaultEntity)
        {
            T result = GetEntity(entity);
            if (result != null)
                return result;
            else
                return defaultEntity;
        }
        public virtual T GetEntity(T entity)
        {
            T result = default(T);

            if (this.HasItem)
                result = this._internalList.FirstOrDefault(item => item.Equals(entity));

            return result;
        }
        public virtual T GetEntityByPSRid(int psRid)
        {
            T result = default(T);

            if (this.HasItem)
            {
                var query = from entity in this._internalList
                            where entity != null && entity.ID == psRid
                            select entity;

                result = query.FirstOrDefault<T>();
            }

            return result;
        }

        public virtual TList GetDirtyEntities<TList>()
            where TList : IVHEntityList<T>, new()
        {
            TList result = new TList();

            if (this.HasItem)
            {
                IEnumerable<T> query = from entity in this._internalList
                                       where entity != null && entity.IsDirty
                                       select entity.CreateAClone<T>();

                result.InternalList.AddRange(query);
            }

            return result;
        }
        public virtual bool UpdateEntityInstance<IT>(IT newEntityInstance, bool ignoreDirtyFlag)
            where IT : T
        {
            bool result = false;

            int indexOfExistingEntity = this.IndexOf(newEntityInstance);
            if (indexOfExistingEntity > -1)
            {
                if (ignoreDirtyFlag)
                {
                    this.InternalList[indexOfExistingEntity] = newEntityInstance;
                }
                else
                {
                    // Replace instance only if it is not dirty
                    if (!this.InternalList[indexOfExistingEntity].IsDirty)
                        this.InternalList[indexOfExistingEntity] = newEntityInstance;
                }
                result = true;
            }

            return result;
        }
        #endregion Methods (Mimics IList<T>)

        #region Methods (Mimics IList<T>) for ObservableList
        public virtual bool HasItemInObservableList
        {
            get { return (this.CountFromObservableList > 0); }
        }
        public virtual int CountFromObservableList
        {
            get
            {
                int items = 0;
                if (this._observableList != null)
                    items += this._observableList.Count;

                if (this._pendingDeletedItems != null)
                    items += this._pendingDeletedItems.Count;

                return items;
            }
        }
        public virtual bool IsObservableListDirty
        {
            get
            {
                bool isDirty = false;

                isDirty = (this._pendingDeletedItems != null && this._pendingDeletedItems.Count > 0);

                if (!isDirty && this.HasItemInObservableList)
                {
                    foreach (T item in this._observableList)
                    {
                        if (item != null)
                        {
                            isDirty = item.IsDirty;
                            if (isDirty)
                                break;
                        }
                    }
                }

                return isDirty;
            }
        }

        public virtual void ClearObservableList()
        {
            this.ObservableList.Clear();
        }
        public virtual bool RemoveFromObservableList(T item)
        {
            bool result = true;
            int itemIndex = -1;

            itemIndex = this.IndexOfWithinObservableList(item);
            if (itemIndex > -1)
                this.RemoveAtInObservableList(itemIndex);

            return result;
        }
        public virtual bool ContainsInObservableList(T item)
        {
            return (this.IndexOfWithinObservableList(item) > -1);
        }
        public virtual int IndexOfWithinObservableList(T item)
        {
            int itemIndex = this.ObservableList.IndexOf(item);

            if (itemIndex < 0)
            {
                for (int i = 0; i < this.ObservableList.Count; i++)
                {
                    if (Object.Equals(this.ObservableList[i], item))
                    {
                        itemIndex = i;
                        break;
                    }
                }
            }

            return itemIndex;
        }
        public virtual void InsertIntoObservableList(int index, T item)
        {
            this.ObservableList.Insert(index, item);
        }
        public virtual void RemoveAtInObservableList(int index)
        {
            this.ObservableList.RemoveAt(index);
        }
        public virtual void AddToObservableList(T item)
        {
            this.ObservableList.Add(item);
        }
        public virtual void CopyToFromObservableList(T[] array, int arrayIndex)
        {
            this.ObservableList.CopyTo(array, arrayIndex);
        }
        public virtual IEnumerator<T> GetEnumeratorFromObservableList()
        {
            return this.ObservableList.GetEnumerator();
        }

        /// <summary>
        /// Will discard item from Observable List without adding it to PendingDeleteItems
        /// </summary>
        /// <param name="item">object to remove</param>
        /// <returns></returns>
        public virtual bool DiscardFromObservableList(T item)
        {
            // Will discard item from Observable List without adding it to PendingDeleteItems
            bool result = false;
            if (this._observableList != null)
            {
                this._observableList.CollectionChanged -= (NotifyCollectionChangedEventHandler)this.OnObservableListCollectionChanged;
                result = this.RemoveFromObservableList(item);
                this._observableList.CollectionChanged += (NotifyCollectionChangedEventHandler)this.OnObservableListCollectionChanged;
            }
            return result;
        }
        /// <summary>
        /// Will discard item from Observable List without adding it to PendingDeleteItems
        /// </summary>
        /// <param name="index">index of object to remove</param>
        /// <returns></returns>
        public virtual void DiscardAtInObservableList(int index)
        {
            if (this._observableList != null)
            {
                this._observableList.CollectionChanged -= (NotifyCollectionChangedEventHandler)this.OnObservableListCollectionChanged;
                this.RemoveAtInObservableList(index);
                this._observableList.CollectionChanged += (NotifyCollectionChangedEventHandler)this.OnObservableListCollectionChanged;
            }
        }

        public virtual List<int> GetPSRidsFromObservableList()
        {
            List<int> results = new List<int>();

            if (this.HasItemInObservableList)
            {
                IEnumerable<int> query = from entity in this._observableList
                                         where entity != null && entity.IsValid
                                         select entity.ID.Value;

                if (query.Count<int>() > 0)
                    results.AddRange(query);
            }

            return results;
        }

        public virtual T GetEntityFromObservableList(T entity, T defaultEntity)
        {
            T result = GetEntityFromObservableList(entity);
            if (result != null)
                return result;
            else
                return defaultEntity;
        }
        public virtual T GetEntityFromObservableList(T entity)
        {
            T result = default(T);

            if (this.HasItemInObservableList)
                result = this._observableList.FirstOrDefault(item => item.Equals(entity));

            return result;
        }
        public virtual T GetEntityByPSRidFromObservableList(int psRid)
        {
            T result = default(T);

            if (this.HasItemInObservableList)
            {
                var query = from entity in this._observableList
                            where entity != null && entity.ID == psRid
                            select entity;

                result = query.FirstOrDefault<T>();
            }

            return result;
        }

        public virtual IVHEntityList<T> ReplicateFromObservableList()
        {
            VHEntityList<T> newList = new VHEntityList<T>(this.GetReplicatedInternalListFromObservableList());
            return newList;
        }
        public virtual List<T> GetReplicatedInternalListFromObservableList()
        {
            return this.ObservableList.ToList();
        }
        public virtual bool UpdateEntityInstanceWithinObservableList<IT>(IT newEntityInstance, bool ignoreDirtyFlag)
            where IT : T
        {
            bool result = false;

            int indexOfExistingEntity = this.IndexOfWithinObservableList(newEntityInstance);
            if (indexOfExistingEntity > -1)
            {
                if (ignoreDirtyFlag)
                {
                    this.ObservableList[indexOfExistingEntity] = newEntityInstance;
                }
                else
                {
                    // Replace instance only if it is not dirty
                    if (!this.ObservableList[indexOfExistingEntity].IsDirty)
                        this.ObservableList[indexOfExistingEntity] = newEntityInstance;
                }
                result = true;
            }

            return result;
        }
        public virtual TList GetDirtyEntitiesFromObservableList<TList>()
            where TList : IVHEntityList<T>, new()
        {
            TList result = default(TList);

            if (this.HasItemInObservableList)
            {
                // Perform a manual sync to a re-consolidated InternalList for dirty items
                this.ReplaceInternalListWithObservableList();

                // Clone dirty item from the re-consolidated InternalList
                result = this.GetDirtyEntities<TList>();
            }
            else if ((this._observableList == null) && (this.Count > 0))
            {
                // Clone dirty item from the re-consolidated InternalList
                result = this.GetDirtyEntities<TList>();
            }
            else
            {
                result = new TList();
            }

            return result;
        }
        public virtual TList GetInvalidEntitiesFromObservableList<TList>()
            where TList : IVHEntityList<T>, new()
        {
            TList result = new TList();

            if (this.HasItemInObservableList)
            {
                var query = from item in this.ObservableList
                            where item != null && !item.IsValid
                            select item;

                result.InternalList = query.ToList();
            }

            return result;
        }

        public virtual bool UndoDeleteToObservableList(T entity)
        {
            bool result = false;

            if ((this._pendingDeletedItems != null) &&
                (this._pendingDeletedItems.Contains(entity)))
            {
                // Only set for pending update if it was an undo of a new item
                if (!entity.IsValid)
                    entity.DirtyState = DirtyState.PendingAddChange;
                else
                    /* TODO - QQHQ - There is still a problem with the logic since 
                     * previous state might be truely PendingAddChange in the case
                     * where entity object got changed by other places */
                    entity.DirtyState = DirtyState.UnChanged;

                this.AddToObservableList(entity);
                this._pendingDeletedItems.Remove(entity);

                result = true;
            }

            return result;
        }
        #endregion Methods (Mimics IList<T>) for ObservableList

        #region Public Methods
        public virtual void MakeClean()
        {
            foreach (T item in this.InternalList)
            {
                if (item != null)
                    item.MakeClean();
            }

            if (this._pendingDeletedItems != null)
                this._pendingDeletedItems.Clear();
        }
        public virtual void MakeObservableListClean()
        {
            foreach (T item in this.ObservableList)
            {
                if (item != null)
                    item.MakeClean();
            }

            if (this._pendingDeletedItems != null)
                this._pendingDeletedItems.Clear();
        }

        public virtual void ReplaceInternalListWithObservableList()
        {
            IEnumerable<T> items = null;

            if (this._observableList != null)
            {
                if (items == null)
                    items = this._observableList.Where(item => item != null);
                else
                    items = items.Union(this._observableList.Where(item => item != null));
            }

            // Also combine PendingDelete into InternalList for service/cloning
            if (this._pendingDeletedItems != null)
            {
                if (items == null)
                    items = this._pendingDeletedItems.Where(item => item != null);
                else
                    items = items.Union(this._pendingDeletedItems.Where(item => item != null));
            }

            if (items != null)
                this.InternalList = items.ToList();
            else
                this.InternalList = new List<T>();
        }
        public virtual void ReplaceObservableListWithInternalList()
        {
            if (this._internalList != null)
            {
                // Filter and handle for non-PendingDeleteItem
                IEnumerable<T> nonPendingDeleteItemQuery = from item in this._internalList
                                                           where item != null && item.DirtyState != DirtyState.PendingDelete
                                                           select item;

                if (this._observableList == null)
                {
                    this._observableList = new MTObservableCollection<T>(nonPendingDeleteItemQuery);
                    this._observableList.CollectionChanged += (NotifyCollectionChangedEventHandler)this.OnObservableListCollectionChanged;
                }
                else
                {
                    this._observableList.CollectionChanged -= (NotifyCollectionChangedEventHandler)this.OnObservableListCollectionChanged;

                    this._observableList.Clear();
                    foreach (T item in nonPendingDeleteItemQuery)
                        this._observableList.Add(item);

                    this._observableList.CollectionChanged += (NotifyCollectionChangedEventHandler)this.OnObservableListCollectionChanged;
                }

                // Filter and handle for PendingDeleteItem
                IEnumerable<T> pendingDeleteItemQuery = from item in this._internalList
                                                        where item != null && item.DirtyState == DirtyState.PendingDelete
                                                        select item;
                if (pendingDeleteItemQuery.Count() > 0)
                {
                    if (this._pendingDeletedItems == null)
                    {
                        this._pendingDeletedItems = new MTObservableCollection<T>(pendingDeleteItemQuery);
                    }
                    else
                    {
                        this._pendingDeletedItems.Clear();
                        foreach (T item in pendingDeleteItemQuery)
                            this._pendingDeletedItems.Add(item);
                    }
                }
            }
        }

        //public virtual TModelList CreateModelListFromInternalList<TModelList, TModel, TList>()
        //    where TModelList : IPSModelList<TModel, TList, T>, new()
        //    where TModel : IPSModel<TModel, T>, new()
        //    where TList : IPSEntityList<T>, new()
        //{
        //    // All TModelList (e.g. CommonPersonModelList) must have valid constructor (TPrimaryList primaryList, bool useInternalListFlag) as Activator expectation
        //    TModelList resultList = (TModelList)Activator.CreateInstance(typeof(TModelList), new object[] { this, true });
        //    return resultList;
        //}
        //public virtual TModelList CreateModelListFromObservableList<TModelList, TModel, TList>()
        //    where TModelList : IPSModelList<TModel, TList, T>, new()
        //    where TModel : IPSModel<TModel, T>, new()
        //    where TList : IPSEntityList<T>, new()
        //{
        //    // All TModelList (e.g. CommonPersonModelList) must have valid constructor (TPrimaryList primaryList, bool useInternalListFlag) as Activator expectation
        //    TModelList resultList = (TModelList)Activator.CreateInstance(typeof(TModelList), new object[] { this, false });
        //    return resultList;
        //}

        public virtual TList CreateACloneFromObservableList<TList>() where TList : IVHEntityList<T>, new()
        {
            TList list = default(TList);
            this.ReplaceInternalListWithObservableList();
            list = base.CreateAClone<TList>();
            list.ReplaceObservableListWithInternalList();
            return list;
        }

        public virtual void SetToNewForInternalList()
        {
            foreach (T item in this.InternalList)
            {
                if (item != null)
                    item.SetToNew();
            }
        }
        public virtual void SetToNewForObservableList()
        {
            foreach (T item in this.ObservableList)
            {
                if (item != null)
                    item.SetToNew();
            }
        }

        public virtual TList GetSelectedItems<TList>()
            where TList : IVHEntityList<T>, new()
        {
            TList result = new TList();

            if (this.ActiveItem != null)
                result.InternalList.Add(this.ActiveItem);

            foreach (T item in this.SelectedItems)
            {
                if (item != null && !result.InternalList.Contains(item))
                    result.InternalList.Add(item);
            }

            return result;
        }
        public virtual void ReplaceInstance(T oldItem, T newItem)
        {
            // Find the index -- This is so later we can SWAP out that exact item veruses add to bottom...
            int index = this.ObservableList.IndexOf(oldItem);
            if (index >= 0)
            {
                // Remember If it is currently selected/active
                bool isSelected = this.SelectedItems.Contains(oldItem);
                bool isActive = object.Equals(this.ActiveItem, oldItem);

                // Always unselect it
                this.SelectedItems.Remove(oldItem);

                if (isActive)
                    this.ActiveItem = default(T);

                // If Delete (remove)
                if (oldItem.IsPendingDelete)
                {
                    // When removing don't put to pending delete
                    this.PropertyChangeEventDisabled = true;
                    this.ObservableList.Remove(oldItem);
                    this.PropertyChangeEventDisabled = false;
                }
                else
                {
                    // Do a Replacement (Remove old (don't put to pending delete) and then insert new)
                    this.PropertyChangeEventDisabled = true;
                    this.ObservableList.Remove(oldItem);
                    this.ObservableList.Insert(index, newItem);
                    this.PropertyChangeEventDisabled = false;
                    // ReActivate it
                    if (isActive)
                        this.ActiveItem = newItem;
                }
            }
        }
        #endregion

        #region Private Methods
        private void OnObservableListCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!PropertyChangeEventDisabled &&
                e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e != null && e.OldItems.Count > 0)
                {
                    IList items = e.OldItems;
                    foreach (T item in items)
                    {
                        if (item.ID.HasValue)
                        {
                            item.DirtyState = DirtyState.PendingDelete;
                            this.PendingDeletedItems.Add(item);
                        }
                    }
                }
            }

            this.NotifyPropertyChanged("HasItemInObservableList");
            this.NotifyPropertyChanged("CountFromObservableList");
            this.NotifyPropertyChanged("IsDirty");
            this.NotifyPropertyChanged("IsDirtyInObservableList");
            this.NotifyPropertyChanged("IsObservableListDirty");
        }
        #endregion

        #region IValidateToNavigate Members
        public override bool IsDirty
        {
            get { return this.IsObservableListDirty; }
        }
        #endregion

        #region IActiveItemsList Members
        public IEnumerable ActiveItemsList
        {
            get
            {
                var query = from item in this.ObservableList
                            where item != null && item.IsValid && item.Status == Status.Active
                            select item;
                return (query.ToList());
            }
        }
        #endregion

        #region PSObject overrides
        public override T CreateAClone<T>()
        {
            T result = base.CreateAClone<T>();
            
            var psObject = result as VHObject;
            if (psObject != null)
                psObject.UniqueId = Helper.GetUniqueId();
            
            return result;
        }
        #endregion
    }
}