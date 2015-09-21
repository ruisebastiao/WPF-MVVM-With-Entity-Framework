using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using VH.Resources;

namespace VH.Model
{
    [Serializable]
    [Table("TBL_CALLREGISTRY")]
    public class CallRegistry : VHEntity<CallRegistry>
    {
        #region Fields
        private string _name;
        private int? _customerID;
        private Customer _customer;
        private DateTime _callDate;
        private string _comment;
        private bool _isSelected;
        private string _callContactNumber;
        private DateTime _callTime;

        #endregion

        #region Properties
        [Column("NAME")]
        public string Name
        {
            get { return _name; }
            set { this.SetProperty("Name", ref _name, value); }
        }

        [Column("CUSTOMER_ID")]
        public int? CustomerID
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

        [Column("CALLER_CONTACT_NUMBER")]
        public string CallContactNumber
        {
            get { return _callContactNumber; }
            set { this.SetProperty("CallContactNumber", ref _callContactNumber, value); }
        }

        [Column("CALLDATE")]
        public DateTime CallDate
        {
            get { return _callDate; }
            set
            {
                this.SetProperty("CallDate", ref _callDate, value);
            }
        }

        [Column("CALLTIME")]
        public DateTime CallTime
        {
            get { return _callTime; }
            set { this.SetProperty("CallTime", ref _callTime, value); }
        }

        [Column("COMMENT")]
        public string Comment
        {
            get { return _comment; }
            set { this.SetProperty("Comment", ref _comment, value); }
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
                return (!string.IsNullOrEmpty(Name));
            }
        }
        #endregion

        #region Constructors
        public CallRegistry()
        {
            this.CallDate = DateTime.Now;
            this.CallTime = DateTime.Now;
        }
        #endregion

        #region Public Methods
        
        #endregion

        #region Override 
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "Name")
                {
                    return string.IsNullOrEmpty(this.Name) ? ErrorResources.RequiredField : null;
                }

                if (columnName == "CallDate")
                {
                    return string.IsNullOrEmpty(this.CallDate.ToString(CultureInfo.InvariantCulture)) ? ErrorResources.RequiredField : null;
                }
                return "";
            }
        }
        #endregion
    }
}