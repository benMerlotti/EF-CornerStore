using Microsoft.EntityFrameworkCore;
using CornerStore.Models;
public class CornerStoreDbContext : DbContext
{
    public DbSet<Cashier> Cashiers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }

    public CornerStoreDbContext(DbContextOptions<CornerStoreDbContext> context) : base(context)
    {

    }

    //allows us to configure the schema when migrating as well as seed data
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed Cashiers
        modelBuilder.Entity<Cashier>().HasData(
            new Cashier { Id = 1, FirstName = "John", LastName = "Doe" },
            new Cashier { Id = 2, FirstName = "Jane", LastName = "Smith" }
        );

        // Seed Categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, CategoryName = "Beverages" },
            new Category { Id = 2, CategoryName = "Snacks" }
        );

        // Seed Products
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, ProductName = "Cola", Price = 1.50m, Brand = "Coca-Cola", CategoryId = 1 },
            new Product { Id = 2, ProductName = "Chips", Price = 2.00m, Brand = "Lay's", CategoryId = 2 },
            new Product { Id = 3, ProductName = "Water", Price = 1.00m, Brand = "Dasani", CategoryId = 1 }
        );

        // Seed Orders
        modelBuilder.Entity<Order>().HasData(
            new Order { Id = 1, CashierId = 1, PaidOnDate = DateTime.Today },
            new Order { Id = 2, CashierId = 2, PaidOnDate = DateTime.Today }
        );

        // Seed OrderProducts
        modelBuilder.Entity<Product>()
        .HasMany(p => p.Orders)
        .WithMany(o => o.Products)
        .UsingEntity(j => j.HasData(
            new { ProductsId = 1, OrdersId = 1, Quantity = 2 },
            new { ProductsId = 2, OrdersId = 1, Quantity = 1 },
            new { ProductsId = 3, OrdersId = 2, Quantity = 3 }
        ));
    }

}