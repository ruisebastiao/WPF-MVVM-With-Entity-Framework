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
    public class CustomerAppointmentViewModel : BaseViewModel<AppointmentCollection>
    {
        private Customer _selectedCustomer;

        #region Fields
        
        #endregion

        #region Properties
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                this.RaisePropertyChanged("SelectedCustomer");
            }
        }

        #endregion

        #region Constructors
        public CustomerAppointmentViewModel(IMessenger messenger, UserLogin userLogin, Customer customer)
            : base(messenger, userLogin)
        {
            SelectedCustomer = customer;
            GetCustomerAppointments();
        }
        #endregion

        #region Override Methods
        public override void Initialize()
        {
            base.Initialize();
            AddCommand = new RelayCommand(OnAddItem);
            DeleteCommand = new RelayCommand(OnDeleteItem, CanDeleteItem);
            EditCommand = new RelayCommand(OnEditItem, CanEditItem);
            RefreshCommand = new RelayCommand(OnRefreshItem);
        }
        #endregion
        #region Command Methods
        public override void OnAddItem()
        {
            try
            {
                var childVM = new AppointmentViewModel(this.Messenger, this.UserLogin, this.SelectedCustomer) { ParentViewModel = this };
                childVM.RefreshCustomerAppointment += this.GetCustomerAppointments;
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
            this.ParentViewModel.ShowProgressBar = true;
            var messageDailog = new MessageDailog(DeleteCustomerAppointment) { Caption = MessageResources.DeleteMessage, DialogButton = DialogButton.OkCancel, Title = TitleResources.Warning };
            MessengerInstance.Send(messageDailog);
        }

        private void DeleteCustomerAppointment(DialogResult dialogResult)
        {
            if (dialogResult == DialogResult.Ok)
            {
                Task.Factory.StartNew(() =>
                {
                    var result = AppointmentAction.DeleteAppointments(this.DBConnectionString,
                                                     this.Entity.InternalList.Where(x => x.IsSelected));

                    if (result)
                    {
                        this.GetCustomerAppointments();
                        var messageDailog = new MessageDailog() { Caption = MessageResources.DeletedSuccessfully, DialogButton = DialogButton.Ok, Title = TitleResources.Information };
                        MessengerInstance.Send(messageDailog);
                    }
                    else
                    {
                        var messageDailog = new MessageDailog() { Caption = MessageResources.DeletionFailed, DialogButton = DialogButton.Ok, Title = TitleResources.Error };
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

        public override bool CanDeleteItem()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Any(x => x.IsSelected));
        }

        public override void OnEditItem()
        {
            var childVM = new AppointmentViewModel(this.Messenger, this.UserLogin, this.Entity.InternalList.FirstOrDefault(x => x.IsSelected), this.SelectedCustomer) { ParentViewModel = this };
            childVM.RefreshCustomerAppointment += this.GetCustomerAppointments;
            var messageDailog = new VMMessageDailog() { ChildViewModel = childVM };
            MessengerInstance.Send(messageDailog);
        }
        public override bool CanEditItem()
        {
            return (this.Entity != null && this.Entity.InternalList != null && this.Entity.InternalList.Count(x => x.IsSelected) == 1);
        }

        public override void OnRefreshItem()
        {
            this.GetCustomerAppointments();
        }

        #endregion

        #region Private Methods
        private void GetCustomerAppointments()
        {
            Task.Factory.StartNew(() =>
                {
                    try
                    {
                        this.Entity = AppointmentAction.GetAppointments(this.DBConnectionString, this.SelectedCustomer);
                    }
                    catch (Exception exception)
                    {
                        NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured,
                                     ExceptionResources.ExceptionOccuredLogDetail);
                    }
                });
        }

        #endregion

    }
}