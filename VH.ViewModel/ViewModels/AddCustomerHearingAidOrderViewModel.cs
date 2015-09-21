using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using VH.DataAccess;
using VH.Model;
using VH.Model.Utilities;
using VH.Resources;
using VH.SimpleUI.Entities;

namespace VH.ViewModel
{
    public class AddCustomerHearingAidOrderViewModel : BaseViewModel<CustomerHearingAidOrder>
    {
        #region Fields
        
        #endregion

        #region Delegate
        public Action RefreshCustomerHearingAIDOrder { get; set; }
        #endregion

        #region Properties

        #region Window Properties
        public override string Title
        {
            get
            {
                return !IsInEditMode ? TitleResources.AddCustomerHearingAidOrder : TitleResources.EditCustomerHearingAidOrder;
            }
        }


        public override double DialogStartupCustomHeight
        {
            get
            {
                return 700;
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

        public ICommand CancelCommand
        {
            get { return this._cancelCommand ?? (this._cancelCommand = new RelayCommand(OnCancel)); }
        }
        #endregion

        #region Constructors
        public AddCustomerHearingAidOrderViewModel(IMessenger messenger, UserLogin userLogin, CustomerHearingAidOrder customerHearingAidOrder)
            : base(messenger, userLogin)
        {
            this.IsInEditMode = true;
            this.Entity = customerHearingAidOrder;
        }

        public AddCustomerHearingAidOrderViewModel(IMessenger messenger, UserLogin userLogin, Customer customer)
            : base(messenger, userLogin)
        {
            this.Entity.Customer = customer;
            this.Entity.CustomerID = customer.ID.Value;

        }

        #endregion

        #region Command Methods

        private void OnSave()
        {
            var returnStatus = false;
            returnStatus = !IsInEditMode
                               ? CustomerAction.AddCustomerHearingAidOrder(this.DBConnectionString, this.Entity)
                               : CustomerAction.UpdateCustomerHearingAidOrder(this.DBConnectionString, this.Entity);

            if (returnStatus)
            {
                if (RefreshCustomerHearingAIDOrder != null)
                    this.RefreshCustomerHearingAIDOrder();

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
            return this.Entity != null && !string.IsNullOrEmpty(this.Entity.OrderDate.ToString()) ;
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

        #endregion

        #region Private Methods

        #endregion
    }
}