using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using VH.DataAccess;
using VH.ErrorLog;
using VH.Model;
using VH.Resources;
using VH.SimpleUI.Entities;

namespace VH.ViewModel
{
    public class CustomerRepairViewModel : BaseViewModel<CustomerRepairCollection>
    {

        #region Fields
        
        #endregion

        #region Properties
        public Customer SelectedCustomer { get; set; }
        #endregion

        #region Command Properties
        private ICommand _updateRepairStatusCommand;
        public ICommand UpdateRepairStatusCommand
        {
            get { return this._updateRepairStatusCommand ?? (this._updateRepairStatusCommand = new RelayCommand(OnUpdateRepairStatus, CanUpdateRepairStatus)); }
        }

        private ICommand _printReceiptStatusCommand;
        public ICommand PrintReceiptCommand
        {
            get { return this._printReceiptStatusCommand ?? (this._printReceiptStatusCommand = new RelayCommand(OnPrintReceipt, CanPrintReceipt)); }
        }
        #endregion

        #region Constructors
        public CustomerRepairViewModel(IMessenger messenger, UserLogin userLogin, Customer customer)
            : base(messenger, userLogin)
        {
            SelectedCustomer = customer;

            if (SelectedCustomer != null && SelectedCustomer.CustomerRepairCollection != null &&
                SelectedCustomer.CustomerRepairCollection.Any())
            {
                this.Entity = new CustomerRepairCollection(SelectedCustomer.CustomerRepairCollection.ToList());
            }
            else
            {
                this.GetRefreshCustomerRepairCollection();
            }
        }
        #endregion

        public override void Initialize()
        {
            base.Initialize();
            AddCommand = new RelayCommand(OnAddItem);
            DeleteCommand = new RelayCommand(OnDeleteItem, CanDeleteItem);
            EditCommand = new RelayCommand(OnEditItem, CanEditItem);
            RefreshCommand = new RelayCommand(OnRefreshItem);
        }

        #region Command Methods
        public override void OnAddItem()
        {
            try
            {
                var childVM = new AddCustomerRepairViewModel(this.Messenger, this.UserLogin, this.SelectedCustomer) { ParentViewModel = this };
                childVM.RefreshCustomerRepair += this.GetRefreshCustomerRepairCollection;
                var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
                MessengerInstance.Send(messageDailog);

            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }
        
        public override void OnDeleteItem()
        {
            this.ParentViewModel.ShowProgressBar = true;
            var messageDailog = new MessageDailog(DeleteCustomerRepair) { Caption = Resources.MessageResources.DeleteMessage, DialogButton = DialogButton.OkCancel, Title = Resources.TitleResources.Warning };
            MessengerInstance.Send(messageDailog);
        }
        public override bool CanDeleteItem()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Any(x => x.IsSelected));
        }

        public override void OnEditItem()
        {
            var childVM = new AddCustomerRepairViewModel(this.Messenger, this.UserLogin, this.Entity.InternalList.FirstOrDefault(x => x.IsSelected)) { ParentViewModel = this };
            childVM.RefreshCustomerRepair += this.GetRefreshCustomerRepairCollection;
            var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
            MessengerInstance.Send(messageDailog);
        }
        public override bool CanEditItem()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Count(x => x.IsSelected) == 1);
        }

        public override void OnRefreshItem()
        {
            this.GetRefreshCustomerRepairCollection();
        }

        private bool CanUpdateRepairStatus()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Count(x => x.IsSelected) == 1);
        }

        private void OnUpdateRepairStatus()
        {
            try
            {
                var childVM = new UpdateCustomerRepairStatusViewModel(this.Messenger, this.UserLogin, this.Entity.InternalList.FirstOrDefault(x =>x.IsSelected)) { ParentViewModel = this };
                childVM.RefreshCustomerRepair += this.GetRefreshCustomerRepairCollection;
                var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
                MessengerInstance.Send(messageDailog);

            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }

        private bool CanPrintReceipt()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Count(x => x.IsSelected) == 1);
        }

        private void OnPrintReceipt()
        {
            try
            {
                var childVM = new PrintViewModel(this.Messenger, this.UserLogin, this.Entity.InternalList.FirstOrDefault(x => x.IsSelected)) { ParentViewModel = this };
                var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
                MessengerInstance.Send(messageDailog);
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }
        #endregion

        #region Public Methods
        
        #endregion

        #region Private Methods
        private void GetRefreshCustomerRepairCollection()
        {

            Task.Factory.StartNew(() =>
                {
                    this.Entity = CustomerAction.GetCustomerRepairList(this.DBConnectionString, this.SelectedCustomer);
                });
        }

        private void DeleteCustomerRepair(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Ok)
            {
                Task.Factory.StartNew(() =>
                {
                    CustomerAction.DeleteCustomerRepairs(this.DBConnectionString, this.Entity.InternalList.Where(x => x.IsSelected));
                    GetRefreshCustomerRepairCollection();
                    this.ParentViewModel.ShowProgressBar = false;
                });
            }
            else
            {
                this.ParentViewModel.ShowProgressBar = false;
            }
        }
        #endregion
    }
}