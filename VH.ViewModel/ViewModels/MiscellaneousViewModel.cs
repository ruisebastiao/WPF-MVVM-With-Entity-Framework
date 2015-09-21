using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using VH.Bases;
using VH.Model;
using VH.Resources;

namespace VH.ViewModel
{
    public class MiscellaneousViewModel : BaseViewModel
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
                        MenuResources.CallRegistry,
                        MenuResources.Reminders
                    };
            }
        }

        public string SelectedMenuItem
        {
            get
            {
                return _selectedMenuItem;
            }
            set
            {
                _selectedMenuItem = value;
                this.RaisePropertyChanged(() => this.SelectedMenuItem);
                OnSelectedMenuItem();
            }
        }

        public CallRegistryViewModel CallRegistryViewModel { get; set; }

        public ReminderViewModel ReminderViewModel { get; set; }

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
        public MiscellaneousViewModel(IMessenger messenger)
            : base(messenger)
        {
        }

        public MiscellaneousViewModel(IMessenger messenger, UserLogin userLogin)
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

        #region Private Methods

        private void OnSelectedMenuItem()
        {
            if (SelectedMenuItem == MenuResources.CallRegistry)
            {
                if (CallRegistryViewModel == null)
                    CallRegistryViewModel = new CallRegistryViewModel(this.Messenger, this.UserLogin)
                    {
                        ParentViewModel = this
                    };

                DetailSectionViewModel = CallRegistryViewModel;
                return;
            }

            if (SelectedMenuItem == MenuResources.Reminders)
            {
                if (ReminderViewModel == null)
                    ReminderViewModel = new ReminderViewModel(this.Messenger, this.UserLogin)
                    {
                        ParentViewModel = this
                    };

                DetailSectionViewModel = ReminderViewModel;
            }
        }


        #endregion
      
    }
}