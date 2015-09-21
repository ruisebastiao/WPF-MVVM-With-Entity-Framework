using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class CustomerOrderViewModel : BaseViewModel<Customer>
    {
        #region Fields

        #endregion

        #region Properties

        protected Customer SelectedCustomer { get; set; }

        #endregion

        #region Command Properties
        public ICommand AddHearingAIDCommand { get; set; }
        public ICommand DeleteHearingAIDCommand { get; set; }
        public ICommand EditHearingAIDCommand { get; set; }
        public ICommand RefreshHearingAIDCommand { get; set; }
        private ICommand _printHearingAidReceiptCommand;
        public ICommand PrintHearingAidReceiptCommand
        {
            get { return this._printHearingAidReceiptCommand ?? (this._printHearingAidReceiptCommand = new RelayCommand(OnPrintHearingAidReceipt, CanPrintHearingAidReceipt)); }
        }

        public ICommand AddEarMoldCommand { get; set; }
        public ICommand DeleteEarMoldCommand { get; set; }
        public ICommand EditEarMoldCommand { get; set; }
        public ICommand RefreshEarMoldCommand { get; set; }

        private ICommand _printEarMoldReceiptCommand;
        public ICommand PrintEarMoldReceiptCommand
        {
            get { return this._printEarMoldReceiptCommand ?? (this._printEarMoldReceiptCommand = new RelayCommand(OnPrintEarMoldReceipt, CanPrintEarMoldReceipt)); }
        }

        #endregion

        #region Constructors
        public CustomerOrderViewModel(IMessenger messenger, UserLogin userLogin, Customer customer)
            : base(messenger, userLogin)
        {
            Entity = customer; 


           if(this.Entity  != null && this.Entity.CustomerEarMoldOrderCollection == null)
               this.RefreshCustomerEarMoldOrderCollection(false);

           if (this.Entity != null && this.Entity.CustomerHearingAidOrderCollection == null)
               this.RefreshCustomerHearingAidOrderCollection(false);
        }

        #endregion

        #region Override Methods
        public override void Initialize()
        {
            base.Initialize();
            AddEarMoldCommand = new RelayCommand(OnAddItem);
            DeleteEarMoldCommand = new RelayCommand(OnDeleteItem, CanDeleteItem);
            EditEarMoldCommand = new RelayCommand(OnEditItem, CanEditItem);
            RefreshEarMoldCommand = new RelayCommand(OnRefreshItem);


            AddHearingAIDCommand = new RelayCommand(OnAddHearingAIDItem);
            DeleteHearingAIDCommand = new RelayCommand(OnDeleteHearingAIDItem, CanDeleteHearingAIDItem);
            EditHearingAIDCommand = new RelayCommand(OnEditHearingAIDItem, CanEditHearingAIDItem);
            RefreshHearingAIDCommand = new RelayCommand(OnRefreshHearingAIDItem);
        }
        #endregion

        #region Command Methods
        public override void OnAddItem()
        {
            try
            {
                var childVM = new AddCustomerEarMoldOrderViewModel(this.Messenger, this.UserLogin, this.Entity) { ParentViewModel = this };
                childVM.RefreshCustomerEarMoldOrder += this.RefreshCustomerEarMoldOrderCollection;
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
            //this.ParentViewModel.ShowProgressBar = true;
            var messageDailog = new MessageDailog(DeleteCustomerEarMoldOrder) { Caption = MessageResources.DeleteMessage, DialogButton = DialogButton.OkCancel, Title = TitleResources.Warning };
            MessengerInstance.Send(messageDailog);
        }
        public override bool CanDeleteItem()
        {
            return (this.Entity != null && this.Entity.CustomerEarMoldOrderCollection != null && this.Entity.CustomerEarMoldOrderCollection.Any(x => x.IsSelected));
        }

        public override void OnEditItem()
        {
            var childVM = new AddCustomerEarMoldOrderViewModel(this.Messenger, this.UserLogin, this.Entity.CustomerEarMoldOrderCollection.FirstOrDefault(x => x.IsSelected)) { ParentViewModel = this };
            childVM.RefreshCustomerEarMoldOrder += this.RefreshCustomerEarMoldOrderCollection;
            var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
            MessengerInstance.Send(messageDailog);
        }
        public override bool CanEditItem()
        {
            return (this.Entity != null && this.Entity.CustomerEarMoldOrderCollection != null && this.Entity.CustomerEarMoldOrderCollection.Count(x => x.IsSelected) == 1);
        }

        public override void OnRefreshItem()
        {
            this.RefreshCustomerEarMoldOrderCollection();
        }

        public void OnAddHearingAIDItem()
        {
            try
            {
                var childVM = new AddCustomerHearingAidOrderViewModel(this.Messenger, this.UserLogin, this.Entity) { ParentViewModel = this };
                childVM.RefreshCustomerHearingAIDOrder += this.RefreshCustomerHearingAidOrderCollection;
                var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
                MessengerInstance.Send(messageDailog);

            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }

        public void OnDeleteHearingAIDItem()
        {
            //this.ParentViewModel.ShowProgressBar = true;
            var messageDailog = new MessageDailog(DeleteCustomerHearingAidOrder) { Caption = MessageResources.DeleteMessage, DialogButton = DialogButton.OkCancel, Title = TitleResources.Warning };
            MessengerInstance.Send(messageDailog);
        }
        public bool CanDeleteHearingAIDItem()
        {
            return (this.Entity != null && this.Entity.CustomerHearingAidOrderCollection != null && this.Entity.CustomerHearingAidOrderCollection.Any(x => x.IsSelected));
        }

        public  void OnEditHearingAIDItem()
        {
            var childVM = new AddCustomerHearingAidOrderViewModel(this.Messenger, this.UserLogin, this.Entity.CustomerHearingAidOrderCollection.FirstOrDefault(x => x.IsSelected)) { ParentViewModel = this };
            childVM.RefreshCustomerHearingAIDOrder += this.RefreshCustomerHearingAidOrderCollection;
            var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
            MessengerInstance.Send(messageDailog);
        }
        public bool CanEditHearingAIDItem()
        {
            return (this.Entity != null && this.Entity.CustomerHearingAidOrderCollection != null && this.Entity.CustomerHearingAidOrderCollection.Count(x => x.IsSelected) == 1);
        }

        public  void OnRefreshHearingAIDItem()
        {
            this.RefreshCustomerHearingAidOrderCollection();
        }

        private bool CanPrintHearingAidReceipt()
        {
            return (this.Entity != null && this.Entity.CustomerHearingAidOrderCollection != null && this.Entity.CustomerHearingAidOrderCollection.Count(x => x.IsSelected) == 1);
        }

        private void OnPrintHearingAidReceipt()
        {
            try
            {
                var childVM = new PrintViewModel(this.Messenger, this.UserLogin, this.Entity.CustomerHearingAidOrderCollection.FirstOrDefault(x => x.IsSelected)) { ParentViewModel = this };
                var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
                MessengerInstance.Send(messageDailog);
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }
        private bool CanPrintEarMoldReceipt()
        {
            return (this.Entity != null && this.Entity.CustomerEarMoldOrderCollection != null && this.Entity.CustomerEarMoldOrderCollection.Count(x => x.IsSelected) == 1);
        }

        private void OnPrintEarMoldReceipt()
        {
            try
            {
                var childVM = new PrintViewModel(this.Messenger, this.UserLogin, this.Entity.CustomerEarMoldOrderCollection.FirstOrDefault(x => x.IsSelected)) { ParentViewModel = this };
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

        #region public Methods

        #endregion

        #region Private Methods
        private void RefreshCustomerEarMoldOrderCollection(bool doAsync = false)
        {
            try
            {
                if (doAsync)
                    Task.Factory.StartNew(() =>
                    {
                        var items = CustomerAction.GetCustomerEarMoldOrderList(this.DBConnectionString, this.Entity);
                        if (items != null && items.InternalList.Any())
                            this.Entity.CustomerEarMoldOrderCollection = items.InternalList;
                    });
                else
                {
                    var items = CustomerAction.GetCustomerEarMoldOrderList(this.DBConnectionString, this.Entity);
                    if (items != null && items.InternalList.Any())
                        this.Entity.CustomerEarMoldOrderCollection = items.InternalList;
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
           
        }

        private void RefreshCustomerEarMoldOrderCollection()
        {
            try
            {
                Task.Factory.StartNew(() =>
                    {
                        var items = CustomerAction.GetCustomerEarMoldOrderList(this.DBConnectionString, this.Entity);
                        if (items != null && items.InternalList.Any())
                            this.Entity.CustomerEarMoldOrderCollection = items.InternalList;
                    });

            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }

        }

        private void RefreshCustomerHearingAidOrderCollection(bool doAsync = false)
        {
            try
            {
                if (doAsync)
                    Task.Factory.StartNew(() =>
                        {
                            var items = CustomerAction.GetCustomerHearingAidOrderList(this.DBConnectionString,
                                                                                      this.Entity);
                            if (items != null && items.InternalList.Any())
                                this.Entity.CustomerHearingAidOrderCollection = items.InternalList;

                        });
                else
                {
                    var items = CustomerAction.GetCustomerHearingAidOrderList(this.DBConnectionString, this.Entity);
                    if (items != null && items.InternalList.Any())
                        this.Entity.CustomerHearingAidOrderCollection = items.InternalList;
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }

        }

        private void RefreshCustomerHearingAidOrderCollection()
        {
            try
            {
                Task.Factory.StartNew(() =>
                    {
                        var items = CustomerAction.GetCustomerHearingAidOrderList(this.DBConnectionString,
                                                                                  this.Entity);
                        if (items != null && items.InternalList.Any())
                            this.Entity.CustomerHearingAidOrderCollection = items.InternalList;

                    });

            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }

        }

        private void DeleteCustomerEarMoldOrder(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Ok)
            {
                Task.Factory.StartNew(() =>
                {
                    CustomerAction.DeleteCustomerEarMoldOrders(this.DBConnectionString, this.Entity.CustomerEarMoldOrderCollection.Where(x => x.IsSelected));
                    RefreshCustomerEarMoldOrderCollection();
                });
            }
            else
            {
                this.ParentViewModel.ShowProgressBar = false;
            }
        }

        private void DeleteCustomerHearingAidOrder(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Ok)
            {
                Task.Factory.StartNew(() =>
                {
                    CustomerAction.DeleteCustomerHearingAidOrders(this.DBConnectionString, this.Entity.CustomerHearingAidOrderCollection.Where(x => x.IsSelected));
                    RefreshCustomerHearingAidOrderCollection();
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
