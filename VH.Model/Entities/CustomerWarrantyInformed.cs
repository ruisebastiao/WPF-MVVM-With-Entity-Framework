using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace VH.Model
{
    [Serializable]
    [DataContract]
    [Table("TBL_CUSTOMER_WARRANTY_INFORMED")]
    public class CustomerWarrantyInformed : VHEntity<CustomerWarrantyInformed>
    {
       #region Fields
        private DateTime _informedDate;
        private string _informedMessage;
        private bool _isSelected;
        private Customer _customer;
        private int _customerID;

        #endregion

        #region Properties

         [DataMember]
        [Column("INFORMED_DATE")]
        public DateTime InformedDate
        {
            get { return _informedDate; }
            set { this.SetProperty("ReminderDate", ref _informedDate, value); }
        }

         [DataMember]
        [Column("INFORMED_MESSAGE")]
        public string InformedMessage
        {
            get { return _informedMessage; }
            set { this.SetProperty("ReminderMessage", ref _informedMessage, value); }
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
            set { this.SetProperty("IsSelected", ref _isSelected, value); }
        }


        #endregion

        #region Constructors

        public CustomerWarrantyInformed()
        {
            this.InformedDate = DateTime.Now;
        }

        #endregion
    }
}