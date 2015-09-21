using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VH.Model;

namespace VH.DataAccess
{
    //public class TestConnection
    //{
    //    public static void CreateConnection()
    //    {
    //        string connection = ConfigurationManager.ConnectionStrings["VHEAPDB"].ConnectionString;
    //        using (var context = new UserDB(connection))
    //        {
    //            var ite = context.Users.Single(x => x.Id == 1);
    //        }
    //    }
    //}
    public class UserDBContext : DbContext
    {
        #region Properties
        public DbSet<User> Users { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        #endregion

        #region Constructors
        public UserDBContext(string connectionString)
            : base(connectionString)
        {
        }
        #endregion

        #region Virtual Methods
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<UserDBContext>(null); // <--- This is what i needed

            modelBuilder.Entity<User>().HasKey(k => k.ID).ToTable("TBL_USER");

            base.OnModelCreating(modelBuilder);
        }
        #endregion
        
    }

    public class UserLoginDBContext : DbContext
    {
        #region Properties
        public DbSet<UserLogin> UserLogins { get; set; }
        #endregion

        #region Constructors
        public UserLoginDBContext(string connectionString)
            : base(connectionString)
        {
        }
        #endregion

        #region Virtual Methods
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<UserLoginDBContext>(null); // <--- This is what i needed

           // modelBuilder.Entity<UserLogin>().HasKey(k => k.ID).ToTable("TBL_USER_LOGIN");
            modelBuilder.Configurations.Add(new UserLoginConfiguration());

            //modelBuilder.Configurations.Add(new LoginConfiguration());

            //modelBuilder.Entity<Login>().Property(p => p.LOGINNAME).HasColumnName("LOGIN_NAME");
            //var entity = modelBuilder.Entity<Login>();
            //entity.Property(x => x.LOGINNAME).HasColumnName("LOGIN_NAME");
            //entity.HasKey(x => x.ID);
            //entity.ToTable("TBL_USER_LOGIN");

            base.OnModelCreating(modelBuilder);
        }
        #endregion
        

    }
}
