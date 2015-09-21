using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using VH.Model;

namespace VH.DataAccess
{
    public class CustomerConfiguration : EntityTypeConfiguration<Customer>
    {
        internal CustomerConfiguration() : base()
        {
            this.HasKey(p => p.ID);

            Property(p => p.ID).
                HasColumnName("ID").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();

            Property(p => p.DOB).
              HasColumnName("DOB").
              IsRequired();

            Property(p => p.FirstName).
              HasColumnName("FIRSTNAME").
              IsRequired();

            Property(p => p.LastName).
              HasColumnName("LASTNAME").
              IsRequired();

            Property(p => p.Gender).
              HasColumnName("GENDER").
              IsRequired();

            ToTable("TBL_CUSTOMER");
        }
    }

    public class CustomerAddressConfiguration : EntityTypeConfiguration<CustomerAddress>
    {
        internal CustomerAddressConfiguration()
            : base()
        {
            this.HasKey(p => p.ID);

            Property(p => p.ID).
                HasColumnName("ID").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();

            Property(p => p.Address).
               HasColumnName("ADDRESS").
               IsRequired();

            Property(p => p.City).
              HasColumnName("CITY").
              IsRequired();

            Property(p => p.Country).
              HasColumnName("COUNTRY").
              IsRequired();

            Property(p => p.State).
              HasColumnName("STATE").
              IsRequired();

            //HasRequired(x => x.Customer).WithMany(x => x.CustomerAddresseCollection).Map(y => y.MapKey("CustomerID"));
            HasRequired(x => x.Customer).WithMany(x => x.CustomerAddresseCollection);

            ToTable("TBL_CUSTOMER_ADDRESS");
        }
    }

    public class CustomerPhoneConfiguration : EntityTypeConfiguration<CustomerPhone>
    {
        internal CustomerPhoneConfiguration()
            : base()
        {
            this.HasKey(p => p.ID);

            Property(p => p.ID).
                HasColumnName("ID").
                HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity).
                IsRequired();

            Property(p => p.PhoneType).
             HasColumnName("PHONE_TYPE").
             IsRequired();

            Property(p => p.PhoneNumber).
             HasColumnName("PHONE_NUMBER").
             IsRequired();

            //HasRequired(x => x.Customer).WithMany(x => x.CustomerPhoneCollection).Map(y => y.MapKey("CustomerID"));
            HasRequired(x => x.Customer).WithMany(x => x.CustomerPhoneCollection);

            ToTable("TBL_CUSTOMER_PHONE");
        }
    }
}
