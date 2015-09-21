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
using VH.Model.Utilities;

namespace VH.ViewModel
{
    public class AppointmentViewModel : BaseViewModel<Appointment>
    {
        #region Delegate
        public Action RefreshCustomerAppointment { get; set; }
        #endregion

        #region Fields
        
        #endregion

        #region Properties
        #region Window Properties
        public override string Title
        {
            get
            {
                return Resources.TitleResources.AddAppointment;
            }
        }

        public override double DialogStartupSizePercentage
        {
            get
            {
                return 95;
            }
        }

        public override double DialogStartupCustomHeight
        {
            get
            {
                return 600;
            }
        }

        public override double DialogStartupCustomWidth
        {
            get
            {
                return 650;
            }
        }

        public override DialogType DialogType
        {
            get
            {
                return DialogType.ByPercentage;
            }
        }
        #endregion
        public TimeSpan DefaultTimeInterval
        {
            get
            {
                return new TimeSpan(0,5,0);
            }
        }

        public AppointmentCollection AppointmentCollection
        {
            get { return _appointmentCollection; }
            set
            {
                _appointmentCollection = value;
                this.RaisePropertyChanged(() => AppointmentCollection);
            }
        }

        public ReminderCollection ReminderList
        {
            get { return _reminderList ?? (_reminderList = new ReminderCollection()); }
            set
            {
                _reminderList = value;
                this.RaisePropertyChanged(() => this.ReminderList);
            }
        }

        public CheckItemCollection CheckItemCollection
        {
            get { return _checkItemCollection ?? (_checkItemCollection = new CheckItemCollection()); }
            set
            {
                _checkItemCollection = value;
                this.RaisePropertyChanged(() =>this.CheckItemCollection);
            }
        }

        public string SelectedAppointmentTypes
        {
            get
            {
                if (this.CheckItemCollection.Any(x => x.IsChecked))
                {
                    string strConcate = string.Empty;
                    this.CheckItemCollection.ForEach(x =>
                        {
                            if (x.IsChecked)
                                strConcate += string.IsNullOrEmpty(strConcate) ? x.ItemName : ", " + x.ItemName;
                        });
                    return strConcate;
                }
                return string.Empty;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;

                var spiltedItem = value.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
                if (spiltedItem.Any())
                {
                    foreach (var curr in spiltedItem.Select(item => this.CheckItemCollection.FirstOrDefault(
                        x => x.ItemName.Trim().ToLower() == item.Trim().ToLower())).Where(curr => curr != null))
                    {
                        curr.IsChecked = true;
                    }
                }
            }
        }
        #endregion

        #region Command Properties
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get { return this._saveCommand ?? (this._saveCommand = new RelayCommand(OnSaveAppointment, CanSaveAppointment)); }
        }

        private ICommand _cancelCommand;
        private AppointmentCollection _appointmentCollection;
        private ReminderCollection _reminderList;
        private CheckItemCollection _checkItemCollection;

        public ICommand CancelCommand
        {
            get { return this._cancelCommand ?? (this._cancelCommand = new RelayCommand(OnCancelAppointment)); }
        }
        
        #endregion

        #region Constructors
        public AppointmentViewModel(IMessenger messenger)
            : base(messenger)
        {
         
        }

        public AppointmentViewModel(IMessenger messenger, UserLogin userLogin)
            : base(messenger, userLogin)
        {
            this.PropertyChangedCapture();

            this.GetAppointmentsByDate(this.Entity, true, true);

        
        }
        public AppointmentViewModel(IMessenger messenger, UserLogin userLogin, Customer customer)
            : base(messenger, userLogin)
        {
           this.PropertyChangedCapture();
            this.Entity.SelectedCustomer = customer;
            this.GetAppointmentsByDate(this.Entity, true, true);

        
        }

        public AppointmentViewModel(IMessenger messenger, UserLogin userLogin, Appointment appointment, Customer customer = null)
            : base(messenger, userLogin)
        {
            this.Entity = appointment;
            this.PropertyChangedCapture();
            this.Entity.SelectedCustomer = customer;
            this.GetAppointmentsByDate(this.Entity, true, true);

          }
        #endregion

        #region Command Methods
        private bool CanSaveAppointment()
        {
            return this.Entity != null && this.Entity.IsSaveEnabled;
        }

