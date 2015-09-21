
using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using VH.DataAccess;
using VH.Model;
using VH.Resources;
using VH.SimpleUI.Entities;

namespace VH.ViewModel
{
    public class UpdateCustomerRepairStatusViewModel : BaseViewModel<CustomerRepair>
    {
        #region Delegate
        public Action RefreshCustomerRepair { get; set; }
        #endregion

        #region Fields
        
        #endregion

        #region Properties
        #region Window Properties
        public override string Title
        {
            get
            {
                return TitleResources.UpdateRepairStatus ;
            }
        }


        public override double DialogStartupCustomHeight
        {
            get
            {
                return 370;
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
            get { return this._saveCommand ?? (this._saveCommand = new RelayCommand(OnSaveCustomerRepair, CanSaveCustomerRepair)); }
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get { return this._cancelCommand ?? (this._cancelCommand = new RelayCommand(OnCancelCustomer)); }
        }
        #endregion

        #region Constructors
        public UpdateCustomerRepairStatusViewModel(IMessenger messenger, UserLogin userLogin, CustomerRepair customerRepair)
            : base(messenger, userLogin)
        {
            this.Entity = customerRepair;
        }
        #endregion

        #region Override Methods
        
        #endregion

        #region Command Methods
        private void OnSaveCustomerRepair()
        {
            var returnStatus = false;
            returnStatus = CustomerAction.UpdateCustomerRepair(this.DBConnectionString, this.Entity);

            if (returnStatus)
            {
                if (RefreshCustomerRepair != null)
                    this.RefreshCustomerRepair();

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

        private bool CanSaveCustomerRepair()
        {
            return this.Entity != null && this.Entity.HasValueToUpdateStatus;
        }

        private void OnCancelCustomer()
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

        #region Private Methdsd
        
        #endregion

    }
}
