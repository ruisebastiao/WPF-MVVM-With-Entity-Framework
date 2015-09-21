using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using VH.DataAccess;
using VH.Model;
using VH.SimpleUI.Entities;

namespace VH.ViewModel
{
    public class AddReminderViewModel : BaseViewModel<Reminder>
    {
        #region Delegate
        public Action<DateTime?> RefreshReminders { get; set; }
        #endregion

        #region Fields
        
        #endregion

        #region Properties

        #region Window Properties
        public override string Title
        {
            get
            {
                return !IsInEditMode ? Resources.TitleResources.AddReminder : Resources.TitleResources.EditReminder;
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
        public AddReminderViewModel(IMessenger messenger)
            : base(messenger)
        {
        }

        public AddReminderViewModel(IMessenger messenger, UserLogin userLogin)
            : base(messenger, userLogin)
        {
        }

        public AddReminderViewModel(IMessenger messenger, UserLogin userLogin, Reminder reminder)
            : base(messenger, userLogin)
        {
            this.IsInEditMode = true;
            this.Entity = reminder;
        }
        #endregion

        #region Command Methods
        private void OnSaveCallLog()
        {
            var returnStatus = false;
            returnStatus = !IsInEditMode ? ReminderAction.AddReminder(this.DBConnectionString, this.Entity) : ReminderAction.UpdateReminder(this.DBConnectionString, this.Entity);

            if (returnStatus)
            {
                if (RefreshReminders != null)
                    this.RefreshReminders(this.Entity.ReminderDate);

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
            return true;
            // return this.Entity != null && this.Entity.HasValueInAllRequiredField;
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
            }) { Caption = Resources.MessageResources.CancelWindowMessage, DialogButton = DialogButton.OkCancel, Title = Resources.TitleResources.Warning };
            MessengerInstance.Send(messageDailog);


        }
        #endregion
       
    }
}