using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Runtime.Serialization;
using VH.Resources;

namespace VH.Model
{
    [Serializable]
     [Table("TBL_CUSTOMER_HEARING_AID_ORDER")]
     [DataContract]
    public class CustomerHearingAidOrder : VHEntity<CustomerRepair>
    {
         #region Fields
         private Company _company;
         private DateTime _orderDate;
         private string _hiModel;
         private string _serialNumberRight;
         private string _serialNumberLeft;
         private HearingAidType _hearingAidType;
         private BatterySize? _batterySize;
         private decimal? _advanceAmtReceived = decimal.Zero;
         private int _customerID;
         private Customer _customer;
         private string _comment;
         private OrderStatus? _orderStatus;
         private bool _isSelected;
         private string _serialNumber;
         private EarSide _earSide;
        private string _hiModelRight;
        private string _hiModelLeft;
        private WarrantyCardGiven _warrantyCardGiven = WarrantyCardGiven.None;
        private decimal? _amount = decimal.Zero;
        private DateTime? _informedDate;
        private DateTime? _receivedDate;
        private DateTime? _deliveredDate;
        private bool _isReceived;
        private bool _isRightModel;
        private bool _isLeftModel;
        private bool _isInformed;
        private bool _isDelivered;

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
        [Column("ORDER_DATE")]
        public DateTime OrderDate
        {
            get { return _orderDate; }
            set { this.SetProperty("OrderDate", ref _orderDate, value); }
        }

        [DataMember]
        [Column("HI_MODEL_R")]
        public string HiModelRight
        {
            get { return _hiModelRight; }
            set { this.SetProperty("HiModelRight", ref _hiModelRight, value); }
        }

        [DataMember]
        [Column("HI_MODEL_L")]
        public string HiModelLeft
        {
            get { return _hiModelLeft; }
            set { this.SetProperty("HiModelLeft", ref _hiModelLeft, value); }
        }

        [DataMember]
        [Column("SERIAL_NUMBER_R")]
        public string SerialNumberRight
        {
            get { return _serialNumberRight; }
            set { this.SetProperty("SerialNumberRight", ref _serialNumberRight, value); }
        }

        [DataMember]
        [Column("SERIAL_NUMBER_L")]
        public string SerialNumberLeft
        {
            get { return _serialNumberLeft; }
            set { this.SetProperty("SerialNumberLeft", ref _serialNumberLeft, value); }
        }

        [DataMember]
         [Column("EAR_SIDE")]
         public EarSide EarSide
         {
             get { return _earSide; }
             set
             {
                 this.SetProperty("EarSide", ref _earSide, value);
                 if (value == EarSide.Left)
                 {
                     this.IsRightModel = false;
                     this.IsLeftModel = true;
                     this.HiModelRight = string.Empty;
                     this.SerialNumberRight = string.Empty;
                 }
                 if (value == EarSide.Right)
                 {
                     this.IsRightModel = true;
                     this.IsLeftModel = false;
                     this.HiModelLeft = string.Empty;
                     this.SerialNumberLeft = string.Empty;
                 }
                 if (value == EarSide.Both)
                 {
                     this.IsRightModel = true;
                     this.IsLeftModel = true;
                 }
             }
         }

        // [Column("SERIAL_NUMB_LEFT")]
        //public string SerialNumberLeft
        //{
        //    get { return _serialNumberLeft; }
        //    set { this.SetProperty("SerialNumberLeft", ref _serialNumberLeft, value); }
        //}

        //[Column("SERIAL_NUMB_RIGHT")]
        //public string SerialNumberRight
        //{
        //    get { return _serialNumberRight; }
        //    set { this.SetProperty("SerialNumberRight", ref _serialNumberRight, value); }
        //}

        [DataMember]
        [Column("HEARING_AID_TYPE")]
        public HearingAidType HearingAidType
        {
            get { return _hearingAidType; }
            set { this.SetProperty("HearingAidType", ref _hearingAidType, value); }
        }

        [DataMember]
        [Column("BATTERY_SIZE")]
        public BatterySize? BatterySize
        {
            get { return _batterySize; }
            set { this.SetProperty("BatterySize", ref _batterySize, value); }
        }

