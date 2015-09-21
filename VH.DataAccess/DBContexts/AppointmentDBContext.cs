using System.Data.Entity;
using VH.Model;

namespace VH.DataAccess
{
    public class AppointmentDBContext : DbContext
    {
        #region Properties - DBSet

        public DbSet<Appointment> Appointments { get; set; }

        #endregion

        #region Contructors

        public AppointmentDBContext(string connectionString)
            : base(connectionString)
        {
        }

        #endregion

        #region Override Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<AppointmentDBContext>(null);
            base.OnModelCreating(modelBuilder);
        }

        #endregion

    }
}