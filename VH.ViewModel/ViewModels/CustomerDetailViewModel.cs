using System.Collections.Generic;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using VH.Bases;
using VH.Model;
using VH.Resources;

namespace VH.ViewModel
{
    public class CustomerDetailViewModel : BaseViewModel
    {
        #region Fields
        private Customer _selectedCustomer;
        private string _selectedMenuItem;
        private IBaseViewModel _detailSectionViewModel;
        //private RelayCommand<object> _selectedMenuItem;

        #endregion

        #region Properties
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                _selectedCustomer = value;
                this.RaisePropertyChanged(() => this.SelectedCustomer);
            }
        }

        public List<string> MenuList
        {
            get
            {
                return new List<string>()
                    {
                        MenuResources.Detail,
                        MenuResources.Orders,
                        MenuResources.Repairs,
                        MenuResources.Appointments,
                        MenuResources.WarrantyInformed,
                    };
            }
        }

        public string SelectedMenuItem
        {
            get
            {
                if (_selectedMenuItem == null)
                    SetSummaryView();
                return _selectedMenuItem;
            }
            set
            {
                _selectedMenuItem = value;
                this.RaisePropertyChanged(() => this.SelectedMenuItem);
                OnSelectedMenuItem();
            }
        }

        public CustomerRepairViewModel CustomerRepairViewModel { get; set; }

        public CustomerAppointmentViewModel CustomerAppointmentViewModel { get; set; }

        public CustomerWarrantyInformedViewModel CustomerWarrantyInformedViewModel { get; set; }

        public CustomerOrderViewModel CustomerOrderViewModel { get; set; }

        public CustomerSummaryViewModel CustomerSummaryViewModel { get; set; }

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

        #region Command Properties

        //public RelayCommand<object> SelectedMenuItem
        //{
        //    get { return _selectedMenuItem; }
        //    set
        //    {
        //        _selectedMenuItem = value;
        //        this.RaisePropertyChanged(() => SelectedMenuItem);
        //    }
        //}

        #endregion

        #region Constructors

        public CustomerDetailViewModel(IMessenger messenger)
            : base(messenger)
        {
        }

        public CustomerDetailViewModel(IMessenger messenger, UserLogin userLogin, Customer selectedCustomer)
            : base(messenger, userLogin)
        {
            SelectedCustomer = selectedCustomer;
        }

        #endregion

        #region Override Methods
        public override void Initialize()
        {
            base.Initialize();
            //SelectedMenuItem = new RelayCommand<object>(OnSelectedMenuItem);
        }

        public override void Unload()
        {
            base.Unload();
            SelectedMenuItem = null;
        }
        #endregion

        #region Command Methods
       
        #endregion

        #region Private Methods
        private void OnSelectedMenuItem()
        {
            if (SelectedMenuItem == MenuResources.Repairs)
            {
                if (CustomerRepairViewModel == null)
                    CustomerRepairViewModel = new CustomerRepairViewModel(this.Messenger, this.UserLogin, this.SelectedCustomer){ParentViewModel = this};

                DetailSectionViewModel = CustomerRepairViewModel;
                return;
            }

            if (SelectedMenuItem == MenuResources.Detail)
            {
                SetSummaryView();
                return; 
            }

            if (SelectedMenuItem == MenuResources.Appointments)
            {
                DetailSectionViewModel = null;

                if (CustomerAppointmentViewModel == null)
                    CustomerAppointmentViewModel = new CustomerAppointmentViewModel(this.Messenger, this.UserLogin, this.SelectedCustomer) { ParentViewModel = this };

                DetailSectionViewModel = CustomerAppointmentViewModel;
                return;
            }

            if (SelectedMenuItem == MenuResources.Orders)
            {
                DetailSectionViewModel = null;

                if (CustomerOrderViewModel == null)
                    CustomerOrderViewModel = new CustomerOrderViewModel(this.Messenger, this.UserLogin, this.SelectedCustomer) { ParentViewModel = this };

                DetailSectionViewModel = CustomerOrderViewModel;
                return;
            }

            if (SelectedMenuItem == MenuResources.WarrantyInformed)
            {
                DetailSectionViewModel = null;

                if (CustomerWarrantyInformedViewModel == null)
                    CustomerWarrantyInformedViewModel = new CustomerWarrantyInformedViewModel(this.Messenger, this.UserLogin, this.SelectedCustomer) { ParentViewModel = this };

                DetailSectionViewModel = CustomerWarrantyInformedViewModel;
            }
        }

        private void SetSummaryView()
        {
            DetailSectionViewModel = null;
            if (CustomerSummaryViewModel == null)
                CustomerSummaryViewModel = new CustomerSummaryViewModel(this.Messenger, this.UserLogin, this.SelectedCustomer) { ParentViewModel = this };

            DetailSectionViewModel = CustomerSummaryViewModel;
        }
        #endregion

    }
}