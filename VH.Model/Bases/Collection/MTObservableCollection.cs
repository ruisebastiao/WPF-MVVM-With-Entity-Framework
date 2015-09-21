using System;
using System.Linq;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace VH.Model
{
    [DataContract]
    public class MTObservableCollection<T> : ObservableCollection<T>
    {
        public MTObservableCollection() : base() { }
        public MTObservableCollection(List<T> list) : base(list) { }
        public MTObservableCollection(IEnumerable<T> collection) : base(collection) { }

        public override event NotifyCollectionChangedEventHandler CollectionChanged;
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (this.CollectionChanged != null)
            {
                Dispatcher dispatcher = (from method in this.CollectionChanged.GetInvocationList()
                                         let dispatcherObject = method.Target as DispatcherObject
                                         where dispatcherObject != null
                                         select dispatcherObject.Dispatcher).FirstOrDefault();

                // Run this method on the Dispatcher thread
                if (dispatcher != null && !dispatcher.CheckAccess())
                {
                    Action<NotifyCollectionChangedEventArgs> method = this.OnCollectionChanged;
                    dispatcher.Invoke(method, DispatcherPriority.DataBind, e);
                }
                else
                {
                    // Trigger all of the CollectChanged event listeners
                    foreach (NotifyCollectionChangedEventHandler handler in this.CollectionChanged.GetInvocationList())
                        handler.Invoke(this, e);
                }
            }
        }
    }
}