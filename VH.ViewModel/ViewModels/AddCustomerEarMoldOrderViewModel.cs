using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using VH.DataAccess;
using VH.Model;
using VH.Resources;
using VH.SimpleUI.Entities;
using VH.Model.Utilities;

namespace VH.ViewModel
{
    public class AddCustomerEarMoldOrderViewModel : BaseViewModel<CustomerEarMoldOrder>
    {
        #region Delegate
        public Action RefreshCustomerEarMoldOrder { get; set; }
        #endregion

        #region Fields
        
        #endregion

        #region Properties

        public IDictionary<int,string> CustomerHearingAidCollection
        {
            get { return _customerHearingAidCollection; }
            set
            {
                _customerHearingAidCollection = value;
                this.RaisePropertyChanged("CustomerHearingAidCollection");
            }
        }

        #region Window Properties
        public override string Title
        {
            get
            {
                return !IsInEditMode ? TitleResources.AddCustomerEarMoldOrder : TitleResources.EditCustomerEarMoldOrder;
            }
        }


        public override double DialogStartupCustomHeight
        {
            get
            {
                return 760;
            }
        }

        public override double DialogStartupCustomWidth
        {
            get
            {
                return 600;
            }
        }

        public override DialogType DialogType
        {
            get
            {
                return DialogType.BySizeInPixel;
            }
        }
        #endregion
        #endregion

        #region Command Properties
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get { return this._saveCommand ?? (this._saveCommand = new RelayCommand(OnSave, CanSave)); }
        }

        private ICommand _cancelCommand;
        private IDictionary<int, string> _customerHearingAidCollection;

        public ICommand CancelCommand
        {
            get { return this._cancelCommand ?? (this._cancelCommand = new RelayCommand(OnCancel)); }
        }
        #endregion

        #region Constructors

        public AddCustomerEarMoldOrderViewModel(IMessenger messenger, UserLogin userLogin,
                                                Customer customer)
            : base(messenger, userLogin)
        {
            //this.SelectedCustomer = customer;
            this.Entity.Customer = customer;
            this.Entity.CustomerID = customer.ID.Value;

            if (this.Entity != null && this.Entity.Customer != null && this.Entity.Customer.CustomerHearingAidOrderCollection != null && this.Entity.Customer.CustomerHearingAidOrderCollection.Any())
            {
                this.Entity.Customer.CustomerHearingAidOrderCollection.ForEach(x =>
                {
                    if (this.CustomerHearingAidCollection == null)
                        this.CustomerHearingAidCollection = new Dictionary<int, string>();
                    this.CustomerHearingAidCollection.Add(x.ID.Value,
                                                          string.Format("{0} : {1} ", x.HearingAidType,
                                                                        x.Company));
                });
            }
        }

        public AddCustomerEarMoldOrderViewModel(IMessenger messenger, UserLogin userLogin, CustomerEarMoldOrder customerEarMoldOrder)
            : base(messenger, userLogin)
        {
            this.IsInEditMode = true;
            this.Entity = customerEarMoldOrder;

            if (this.Entity != null && this.Entity.Customer != null && this.Entity.Customer.CustomerHearingAidOrderCollection != null && this.Entity.Customer.CustomerHearingAidOrderCollection.Any())
            {
                this.Entity.Customer.CustomerHearingAidOrderCollection.ForEach(x =>
                {
                    if (this.CustomerHearingAidCollection == null)
                        this.CustomerHearingAidCollection = new Dictionary<int, string>();
                    this.CustomerHearingAidCollection.Add(x.ID.Value,
                                                          string.Format("{0} : {1} ", x.HearingAidType,
                                                                        x.Company));
                });
            }
        }
        #endregion

        #region Command Methods

        private void OnSave()
        {
            var returnStatus = false;
            returnStatus = !IsInEditMode
                               ? CustomerAction.AddCustomerEarMoldOrder(this.DBConnectionString, this.Entity)
                               : CustomerAction.UpdateCustomerEarMoldOrder(this.DBConnectionString, this.Entity);

            if (returnStatus)
            {
                if (RefreshCustomerEarMoldOrder != null)
                    this.RefreshCustomerEarMoldOrder();

                var messageDailog = new MessageDailog()
                {
                    Caption = MessageResources.DataSavedSuccessfully,
                    DialogButton = DialogButton.Ok,
                    Title = TitleResources.Information
                };

                MessengerInstance.Send(messageDailog);

                if (this.CloseWindow != null)
                    this.CloseWindow();
            }
            else
            {
                var messageDailog = new MessageDailog()
                {
                    Caption = MessageResources.DataSavedFailed,
                    DialogButton = DialogButton.Ok,
                    Title = TitleResources.Error
                };
                MessengerInstance.Send(messageDailog);
            }
        }

        private bool CanSave()
        {
            return this.Entity != null && !string.IsNullOrEmpty(this.Entity.OrderDate.ToString());
        }

        private void OnCancel()
        {
            var messageDailog = new MessageDailog((result) =>
            {
                if (result == DialogResult.Ok)
                {
                    if (this.ParentViewModel != null)
                        this.ParentViewModel.ChildViewModel = null;
                    this.Unload();

                    if (this.CloseWindow != null)
                        this.CloseWindow();
                }
            }) { Caption = MessageResources.CancelWindowMessage, DialogButton = DialogButton.OkCancel, Title = TitleResources.Warning };
            MessengerInstance.Send(messageDailog);


        }
        #endregion

        #region Public Methods

        #endregion

        #region Override Methods
        public override void Initialize()
        {
            base.Initialize();

           
        }
        #endregion

        #region Private Methods

        #endregion
    }
}