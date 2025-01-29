using Microsoft.EntityFrameworkCore;
using WebStokYApp.Models.Entities;

namespace WebStokYApp.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        // DbSets - Veritabanındaki tabloları temsil eder
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Cart> Carts { get; set; }
        // OnModelCreating Metodu
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Customers Tablosu
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customers"); // Tablo adı
                entity.HasKey(e => e.CustomerID); // Primary Key

                entity.Property(e => e.CustomerName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Budget)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.CustomerType)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.TotalSpent)
                      .HasColumnType("decimal(18,2)");

                modelBuilder.Entity<Customer>()
    .Property(c => c.Password)
    .HasDefaultValue("123");
            });

            // Products Tablosu
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(e => e.ProductID);

                entity.Property(e => e.ProductName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Stock)
                      .IsRequired();

                entity.Property(e => e.Price)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)");
            });

            // Logs Tablosu
            modelBuilder.Entity<Log>(entity =>
            {
                entity.ToTable("Logs");
                entity.HasKey(e => e.LogID);

                entity.Property(e => e.LogDate)
                      .IsRequired();

                entity.Property(e => e.LogType)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.LogDetails)
                      .HasMaxLength(500);

                // Foreign Key: CustomerID
                entity.HasOne(l => l.Customer)
                      .WithMany()
                      .HasForeignKey(l => l.CustomerID)
                      .OnDelete(DeleteBehavior.Restrict);

                // Foreign Key: OrderID
                entity.HasOne(l => l.Order)
                      .WithMany()
                      .HasForeignKey(l => l.OrderID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Orders Tablosu
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.HasKey(e => e.OrderID);

                entity.Property(e => e.Quantity)
                      .IsRequired();

                entity.Property(e => e.TotalPrice)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.OrderDate)
                      .IsRequired();

                entity.Property(e => e.OrderStatus)
                      .HasMaxLength(50)
                      .IsRequired();

                // Foreign Key: CustomerID
                entity.HasOne(o => o.Customer)
                      .WithMany()
                      .HasForeignKey(o => o.CustomerID)
                      .OnDelete(DeleteBehavior.Cascade);

                // Foreign Key: ProductID
                entity.HasOne(o => o.Product)
                      .WithMany()
                      .HasForeignKey(o => o.ProductID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Cart Tablosu
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Carts"); // Tablo adı
                entity.HasKey(e => e.Id); // Primary Key

                entity.Property(e => e.Quantity)
                      .IsRequired()
                      .HasDefaultValue(1); // Varsayılan değer 1

                // Foreign Key: CustomerID
                entity.HasOne(c => c.Customer)
                      .WithMany() // Her müşteri birçok sepete sahip olabilir
                      .HasForeignKey(c => c.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade); // Müşteri silindiğinde sepet de silinsin

                // Foreign Key: ProductID
                entity.HasOne(c => c.Product)
                      .WithMany() // Her ürün birçok sepetle ilişkilendirilebilir
                      .HasForeignKey(c => c.ProductId)
                      .OnDelete(DeleteBehavior.Restrict); // Ürün silindiğinde sepeti etkilemesin
            });


            // Rastgele Müşteri Seed Verisi
            Random rnd = new Random();
            var customers = new List<Customer>();


            int totalCustomers = rnd.Next(5, 11); 
            int premiumCount = 0;

            for (int i = 1; i <= totalCustomers; i++)
            {
                string customerType = premiumCount < 2 ? "Premium" : (rnd.Next(0, 2) == 0 ? "Normal" : "Premium");
                if (customerType == "Premium") premiumCount++;

                customers.Add(new Customer
                {
                    CustomerID = i,
                    CustomerName = $"Customer{i}",
                    Budget = rnd.Next(500, 3001),
                    CustomerType = customerType,
                    TotalSpent = 0,
                    Password = $"123{i}" ,
                  
                });
            }
           
            // HasData kullanarak seed verileri ekle
            modelBuilder.Entity<Customer>().HasData(customers);

          
            var products = new List<Product>
    {
        new Product { ProductID = 1, ProductName = "Product1", Stock = 500, Price = 100 },
        new Product { ProductID = 2, ProductName = "Product2", Stock = 10, Price = 50 },
        new Product { ProductID = 3, ProductName = "Product3", Stock = 200, Price = 45 },
        new Product { ProductID = 4, ProductName = "Product4", Stock = 75, Price = 75 },
        new Product { ProductID = 5, ProductName = "Product5", Stock = 0, Price = 500 },
    };

            modelBuilder.Entity<Product>().HasData(products);

            base.OnModelCreating(modelBuilder);
        }

    }
}

