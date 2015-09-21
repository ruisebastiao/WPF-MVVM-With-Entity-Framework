using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using VH.Resources;

namespace VH.Model
{
    [Table("TBL_USER_LOGIN")]
    public class UserLogin : VHEntity<UserLogin>
    {
        #region Fields
        private string _loginName;
        private string _loginPassword;
        private int _userId;
        private User _user;
        private bool _isAdmin;
        private bool _isSelected;
        private string _confirmPassword;
        private string _oldPassword;
        private string _newPassword;
        private bool _isInEditMode;

        #endregion

        #region Properties
        [Column("LOGIN_NAME")]
        public string LoginName
        {
            get { return _loginName; }
            set { this.SetProperty("LoginName", ref _loginName, value); }
        }

        [Column("LOGIN_PASSWORD")]
        public string LoginPassword
        {
            get { return _loginPassword; }
            set { this.SetProperty("LoginPassword", ref _loginPassword, value); }
        }

        [Column("IS_ADMIN")]
        public Boolean IsAdmin
        {
            get { return _isAdmin; }
            set { this.SetProperty("IsAdmin", ref _isAdmin, value); }
        }

        [Column("USERID")]
        public int UserId
        {
            get { return _userId; }
            set { this.SetProperty("UserId", ref _userId, value); }
        }

        [ForeignKey("UserId")]
        public virtual User User
        {
            get { return _user; }
            set { this.SetProperty("User", ref _user, value); }
        }

        [NotMapped]
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                this.NotifyPropertyChanged("IsSelected");
            }
        }

        [NotMapped]
        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set { this.SetProperty("ConfirmPassword", ref _confirmPassword, value); }
        }

        [NotMapped]
        public string OldPassword
        {
            get { return _oldPassword; }
            set { this.SetProperty("OldPassword", ref _oldPassword, value); }
        }

        [NotMapped]
        public string NewPassword
        {
            get { return _newPassword; }
            set { this.SetProperty("NewPassword", ref _newPassword, value); }
        }

        [NotMapped]
        public bool IsInEditMode
        {
            get { return _isInEditMode; }
            set { this.SetProperty("IsInEditMode", ref _isInEditMode, value); }
        }
        #endregion
        
        #region Constructors
        public UserLogin()
        {
            //User = new User();
        }
        #endregion

        #region Public Methods
        public bool VaidateChangePassword()
        {
            return !string.IsNullOrEmpty(NewPassword) && !string.IsNullOrEmpty(ConfirmPassword) && string.Equals(this.NewPassword, this.ConfirmPassword);
        }
        public bool VaidateCurrentPassword()
        {
            return string.Equals(this.OldPassword, this.LoginPassword);
        }

        public bool VaidateNewPassword()
        {
            return !string.IsNullOrEmpty(this.LoginName) && !string.IsNullOrEmpty(LoginPassword) && !string.IsNullOrEmpty(ConfirmPassword) && string.Equals(this.LoginPassword, this.ConfirmPassword);
        }

        #endregion

        #region Override Methods
        public void ClearValues()
        {
            this.OldPassword = string.Empty;
            this.ConfirmPassword = string.Empty;
            this.NewPassword = string.Empty;
        }

        public override string this[string columnName]
        {
            get
            {
                if (columnName == "OldPassword")
                {
                    return !this.VaidateCurrentPassword() ? "Invalid Password" : null;
                }

                if (columnName == "ConfirmPassword")
                {
                    return IsInEditMode ?  !this.VaidateChangePassword() ? "Password not matched" : null : !this.VaidateNewPassword() ? "Password not matched"  : null;
                }

                 if (columnName == "LoginName")
                 {
                     return string.IsNullOrEmpty(LoginName) ? "Login Name cannot be empty" : null;
                 }

                return "";
            }
        }
        #endregion
    }
}
