using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using VH.Model.Utilities;
using VH.Resources;

namespace VH.Model
{
     [Serializable]
    [DataContract]
    [Table("TBL_CUSTOMER_REPAIR")]
    public class CustomerRepair : VHEntity<CustomerRepair>
    {
        #region Fields
        private int _customerID;
        private Customer _customer;
        private Company _company;
        private string _model;
        private string _serialNumberLeft;
        private string _serialNumberRight;
        private WarrantyType _warrantyType;
        private HearingAidType _hearingAidType;
        private string _accessoriesReceived;
        private YesNo _spareAidGiven;
        private decimal _advanceAmount = decimal.Zero;
        private string _complaintDescription;
        private bool _isSelected;
        private DateTime _receivedDate;
        private string _dueDate;
        private decimal? _repairAmtLessThan;
        private RepairStatus? _repairStatus = Model.RepairStatus.NotProcessed;
        private DateTime? _dispatchedToCompanyDate;
        private DateTime? _companyReceivedDate;
        private DateTime? _companyDispatcedDate;
        private DateTime? _vhReceivedDate;
        private DateTime? _deliveredToCustomerDate;
        private string _additionalComment;
        private MoldRepair? _moldRepair = VH.Model.MoldRepair.None;
        private string _hearingAidCondition;
         private DateTime? _informedDate;
         private decimal? _amount = decimal.Zero;
         private YesNo? _customerApprovalOnEstimateAmt;
         private string _spareAidGivenDetails;
         private bool _isSpareAidGiven;
         private string _modelLeft;
         private string _modelRight;
         private string _newSerialNumberRight;
         private string _newSerialNumberLeft;
         private bool _isSerialNumberRevised;
         private bool _isSpareAidReceived;

         #endregion

        #region Properties
         [DataMember]
        [Column("COMPANY")]
        public Company Company
        {
            get { return _company; }
            set { this.SetProperty("Company", ref _company, value); }
        }

          [DataMember]
        [Column("RECEIVED_DATE")]
        public DateTime ReceivedDate
        {
            get { return _receivedDate; }
            set { this.SetProperty("ReceivedDate", ref _receivedDate, value); }
        }

          [DataMember]
        [Column("SERIAL_NUMB_LEFT")]
        public string SerialNumberRight
        {
            get { return _serialNumberRight; }
            set { this.SetProperty("SerialNumberRight", ref _serialNumberRight, value); }
        }

          [DataMember]
        [Column("SERIAL_NUMB_RIGHT")]
        public string SerialNumberLeft
        {
            get { return _serialNumberLeft; }
            set { this.SetProperty("SerialNumberLeft", ref _serialNumberLeft, value); }
        }

          [DataMember]
          [Column("IS_SL_NO_REVISED")]
         public bool IsSerialNumberRevised
         {
             get { return _isSerialNumberRevised; }
             set { this.SetProperty("IsSerialNumberRevised", ref _isSerialNumberRevised, value); }
         }

         [DataMember]
         [Column("NEW_SERIAL_NUMB_LEFT")]
          public string NewSerialNumberRight
          {
              get { return _newSerialNumberRight; }
              set { this.SetProperty("NewSerialNumberRight", ref _newSerialNumberRight, value); }
          }

          [DataMember]
          [Column("NEW_SERIAL_NUMB_RIGHT")]
          public string NewSerialNumberLeft
          {
              get { return _newSerialNumberLeft; }
              set { this.SetProperty("NewSerialNumberLeft", ref _newSerialNumberLeft, value); }
          }

          [DataMember]
        [Column("WARRANTY_TYPE")]
        public WarrantyType WarrantyType
        {
            get { return _warrantyType; }
            set { this.SetProperty("WarrantyType", ref _warrantyType, value); }
        }

          [DataMember]
        [Column("HEARING_AID_TYPE")]
        public HearingAidType HearingAidType
        {
            get { return _hearingAidType; }
            set { this.SetProperty("HearingAidType", ref _hearingAidType, value); }
        }

          [DataMember]
         [Column("ACCESSORIES_RECEIVED")]
        public string AccessoriesReceived
        {
            get { return _accessoriesReceived; }
            set { this.SetProperty("AccessoriesReceived", ref _accessoriesReceived, value); }
        }

          [DataMember]
          [Column("CUSTOMER_APPROVAL_ESTIMATE_AMT")]
        public YesNo? CustomerApprovalOnEstimateAmt
        {
            get { return _customerApprovalOnEstimateAmt; }
            set { this.SetProperty("CustomerApprovalOnEstimateAmt", ref _customerApprovalOnEstimateAmt, value); }
        }

