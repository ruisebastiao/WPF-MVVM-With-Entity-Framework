using System;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using VH.DataAccess;
using VH.ErrorLog;
using VH.Model;
using VH.Resources;
using VH.SimpleUI.Entities;

namespace VH.ViewModel
{
    public class ReminderViewModel : BaseViewModel<ReminderCollection>
    {
       #region Fields

        private DateTime? _searchDateTime = DateTime.Now;
        private RelayCommand _searchCommnad;

        #endregion

        #region Properties
        public DateTime? SearchDateTime
        {
            get { return _searchDateTime; }
            set
            {
                _searchDateTime = value;
                this.RaisePropertyChanging(() => this.SearchDateTime);
            }
        }
        #endregion

        #region Constructors
        public ReminderViewModel(IMessenger messenger)
            : base(messenger)
        {
        }

        public ReminderViewModel(IMessenger messenger, UserLogin userLogin)
            : base(messenger, userLogin)
        {
        }
        #endregion

        #region Command Methods

        public RelayCommand SearchCommand
        {
            get { return _searchCommnad; }
            set
            {
                _searchCommnad = value;
                this.RaisePropertyChanging(() => this.SearchCommand);
            }
        }

        private void OnSearch()
        {
            this.GetReminderCollectionByDate(this.SearchDateTime);
        }
        #endregion

        #region Override Methods
        public override void Initialize()
        {
            base.Initialize();

            SearchDateTime = DateTime.Now.Date;
            GetReminderCollectionByDate(this.SearchDateTime);

            AddCommand = new RelayCommand(OnAddItem, CanAddItem);
            DeleteCommand = new RelayCommand(OnDeleteItem, CanDeleteItem);
            EditCommand = new RelayCommand(OnEditItem, CanEditItem);
            RefreshCommand = new RelayCommand(OnRefreshItem);
            SearchCommand = new RelayCommand(OnSearch);
        }

        //public override void HandleViewModeChanges(dynamic data)
        //{
        //    //base.HandleViewModeChanges(data);
        //    var model = this.ParentViewModel as MainWindowViewModel;
        //    if (model != null)
        //    {
        //        model.IsCallRegistryTabSelected = true;
        //    }
        //}

        #endregion

        #region Override Command Methods
        public override void OnAddItem()
        {
            try
            {
                var childVM = new AddReminderViewModel(this.Messenger, this.UserLogin) { ParentViewModel = this };
                childVM.RefreshReminders += this.GetReminderCollectionByDate;
                var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
                MessengerInstance.Send(messageDailog);

            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }
        public override bool CanAddItem()
        {
            return true;
        }
        public override void OnDeleteItem()
        {
            ShowProgressBar = true;
            var messageDailog = new MessageDailog(DeleteReminder) { Caption = Resources.MessageResources.DeleteMessage, DialogButton = DialogButton.OkCancel, Title = Resources.TitleResources.Warning };
            MessengerInstance.Send(messageDailog);
        }
        public override bool CanDeleteItem()
        {
            return (this.Entity.InternalList != null && this.Entity.InternalList.Any(x => x.IsSelected));
        }
        public override void OnEditItem()
        {
            var childVM = new AddReminderViewModel(this.Messenger, this.UserLogin, this.Entity.InternalList.FirstOrDefault(x => x.IsSelected)) { ParentViewModel = this };
            childVM.RefreshReminders += this.GetReminderCollectionByDate;
            var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
            MessengerInstance.Send(messageDailog);
        }
        public override bool CanEditItem()
        {
            return (this.Entity.InternalList != null && this.Entity.InternalList.Any(x => x.IsSelected));
        }
        public override void OnRefreshItem()
        {
            this.GetReminderCollectionByDate(this.SearchDateTime);
        }
        #endregion

        #region Private Methods

        private void GetReminderCollectionByDate(DateTime? dateTime)
        {

            this.ShowProgressBar = true;
            this.SearchDateTime = null;
            this.SearchDateTime = dateTime;
           
            Task.Factory.StartNew(() =>
            {
                this.Entity = dateTime.HasValue ? ReminderAction.GetReminders(this.DBConnectionString, dateTime.Value) : ReminderAction.GetReminders(this.DBConnectionString);
                this.ShowProgressBar = false;
            });
        }

        private void DeleteReminder(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Ok)
            {
                Task.Factory.StartNew(() =>
                {
                    ReminderAction.DeleteReminders(this.DBConnectionString, this.Entity.InternalList.Where(x => x.IsSelected));
                    GetReminderCollectionByDate(this.SearchDateTime);
                    ShowProgressBar = false;
                });
            }
            else
            {
                ShowProgressBar = false;
            }
        }
        #endregion
       
    }
}