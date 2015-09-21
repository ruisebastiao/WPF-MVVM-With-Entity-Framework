using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using VH.DataAccess;
using VH.ErrorLog;
using VH.Model;
using VH.Resources;

namespace VH.ViewModel
{
    public class UserLoginViewModel : BaseViewModel<UserLogin>
    {
        private RelayCommand _signInCommand;

        #region Fields

        #endregion

        #region Properties
        public override string Title
        {
            get
            {
                return "IDENTIFICATION";
            }
        }

        public override double DialogStartupSizePercentage
        {
            get
            {
                return 50;
            }
        }

        public override double WindowHeight
        {
            get
            {
                return 600;
            }
        }

        public override double WindowWidth
        {
            get
            {
                return 600;
            }
        }

        public string LoginName
        {
            get { return this.Entity.LoginName; }
            set
            {
                if (this.Entity.LoginName != value)
                {
                    this.Entity.LoginName = value;
                    this.RaisePropertyChanged(() => LoginName);
                }
            }
        }

        public string LoginPassword
        {
            get { return this.Entity.LoginPassword; }
            set
            {
                if (this.Entity.LoginPassword != value)
                {
                    this.Entity.LoginPassword = value;
                    this.RaisePropertyChanged(() => LoginPassword);

                }
            }
        }        
        #endregion

        #region Commands

        public RelayCommand SignInCommand
        {
            get { return _signInCommand; }
            set
            {
                _signInCommand = value;
                this.RaisePropertyChanged(() => SignInCommand);
            }
        }

        #endregion

        #region Constructors
        public UserLoginViewModel(IMessenger messenger)
            : base(messenger)
        {
           
        }
        #endregion

        #region Public Methods
        
        #endregion

        #region Privae Methods
        
        #endregion

        #region Command Methods
        //public void ExecuteSingInCommand()
        //{
        //    this.OnDataProcess();
        //}
        public bool CanExecuteSingInCommand()
        {
            return (!string.IsNullOrEmpty(this.LoginName) && !string.IsNullOrEmpty(this.LoginPassword)) ;
        }
        #endregion

        #region Virtual Methods
        public override void Initialize()
        {
            base.Initialize();
            this.SignInCommand = new RelayCommand(OnDataProcess, CanExecuteSingInCommand);
        }

        protected override void OnAction(ActionResult<UserLogin> result)
        {
            try
            {
                var userLoginActions = new UserLoginAction();
                result.Data = userLoginActions.AuthenticateUserLogin(this.DBConnectionString, this.Entity);
                if (result.Data != null)
                {
                    this.ParentViewModel.UpdateUserLogin(result.Data);
                    result.Result = ActionResultType.DataFetched;
                    this.CloseChild();
                }
                else
                {
                    StatusMessage = "Login Failed";
                    this.Entity.LoginPassword = string.Empty;
                    result.Result = ActionResultType.DataNotFound;
                }
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
