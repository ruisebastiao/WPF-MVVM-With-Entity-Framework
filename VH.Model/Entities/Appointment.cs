using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using VH.Resources;

namespace VH.Model
{
    [Serializable]
    [Table("TBL_APPOINTMENT")]
    public class Appointment : VHEntity<Appointment>
    {

        #region Fileds

        private string _name;
        private int? _customerID;
        private Customer _customer;
        private string _appointmentType;
        private DateTime _appointmentDate;
        private DateTime _beginTime;
        private DateTime _endTime;
        private string _contactNumber;
        private bool _isSelected;
        private bool _isAppointmentOverlapped;
        private Customer _selectedCustomer;
        private DateTime _appointmentTime;
        private string _comment;

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

        [Column("APPOINTMENTTYPE")]
        public string AppointmentType
        {
            get { return _appointmentType; }
            set { this.SetProperty("AppointmentType", ref _appointmentType, value); }
        }

        [Column("APPOINTMENTDATE")]
        public DateTime AppointmentDate
        {
            get { return _appointmentDate.Date; }
            set
            {
                this.SetProperty("AppointmentDate", ref _appointmentDate, value.Date);
             
            }
        }

        [Column("APPOINTMENTTIME")]
        public DateTime AppointmentTime
        {
            get { return _appointmentTime; }
            set
            {
                this.SetProperty("AppointmentTime", ref _appointmentTime, value);

            }
        }

        //[Column("BEGINTIME")]
        //public DateTime BeginTime
        //{
        //    get { return _beginTime; }
        //    set
        //    {
        //        this.SetProperty("BeginTime", ref _beginTime, value);
        //        EndTime = value.AddMinutes(30);
        //    }
        //}

        //[Column("ENDTIME")]
        //public DateTime EndTime
        //{
        //    get { return _endTime; }
        //    set
        //    {
        //        if (value > BeginTime)
        //            this.SetProperty("EndTime", ref _endTime, value);
        //    }
        //}

        [Column("CONTACTNUMBER")]
        public string ContactNumber
        {
            get { return _contactNumber; }
            set { this.SetProperty("ContactNumber", ref _contactNumber, value); }
        }

        [Column("COMMENTS")]
        public string Comment
        {
            get { return _comment; }
            set { this.SetProperty("Comment", ref _comment, value); }
        }

        [NotMapped]
        public bool IsSelected
        {
            get { return _isSelected; }
            set { this.SetProperty("IsSelected", ref _isSelected, value); }
        }

        //[NotMapped]
        //public string AppointmentTime
        //{
        //    get
        //    {
        //        return string.Format("{0} - {1}", this.BeginTime.ToString("hh:mm tt"),
        //                             this.EndTime.ToString("hh:mm tt"));
        //    }
        //}

        [NotMapped]
        public bool IsAppointmentOverlapped
        {
            get { return _isAppointmentOverlapped; }
            set { this.SetProperty("IsAppointmentOverlapped", ref _isAppointmentOverlapped, value); }
        }

        [NotMapped]
        public bool IsSaveEnabled
        {
            get
            {
                return this.AppointmentDate != null && !string.IsNullOrEmpty(this.Name) &&
                       !string.IsNullOrEmpty(this.ContactNumber) ;
            }
        }

    //    [NotMapped]
    //    public bool IsValidAppiontSechduleDateRange {
    //        get
    //        {
    //            return this.AppointmentDate >= DateTime.Now.Date && this.BeginTime >=
    //                                                                                    DateTime.Now && this.EndTime >= DateTime.Now;
    //        }
    //}

        [NotMapped]
        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                this.SetProperty("SelectedCustomer", ref _selectedCustomer, value);
                this.CustomerID = value.ID;
                this.Name = value.FirstName;
                this.ContactNumber = value.FirstOrDefaultPhone != null ? value.FirstOrDefaultPhone.PhoneNumber : null;
                this.NotifyPropertyChanged("FormattedExistingCustomerName");
            }
        }

        public string FormattedExistingCustomerName
        {
            get
            {
                return this.SelectedCustomer != null
                           ? string.Format("{0} - {1}", this.SelectedCustomer.ID, this.SelectedCustomer.FormattedName)
                           : "";
            }
        }

        #endregion

        #region Constructors
        public Appointment()
        {
            this.CreateDefaultValue();
        }
        #endregion

        #region Private Methods
        private void CreateDefaultValue()
        {
            this.AppointmentDate = DateTime.Now;
            this.AppointmentTime = DateTime.Now;
        }

        public DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }
        #endregion

        #region Public Methods

        #endregion

        #region Override Methods

         public override string this[string columnName]
         {
             get
             {
                 if (columnName == "Name")
                 {
                     return string.IsNullOrEmpty(this.Name) ? ErrorResources.RequiredField : null;
                 }
                 if (columnName == "ContactNumber")
                 {
                     return string.IsNullOrEmpty(this.ContactNumber) ? ErrorResources.RequiredField : null;
                 }
                 if (columnName == "AppointmentDate")
                 {
                     if (!(this.AppointmentDate >= DateTime.Now.Date))
                         return ErrorResources.AppointmentDateValidation;

                     return string.IsNullOrEmpty(this.AppointmentDate.ToString(CultureInfo.InvariantCulture)) ? ErrorResources.RequiredField : null;
                 }
                 if (columnName == "AppointmentTime")
                 {
                return string.IsNullOrEmpty(this.AppointmentTime.ToString(CultureInfo.InvariantCulture)) ? ErrorResources.RequiredField : null;
                 }
                 //if (columnName == "BeginTime")
                 //{
                 //    if (BeginTime > EndTime)
                 //        return ErrorResources.BeginDateValidation;
                 //    if (!(this.BeginTime >= DateTime.Now))
                 //        return ErrorResources.BeginDatePastTimeValidation;
                 //}
                 //if (columnName == "EndTime")
                 //{
                 //    if (EndTime < BeginTime)
                 //        return ErrorResources.EndDateValidation;

                 //    if (!(this.EndTime >= DateTime.Now))
                 //        return ErrorResources.EndDatePastTimeValidation;
                 //}
                 
                 return "";
             }
         }

         #endregion
    }
}