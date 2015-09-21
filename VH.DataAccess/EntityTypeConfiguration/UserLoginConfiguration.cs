using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using VH.Model;

namespace VH.DataAccess
{
    public class UserLoginConfiguration : EntityTypeConfiguration<UserLogin>
    {
        internal UserLoginConfiguration() : base()
        {
            this.HasKey(p => p.ID);
            Property(p => p.ID).
                HasColumnName("ID").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();

            Property(p => p.Status).
               HasColumnName("STATUS").
               IsRequired();

            HasRequired(x => x.User).
                WithMany().
                Map(x => x.MapKey("UserId"));
            ToTable("TBL_USER_LOGIN");
        }
    }
}
