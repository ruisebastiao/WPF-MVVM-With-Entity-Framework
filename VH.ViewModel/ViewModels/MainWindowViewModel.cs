using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using VH.Bases;
using VH.DataAccess;
using VH.ErrorLog;
using VH.Model;
using VH.Resources;
using VH.SimpleUI.Entities;

namespace VH.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region · Fields ·

        private WindowState windowState;
        private string userName;
        private bool _isVisibile;
        private string _searchText;
        private IDictionary<String, String> _customerSearchList;
        private int _selectedCustomerID;
        private bool _isPopupOpen;

        #endregion

        #region · Commands Fileds·

        private ICommand _maximizeCommand;
        private ICommand _minimizeCommand;
        private ICommand _shutdownCommand;
        private ICommand _showAboutBoxCommand;

        private ICommand _customerCommand;
        private ICommand _searchCommand;
        private ICommand _selectSearchItemCommand;
        private ICommand _appointmentCommand;


        private ICommand _homeCommand;
        private ICommand _callRegistryCommand;
        private bool _isHomeSelected;
        private bool _isCustomerTabSelected;
        private bool _isAppointmentTabSelected;
        private bool _isCallRegistryTabSelected;
        private ICommand _settingCommand;
        private bool _isSettingTabSelected;
        private ICommand _miscellaneousCommand;
        private bool _isMiscellaneousTabSelected;

        #endregion

        #region · Commands ·

        /// <summary>
        /// Gets the maximize command.
        /// </summary>
        /// <value>The maximize command.</value>
        public ICommand MaximizeCommand
        {
            get
            {
                if (this._maximizeCommand == null)
                {
                    this._maximizeCommand = new RelayCommand(OnMaximizeWindow);
                }

                return this._maximizeCommand;
            }
        }

        /// <summary>
        /// Gets the minimize command.
        /// </summary>
        /// <value>The minimize command.</value>
        public ICommand MinimizeCommand
        {
            get { return this._minimizeCommand ?? (this._minimizeCommand = new RelayCommand(OnMinimizeWindow)); }
        }


        /// <summary>
        /// Gets the about box command.
        /// </summary>
        /// <value>The about box command.</value>
        public ICommand ShowAboutBoxCommand
        {
            get { return this._showAboutBoxCommand ?? (this._showAboutBoxCommand = new RelayCommand(OnShowAboutBoxCommand)); }
        }

        /// <summary>
        /// Gets the shutdown command
        /// </summary>
        public ICommand ShutdownCommand
        {
            get { return this._shutdownCommand ?? (this._shutdownCommand = new RelayCommand(OnShutdown)); }
        }

        /// <summary>
        /// Gets the log off command
        /// </summary>
        public ICommand CloseSessionCommand
        {
            get { return this._customerCommand ?? (this._customerCommand = new RelayCommand(OnCloseSession)); }
        }

        public ICommand CustomerCommand
        {
            get
            {
                return this._customerCommand ??
                       (this._customerCommand = new RelayCommand(OnCustomerSelect, CanCustomerSelect));
            }
        }

        public ICommand SearchCommand
        {
            get { return this._searchCommand ?? (this._searchCommand = new RelayCommand(OnSearchSelect, CanSearchSelect)); }
        }

        public ICommand SelectSearchItemCommand
        {
            get
            {
                return this._selectSearchItemCommand ??
                       (this._selectSearchItemCommand = new RelayCommand(OnSelectSearchItem));
            }
        }

        public ICommand AppointmentCommand
        {
            get { return this._appointmentCommand ?? (this._appointmentCommand = new RelayCommand(OnAppointmentSelected)); }
        }

        public ICommand HomeCommand
        {
            get { return this._homeCommand ?? (this._homeCommand = new RelayCommand(OnHomeSelected)); }
        }

        public ICommand CallRegistryCommand
        {
            get { return _callRegistryCommand ?? (_callRegistryCommand = new RelayCommand(OnCallRegistrySelected)); }
        }

        public ICommand SettingCommand
        {
            get { return _settingCommand ?? (_settingCommand = new RelayCommand(OnSettingSelected)); }
        }

        public ICommand MiscellaneousCommand
        {
            get { return _miscellaneousCommand ?? (_miscellaneousCommand = new RelayCommand(OnMiscellaneousSelected)); }
        }
        

        #endregion

        #region Constructors

        public MainWindowViewModel(IMessenger messenger)
            : base(messenger)
        {
            if (UserLogin == null)
                this.ChildViewModel = new UserLoginViewModel(messenger) {ParentViewModel = this};

            this.OnHomeSelected();
            //this.ContentViewModel = new AddCustomerViewModel(messenger) { ParentViewModel = this };
        }

        #endregion

        #region · Properties ·

        /// <summary>
        /// Gets or sets the state of the window.
        /// </summary>
        /// <value>The state of the window.</value>
        public WindowState WindowState
        {
            get { return this.windowState; }
            set
            {
                if (this.windowState == value) return;
                this.windowState = value;
                this.RaisePropertyChanged(() => WindowState);
            }
        }

        /// <summary>
        /// Gets the logged in user name
        /// </summary>
        public string UserName
        {
            get
            {
                return (this.UserLogin != null && !String.IsNullOrEmpty(this.UserLogin.LoginName)
                            ? this.UserLogin.LoginName
                            : "No user Logged in");
            }
            //private set
            //{
            //    this.userName = value;
            //    this.RaisePropertyChanged(() => UserName);
            //}
        }

        public bool IsHomeTabSelected
        {
            get { return _isHomeSelected; }
            set
            {
                _isHomeSelected = value;
                this.RaisePropertyChanged(() => this.IsHomeTabSelected);
            }
        }

        public bool IsCustomerTabSelected
        {
            get { return _isCustomerTabSelected; }
            set
            {
                _isCustomerTabSelected = value;
                this.RaisePropertyChanged(() => this.IsCustomerTabSelected);
            }
        }

        public bool IsAppointmentTabSelected
        {
            get { return _isAppointmentTabSelected; }
            set
            {
                _isAppointmentTabSelected = value;
                this.RaisePropertyChanged(() => this.IsAppointmentTabSelected);
            }
        }

        public bool IsCallRegistryTabSelected
        {
            get { return _isCallRegistryTabSelected; }
            set
            {
                _isCallRegistryTabSelected = value;
                this.RaisePropertyChanged(() => this.IsCallRegistryTabSelected);
            }
        }

        public bool IsMiscellaneousTabSelected
        {
            get { return _isMiscellaneousTabSelected; }
            set
            {
                _isMiscellaneousTabSelected = value;
                this.RaisePropertyChanged(() => this.IsMiscellaneousTabSelected);
            }
        }

        public bool IsSettingTabSelected
        {
            get { return _isSettingTabSelected; }
            set
            {
                _isSettingTabSelected = value;
                this.RaisePropertyChanged(() => this.IsSettingTabSelected);
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                this.RaisePropertyChanged(() => SearchText);
                GetSearchList(_searchText);
            }
        }

        public IDictionary<String, String> CustomerSearchList
        {
            get { return _customerSearchList; }
            set
            {
                _customerSearchList = value;
                this.RaisePropertyChanged(() => CustomerSearchList);
            }
        }

        public bool IsVisibile
        {
            get { return _isVisibile; }
            set
            {
                _isVisibile = value;
                this.RaisePropertyChanged(() => IsVisibile);
            }
        }

        public int SelectedCustomerID
        {
            get { return _selectedCustomerID; }
            set
            {
                _selectedCustomerID = value;
                this.RaisePropertyChanged(() => SelectedCustomerID);
            }
        }

        public bool IsPopupOpen
        {
            get { return _isPopupOpen; }
            set
            {
                _isPopupOpen = value;
                this.RaisePropertyChanged(() => IsPopupOpen);
            }
        }

        #endregion

        #region · Command Actions ·

        private void OnHomeSelected()
        {
            this.ContentViewModel = null;
            this.ContentViewModel = new HomeViewModel(this.Messenger, this.UserLogin) {ParentViewModel = this};
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<IBaseViewModel>(this.ContentViewModel);
        }

        /// <summary>
        /// Handles the shutdown command action
        /// </summary>
        private void OnShutdown()
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Closes the session
        /// </summary>
        private void OnCloseSession()
        {

        }

        /// <summary>
        /// Maximizes the window.
        /// </summary>
        private void OnMaximizeWindow()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }

        }

        /// <summary>
        /// Handles the minimize window command action
        /// </summary>
        private void OnMinimizeWindow()
        {
            this.WindowState = WindowState.Minimized;
        }

        private void OnShowAboutBoxCommand()
        {

        }

        private void OnCustomerSelect()
        {
            this.ContentViewModel = null;
            this.ContentViewModel = new CustomerViewModel(this.Messenger, this.UserLogin) {ParentViewModel = this};
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<IBaseViewModel>(this.ContentViewModel);
        }

        private bool CanCustomerSelect()
        {
            return true;
        }

        private void OnSearchSelect()
        {
            this.ContentViewModel = null;
        }

        private void OnAppointmentSelected()
        {
            this.ContentViewModel = null;
            this.ContentViewModel = new AppointmentViewModel(this.Messenger, this.UserLogin) {ParentViewModel = this};
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<IBaseViewModel>(this.ContentViewModel);
        }

        private void OnCallRegistrySelected()
        {
            this.ContentViewModel = null;
            this.ContentViewModel = new CallRegistryViewModel(this.Messenger, this.UserLogin) { ParentViewModel = this };
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<IBaseViewModel>(this.ContentViewModel);
        }

        private bool CanSearchSelect()
        {
            return true;
        }

        private void OnSelectSearchItem()
        {
            IsPopupOpen = false;
            this.SearchText = string.Empty;

            if (this.SelectedCustomerID != 0)
            {
                var selectedCustomer = CustomerAction.GetCustomerByID(this.DBConnectionString,
                                                                                this.SelectedCustomerID.ToString(
                                                                                    CultureInfo.InvariantCulture));
                if (this.ContentViewModel != null && this.ContentViewModel.GetType() == typeof(AppointmentViewModel))
                {
                    var vm = ((AppointmentViewModel)this.ContentViewModel);
                    vm.Entity.SelectedCustomer = selectedCustomer;
                    return;
                }

                if (selectedCustomer != null)
                {
                    this.ContentViewModel = null;
                    this.ContentViewModel = new CustomerDetailViewModel(this.Messenger, this.UserLogin, selectedCustomer) { ParentViewModel = this };
                    GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<IBaseViewModel>(this.ContentViewModel);
                }

            }
        }

        private void OnSettingSelected()
        {
            this.ContentViewModel = null;
            this.ContentViewModel = new SettingsViewModel(this.Messenger, this.UserLogin) { ParentViewModel = this };
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<IBaseViewModel>(this.ContentViewModel);
        }

        private void OnMiscellaneousSelected()
        {
            this.ContentViewModel = null;
            this.ContentViewModel = new MiscellaneousViewModel(this.Messenger, this.UserLogin) { ParentViewModel = this };
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<IBaseViewModel>(this.ContentViewModel);
        }
        #endregion

        #region Override Methods

        public override void Initialize()
        {
            base.Initialize();

            this.SelectDefaultTab();
        }

        #endregion

        #region Private Methods

        private void GetSearchList(string strSearchText)
        {
            try
            {
                if (string.IsNullOrEmpty(strSearchText))
                {
                    CustomerSearchList = null;
                    IsPopupOpen = false;
                    return;
                }

                Task.Factory.StartNew(() =>
                {
                    var customerActions = new CustomerAction();
                    CustomerSearchList = customerActions.GetCustomerSearchDictionary(this.DBConnectionString,
                                                                                     strSearchText);
                    IsPopupOpen = true;
                    if (CustomerSearchList == null || !CustomerSearchList.Any())
                    {
                        CustomerSearchList = new Dictionary<String, String>();
                        CustomerSearchList.Add(new KeyValuePair<string, string>("", "No records available"));
                    }
                });

                this.RaisePropertyChanged(() => CustomerSearchList);
            }
            catch (Exception exception)
            {
                NLogLogger.LogError(exception, TitleResources.Error, ExceptionResources.ExceptionOccured, ExceptionResources.ExceptionOccuredLogDetail);
            }
         
        }

        private void SelectDefaultTab()
        {
            if (!this.IsHomeTabSelected && !this.IsAppointmentTabSelected && !this.IsCustomerTabSelected &&
                !this.IsCallRegistryTabSelected)
                this.IsHomeTabSelected = true;
        }

        #endregion
    }

}

