using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Runtime.Serialization;
using VH.Resources;

namespace VH.Model
{
    [DataContract]
    [Table("TBL_CUSTOMER_EAR_MOLD_ORDER")]
    public class CustomerEarMoldOrder : VHEntity<CustomerRepair>
    {
        #region Fields

        private DateTime _orderDate;
        private string _hiModelRight;
        private int _customerID;
        private Customer _customer;
        private int? _hearingAidOrderId;
        private CustomerHearingAidOrder _hearingAidOrder;
        private EarSide _earSide;
        private EarMoldType _earMoldType;
        private EarMoldHearingAidType _earMoldHearingAidType;
        private EarMoldDesign _earMoldDesign;
        private SoftMoldTubingType? _softMoldTubingType;
        private VentSize? _ventSize;
        private VentType? _ventType;
        private VentLength? _ventLength;
        private decimal? _advanceAmtReceived;
        private string _comment;
        private OrderStatus? _orderStatus = Model.OrderStatus.OrderTaken;
        private bool _isSelected;
        private Company _company;
        private decimal? _moldAmount;
        private string _hiModelLeft;
        private bool _isRightModel;
        private bool _isLeftModel;
        private DateTime? _informedDate;
        private DateTime? _receivedDate;
        private DateTime? _deliveredDate;
        private bool _isInformed;
        private bool _isReceived;
        private bool _isDelivered;

        #endregion

        #region Properties
        [DataMember]
        [Column("ORDER_DATE")]
        public DateTime OrderDate
        {
            get { return _orderDate; }
            set { this.SetProperty("OrderDate", ref _orderDate, value); }
        }

        [DataMember]
        [Column("COMPANY")]
        public Company Company
        {
            get { return _company; }
            set { this.SetProperty("Company", ref _company, value); }
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
                }
                if (value == EarSide.Right)
                {
                    this.IsRightModel = true;
                    this.IsLeftModel = false;
                    this.HiModelLeft = string.Empty;
                }
                if (value == EarSide.Both)
                {
                    this.IsRightModel = true;
                    this.IsLeftModel = true;
                }
            }
        }

        [DataMember]
        [Column("EAR_MOLD_TYPE")]
        public EarMoldType EarMoldType
        {
            get { return _earMoldType; }
            set { this.SetProperty("EarMoldType", ref _earMoldType, value); }
        }

        [DataMember]
        [Column("EAR_MOLD_AID_TYPE")]
        public EarMoldDesign EarMoldDesign
        {
            get { return _earMoldDesign; }
            set { this.SetProperty("EarMoldDesign", ref _earMoldDesign, value); }
        }

        [DataMember]
        [Column("EAR_MOLD_DESIGN")]
        public EarMoldHearingAidType EarMoldHearingAidType
        {
            get { return _earMoldHearingAidType; }
            set { this.SetProperty("EarMoldHearingAidType", ref _earMoldHearingAidType, value); }
        }

        [DataMember]
        [Column("SOFT_MOLD_TUBING_TYPE")]
        public SoftMoldTubingType? SoftMoldTubingType
        {
            get { return _softMoldTubingType; }
            set { this.SetProperty("SoftMoldTubingType", ref _softMoldTubingType, value); }
        }

        [DataMember]
        [Column("VENT_SIZE")]
        public VentSize? VentSize
        {
            get { return _ventSize; }
            set { this.SetProperty("VentSize", ref _ventSize, value); }
        }

        [DataMember]
        [Column("VENT_TYPE")]
        public VentType? VentType
        {
            get { return _ventType; }
            set { this.SetProperty("VentType", ref _ventType, value); }
        }

        [DataMember]
        [Column("VENT_LENGTH")]
        public VentLength? VentLength
        {
            get { return _ventLength; }
            set { this.SetProperty("VentLength", ref _ventLength, value); }
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
        [Column("MOLD_AMOUNT")]
        public decimal? MoldAmount
        {
            get { return _moldAmount; }
            set
            {
                this.SetProperty("MoldAmount", ref _moldAmount, value);
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
        [Column("CUSTOMER_ID")]
        public int CustomerID
        {
            get { return _customerID; }
            set { this.SetProperty("CustomerID", ref _customerID, value); }
        }

        [DataMember]
        [Column("HEARING_AID_ORDER_ID")]
        public int? HearingAidOrderId
        {
            get { return _hearingAidOrderId; }
            set { this.SetProperty("HearingAidOrderId", ref _hearingAidOrderId, value); }
        }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer
        {
            get { return _customer; }
            set { this.SetProperty("Customer", ref _customer, value); }
        }

        //[ForeignKey("HearingAidOrderId")]
        public virtual CustomerHearingAidOrder HearingAidOrder
        {
            get { return _hearingAidOrder; }
            set { this.SetProperty("HearingAidOrder", ref _hearingAidOrder, value); }
        }

        //[DataMember]
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
                return this.MoldAmount.HasValue && this.AdvanceAmtReceived.HasValue ? this.MoldAmount.Value - this.AdvanceAmtReceived.Value : decimal.Zero;
            }
            set{}
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

        public CustomerEarMoldOrder()
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

                //if (columnName == "HiModelLeft")
                //{
                //    return string.IsNullOrEmpty(this.HiModelLeft) ? ErrorResources.RequiredField : null;
                //}

                if (columnName == "EarMoldType")
                {
                    return string.IsNullOrEmpty(this.EarMoldType.ToString()) ? ErrorResources.RequiredField : null;
                }

                if (columnName == "EarMoldHearingAidType")
                {
                    return string.IsNullOrEmpty(this.EarMoldHearingAidType.ToString()) ? ErrorResources.RequiredField : null;
                }

                if (columnName == "EarMoldDesign")
                {
                    return string.IsNullOrEmpty(this.EarMoldDesign.ToString()) ? ErrorResources.RequiredField : null;
                }
                return base[columnName];
            }
        }
        #endregion

    }
}