          [DataMember]
          [Column("SPARE_AID_GIVEN")]
          public YesNo SpareAidGiven
          {
              get { return _spareAidGiven; }
              set
              {
                  this.SetProperty("SpareAidGiven", ref _spareAidGiven, value);
                  IsSpareAidGiven = value == YesNo.Yes;
              }
          }

          [DataMember]
          [Column("SPARE_AID_DETAILS")]
          public string SpareAidGivenDetails
          {
              get { return _spareAidGivenDetails; }
              set { this.SetProperty("SpareAidGivenDetails", ref _spareAidGivenDetails, value); }
          }

          [DataMember]
          [Column("AMOUNT")]
          public decimal? Amount
          {
              get { return _amount; }
              set
              {
                  this.SetProperty("Amount", ref _amount, value);
                  this.NotifyPropertyChanged("BalanceAmount");
              }
          }


          [DataMember]
        [Column("ADV_AMT_RECEIVED")]
        public decimal AdvanceAmount
        {
            get { return _advanceAmount; }
            set
            {
                this.SetProperty("AdvanceAmount", ref _advanceAmount, value);
                this.NotifyPropertyChanged("BalanceAmount");
            }
        }

          [DataMember]
          [NotMapped]
          public decimal BalanceAmount
          {
              get
              {
                  return this.Amount.HasValue ? this.Amount.Value - this.AdvanceAmount : decimal.Zero;
              }
              set { }
          }

          [DataMember]
        [Column("MOLDREPAIR")]
        public MoldRepair? MoldRepair
        {
            get { return _moldRepair; }
            set { this.SetProperty("MoldRepair", ref _moldRepair, value); }
        }

          [DataMember]
        [Column("HEARINGAID_CONDITION")]
        public string HearingAidCondition
        {
            get { return _hearingAidCondition; }
            set { this.SetProperty("HearingAidCondition", ref _hearingAidCondition, value); }
        }

          [DataMember]
        [Column("COMPLAINT_DESC")]
        public string ComplaintDescription
        {
            get { return _complaintDescription; }
            set { this.SetProperty("ComplaintDescription", ref _complaintDescription, value); }
        }

          [DataMember]
        [Column("DUEDATE")]
        public string DueDate
        {
            get { return _dueDate; }
            set { this.SetProperty("DueDate", ref _dueDate, value); }
        }

          [DataMember]
        [Column("REPAIR_AMT_LESS_THAN")]
        public decimal? RepairAmtLessThan
        {
            get { return _repairAmtLessThan; }
            set { this.SetProperty("RepairAmtLessThan", ref _repairAmtLessThan, value); }
        }

          [DataMember]
        [Column("REPAIR_STATUS")]
        public RepairStatus? RepairStatus
        {
            get { return _repairStatus; }
            set { this.SetProperty("RepairStatus", ref _repairStatus, value); }
        }

          [DataMember]
        [Column("DISPATCHED_TO_COMPANY")]
        public DateTime? DispatchedToCompanyDate
        {
            get { return _dispatchedToCompanyDate; }
            set { this.SetProperty("DispatchedToCompanyDate", ref _dispatchedToCompanyDate, value); }
        }

          [DataMember]
        [Column("COMPANY_RECEIVED_DATE")]
        public DateTime? CompanyReceivedDate
        {
            get { return _companyReceivedDate; }
            set { this.SetProperty("CompanyReceivedDate", ref _companyReceivedDate, value); }
        }

          [DataMember]
        [Column("COMPANY_DISPATCHED_DATE")]
        public DateTime? CompanyDispatchedDate
        {
            get { return _companyDispatcedDate; }
            set { this.SetProperty("CompanyDispatcedDate", ref _companyDispatcedDate, value); }
        }

          [DataMember]
        [Column("VH_RECEIVED_DATE")]
        public DateTime? VHReceivedDate
        {
            get { return _vhReceivedDate; }
            set { this.SetProperty("VHReceivedDate", ref _vhReceivedDate, value); }
        }

          [DataMember]
        [Column("DELIVERED_TO_CUSTOMER_DATE")]
        public DateTime? DeliveredToCustomerDate
        {
            get { return _deliveredToCustomerDate; }
            set
            {
                this.SetProperty("DeliveredToCustomerDate", ref _deliveredToCustomerDate, value);
                this.NotifyPropertyChanged("HighlightIfSpareAidNotReceived");
            }
        }

