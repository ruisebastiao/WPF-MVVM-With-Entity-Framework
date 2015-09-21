using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using VH.Resources;

namespace VH.Model
{
    [Serializable]
    public class CustomerAddress : VHEntity<CustomerAddress>
   {
       #region Fields
       private string _address;
       private string _address1;
        private string _city;
        private string _state;
        private string _country;
        private int _zipcode;
        private int _customerID;
        private Customer _customer;
        private bool _isPrimary;
        private bool _isSelected;
        private bool _isDeleted;
        private string _otherCity;
        private bool _isOtherCity;

        #endregion

       #region Properties
        [Column("ADDRESS")]
       public string Address
       {
           get { return _address; }
           set { this.SetProperty("Address", ref _address, value); }
       }

        [Column("ADDRESS1")]
       public string Address1
       {
           get { return _address1; }
           set { this.SetProperty("Address1", ref _address1, value); }
       }

        [Column("CITY")]
       public string City
       {
           get { return _city; }
           set { this.SetProperty("City", ref _city, value); }
       }

        [Column("OTHER_CITY")]
        public string OtherCity
        {
            get { return _otherCity; }
            set { this.SetProperty("OtherCity", ref _otherCity, value); }
        }

        [Column("STATE")]
       public string State
       {
           get { return _state; }
           set { this.SetProperty("State", ref _state, value); }
       }

        [Column("COUNTRY")]
       public string Country
       {
           get { return _country; }
           set { this.SetProperty("Country", ref _country, value); }
       }
        [Column("ZIPCODE")]
       public int ZipCode
       {
           get { return _zipcode; }
           set { this.SetProperty("ZipCode", ref _zipcode, value); }
       }

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
         public bool IsOtherCity
         {
             get { return _isOtherCity; }
             set { this.SetProperty("IsOtherCity", ref _isOtherCity, value); }
         }

        [NotMapped]
        public string FormattedAddress
        {
            get
            {
               return this.Address + Environment.NewLine +
                   (this.City + (!string.IsNullOrEmpty(this.OtherCity) ?  " : " + this.OtherCity : "")) + ", " + this.State +
                                              Environment.NewLine + this.Country + "  : " +
                                              this.ZipCode;
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
                 if (columnName == "Address")
                     return String.IsNullOrEmpty(_address) ? ErrorResources.RequiredField : "";

                 if (columnName == "City")
                     return String.IsNullOrEmpty(_city) ? ErrorResources.RequiredField : "";

                 if (columnName == "State")
                     return String.IsNullOrEmpty(_state) ? ErrorResources.RequiredField : "";

                 if (columnName == "Country")
                     return String.IsNullOrEmpty(_country) ? ErrorResources.RequiredField : "";

                 return "";
             }
         }
       #endregion
   }
}
