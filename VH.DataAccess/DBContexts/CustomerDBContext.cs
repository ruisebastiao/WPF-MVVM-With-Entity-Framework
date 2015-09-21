using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using VH.Model;

namespace VH.DataAccess
{
    public class CustomerDBContext : DbContext
    {
        #region Properties - DBSet
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerRepair> CustomerRepairs { get; set; }
        public DbSet<CustomerHearingAidOrder> CustomerHearingAidOrders { get; set; }
        public DbSet<CustomerEarMoldOrder> CustomerEarMoldOrders { get; set; }
        public DbSet<CustomerWarrantyInformed> CustomerWarrantyInformeds { get; set; }

        
        
        #endregion

        #region Contructors
        public CustomerDBContext(string connectionString)
            : base(connectionString)
        {
        }
        #endregion
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CustomerConfiguration());
            modelBuilder.Configurations.Add(new CustomerAddressConfiguration());
            modelBuilder.Configurations.Add(new CustomerPhoneConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
