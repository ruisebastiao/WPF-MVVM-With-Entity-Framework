using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace VH.Model
{
    public interface IVHObjectList<TPSObjectList, TPrimary> : IVHObject
        where TPSObjectList : IVHObjectList<TPSObjectList, TPrimary>
    {
        #region Properties
        List<TPrimary> InternalList { get; set; }
        MTObservableCollection<TPrimary> ObservableList { get; set; }
        MTObservableCollection<TPrimary> PendingDeletedItems { get; }
        #endregion

        #region Methods (Mimics IList<TPrimary>)
        bool HasItem { get; }
        int Count { get; }
        TPrimary this[int index] { get; set; }

        ReadOnlyCollection<TPrimary> AsReadOnly();
        TPrimary Find(Predicate<TPrimary> match);
        List<TPrimary> FindAll(Predicate<TPrimary> match);
        bool Append(TPSObjectList additionalList);
        bool Append(IList<TPrimary> additionalItems);
        void Clear();
        bool Remove(TPrimary item);
        bool Contains(TPrimary item);
        int IndexOf(TPrimary item);
        void Insert(int index, TPrimary item);
        void RemoveAt(int index);
        void Add(TPrimary item);
        void CopyTo(TPrimary[] array, int arrayIndex);
        IEnumerator<TPrimary> GetEnumerator();
        #endregion

        #region Methods (Mimics IList<TPrimary>) for ObservableList
        bool HasItemInObservableList { get; }
        int CountFromObservableList { get; }

        void ClearObservableList();
        bool RemoveFromObservableList(TPrimary item);
        bool ContainsInObservableList(TPrimary item);
        int IndexOfWithinObservableList(TPrimary item);
        void InsertIntoObservableList(int index, TPrimary item);
        void RemoveAtInObservableList(int index);
        void AddToObservableList(TPrimary item);
        void CopyToFromObservableList(TPrimary[] array, int arrayIndex);
        IEnumerator<TPrimary> GetEnumeratorFromObservableList();
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
    [DataContract(Name = "PSObjectList{0}{1}", IsReference = true)]
    public class VHObjectList<TPSObjectList, TPrimary> : VHObject, IVHObjectList<TPSObjectList, TPrimary>
        where TPSObjectList : IVHObjectList<TPSObjectList, TPrimary>
    {
        #region Fields
        [DataMember]
        private List<TPrimary> _internalList = new List<TPrimary>();
        // Do not serialize this - _internalList will hold the items
        private MTObservableCollection<TPrimary> _observableList = null;
        // Do not serialize this - _internalList will hold the items
        private MTObservableCollection<TPrimary> _pendingDeletedItems = null;
        #endregion

        #region Properties
        public List<TPrimary> InternalList
        {
            get
            {
                this._internalList = this._internalList ?? new List<TPrimary>();
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
        public MTObservableCollection<TPrimary> ObservableList
        {
            get
            {
                if (this._observableList == null)
                    this.ObservableList = new MTObservableCollection<TPrimary>(this._internalList);

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
        public MTObservableCollection<TPrimary> PendingDeletedItems
        {
            get
            {
                if (this._pendingDeletedItems == null)
                    this._pendingDeletedItems = new MTObservableCollection<TPrimary>();

                return this._pendingDeletedItems;
            }
        }
        #endregion

        #region Constructors
        protected VHObjectList() : base() { }
        protected VHObjectList(TPSObjectList itemList) : this(itemList, true) { }
        protected VHObjectList(TPSObjectList itemList, bool useInternalListFlag)
            : base()
        {
            if (useInternalListFlag)
                this._internalList.AddRange(itemList.InternalList);
            else
                this._internalList.AddRange(itemList.ObservableList);
        }
        protected VHObjectList(IList<TPrimary> internalList)
            : base()
        {
            if (internalList is List<TPrimary>)
                this._internalList = (List<TPrimary>)internalList;
            else if (internalList != null)
                this._internalList = internalList.ToList();
        }
        #endregion

        #region Methods (Mimics IList<TPrimary>)
        public bool HasItem
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
        public virtual TPrimary this[int index]
        {
            get { return this.InternalList[index]; }
            set { this.InternalList[index] = value; }
        }

        public virtual ReadOnlyCollection<TPrimary> AsReadOnly()
        {
            return this.InternalList.AsReadOnly();
        }
        public virtual TPrimary Find(Predicate<TPrimary> match)
        {
            return this.InternalList.Find(match);
        }
        public virtual List<TPrimary> FindAll(Predicate<TPrimary> match)
        {
            return this.InternalList.FindAll(match);
        }
        public virtual bool Append(TPSObjectList additionalList)
        {
            bool appendedResult = false;

            if (additionalList != null)
                appendedResult = this.Append(additionalList.InternalList);

            return appendedResult;
        }
        public virtual bool Append(IList<TPrimary> additionalList)
        {
            bool appendedResult = false;

            if (additionalList != null && additionalList.Count > 0)
            {
                this.InternalList.AddRange(additionalList.Where(item => item != null).ToList());
                appendedResult = true;
            }

            return appendedResult;
        }
        public virtual void Clear()
        {
            this.InternalList.Clear();

            if (this.HasItemInObservableList)
                this.ClearObservableList();
        }
        public virtual bool Remove(TPrimary item)
        {
            return this.InternalList.Remove(item);
        }
        public virtual bool Contains(TPrimary item)
        {
            return (this.InternalList.Contains(item));
        }
        public virtual int IndexOf(TPrimary item)
        {
            return this.InternalList.IndexOf(item);
        }
        public virtual void Insert(int index, TPrimary item)
        {
            this.InternalList.Insert(index, item);
        }
        public virtual void RemoveAt(int index)
        {
            this.InternalList.RemoveAt(index);
        }
        public virtual void Add(TPrimary item)
        {
            this.InternalList.Add(item);
        }
        public virtual void CopyTo(TPrimary[] array, int arrayIndex)
        {
            this.InternalList.CopyTo(array, arrayIndex);
        }
        public virtual IEnumerator<TPrimary> GetEnumerator()
        {
            return this.InternalList.GetEnumerator();
        }
        #endregion

        #region Methods (Mimics IList<TPrimary>) for ObservableList
        public bool HasItemInObservableList
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
                    foreach (IVHObject item in this._observableList)
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

        public virtual ReadOnlyObservableCollection<TPrimary> AsReadOnlyFromObservableList()
        {
            return new ReadOnlyObservableCollection<TPrimary>(this.ObservableList);
        }
        public virtual TPrimary FindFromObservableList(TPrimary value)
        {
            return this.ObservableList.FirstOrDefault(item => item.Equals(value));
        }
        public virtual bool AppendToObservableList(TPSObjectList additionalList)
        {
            bool appendedResult = false;

            if (additionalList != null && additionalList.HasItemInObservableList)
            {
                foreach (TPrimary item in additionalList.ObservableList)
                {
                    if (item != null)
                        this.ObservableList.Add(item);
                }

                appendedResult = true;
            }

            return appendedResult;
        }
        public virtual bool AppendToObservableList(IList<TPrimary> additionalList)
        {
            bool appendedResult = false;

            if (additionalList != null && additionalList.Count > 0)
            {
                foreach (TPrimary item in additionalList)
                {
                    if (item != null)
                        this.ObservableList.Add(item);
                }

                appendedResult = true;
            }

            return appendedResult;
        }

        public virtual void ClearObservableList()
        {
            this.ObservableList.Clear();
        }
        public virtual bool RemoveFromObservableList(TPrimary item)
        {
            return this.ObservableList.Remove(item);
        }
        public virtual bool ContainsInObservableList(TPrimary item)
        {
            return (this.IndexOfWithinObservableList(item) > -1);
        }
        public virtual int IndexOfWithinObservableList(TPrimary item)
        {
            return this.ObservableList.IndexOf(item);
        }
        public virtual void InsertIntoObservableList(int index, TPrimary item)
        {
            this.ObservableList.Insert(index, item);
        }
        public virtual void RemoveAtInObservableList(int index)
        {
            this.ObservableList.RemoveAt(index);
        }
        public virtual void AddToObservableList(TPrimary item)
        {
            this.ObservableList.Add(item);
        }
        public virtual void CopyToFromObservableList(TPrimary[] array, int arrayIndex)
        {
            this.ObservableList.CopyTo(array, arrayIndex);
        }
        public virtual IEnumerator<TPrimary> GetEnumeratorFromObservableList()
        {
            return this.ObservableList.GetEnumerator();
        }

        public virtual bool DiscardFromObservableList(TPrimary item)
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

        public virtual void ReplaceInternalListWithObservableList()
        {
            this._internalList = new List<TPrimary>();

            if (this._observableList != null)
                this._internalList.AddRange(this._observableList);

            // Also combine PendingDelete into InternalList for service/cloning
            if (this._pendingDeletedItems != null)
                this._internalList.AddRange(this._pendingDeletedItems);
        }
        public virtual void ReplaceObservableListWithInternalList()
        {
            if (this._internalList != null)
            {
                // Filter and handle for non-PendingDeleteItem
                IEnumerable<TPrimary> nonPendingDeleteItemQuery = from item in this._internalList
                                                                  let psObject = item as VHObject
                                                                  where psObject == null || psObject.DirtyState != DirtyState.PendingDelete
                                                                  select item;

                if (this._observableList == null)
                {
                    this._observableList = new MTObservableCollection<TPrimary>(nonPendingDeleteItemQuery);
                    this._observableList.CollectionChanged += (NotifyCollectionChangedEventHandler)this.OnObservableListCollectionChanged;
                }
                else
                {
                    this._observableList.CollectionChanged -= (NotifyCollectionChangedEventHandler)this.OnObservableListCollectionChanged;

                    this._observableList.Clear();
                    foreach (TPrimary item in nonPendingDeleteItemQuery)
                        this._observableList.Add(item);

                    this._observableList.CollectionChanged += (NotifyCollectionChangedEventHandler)this.OnObservableListCollectionChanged;
                }

                // Filter and handle for PendingDeleteItem
                IEnumerable<TPrimary> pendingDeleteItemQuery = from item in this._internalList
                                                               let psObject = item as VHObject
                                                               where psObject != null && psObject.DirtyState == DirtyState.PendingDelete
                                                               select item;
                if (pendingDeleteItemQuery.Count() > 0)
                {
                    if (this._pendingDeletedItems == null)
                    {
                        this._pendingDeletedItems = new MTObservableCollection<TPrimary>(pendingDeleteItemQuery);
                    }
                    else
                    {
                        this._pendingDeletedItems.Clear();
                        foreach (TPrimary item in pendingDeleteItemQuery)
                            this._pendingDeletedItems.Add(item);
                    }
                }
            }
        }
        #endregion

        #region IValidateToNavigate Members
        public override bool IsDirty
        {
            get { return this.IsObservableListDirty; }
        }
        #endregion

        #region Private Methods
        private void OnObservableListCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e != null && e.OldItems.Count > 0)
                {
                    IList items = e.OldItems;
                    foreach (TPrimary item in items)
                        this.PendingDeletedItems.Add(item);
                }
            }
            this.NotifyPropertyChanged("HasItemInObservableList");
            this.NotifyPropertyChanged("CountFromObservableList");
            this.NotifyPropertyChanged("IsDirty");
            this.NotifyPropertyChanged("IsObservableListDirty");
        }
        #endregion
    }
}