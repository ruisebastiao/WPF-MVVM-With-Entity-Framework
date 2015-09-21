using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using VH.Resources;

namespace VH.Model
{
    [Serializable]
    [DataContract]
    [Table("TBL_CUSTOMER")]
    public class Customer : VHEntity<Customer>
    {
        #region Fields
        private string _firstName;
        private string _lastName;
        private Gender _gender;
        private string _email;
        private string _middleName;
        private DateTime _dob = DateTime.Now;
        private ICollection<CustomerAddress> _customerAddresseCollection;
        private ICollection<CustomerPhone> _customerPhoneCollection;
        private bool _isSelected;
        private ICollection<CustomerRepair> _customerRepairCollection;
        private ICollection<CustomerEarMoldOrder> _customerEarMoldOrderCollection;
        private ICollection<CustomerHearingAidOrder> _customerHearingAidOrderCollection;
        private ICollection<CustomerWarrantyInformed> _customerWarrantyInformedCollection;

        #endregion

        #region Properties
        [Column("FIRSTNAME")]
        public string FirstName
        {
            get { return _firstName; }
            set { this.SetProperty("FirstName", ref _firstName, value); }
        }
        [Column("LASTNAME")]
        public string LastName
        {
            get { return _lastName; }
            set { this.SetProperty("LastName", ref _lastName, value); }
        }
        [Column("MIDDLENAME")]
        public string MiddleName
        {
            get { return _middleName; }
            set { this.SetProperty("MIDDLENAME", ref _middleName, value); }
        }
        [Column("GENDER")]
        public Gender Gender
        {
            get { return _gender; }
            set { this.SetProperty("Gender", ref _gender, value); }
        }

        [Column("EMAIL")]
        public string Email
        {
            get { return _email; }
            set { this.SetProperty("Email", ref _email, value); }
        }

        [Column("DOB")]
        public DateTime DOB
        {
            get { return _dob; }
            set { this.SetProperty("DOB", ref _dob, value); }
        }

        public virtual ICollection<CustomerAddress> CustomerAddresseCollection
        {
            get { return _customerAddresseCollection; }
            set { this.SetProperty("CustomerAddresses", ref _customerAddresseCollection, value); }
        }

        public virtual ICollection<CustomerPhone> CustomerPhoneCollection
        {
            get { return _customerPhoneCollection; }
            set
            {
                this.SetProperty("CustomerPhoneCollection", ref _customerPhoneCollection, value);
                this.NotifyPropertyChanged("HasCustomerRepair");
            }
        }

        public ICollection<CustomerRepair> CustomerRepairCollection
        {
            get { return _customerRepairCollection; }
            set { this.SetProperty("CustomerRepairCollection", ref _customerRepairCollection, value); }
        }

        public ICollection<CustomerEarMoldOrder> CustomerEarMoldOrderCollection
        {
            get { return _customerEarMoldOrderCollection; }
            set
            {
                this.SetProperty("CustomerEarMoldOrderCollection", ref _customerEarMoldOrderCollection, value);
                this.NotifyPropertyChanged("HasCustomerEarMoldOrder");
            }
        }

        public ICollection<CustomerHearingAidOrder> CustomerHearingAidOrderCollection
        {
            get { return _customerHearingAidOrderCollection; }
            set
            {
                this.SetProperty("CustomerHearingAidOrderCollection", ref _customerHearingAidOrderCollection, value);
                this.NotifyPropertyChanged("HasCustomerHearingAidOrder");
            }
        }

        public ICollection<CustomerWarrantyInformed> CustomerWarrantyInformedCollection
        {
            get { return _customerWarrantyInformedCollection; }
            set { this.SetProperty("CustomerWarrantyInformedCollection", ref _customerWarrantyInformedCollection, value); }
        }

        [NotMapped]
        public string FormattedName
        {
            get { return !string.IsNullOrEmpty(LastName) ? this.LastName + ", " + this.FirstName : this.FirstName; }
        }

        [NotMapped]
        public bool HasCustomerEarMoldOrder
        {
            get { return CustomerEarMoldOrderCollection != null && CustomerEarMoldOrderCollection.Any(); }
        }

        [NotMapped]
        public bool HasCustomerHearingAidOrder
        {
            get { return CustomerHearingAidOrderCollection != null && CustomerHearingAidOrderCollection.Any(); }
        }

        [NotMapped]
        public bool HasCustomerRepair
        {
            get { return CustomerRepairCollection != null && CustomerRepairCollection.Any(); }
        }