        private void OnSaveAppointment()
        {
            this.ShowProgressBar = true;
            //if (!this.Entity.IsValidAppiontSechduleDateRange)
            //{
            //    var msg = new MessageDailog() { Caption = Resources.MessageResources.InvalidAppointmentDate, DialogButton = DialogButton.Ok, Title = Resources.TitleResources.Warning };
            //    MessengerInstance.Send(msg);
            //    return;
            //}
            bool returnStatus = false;
            if (!this.Entity.ID.HasValue)
                returnStatus = AppointmentAction.AddAppointment(this.DBConnectionString, this.Entity);
            if (this.Entity.ID.HasValue)
                returnStatus = AppointmentAction.UpdateAppointment(this.DBConnectionString, this.Entity);

            if (returnStatus)
            {
                this.Entity = new Appointment();
                this.PropertyChangedCapture();
                this.GetAppointmentsByDate(this.Entity, true, true);

                if (RefreshCustomerAppointment != null)
                    RefreshCustomerAppointment();
                this.CheckItemCollection.ForEach(x => x.IsChecked = false);
            }

            var messageDailog = returnStatus ? new MessageDailog() { Caption = Resources.MessageResources.DataSavedSuccessfully, DialogButton = DialogButton.Ok, Title = Resources.TitleResources.Information } :
                new MessageDailog() { Caption = Resources.MessageResources.DataSavedFailed, DialogButton = DialogButton.Ok, Title = Resources.TitleResources.Error };
            MessengerInstance.Send(messageDailog);

            this.ShowProgressBar = false;
        }

        private void OnCancelAppointment()
        {
            var messageDailog = new MessageDailog((result) =>
            {
                if (result == DialogResult.Ok)
                {
                    this.Entity = new Appointment();
                    //FindNextAvailableAppointmentTime();
                    this.PropertyChangedCapture();
                    OnRefreshAppointment();
                    this.CheckItemCollection.ForEach(x => x.IsChecked = false);
                }
            }) { Caption = Resources.MessageResources.ClearMessage, DialogButton = DialogButton.OkCancel, Title = Resources.TitleResources.Warning };
            MessengerInstance.Send(messageDailog);
        }

        private bool CanDeleteAppointment()
        {
            return (this.AppointmentCollection != null && this.AppointmentCollection.InternalList != null && this.AppointmentCollection.InternalList.Any(x => x.IsSelected));
        }

        private void OnDeleteAppointment()
        {
            ShowProgressBar = true;
            var messageDailog = new MessageDailog(DeleteAppointment) { Caption = Resources.MessageResources.DeleteMessage, DialogButton = DialogButton.OkCancel, Title = Resources.TitleResources.Warning };
            MessengerInstance.Send(messageDailog);
        }

       
        private void OnRefreshAppointment()
        {
            this.ShowProgressBar = true;
            this.GetAppointmentsByDate(this.Entity, true);
        }

        private bool CanEditAppointment()
        {
            return this.AppointmentCollection != null &&
                   (this.AppointmentCollection.InternalList.Count(x => x.IsSelected) == 1);
        }

        private void OnEditAppointment()
        {
            this.Entity = this.AppointmentCollection.InternalList.FirstOrDefault(x => x.IsSelected);
            if (this.Entity != null) this.SelectedAppointmentTypes = this.Entity.AppointmentType;
            //this.CheckTimeOverLap(this.AppointmentCollection, this.Entity);
            this.PropertyChangedCapture();
        }
        #endregion

        #region Override Methods
        public override void HandleViewModeChanges(dynamic data)
        {
            //base.HandleViewModeChanges(data);
            if (this.ParentViewModel is MainWindowViewModel)
            {
                ((MainWindowViewModel) this.ParentViewModel).IsAppointmentTabSelected = true;
            }
                
        }

