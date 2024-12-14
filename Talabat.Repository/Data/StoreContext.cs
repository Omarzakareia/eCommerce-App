using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data;
public class StoreContext : DbContext
{
    public StoreContext(DbContextOptions<StoreContext> options):base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.ApplyConfiguration(new ProductConfigurations());
        //modelBuilder.ApplyConfiguration(new ProductBrandConfigurations());
        //modelBuilder.ApplyConfiguration(new ProductTypeConfigurations());
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder); 
    }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductBrand> ProductBrands { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

}