          [DataMember]
          [Column("IS_SPARE_AID_RECEIVED")]
          public bool IsSpareAidReceived
          {
              get { return _isSpareAidReceived; }
              set
              {
                  this.SetProperty("IsSpareAidReceived", ref _isSpareAidReceived, value);
                  this.NotifyPropertyChanged("HighlightIfSpareAidNotReceived");
              }
              
          }

          [DataMember]
          [Column("INFORMEDDATETIME")]
          public DateTime? InformedDate
          {
              get { return _informedDate; }
              set { this.SetProperty("InformedDate", ref _informedDate, value); }
          }

          [DataMember]
          [Column("MODEL_LEFT")]
        public string ModelLeft
        {
            get { return _modelLeft; }
            set { this.SetProperty("ModelLeft", ref _modelLeft, value); }
        }

          [DataMember]
          [Column("MODEL_RIGHT")]
          public string ModelRight
          {
              get { return _modelRight; }
              set { this.SetProperty("ModelRight", ref _modelRight, value); }
          }

          [DataMember]
        [Column("ADDITIONAL_COMMENT")]
        public string AdditionalComment
        {
            get { return _additionalComment; }
            set { this.SetProperty("AdditionalComment", ref _additionalComment, value); }
        }

          [DataMember]
        [Column("CUSTOMER_ID")]
        public int CustomerID
        {
            get { return _customerID; }
            set { this.SetProperty("CustomerID", ref _customerID, value); }
        }

        //  [DataMember]
        [ForeignKey("CustomerID")]
        public virtual Customer Customer
        {
            get { return _customer; }
            set { this.SetProperty("Customer", ref _customer, value); }
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
        public bool HasValueInAllRequiredField
        {
            get
            {
                return !string.IsNullOrEmpty(this.ReceivedDate.ToString(CultureInfo.InvariantCulture)) &&
                       !string.IsNullOrEmpty(this.Company.ToString())
                       && !string.IsNullOrEmpty(this.WarrantyType.ToString()) &&
                       !string.IsNullOrEmpty(this.HearingAidType.ToString())
                       && !string.IsNullOrEmpty(this.SpareAidGiven.ToString());
            }
        }

        [NotMapped]
        public bool HasValueToUpdateStatus
        {
            get
            {
                if (RepairStatus.HasValue && RepairStatus.Value == VH.Model.RepairStatus.Received)
                    return !string.IsNullOrEmpty(this.VHReceivedDate.ToString());

                if (RepairStatus.HasValue && RepairStatus.Value == VH.Model.RepairStatus.DispatchedToCompany)
                    return !string.IsNullOrEmpty(this.DispatchedToCompanyDate.ToString());

                if (RepairStatus.HasValue && RepairStatus.Value == VH.Model.RepairStatus.CompanyDispatched)
                    return !string.IsNullOrEmpty(this.CompanyDispatchedDate.ToString());

                if (RepairStatus.HasValue && RepairStatus.Value == VH.Model.RepairStatus.CompanyReceived)
                    return !string.IsNullOrEmpty(this.CompanyReceivedDate.ToString());

                if (RepairStatus.HasValue && RepairStatus.Value == VH.Model.RepairStatus.Received)
                    return !string.IsNullOrEmpty(this.VHReceivedDate.ToString());

                if (RepairStatus.HasValue && RepairStatus.Value == VH.Model.RepairStatus.DeliveredToCustomer)
                    return !string.IsNullOrEmpty(this.DeliveredToCustomerDate.ToString());

                if (RepairStatus.HasValue && RepairStatus.Value == VH.Model.RepairStatus.Informed)
                    return !string.IsNullOrEmpty(this.InformedDate.ToString());

                return false;
            }
        }

        [NotMapped]
        public string StatusToolTip
        {
            get
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine(string.Format("{0} - {1}", VH.Model.RepairStatus.DispatchedToCompany.GetResourceValueForEnum(),
                                                   this.DispatchedToCompanyDate.HasValue
                                                       ? this.DispatchedToCompanyDate.Value.ToString("d")
                                                       : MessageResources.NoData));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.AppendLine(string.Format("{0} - {1}", VH.Model.RepairStatus.CompanyReceived.GetResourceValueForEnum(),
                                                 this.CompanyReceivedDate.HasValue
                                                     ? this.CompanyReceivedDate.Value.ToString("d")
                                                     : MessageResources.NoData));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.AppendLine(string.Format("{0} - {1}", VH.Model.RepairStatus.CompanyDispatched.GetResourceValueForEnum(),
                                                 this.CompanyDispatchedDate.HasValue
                                                     ? this.CompanyDispatchedDate.Value.ToString("d")
                                                     : MessageResources.NoData));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.AppendLine(string.Format("{0} - {1}", VH.Model.RepairStatus.Received.GetResourceValueForEnum(),
                                                this.VHReceivedDate.HasValue
                                                    ? this.VHReceivedDate.Value.ToString("d")
                                                    : MessageResources.NoData));
                stringBuilder.Append(Environment.NewLine);
                stringBuilder.AppendLine(string.Format("{0} - {1}", VH.Model.RepairStatus.DeliveredToCustomer.GetResourceValueForEnum(),
                                               this.DeliveredToCustomerDate.HasValue
                                                   ? this.DeliveredToCustomerDate.Value.ToString("d")
                                                   : MessageResources.NoData));

                return stringBuilder.ToString();
            }
        }

