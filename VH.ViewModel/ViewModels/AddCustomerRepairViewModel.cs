using System;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using VH.DataAccess;
using VH.ErrorLog;
using VH.Model;
using VH.Model.Utilities;
using VH.Resources;
using VH.SimpleUI.Entities;

namespace VH.ViewModel
{
    public class AddCustomerRepairViewModel : BaseViewModel<CustomerRepair>
    {
        #region Delegate
        public Action RefreshCustomerRepair { get; set; }
        #endregion

        #region Fields
        private CheckItemCollection _checkItemCollection;
        #endregion

        #region Properties
        #region Window Properties
        public override string Title
        {
            get
            {
                return !IsInEditMode ? TitleResources.AddCustomerRepair : TitleResources.EditCustomerRepair;
            }
        }


        public override double DialogStartupCustomHeight
        {
            get
            {
                return 850;
            }
        }

        public override double DialogStartupCustomWidth
        {
            get
            {
                return 610;
            }
        }

        public override DialogType DialogType
        {
            get
            {
                return DialogType.BySizeInPixel;
            }
        }
        #endregion

        public CheckItemCollection CheckItemCollection
        {
            get { return _checkItemCollection ?? ( _checkItemCollection = new CheckItemCollection()); }
            set
            {
                _checkItemCollection = value;
                this.RaisePropertyChanged(() => this.CheckItemCollection);
            }
        }

        public string SelectedHearingAidConditionTypes
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
                if(string.IsNullOrEmpty(value))
                    return;

                var spiltedItem = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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
            get { return this._saveCommand ?? (this._saveCommand = new RelayCommand(OnSaveCustomerRepair, CanSaveCustomerRepair)); }
        }

        private ICommand _cancelCommand;
        

        public ICommand CancelCommand
        {
            get { return this._cancelCommand ?? (this._cancelCommand = new RelayCommand(OnCancelCustomer)); }
        }
        #endregion

        #region Constructors
        public AddCustomerRepairViewModel(IMessenger messenger, UserLogin userLogin, Customer customer)
            : base(messenger, userLogin)
        {
            this.Entity.Customer = customer;
            if (customer.ID != null) this.Entity.CustomerID = customer.ID.Value;
        }

        public AddCustomerRepairViewModel(IMessenger messenger, UserLogin userLogin, CustomerRepair customerRepair)
            : base(messenger, userLogin)
        {
            this.IsInEditMode = true;
            this.Entity = customerRepair;
            this.SelectedHearingAidConditionTypes = this.Entity.HearingAidCondition;
        }
        #endregion

        #region Override Methods
        public override void Initialize()
        {
            base.Initialize();

            var hearingAidConditions = Enum.GetValues(typeof (HearingAidCondition));

            foreach (var hearingAidCondition in hearingAidConditions)
            {
                this.CheckItemCollection.Add(new CheckItem() { ItemName = hearingAidCondition.ToString() });
            }

         
            foreach (var item in this.CheckItemCollection)
            {
                item.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "IsChecked")
                    {
                        this.RaisePropertyChanged(() => this.SelectedHearingAidConditionTypes);
                        this.Entity.HearingAidCondition = this.SelectedHearingAidConditionTypes;
                    }
                };
            }
        }
        #endregion

        #region Command Methods

        private void OnSaveCustomerRepair()
        {
            var returnStatus = false;
            returnStatus = !IsInEditMode
                               ? CustomerAction.AddCustomerRepair(this.DBConnectionString, this.Entity)
                               : CustomerAction.UpdateCustomerRepair(this.DBConnectionString, this.Entity);

            if (returnStatus)
            {
                if (RefreshCustomerRepair != null)
                    this.RefreshCustomerRepair();

                var messageDailog = new MessageDailog()
                    {
                        Caption = MessageResources.DataSavedSuccessfully,
                        DialogButton = DialogButton.Ok,
                        Title = TitleResources.Information
                    };

                MessengerInstance.Send(messageDailog);

                if (this.CloseWindow != null)
                    this.CloseWindow();
            }
            else
            {
                var messageDailog = new MessageDailog()
                    {
                        Caption = MessageResources.DataSavedFailed,
                        DialogButton = DialogButton.Ok,
                        Title = TitleResources.Error
                    };
                MessengerInstance.Send(messageDailog);
            }
        }

        private bool CanSaveCustomerRepair()
        {
            return this.Entity != null && this.Entity.HasValueInAllRequiredField;
        }

        private void OnCancelCustomer()
        {
            var messageDailog = new MessageDailog((result) =>
            {
                if (result == DialogResult.Ok)
                {
                    if (this.ParentViewModel != null)
                        this.ParentViewModel.ChildViewModel = null;
                    this.Unload();

                    if (this.CloseWindow != null)
                        this.CloseWindow();
                }
            }) { Caption = MessageResources.CancelWindowMessage, DialogButton = DialogButton.OkCancel, Title = TitleResources.Warning };
            MessengerInstance.Send(messageDailog);


        }
        #endregion

        #region Private Methods
        
        #endregion

        #region Public Methods

        #endregion


    }
}