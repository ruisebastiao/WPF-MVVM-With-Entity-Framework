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
    public class CallRegistryViewModel : BaseViewModel<CallRegistryCollection>
    {
        

        #region Fields
        private DateTime? _searchDateTime;
        private string _searchName;
        private RelayCommand _searchCommnad;

        #endregion

        #region Properties

        public string SearchName
        {
            get { return _searchName; }
            set
            {
                _searchName = value;
                this.RaisePropertyChanging(() => this.SearchName);
            }
        }

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

        #region Command Properties
        
        #endregion

        #region Constructors
        public CallRegistryViewModel(IMessenger messenger)
            : base(messenger)
        {
        }

        public CallRegistryViewModel(IMessenger messenger, UserLogin userLogin)
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

        #endregion

        #region Override Methods
        public override void Initialize()
        {
            base.Initialize();

            SearchDateTime = DateTime.Now.Date;
            GetCallRegisterCollectionByDate(this.SearchDateTime, this.SearchName);

            AddCommand = new RelayCommand(OnAddItem, CanAddItem);
            DeleteCommand = new RelayCommand(OnDeleteItem, CanDeleteItem);
            EditCommand = new RelayCommand(OnEditItem, CanEditItem);
            RefreshCommand = new RelayCommand(OnRefreshItem);
            SearchCommand = new RelayCommand(OnSearch);
        }

        public override void HandleViewModeChanges(dynamic data)
        {
            //base.HandleViewModeChanges(data);
            var model = this.ParentViewModel as MainWindowViewModel;
            if (model != null)
            {
                model.IsCallRegistryTabSelected = true;
            }
        }

        #endregion

        #region Override Command Methods
        public override void OnAddItem()
        {
            try
            {
                var childVM = new AddCallLogViewModel(this.Messenger, this.UserLogin) {ParentViewModel = this};
                childVM.RefreshCallRegistry += this.GetCallRegisterCollectionByDate;
                var messageDailog = new VMMessageDailog() {ChildViewModel = childVM};
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
            var messageDailog = new MessageDailog(DeleteCustomer) { Caption = Resources.MessageResources.DeleteMessage, DialogButton = DialogButton.OkCancel, Title = Resources.TitleResources.Warning };
            MessengerInstance.Send(messageDailog);
        }
        public override bool CanDeleteItem()
        {
            return (this.Entity.InternalList != null && this.Entity.InternalList.Any(x => x.IsSelected));
        }
        public override void OnEditItem()
        {
            var childVM = new AddCallLogViewModel(this.Messenger, this.UserLogin, this.Entity.InternalList.FirstOrDefault(x => x.IsSelected)) { ParentViewModel = this };
            childVM.RefreshCallRegistry += this.GetCallRegisterCollectionByDate;
            var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
            MessengerInstance.Send(messageDailog);
        }
        public override bool CanEditItem()
        {
            return (this.Entity.InternalList != null && this.Entity.InternalList.Any(x => x.IsSelected));
        }
        public override void OnRefreshItem()
        {
            this.GetCallRegisterCollectionByDate(this.SearchDateTime, this.SearchName);
        }
        #endregion

        #region Command Methods
        private void OnSearch()
        {
            this.GetCallRegisterCollectionByDate(this.SearchDateTime, this.SearchName);
        }

        private bool CanSearch()
        {
            return !string.IsNullOrEmpty(this.SearchName) || this.SearchDateTime.HasValue;
        }
        #endregion

        #region Public Methds

        #endregion

        #region Private Methods
        private void GetCallRegisterCollectionByDate(DateTime? dateTime, string searchName)
        {
            this.ShowProgressBar = true;
            Task.Factory.StartNew(() =>
            {
                this.Entity = CallRegistryAction.GetCallRegistries(this.DBConnectionString, searchName, dateTime);
                this.ShowProgressBar = false;
            });
        }

        private void GetCallRegisterCollectionByDate(DateTime dateTime)
        {
            this.ShowProgressBar = true;
            this.SearchDateTime = null;
            this.SearchDateTime = dateTime;
            Task.Factory.StartNew(() =>
            {
                this.Entity = CallRegistryAction.GetCallRegistries(this.DBConnectionString, "", dateTime);
                this.ShowProgressBar = false;
            });
        }

        private void DeleteCustomer(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Ok)
            {
                Task.Factory.StartNew(() =>
                {
                    CallRegistryAction.DeleteCallRegistries(this.DBConnectionString, this.Entity.InternalList.Where(x => x.IsSelected));
                    GetCallRegisterCollectionByDate(this.SearchDateTime, this.SearchName);
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