        [NotMapped]
        public bool IsSpareAidGiven
        {
            get { return _isSpareAidGiven; }
            set
            {
                _isSpareAidGiven = value;
                this.NotifyPropertyChanged("IsSpareAidGiven");
            }
        }

         public bool HighlightIfSpareAidNotReceived
         {
             get { return this.RepairStatus == Model.RepairStatus.DeliveredToCustomer && !this.IsSpareAidReceived; }
         }
        #endregion

        #region Constructors
        public CustomerRepair()
        {
            this.ReceivedDate = DateTime.Now;
            this.RepairStatus = VH.Model.RepairStatus.NotProcessed;
        }

        #endregion

        #region Override Methods
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "ReceivedDate")
                {
                    return string.IsNullOrEmpty(this.ReceivedDate.ToString(CultureInfo.InvariantCulture)) ? ErrorResources.RequiredField : null;
                }

                if (columnName == "Company")
                {
                    return string.IsNullOrEmpty(this.Company.ToString()) ? ErrorResources.RequiredField : null;
                }

                //if (columnName == "Model")
                //{
                //    return string.IsNullOrEmpty(this.Model) ? ErrorResources.RequiredField : null;
                //}

                if (columnName == "WarrantyType")
                {
                    return string.IsNullOrEmpty(this.WarrantyType.ToString()) ? ErrorResources.RequiredField : null;
                }

                if (columnName == "HearingAidType")
                {
                    return string.IsNullOrEmpty(this.HearingAidType.ToString()) ? ErrorResources.RequiredField : null;
                }

                if (columnName == "SpareAidGiven")
                {
                    return string.IsNullOrEmpty(this.SpareAidGiven.ToString()) ? ErrorResources.RequiredField : null;
                }

                if (columnName == "SpareAidGiven")
                {
                    return string.IsNullOrEmpty(this.SpareAidGiven.ToString()) ? ErrorResources.RequiredField : null;
                }

                if (columnName == "DispatchedToCompanyDate")
                {
                  //  if (RepairStatus.HasValue && RepairStatus.Value == VH.Model.RepairStatus.DispatchedToCompany)
                        return string.IsNullOrEmpty(this.DispatchedToCompanyDate.ToString())
                                   ? ErrorResources.RequiredField
                                   : null;
                }

                if (columnName == "CompanyReceivedDate")
                {
                    //if (RepairStatus.HasValue && RepairStatus.Value == VH.Model.RepairStatus.CompanyReceived)
                        return string.IsNullOrEmpty(this.CompanyReceivedDate.ToString())
                                   ? ErrorResources.RequiredField
                                   : null;
                }

                if (columnName == "CompanyDispatchedDate")
                {
                  //  if (RepairStatus.HasValue && RepairStatus.Value == VH.Model.RepairStatus.CompanyDispatched)
                        return string.IsNullOrEmpty(this.CompanyDispatchedDate.ToString())
                                   ? ErrorResources.RequiredField
                                   : null;
                }


                if (columnName == "VHReceivedDate")
                {
                   // if (RepairStatus.HasValue && RepairStatus.Value == VH.Model.RepairStatus.Received)
                        return string.IsNullOrEmpty(this.VHReceivedDate.ToString())
                                   ? ErrorResources.RequiredField
                                   : null;
                }

                if (columnName == "DeliveriedToCustomerDate")
                {
                    if (RepairStatus.HasValue && RepairStatus.Value == VH.Model.RepairStatus.DeliveredToCustomer)
                        return string.IsNullOrEmpty(this.DeliveredToCustomerDate.ToString())
                                   ? ErrorResources.RequiredField
                                   : null;
                }

                
                return base[columnName];
            }
        }
        #endregion

        #region Public Methods

        #endregion
    }
}