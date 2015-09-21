using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using VH.DataAccess;
using VH.ErrorLog;
using VH.Model;
using VH.Resources;

namespace VH.ViewModel
{
    public class HomeViewModel : BaseViewModel
    {
        #region Fields
        private ReminderCollection _reminderList;
        #endregion

        #region Properties
        public ReminderCollection ReminderList
        {
            get { return _reminderList ?? (_reminderList = new ReminderCollection()); }
            set
            {
                _reminderList = value;
                this.RaisePropertyChanged(() => this.ReminderList);
            }
        }
        #endregion

        #region Constructors

        public HomeViewModel(IMessenger messenger)
            : base(messenger)
        {
        }

        public HomeViewModel(IMessenger messenger, UserLogin userLogin)
            : base(messenger, userLogin)
        {
        }

        #endregion

        #region Override Methods
        public override void Initialize()
        {
            base.Initialize();
            this.GetReminders(DateTime.Now);
        }
        public override void HandleViewModeChanges(dynamic data)
        {
            //base.HandleViewModeChanges(data);
            var model = this.ParentViewModel as MainWindowViewModel;
            if (model != null)
            {
                model.IsHomeTabSelected = true;
            }
        }
        #endregion

        #region Private Methods
        private void GetReminders(DateTime dateTime)
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    this.ReminderList = ReminderAction.GetReminders(this.DBConnectionString,
                                                                    dateTime);


                });
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }
        #endregion
    }
}