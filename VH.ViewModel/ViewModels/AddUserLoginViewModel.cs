using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using VH.DataAccess;
using VH.Model;
using VH.SimpleUI.Entities;

namespace VH.ViewModel
{
    public class AddUserLoginViewModel : BaseViewModel<UserLogin>
    {
        #region Delegate
        public Action RefreshUserLogin { get; set; }
        #endregion

        #region Fields
        
        #endregion

        #region Properties
        #region Window Properties
        public override string Title
        {
            get
            {
                return !IsInEditMode ? Resources.TitleResources.AddUserLogin : Resources.TitleResources.EditUserLogin;
            }
        }


        public override double DialogStartupCustomHeight
        {
            get
            {
                return 340;
            }
        }

        public override double DialogStartupCustomWidth
        {
            get
            {
                return 490;
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

        #region Commands
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get { return this._saveCommand ?? (this._saveCommand = new RelayCommand(OnSaveCallLog, CanSaveCallLog)); }
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get { return this._cancelCommand ?? (this._cancelCommand = new RelayCommand(OnCancelCustomer)); }
        }
        #endregion

       #region Constructors

        public AddUserLoginViewModel(IMessenger messenger, UserLogin userLogin)
            : base(messenger, userLogin)
        {
        }

        public AddUserLoginViewModel(IMessenger messenger, UserLogin userLogin, UserLogin currUserLogin)
            : base(messenger, userLogin)
        {
            this.IsInEditMode = true;
            this.Entity = currUserLogin;
            this.Entity.IsInEditMode = true;
        }

        #endregion

        #region Override Methods
        private void OnSaveCallLog()
        {
            var returnStatus = false;
            returnStatus = !IsInEditMode ? UserLoginAction.AddUserLogin(this.DBConnectionString, this.Entity) : UserLoginAction.UpdateUserLogin(this.DBConnectionString, this.Entity);

            if (returnStatus)
            {
                this.Entity.ClearValues();

                if (RefreshUserLogin != null)
                    this.RefreshUserLogin();

                var messageDailog = new MessageDailog()
                {
                    Caption = Resources.MessageResources.DataSavedSuccessfully,
                    DialogButton = DialogButton.Ok,
                    Title = Resources.TitleResources.Information
                };

                MessengerInstance.Send(messageDailog);

                if (this.CloseWindow != null)
                    this.CloseWindow();

                
            }
            else
            {
                var messageDailog = new MessageDailog() { Caption = Resources.MessageResources.DataSavedFailed, DialogButton = DialogButton.Ok, Title = Resources.TitleResources.Error };
                MessengerInstance.Send(messageDailog);
            }
        }
        private bool CanSaveCallLog()
        {
            return this.Entity != null && ( this.IsInEditMode ? this.Entity.VaidateChangePassword() && this.Entity.VaidateCurrentPassword() : ( this.Entity.VaidateNewPassword() && !string.IsNullOrEmpty(this.Entity.LoginName)));
        }

        private void OnCancelCustomer()
        {
            var messageDailog = new MessageDailog((result) =>
            {
                if (result == DialogResult.Ok)
                {
                    this.Entity.ClearValues();

                    if (this.ParentViewModel != null)
                        this.ParentViewModel.ChildViewModel = null;
                    this.Unload();

                    if (this.CloseWindow != null)
                        this.CloseWindow();

                  
                }
            }) { Caption = Resources.MessageResources.CancelWindowMessage, DialogButton = DialogButton.OkCancel, Title = Resources.TitleResources.Warning };
            MessengerInstance.Send(messageDailog);


        }
        #endregion

        #region Private Methods
        
        #endregion

    }
}