        [DataMember]
        [Column("ADV_AMT_RECEIVED")]
        public decimal? AdvanceAmtReceived
        {
            get { return _advanceAmtReceived; }
            set
            {
                this.SetProperty("AdvanceAmtReceived", ref _advanceAmtReceived, value);
                this.NotifyPropertyChanged("BalanceAmount");
            }
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
        [Column("COMMENT")]
        public String Comment
        {
            get { return _comment; }
            set { this.SetProperty("Comment", ref _comment, value); }
        }

        [DataMember]
        [Column("ORDER_STATUS")]
        public OrderStatus? OrderStatus
        {
            get { return _orderStatus; }
            set
            {
                this.SetProperty("OrderStatus", ref _orderStatus, value);
                if (value == Model.OrderStatus.Delivered)
                {
                    this.IsDelivered = true;
                    this.IsInformed = false;
                    this.IsReceived = false;
                }
                else if (value == Model.OrderStatus.Informed)
                {
                    this.IsDelivered = false;
                    this.IsInformed = true;
                    this.IsReceived = false;
                }
                else if (value == Model.OrderStatus.Received)
                {
                    this.IsDelivered = false;
                    this.IsInformed = false;
                    this.IsReceived = true;
                }
                else
                {
                    this.IsDelivered = false;
                    this.IsInformed = false;
                    this.IsReceived = false;
                }
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
        [Column("RECEIVEDDATETIME")]
        public DateTime? ReceivedDate
        {
            get { return _receivedDate; }
            set { this.SetProperty("ReceivedDate", ref _receivedDate, value); }
        }

        [DataMember]
        [Column("DELIVEREDDATETIME")]
        public DateTime? DeliveredDate
        {
            get { return _deliveredDate; }
            set { this.SetProperty("DeliveredDate", ref _deliveredDate, value); }
        }

        [DataMember]
        [Column("WARRANTY_CARD_GIVEN")]
        public WarrantyCardGiven WarrantyCardGiven
        {
            get { return _warrantyCardGiven; }
            set { this.SetProperty("WarrantyCardGiven", ref _warrantyCardGiven, value); }
        }

        [DataMember]
        [Column("CUSTOMER_ID")]
        public int CustomerID
        {
            get { return _customerID; }
            set { this.SetProperty("CustomerID", ref _customerID, value); }
        }

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
        public bool IsRightModel
        {
            get { return _isRightModel; }
            set
            {
                _isRightModel = value;
                this.NotifyPropertyChanged("IsRightModel");
            }
        }

        [NotMapped]
        public bool IsLeftModel
        {
            get { return _isLeftModel; }
            set
            {
                _isLeftModel = value;
                this.NotifyPropertyChanged("IsLeftModel");
            }
        }

        [NotMapped]
        public bool IsInformed
        {
            get { return _isInformed; }
            set
            {
                _isInformed = value;
                this.NotifyPropertyChanged("IsInformed");
            }
        }

        [NotMapped]
        public bool IsDelivered
        {
            get { return _isDelivered; }
            set
            {
                _isDelivered = value;
                this.NotifyPropertyChanged("IsDelivered");
            }
        }

        [NotMapped]
        public bool IsReceived
        {
            get { return _isReceived; }
            set
            {
                _isReceived = value;
                this.NotifyPropertyChanged("IsReceived");
            }
        }

        [DataMember]
        [NotMapped]
        public decimal BalanceAmount
        {
            get
            {
                return this.Amount.HasValue && this.AdvanceAmtReceived.HasValue ? this.Amount.Value - this.AdvanceAmtReceived.Value : decimal.Zero;
            }
            set { }
        }

        [NotMapped]
        public string StatusToolTip
        {
            get
            {
                return LabelResources.ReceivedDate + " " +
                    (this.ReceivedDate.HasValue ? " : " + this.ReceivedDate.Value.ToLongDateString() : " : " + MessageResources.NotYetReceived) + Environment.NewLine +
                 LabelResources.InformedDate + " " +
                 (this.InformedDate.HasValue ? " : " + this.InformedDate.Value.ToLongDateString() : " : " + MessageResources.NotYetInformed) + Environment.NewLine +
                 LabelResources.DeliveredDate + " " +
                 (this.DeliveredDate.HasValue ? " : " + this.DeliveredDate.Value.ToLongDateString() : " : " + MessageResources.NotYetDelivered) + Environment.NewLine;
            }
        }
        #endregion

        #region Constructors
        public CustomerHearingAidOrder()
        {
            this.OrderDate = DateTime.Now;
            this.EarSide = EarSide.Right;
        }
        #endregion

        #region Public Methods
        
        #endregion

        #region Private Methods
        
        #endregion

        #region Override Methods
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "OrderDate")
                {
                    return string.IsNullOrEmpty(this.OrderDate.ToString(CultureInfo.InvariantCulture)) ? ErrorResources.RequiredField : null;
                }

                if (columnName == "EarSide")
                {
                    return string.IsNullOrEmpty(this.EarSide.ToString()) ? ErrorResources.RequiredField : null;
                }

                //if (columnName == "HIModel")
                //{
                //    return string.IsNullOrEmpty(this.HIModel) ? ErrorResources.RequiredField : null;
                //}

                //if (columnName == "SerialNumber")
                //{
                //    return string.IsNullOrEmpty(this.SerialNumber) ? ErrorResources.RequiredField : null;
                //}

                return base[columnName];
            }
        }
        #endregion
    }
}