        public override void Initialize()
        {
            base.Initialize();

            DeleteCommand = new RelayCommand(OnDeleteItem, CanDeleteItem);
            EditCommand = new RelayCommand(OnEditItem, CanEditItem);
            RefreshCommand = new RelayCommand(OnRefreshItem);
            GetReminders(this.Entity.AppointmentDate);

            var appointmentTypes = Enum.GetValues(typeof(AppointmentType));
            foreach (var appointmentType in appointmentTypes)
            {
                this.CheckItemCollection.Add(new CheckItem() { ItemName = appointmentType.ToString() });
            }

            foreach (var item in this.CheckItemCollection)
            {
                item.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == "IsChecked")
                        {
                            this.RaisePropertyChanged(() => this.SelectedAppointmentTypes);
                            this.Entity.AppointmentType = this.SelectedAppointmentTypes;
                        }
                    };
            }
        }
        #endregion

        #region Override Command Methods
        public override  void OnDeleteItem()
        {
            this.OnDeleteAppointment();
        }
        public override  bool CanDeleteItem()
        {
            return this.CanDeleteAppointment();
        }
        public override  void OnEditItem()
        {
            this.OnEditAppointment();
        }
        public override  bool CanEditItem()
        {
            return this.CanEditAppointment();
        }
        public override  void OnRefreshItem()
        {
            this.OnRefreshAppointment();
        }
        #endregion

        #region Public Methods

        #endregion

        #region Private Methods
        private void DeleteAppointment(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Ok)
            {
                Task.Factory.StartNew(() =>
                {
                   var result =  AppointmentAction.DeleteAppointments(this.DBConnectionString,
                                                    this.AppointmentCollection.InternalList.Where(x => x.IsSelected));
                    if (RefreshCustomerAppointment != null)
                        RefreshCustomerAppointment();
                    
                    if (result)
                    {
                        GetAppointmentsByDate(this.Entity);
                        var messageDailog = new MessageDailog() { Caption = Resources.MessageResources.DeletedSuccessfully, DialogButton = DialogButton.Ok, Title = Resources.TitleResources.Information };
                        MessengerInstance.Send(messageDailog);
                    }
                    else
                    {
                        var messageDailog = new MessageDailog() { Caption = Resources.MessageResources.DeletionFailed, DialogButton = DialogButton.Ok, Title = Resources.TitleResources.Error };
                        MessengerInstance.Send(messageDailog);
                    }
                    ShowProgressBar = false;
                });
            }
            else
            {
                ShowProgressBar = false;
            }
        }
        private void PropertyChangedCapture()
        {
            try
            {
                this.Entity.PropertyChanged += (sender, args) =>
                    {
                        if (args.PropertyName == "AppointmentDate")
                        {
                            Task.Factory.StartNew(() =>
                                {
                                    var item = (Appointment)sender;
                                   this.GetReminders(item.AppointmentDate);
                                });
                        }
                    };
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }

        //private void CheckTimeOverLap(AppointmentCollection collection, Appointment appointment)
        //{
        //    try
        //    {
        //        collection.ObservableList.ForEach(x => x.IsAppointmentOverlapped = false);
        //        appointment.IsAppointmentOverlapped = false;
        //        var query =
        //            collection.ObservableList.Where(
        //                x =>
        //                x.EndTime > appointment.BeginTime &&
        //                x.BeginTime < appointment.EndTime);
        //        var appointments = query as IList<Appointment> ?? query.ToList();
        //        if (appointments.Any(x =>
        //            {
        //                if (!appointment.ID.HasValue)
        //                    return true;
        //                return appointment.ID != x.ID;
        //            }))
        //        {
        //            appointments.ForEach(x => x.IsAppointmentOverlapped = true);
        //            appointment.IsAppointmentOverlapped = true;
        //        }

        //    }
        //    catch (Exception exception)
        //    {
        //        NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
        //                            ExceptionResources.ExceptionOccuredLogDetail);
        //    }
        //}

        private void GetAppointmentsByDate(Appointment appointment, bool doAsync = false, bool findNextAppointment = false)
        {
            try
            {
                if (doAsync)
                    Task.Factory.StartNew(() =>
                        {
                            this.AppointmentCollection = AppointmentAction.GetAppointments(this.DBConnectionString,
                                                                                           appointment.AppointmentDate);
                           
                            this.ShowProgressBar = false;
                        });
                else
                {
                    this.AppointmentCollection = AppointmentAction.GetAppointments(this.DBConnectionString,
                                                                                   appointment.AppointmentDate);
                    this.ShowProgressBar = false;
                }
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                    ExceptionResources.ExceptionOccuredLogDetail);
            }
        }

        //private void FindNextAvailableAppointmentTime()
        //{
        //    try
        //    {
        //        if (this.AppointmentCollection != null &&
        //            this.AppointmentCollection.InternalList.Any(x => x.BeginTime == this.Entity.BeginTime))
        //        {
        //            this.Entity.BeginTime = this.Entity.BeginTime.AddMinutes(30);
        //            FindNextAvailableAppointmentTime();
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
        //                            ExceptionResources.ExceptionOccuredLogDetail);
        //    }
        //}

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