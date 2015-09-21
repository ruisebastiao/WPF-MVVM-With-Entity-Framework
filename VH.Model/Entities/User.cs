using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VH.Model
{
    [Table("TBL_USER")]
    public class User : VHEntity<User>
    {
        #region Fileds
        private string _firstName;
        private string _lastName;
        private int _age;
        private Gender _gender;
        private string _email;
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

        [Column("AGE")]    
        public int Age
        {
            get { return _age; }
            set { this.SetProperty("Age", ref _age, value);  }
        }

        [Column("GENDER")]
        public Gender Gender
        {
            get { return _gender; }
            set { this.SetProperty("Gender", ref _gender, value); }
        }

        [Column("EMAIL")]
        public string  Email
        {
            get { return _email; }
            set { this.SetProperty("Email", ref _email, value); }
        }

        #endregion

        #region Constructors
        
        #endregion

        #region Public Methods
        
        #endregion
    }
}
