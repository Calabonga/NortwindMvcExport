using System.Data.Entity;

namespace Northwind.Data.Common {

    public abstract class DataContext : DbContext, IDbContext {
        
        protected DataContext(string nameOrConnectionString):base(nameOrConnectionString) {
            Configuration.AutoDetectChangesEnabled = true;
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
        }

    }
}
