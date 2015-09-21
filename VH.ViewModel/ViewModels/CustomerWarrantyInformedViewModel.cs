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
    public class CustomerWarrantyInformedViewModel : BaseViewModel<CustomerWarrantyInformedCollection>
    {
        #region Properties
        public Customer SelectedCustomer { get; set; }
        #endregion

        #region Constructors
       

        public CustomerWarrantyInformedViewModel(IMessenger messenger, UserLogin userLogin, Customer customer)
            : base(messenger, userLogin)
        {
            SelectedCustomer = customer;

            if (SelectedCustomer != null && SelectedCustomer.CustomerWarrantyInformedCollection != null &&
                SelectedCustomer.CustomerWarrantyInformedCollection.Any())
            {
                this.Entity = new CustomerWarrantyInformedCollection(SelectedCustomer.CustomerWarrantyInformedCollection.ToList());
            }
            else
            {
                this.GetRefreshCustomerWarrantyInformedCollection();
            }
        }
        #endregion

        #region Override Methods
        public override void Initialize()
        {
            base.Initialize();

            AddCommand = new RelayCommand(OnAddItem, CanAddItem);
            DeleteCommand = new RelayCommand(OnDeleteItem, CanDeleteItem);
            EditCommand = new RelayCommand(OnEditItem, CanEditItem);
            RefreshCommand = new RelayCommand(OnRefreshItem);
        }
        #endregion

        #region Override Command Methods
        public override void OnAddItem()
        {
            try
            {
                var childVM = new AddCustomerWarrantyInformedViewModel(this.Messenger, this.UserLogin, this.SelectedCustomer) { ParentViewModel = this };
                childVM.RefreshCustomerWarrantyInformed += this.GetRefreshCustomerWarrantyInformedCollection;
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
            this.ParentViewModel.ShowProgressBar = true;
            var messageDailog = new MessageDailog(DeleteCustomerWarrantyInformed) { Caption = Resources.MessageResources.DeleteMessage, DialogButton = DialogButton.OkCancel, Title = Resources.TitleResources.Warning };
            MessengerInstance.Send(messageDailog);
        }
        public override bool CanDeleteItem()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Any(x => x.IsSelected));
        }

        public override void OnEditItem()
        {
            var childVM = new AddCustomerWarrantyInformedViewModel(this.Messenger, this.UserLogin, this.Entity.InternalList.FirstOrDefault(x => x.IsSelected)) { ParentViewModel = this };
            childVM.RefreshCustomerWarrantyInformed += this.GetRefreshCustomerWarrantyInformedCollection;
            var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
            MessengerInstance.Send(messageDailog);
        }
        public override bool CanEditItem()
        {
            return (this.Entity.InternalList != null && this.Entity.InternalList.Any(x => x.IsSelected));
        }
        public override void OnRefreshItem()
        {
           this.GetRefreshCustomerWarrantyInformedCollection();
        }
        #endregion

        #region Private Methods
        private void GetRefreshCustomerWarrantyInformedCollection()
        {

            Task.Factory.StartNew(() =>
            {
                this.Entity = CustomerAction.GetCustomerWarrantyInformedList(this.DBConnectionString, this.SelectedCustomer);
            });
        }

        private void DeleteCustomerWarrantyInformed(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Ok)
            {
                Task.Factory.StartNew(() =>
                {
                    CustomerAction.DeleteCustomerWarrantyInformeds(this.DBConnectionString, this.Entity.InternalList.Where(x => x.IsSelected));
                    GetRefreshCustomerWarrantyInformedCollection();
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