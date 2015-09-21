using System;
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
    public class AddCustomerWarrantyInformedViewModel : BaseViewModel<CustomerWarrantyInformed>
    {
        #region Delegate
        public Action RefreshCustomerWarrantyInformed { get; set; }
        #endregion

        #region Properties
        #region Window Properties
        public override string Title
        {
            get
            {
                return !IsInEditMode ? TitleResources.AddWarrantyInformed : TitleResources.EditWarrantyInformed;
            }
        }


        public override double DialogStartupCustomHeight
        {
            get
            {
                return 260;
            }
        }

        public override double DialogStartupCustomWidth
        {
            get
            {
                return 480;
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
            get { return this._saveCommand ?? (this._saveCommand = new RelayCommand(OnSaveCustomerWarrantyInformed, CanSaveCustomerWarrantyInformed)); }
        }

        private ICommand _cancelCommand;


        public ICommand CancelCommand
        {
            get { return this._cancelCommand ?? (this._cancelCommand = new RelayCommand(OnCancelCustomerWarrantyInformed)); }
        }
        #endregion

         #region Constructors
        public AddCustomerWarrantyInformedViewModel(IMessenger messenger, UserLogin userLogin, Customer customer)
            : base(messenger, userLogin)
        {
            this.Entity.Customer = customer;
            if (customer.ID != null) this.Entity.CustomerID = customer.ID.Value;
        }

        public AddCustomerWarrantyInformedViewModel(IMessenger messenger, UserLogin userLogin, CustomerWarrantyInformed customerWarrantyInformed)
            : base(messenger, userLogin)
        {
            this.IsInEditMode = true;
            this.Entity = customerWarrantyInformed;
        }
        #endregion

        #region Command Methods

        private void OnSaveCustomerWarrantyInformed()
        {
            var returnStatus = false;
            returnStatus = !IsInEditMode
                               ? CustomerAction.AddCustomerWarrantyInformed(this.DBConnectionString, this.Entity)
                               : CustomerAction.UpdateCustomerWarrantyInformed(this.DBConnectionString, this.Entity);

            if (returnStatus)
            {
                if (RefreshCustomerWarrantyInformed != null)
                    this.RefreshCustomerWarrantyInformed();

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

        private bool CanSaveCustomerWarrantyInformed()
        {
            return this.Entity != null;
        }

        private void OnCancelCustomerWarrantyInformed()
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
    }
}