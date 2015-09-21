using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VH.Model
{
    [Serializable]
    [Table("TBL_REMINDER")]
    public class Reminder : VHEntity<Reminder>
    {
       #region Fields
        private DateTime _reminderDate;
        private ReminderType _reminderType;
        private string _reminderMessage;
        private bool _isSelected;
        private string _reminderName;

        #endregion

        #region Properties
        [Column("REMINDER_NAME")]
        public string ReminderName
        {
            get { return _reminderName; }
            set { this.SetProperty("ReminderName", ref _reminderName, value); }
        }

        [Column("REMINDER_DATE")]
        public DateTime ReminderDate
        {
            get { return _reminderDate; }
            set { this.SetProperty("ReminderDate", ref _reminderDate, value); }
        }

        [Column("REMINDER_TYPE")]
        public ReminderType ReminderType
        {
            get { return _reminderType; }
            set { this.SetProperty("ReminderType", ref _reminderType, value); }
        }

        [Column("REMINDER_MESSAGE")]
        public string ReminderMessage
        {
            get { return _reminderMessage; }
            set { this.SetProperty("ReminderMessage", ref _reminderMessage, value); }
        }

        [NotMapped]
        public bool IsSelected
        {
            get { return _isSelected; }
            set { this.SetProperty("IsSelected", ref _isSelected, value); }
        }

         [NotMapped]
        public string FormattedReminder
        {
            get
            {
                return string.Format("{0} : {1} : {2}", this.ReminderName, this.ReminderType, this.ReminderDate.Date.ToLongDateString()) + Environment.NewLine + this.ReminderMessage;
            }
        }

        #endregion

        #region Constructors

        public Reminder()
        {
            this.ReminderDate = DateTime.Now;
        }

        #endregion
    }
}