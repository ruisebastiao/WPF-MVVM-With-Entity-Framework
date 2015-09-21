using System.Data.Entity;
using VH.Model;

namespace VH.DataAccess
{
    public class ReminderDBContext : DbContext
    {
           #region Properties - DBSet
        public DbSet<Reminder> Reminders { get; set; }
        #endregion

        #region Constructors
        public ReminderDBContext(string connectionString)
            : base(connectionString)
        {
        }
        #endregion

        #region Override Methods
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ReminderDBContext>(null);
            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}