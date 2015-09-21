using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using VH.Resources;

namespace VH.Model
{
    [Serializable]
    public class CustomerPhone : VHEntity<CustomerPhone>
    {
        #region Fields
        private PhoneType _phoneType;
        private string _phoneNumber;
        private int _customerID;
        private Customer _customer;
        private bool _isPrimary;
        private bool _isSelected;
        private bool _isDeleted;
        private Relationship _contactRelationship = Relationship.Self;
        private string _contactRelationshipOther;

        #endregion

        #region Properties
        [Column("PHONE_TYPE")]
        public PhoneType PhoneType
        {
            get { return _phoneType; }
            set { this.SetProperty("PhoneType", ref _phoneType, value); }
        }

        [Column("PHONE_NUMBER")]
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { this.SetProperty("PhoneNumber", ref _phoneNumber, value); }
        }

        [Column("CUSTOMER_ID")]
        public int CustomerID
        {
            get { return _customerID; }
            set { this.SetProperty("CustomerID", ref _customerID, value); }
        }

        [Column("CONTACT_RELATIONSHIP")]
        public Relationship ContactRelationship
        {
            get { return _contactRelationship; }
            set
            {
                this.SetProperty("ContactRelationship", ref _contactRelationship, value);
                this.NotifyPropertyChanged("IsContactRelationshipOther");
            }
        }

        [Column("CONTACT_RELATIONSHIP_OTHER")]
        public string ContactRelationshipOther
        {
            get { return _contactRelationshipOther; }
            set { this.SetProperty("ContactRelationshipOther", ref _contactRelationshipOther, value); }
        }

        public bool IsContactRelationshipOther
        {
            get { return this.ContactRelationship == Relationship.Other; }
        }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer
        {
            get { return _customer; }
            set { this.SetProperty("Customer", ref _customer, value); }
        }

        [Column("ISPRIMARY")]
        public bool IsPrimary
        {
            get { return _isPrimary; }
            set { this.SetProperty("IsPrimary", ref _isPrimary, value); }
        }

        [NotMapped]
        public bool IsSelected
        {
            get { return _isSelected; }
            set { this.SetProperty("IsSelected", ref _isSelected, value); }
        }

        [NotMapped]
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { this.SetProperty("IsDeleted", ref _isDeleted, value); }
        }

        [NotMapped]
        public string FormattedContactDetail
        {
            get
            {
                return this.PhoneType + " : " +
                       this.PhoneNumber;
            }
        }

        #endregion

        #region Constructors
        
        #endregion

        #region Override Methods
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "PhoneNumber")
                {
                    if ( string.IsNullOrEmpty(_phoneNumber))
                        return ErrorResources.RequiredField;
                    
                }
                return "";
            }
        }
        #endregion
    }
}
