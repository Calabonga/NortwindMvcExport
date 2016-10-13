using System.Data.Entity;
using Northwind.Data.Common;
using Northwind.Models;

namespace Northwind.Data {

    public interface IContext : IDbContext {

        DbSet<Category> Category { get; set; }

        DbSet<Customer> Customer { get; set; }
        
        DbSet<Employee> Employee { get; set; }

        DbSet<Order> Order { get; set; }

        DbSet<OrderDetail> OrderDetail { get; set; }

        DbSet<Product> Product { get; set; }

        DbSet<Shipper> Shipper { get; set; }

        DbSet<Supplier> Supplier { get; set; }
    }
}