using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using VH.Bases;
using VH.Model;
using VH.SimpleUI.Entities;

namespace VH.ViewModel
{
    public abstract class BaseViewModel : ViewModelBase, IBaseViewModel
    {
        #region Fileds
        private IBaseViewModel _childViewModel;
        private IBaseViewModel _parentViewModel;
        private IMessenger _messenger;
        private UserLogin _userLogin;
        private string _statusMessage;
        private string _notificationMessage;
        private ViewModeType _viewMode = ViewModeType.Default;
        private bool _isActive = true;
        private IBaseViewModel _contentViewModel;
        private bool _showProgressBar;

        #endregion

        #region Properties

        public bool ShowProgressBar
        {
            get { return _showProgressBar; }
            set
            {
                _showProgressBar = value;
                this.RaisePropertyChanged(() => this.ShowProgressBar);
            }
        }

        public dynamic Messenger
        {
            get { return _messenger; }
            set
            {
                _messenger = value;
                this.RaisePropertyChanged(() => this.Messenger);
            }
        }

        public virtual string Title
        {
            get
            {
                return "Sample Window";
            }
        }

        public virtual double DialogStartupSizePercentage
        {
            get { return 40; }
        }

        public virtual double DialogStartupCustomHeight { get { return 400; } }
        public virtual double DialogStartupCustomWidth { get { return 400; } }

        public virtual DialogType DialogType
        {
            get { return DialogType.ByPercentage; }
        }
        public virtual bool ShowMinimizeButton { get { return false; }}
        public virtual bool ShowMaximizeButton { get { return false; } }
        public virtual bool ShowCloseButton { get { return true; } }

        public virtual double WindowHeight
        {
            get { return 200; }
        }

        public virtual double WindowWidth
        {
            get { return 200; }
        }

        public IBaseViewModel ParentViewModel
        {
            get { return _parentViewModel; }
            set
            {
                _parentViewModel = value;
                this.RaisePropertyChanged(() => ParentViewModel);
            }
        }

        public IBaseViewModel ChildViewModel
        {
            get { return _childViewModel; }
            set
            {
                _childViewModel = value;
                this.RaisePropertyChanged(() => ChildViewModel);
            }
        }

        public IBaseViewModel ContentViewModel
        {
            get { return _contentViewModel; }
            set
            {
                _contentViewModel = value;
                this.RaisePropertyChanged(() => ContentViewModel);
            }
        }

