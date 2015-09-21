using System.Data.Entity;
using VH.Model;

namespace VH.DataAccess
{
    public class CallRegistryDBContext : DbContext
    {
        #region Properties - DBSet
        public DbSet<CallRegistry> CallRegistries { get; set; }
        #endregion

        #region Constructors
        public CallRegistryDBContext(string connectionString)
            : base(connectionString)
        {
        }
        #endregion

        #region Override Methods
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<CallRegistryDBContext>(null);
            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}