using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using VH.Bases;
using VH.Model;
using VH.Resources;

namespace VH.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Fields
        private string _selectedMenuItem;
        private IBaseViewModel _detailSectionViewModel;
        #endregion

        #region Properties
        public List<string> MenuList
        {
            get
            {
                return new List<string>
                    {
                        MenuResources.ChangePassword
                    };
            }
        }

        public string SelectedMenuItem
        {
            get
            {
                //if (_selectedMenuItem == null)
                //    SetSummaryView();
                return _selectedMenuItem;
            }
            set
            {
                _selectedMenuItem = value;
                this.RaisePropertyChanged(() => this.SelectedMenuItem);
                OnSelectedMenuItem();
            }
        }

        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }

        public IBaseViewModel DetailSectionViewModel
        {
            get { return _detailSectionViewModel; }
            set
            {
                _detailSectionViewModel = value;
                this.RaisePropertyChanged(() => this.DetailSectionViewModel);
            }
        }
        #endregion

        #region Constructors

        public SettingsViewModel(IMessenger messenger, UserLogin userLogin)
            : base(messenger, userLogin)
        {
        }

        #endregion

        #region Override Methods
        public override void HandleViewModeChanges(dynamic data)
        {
            //base.HandleViewModeChanges(data);
            var model = this.ParentViewModel as MainWindowViewModel;
            if (model != null)
            {
                model.IsSettingTabSelected = true;
            }
        }
        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        private void OnSelectedMenuItem()
        {
            if (SelectedMenuItem == MenuResources.ChangePassword)
            {
                if (ChangePasswordViewModel == null)
                    ChangePasswordViewModel = new ChangePasswordViewModel(this.Messenger, this.UserLogin)
                        {
                            ParentViewModel = this
                        };

                DetailSectionViewModel = ChangePasswordViewModel;
                return;
            }
        }

        
        #endregion

    }
}
