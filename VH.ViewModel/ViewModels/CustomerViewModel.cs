using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using VH.Bases;
using VH.DataAccess;
using VH.ErrorLog;
using VH.Model;
using VH.Resources;
using VH.SimpleUI.Entities;

namespace VH.ViewModel
{
    public class CustomerViewModel : BaseViewModel<CustomerCollection>
    {
        #region Fields
        private bool _showProgressBar;
        #endregion

        #region Command Properties
        private ICommand _addAppointmentCommand;
        private ICommand _customerDoubleClick;

        public ICommand AddAppointmentCommand
        {
            get { return this._addAppointmentCommand ?? (this._addAppointmentCommand = new RelayCommand(OnAddAppointment, CanAddAppointment)); }
        }

        public ICommand CustomerDoubleClick
        {
            get { return _customerDoubleClick; }
            set
            {
                _customerDoubleClick = value;
                this.RaisePropertyChanged(() => CustomerDoubleClick);
            }
        }

        #endregion

      #region Properties

      #endregion

      #region Constructors
      public CustomerViewModel(IMessenger messenger)
            : base(messenger)
        {
            GetCustomerCollection();
        }

        public CustomerViewModel(IMessenger messenger, UserLogin userLogin)
            : base(messenger, userLogin)
        {
            GetCustomerCollection();
        }
        #endregion

        #region Override
        public override void Initialize()
        {
            base.Initialize();
            AddCommand = new RelayCommand(OnAddItem, CanAddItem);
            DeleteCommand = new RelayCommand(OnDeleteItem, CanDeleteItem);
            EditCommand = new RelayCommand(OnEditItem, CanEditItem);
            RefreshCommand = new RelayCommand(OnRefreshItem);
            CustomerDoubleClick = new RelayCommand(OnCustomerDoubleClick, CanCustomerDoubleClick);
        }

        public override void HandleViewModeChanges(dynamic data)
        {
            //base.HandleViewModeChanges(data);
            if (this.ParentViewModel is MainWindowViewModel)
            {
                ((MainWindowViewModel)this.ParentViewModel).IsCustomerTabSelected = true;
            }

        }

        public override void Unload()
        {
            base.Unload();
            this._addAppointmentCommand = null;
        }
        #endregion

        #region Override Command Methods
        public override void OnAddItem()
        {
            this.OnAddCustomer();
        }
        public override  bool CanAddItem()
        {
            return this.CanAddCustomer();
        }
        public override  void OnDeleteItem()
        {
            this.OnDeleteCustomer();
        }
        public override  bool CanDeleteItem()
        {
            return this.CanDeleteCustomer();
        }
        public override  void OnEditItem()
        {
            this.OnEditCustomer();
        }
        public override  bool CanEditItem()
        {
            return this.CanEditCustomer();
        }
        public override  void OnRefreshItem()
        {
            this.OnRefreshCustomer();
        }
        #endregion

        #region Command Methods
        private bool CanAddCustomer()
        {
            return true;
        }

        private void OnAddCustomer()
        {
            try
            {
                var childVM = new AddCustomerViewModel(this.Messenger, this.UserLogin) {ParentViewModel = this};
                childVM.RefreshCustomers += this.GetCustomerCollection;
                var messageDailog = new VMMessageDailog() {ChildViewModel = childVM};
                MessengerInstance.Send(messageDailog);
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }

        private void OnEditCustomer()
        {
            try
            {
                var childVM = new AddCustomerViewModel(this.Messenger, this.UserLogin,
                                                       this.Entity.InternalList.FirstOrDefault(x => x.IsSelected))
                    {
                        ParentViewModel = this
                    };
                childVM.RefreshCustomers += this.GetCustomerCollection;
                //childVM.SaveCustomer +=
                //    customer => CustomerAction.UpdateCustomer(this.DBConnectionString,
                //                                              this.Entity.InternalList.FirstOrDefault(x => x.IsSelected));
                var messageDailog = new VMMessageDailog() {ChildViewModel = childVM};
                MessengerInstance.Send(messageDailog);

            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }

        private bool CanEditCustomer()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Count(x => x.IsSelected) == 1);
        }

        private bool CanDeleteCustomer()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Any(x => x.IsSelected));
        }

        private void OnDeleteCustomer()
        {
            ShowProgressBar = true;
            var messageDailog = new MessageDailog(DeleteCustomer) {Caption = Resources.MessageResources.DeleteMessage, DialogButton = DialogButton.OkCancel, Title = Resources.TitleResources.Warning};
            MessengerInstance.Send(messageDailog);
        }

        private void DeleteCustomer(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Ok)
            {
               
                Task.Factory.StartNew(() =>
                    {
                        CustomerAction.DeleteCustomers(this.DBConnectionString,
                                                        this.Entity.InternalList.Where(x => x.IsSelected));
                        GetCustomerCollection();
                        ShowProgressBar = false;
                    });
            }
            else
            {
                ShowProgressBar = false;
            }
        }

        private void OnRefreshCustomer()
        {
            GetCustomerCollection();
        }

        private bool CanAddAppointment()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Count(x => x.IsSelected) == 1);
        }

        private void OnAddAppointment()
        {
            try
            {
                var childVM = new AppointmentViewModel(this.MessengerInstance, this.UserLogin,
                                                       this.Entity.InternalList.FirstOrDefault(x => x.IsSelected))
                    {
                        ParentViewModel = this.ParentViewModel
                    };
                var messageDailog = new VMMessageDailog() {ChildViewModel = childVM};
                MessengerInstance.Send(messageDailog);

            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }

        private bool CanCustomerDoubleClick()
        {
            return true;
        }

        private void OnCustomerDoubleClick()
        {
            var selectedCustomer = this.Entity.InternalList.FirstOrDefault(x => x.IsSelected);
            if (selectedCustomer != null)
            {
                this.ContentViewModel = null;
                this.ContentViewModel = new CustomerDetailViewModel(this.Messenger, this.UserLogin, selectedCustomer) { ParentViewModel = this };
                GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<IBaseViewModel>(this.ContentViewModel);
            }
        }
        #endregion

        #region Private Methods
        private void GetCustomerCollection()
        {
            try
            {
                ShowProgressBar = true;
                Task.Factory.StartNew(() =>
                    {
                        this.Entity = CustomerAction.GetCustomerList(this.DBConnectionString);
                        //this.Entity.MakeObservableListClean();
                        //this.Entity.MakeClean();
                        ShowProgressBar = false;
                    });

            }
            catch (Exception)
            {
                throw ;
                throw;
            }
        }

        #endregion

        #region Public Methods

        #endregion

    }
}