        public Action CloseWindow { get; set; }

        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                this.RaisePropertyChanged(() => StatusMessage);
            }
        }

        public string NotificationMessage
        {
            get { return _notificationMessage; }
            set
            {
                _notificationMessage = value;
                this.RaisePropertyChanged(() => StatusMessage);
            }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                this.RaisePropertyChanged(() => IsActive);
            }
        }

        public UserLogin UserLogin
        {
            get { return _userLogin; }
            private set
            {
                _userLogin = value;
                this.RaisePropertyChanged(() => UserLogin);
            }
        }

        public string DBConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["VHEAPDB"] != null
                           ? ConfigurationManager.ConnectionStrings["VHEAPDB"].ConnectionString
                           : "";
            }
        }

        public ViewModeType ViewMode
        {
            get { return _viewMode; }
            set
            {
                _viewMode = value;
                this.RaisePropertyChanged(() => ViewMode);
            }
        }

        protected bool IsInEditMode { get; set; }
        #endregion

        #region Command Properties
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        #endregion

        #region Constructors

        protected BaseViewModel(IMessenger messenger)
        {
            Messenger = messenger;
            Initialize();
        }

        protected BaseViewModel(IMessenger messenger, UserLogin userLogin)
        {
            Messenger = messenger;
            Initialize();
            if (UserLogin == null)
                UserLogin = userLogin;
        }
        #endregion

        #region Public Methods
        public void UpdateUserLogin(dynamic userLogin)
        {
            if (UserLogin == null)
                UserLogin = userLogin;
        }

        public virtual void HandleViewModeChanges(dynamic data)
        {
            
        }

        public virtual void Initialize()
        {
            CloseCommand = new RelayCommand(() =>  Application.Current.Shutdown());
        }

        public void CloseChild()
        {
            if (this.ParentViewModel != null)
                this.ParentViewModel.ChildViewModel = null;
        }

        #endregion

        #region Virtual Methods
        public virtual void Unload()
        {
            this.UserLogin = null;
            this.ParentViewModel = null;
            this.ContentViewModel = null;
            CloseWindow = null;
            Messenger = null;
            this.AddCommand = null;
            this.EditCommand = null;
            this.DeleteCommand = null;
            this.RefreshCommand = null;
            this.CloseCommand = null;
        }

        #region Command Methods
        public virtual void OnAddItem()
        {

        }
        public virtual bool CanAddItem()
        {
            return true;
        }
        public virtual void OnDeleteItem()
        {

        }
        public virtual bool CanDeleteItem()
        {
            return true;
        }
        public virtual void OnEditItem()
        {

        }
        public virtual bool CanEditItem()
        {
            return true;
        }
        public virtual void OnRefreshItem()
        {

        }
        public virtual bool CanRefreshItem()
        {
            return true;
        }
        #endregion

        #endregion
    }

    public abstract class BaseViewModel<TEntity> : ViewModelBase, IBaseViewModel where TEntity: class, new()
    {
        #region Fileds
        private IBaseViewModel _childViewModel;
        private IBaseViewModel _parentViewModel;
        private IMessenger _messenger;
        private TEntity entity;
        private TEntity originalEntity;
        private string _statusMessage;
        private string _notificationMessage;
        private ViewModeType _viewMode = ViewModeType.Default;
        private bool _isActive = true;
        private IBaseViewModel _contentViewModel;
        private bool _showProgressBar;

        #endregion

        #region Properties

        public bool ShowProgressBar
        {
            get { return _showProgressBar; }
            set
            {
                _showProgressBar = value;
                this.RaisePropertyChanged(() => this.ShowProgressBar);
            }
        }

        public dynamic Messenger
        {
            get { return _messenger; }
            set
            {
                _messenger = value;
                this.RaisePropertyChanged(() => this.Messenger);
            }
        }

        public virtual string Title
        {
            get
            {
                return "Sample Window";
            }
        }

        public virtual double DialogStartupSizePercentage
        {
            get { return 40; }
        }
        public virtual double DialogStartupCustomHeight { get { return 400; } }

        public virtual double DialogStartupCustomWidth { get { return 400; } }

        public virtual bool ShowMinimizeButton { get { return false; } }

        public virtual bool ShowMaximizeButton { get { return false; } }

        public virtual bool ShowCloseButton { get { return true; } }

        public virtual double WindowHeight
        {
            get { return 200; }
        }

        public virtual double WindowWidth
        {
            get { return 200; }
        }

        public virtual DialogType DialogType
        {
            get { return DialogType.ByPercentage; }
        }

        public ViewModeType ViewMode
        {
            get { return _viewMode; }
            set
            {
                _viewMode = value;
                this.RaisePropertyChanged(() => ViewMode);
            }
        }

        public TEntity Entity
        {
            get { return this.entity; }
            protected set
            {
                if (!Object.ReferenceEquals(this.entity, value))
                {
                    this.entity = value;
                    this.RaisePropertyChanged(() => Entity);
                }
            }
        }

        protected TEntity OriginalEntity
        {
            get { return this.originalEntity; }
            set { this.originalEntity = value; }
        }
        public IBaseViewModel ParentViewModel
        {
            get { return _parentViewModel; }
            set
            {
                _parentViewModel = value;
                this.RaisePropertyChanged(() => ParentViewModel);
            }
        }

        public IBaseViewModel ChildViewModel
        {
            get { return _childViewModel; }
            set
            {
                _childViewModel = value;
                this.RaisePropertyChanged(() => ChildViewModel);
            }
        }

        public IBaseViewModel ContentViewModel
        {
            get { return _contentViewModel; }
            set
            {
                _contentViewModel = value;
                this.RaisePropertyChanged(() => ContentViewModel);
            }
        }

        public Action CloseWindow { get; set; }

        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                this.RaisePropertyChanged(() => StatusMessage);
            }
        }

        public string NotificationMessage
        {
            get { return _notificationMessage; }
            set
            {
                _notificationMessage = value;
                this.RaisePropertyChanged(() => StatusMessage);
            }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                this.RaisePropertyChanged(() => IsActive);
            }
        }

        public UserLogin UserLogin { get;  private set; }

        public string DBConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["VHEAPDB"] != null
                           ? ConfigurationManager.ConnectionStrings["VHEAPDB"].ConnectionString
                           : "";
            }
        }

        protected bool IsInEditMode { get; set; }
        #endregion

        #region Command Properties
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        #endregion

        #region Constructors

        protected BaseViewModel(IMessenger messenger)
        {
            Messenger = messenger;
            this.Entity = new TEntity();
            Initialize();
        }

        protected BaseViewModel(IMessenger messenger, UserLogin userLogin)
        {
            Messenger = messenger;
            this.Entity = new TEntity();
            Initialize();
            if (UserLogin == null)
                UserLogin = userLogin;
        }
        #endregion

        #region Public Methods
        public void UpdateUserLogin(dynamic userLogin)
        {
            if (UserLogin == null)
                UserLogin = userLogin;
        }

        public virtual void HandleViewModeChanges(dynamic data)
        {
            
        }

        public virtual void Initialize()
        {
            CloseCommand = new RelayCommand(() => Application.Current.Shutdown());
        }

        public void CloseChild()
        {
            if (this.ParentViewModel != null)
                this.ParentViewModel.ChildViewModel = null;
        }

        #endregion

        #region ProtectedMethods
        protected void OnDataProcess()
        {
            var result = new ActionResult<TEntity>
            {
                Result = ActionResultType.DataNotFound
            };
            this.IsActive = false;
            this.ViewMode = ViewModeType.Busy;
            this.StatusMessage = "Processing ...";

            Task task = Task.Factory.StartNew
            (
                (o) =>
                {
                    this.OnAction(result);
                }, result
            );

            task.ContinueWith
            (
                (t) =>
                {

                    this.OnActionFailed();

                    Exception exception = null;
                    this.OriginalEntity = null;

                    if (t.Exception.InnerException != null)
                    {
                        exception = t.Exception.InnerException;
                    }
                    else
                    {
                        exception = t.Exception;
                    }

                    this.NotificationMessage = exception.Message;
                },
                CancellationToken.None,
                TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.AttachedToParent,
                TaskScheduler.FromCurrentSynchronizationContext()
            );

            task.ContinueWith
            (
                _ =>
                {

                    //this.StatusMessage = null;
                    this.OnActionComplete(result);
                },
                CancellationToken.None,
                TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.AttachedToParent,
                TaskScheduler.FromCurrentSynchronizationContext()
            );
        }

        protected virtual void OnAction(ActionResult<TEntity> result) { }

        protected virtual void OnActionComplete(ActionResult<TEntity> result)
        {
            switch (result.Result)
            {
                case ActionResultType.DataNotFound:
                    this.OriginalEntity = null;

                    //this.ResetDataModel();

                    this.ViewMode = ViewModeType.ViewOnly;
                    this.IsActive = true;
                    break;

                case ActionResultType.RequestedNew:
                    throw new InvalidOperationException("Requested New is not valid on this type of ViewModel");

                case ActionResultType.DataFetched:
                    this.ResetDataModel(result.Data);

                    this.OriginalEntity = this.entity;
                    this.IsActive = true;
                    this.ViewMode = ViewModeType.ViewOnly;
                    break;
            }
        }

        protected virtual void OnActionFailed()
        {
            this.ViewMode = ViewModeType.ViewOnly;
            this.IsActive = true;
        }
        #endregion

        #region · DataModel Reset Methods ·

        protected void ResetDataModel()
        {
            this.ResetDataModel(new TEntity());
        }

        protected virtual void ResetDataModel(TEntity model)
        {
            this.OnResetingDataModel(this.Entity, model);

            this.OriginalEntity = null;
            this.Entity = model;

            this.OnResetedDataModel(model);
        }

        protected virtual void OnResetingDataModel(TEntity oldModel, TEntity newModel)
        {
        }

        protected virtual void OnResetedDataModel(TEntity newModel)
        {
            //this.NotifyAllPropertiesChanged();
        }

        #endregion        

        #region Virtual Methods
        public virtual void Unload()
        {
            this.Entity = null;
            this.UserLogin = null;
            this.ParentViewModel = null;
            this.ContentViewModel = null;
            Messenger = null;
            this.AddCommand = null;
            this.EditCommand = null;
            this.DeleteCommand = null;
            this.RefreshCommand = null;
            this.CloseCommand = null;
        }

        #region Command Methods
        public virtual void OnAddItem()
        {

        }
        public virtual bool CanAddItem()
        {
            return true;
        }
        public virtual void OnDeleteItem()
        {

        }
        public virtual bool CanDeleteItem()
        {
            return true;
        }
        public virtual void OnEditItem()
        {

        }
        public virtual bool CanEditItem()
        {
            return true;
        }
        public virtual void OnRefreshItem()
        {

        }
        public virtual bool CanRefreshItem()
        {
            return true;
        }
        #endregion

        #endregion
    }
    
}