        [NotMapped]
        public bool HasValueInAllRequiredField
        {
            get { return ( !string.IsNullOrEmpty(LastName) &&  !string.IsNullOrEmpty(FirstName) && this.CustomerAddresseCollection != null &&
                !string.IsNullOrEmpty(this.CustomerAddresseCollection.FirstOrDefault().Address) && !string.IsNullOrEmpty(this.CustomerAddresseCollection.FirstOrDefault().City) && !string.IsNullOrEmpty(this.CustomerAddresseCollection.FirstOrDefault().Country)
                && this.CustomerPhoneCollection != null && !string.IsNullOrEmpty(this.CustomerPhoneCollection.FirstOrDefault().PhoneNumber) );
            }
        }

        [NotMapped]
        public CustomerAddress FirstOrDefaultAddress
        {
            get
            {
                return this.CustomerAddresseCollection != null && this.CustomerAddresseCollection.Any()
                           ? this.CustomerAddresseCollection.FirstOrDefault()
                           : null;
            }
        }

        [NotMapped]
        public CustomerPhone  FirstOrDefaultPhone
        {
            get
            {
                return this.CustomerPhoneCollection != null && this.CustomerPhoneCollection.Any()
                           ? this.CustomerPhoneCollection.FirstOrDefault()
                           : null;
            }
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
        public string FormattedAddressPhone
        {
            get
            {
                string strFormattedAdressPhone = string.Empty;
                //if (FirstOrDefaultAddress != null)
                //    strFormattedAdressPhone = FirstOrDefaultAddress.Address + Environment.NewLine +
                //                              FirstOrDefaultAddress.City + ", " + FirstOrDefaultAddress.State +
                //                              Environment.NewLine + FirstOrDefaultAddress.Country + "  : " +
                //                              FirstOrDefaultAddress.ZipCode;

                if (this.CustomerAddresseCollection != null && this.CustomerAddresseCollection.Any())
                {
                    strFormattedAdressPhone = CustomerAddresseCollection.Aggregate(string.Empty, (current, customerAddress) => current + (string.IsNullOrEmpty(current) ? customerAddress.FormattedAddress : Environment.NewLine  + customerAddress.FormattedAddress));
                }

                if (this.CustomerPhoneCollection != null && this.CustomerPhoneCollection.Any())
                {
                    string contacts = this.CustomerPhoneCollection.Aggregate(string.Empty, (current, customerPhone) => current + (string.IsNullOrEmpty(current) ? customerPhone.FormattedContactDetail : ", " + customerPhone.FormattedContactDetail));
                    strFormattedAdressPhone += Environment.NewLine + contacts;
                }
               
                
                return strFormattedAdressPhone.Trim();
            }
        }

        [NotMapped]
        public string ConcatenatedPhone
        {
            get
            {
                string value = string.Empty;
                if (this.CustomerPhoneCollection != null && this.CustomerPhoneCollection.Any())
                {
                    value = this.CustomerPhoneCollection.Aggregate(value, (current, customerPhone) => current + (string.IsNullOrEmpty(current) ? customerPhone.PhoneNumber + " " + customerPhone.ContactRelationship : ", " + customerPhone.PhoneNumber + " " + customerPhone.ContactRelationship));
                }
                return value;
            }
        }

        [NotMapped]
        public string ConcatenatedAddress
        {
            get
            {
                string value = string.Empty;
                if (this.CustomerAddresseCollection != null && this.CustomerAddresseCollection.Any())
                {
                    value = this.CustomerAddresseCollection.Aggregate(value, (current, customerAddress) => current + (string.IsNullOrEmpty(current) ? customerAddress.FormattedAddress :Environment.NewLine + customerAddress.FormattedAddress));
                }
                return value;
            }
        }

        #endregion

        #region Constructors
        
        #endregion

        #region Public Methods
        
        #endregion

        #region Override Methods
        public override string this[string columnName]
        {
            get
            {
                if (columnName == "FirstName")
                {
                    return string.IsNullOrEmpty(this._firstName) ? ErrorResources.RequiredField : null;
                }
                if (columnName == "LastName")
                {
                    return string.IsNullOrEmpty(this._lastName) ? ErrorResources.RequiredField : null;
                }

                if (columnName == "Email")
                {
                    if (!String.IsNullOrEmpty(_email))
                    {
                        var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                        var match = regex.Match(_email);
                        return match.Success ? "" : ErrorResources.InvalidEmailFormat;

                    }
                }
                return null;
            }
        }
        #endregion
    }
}
