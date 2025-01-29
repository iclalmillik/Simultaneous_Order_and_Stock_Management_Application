using WebStokYApp.Models.Entities;
using WebStokYApp.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider, AppDbContext context)
    {
        context.Database.EnsureCreated(); // Veritabanının var olup olmadığını kontrol eder.

        // Eğer ürünler zaten varsa, eklemeyelim
        if (context.Products.Any())
        {
            return; // Veritabanı zaten doldurulmuş
        }

        // Başlangıç ürünlerini ekleyelim
        var products = new Product[]
        {
            new Product { ProductName = "Product1", Stock = 500, Price = 100 },
            new Product { ProductName = "Product2", Stock = 10, Price = 200 },
            new Product { ProductName = "Product3", Stock = 75, Price = 75 },
            new Product { ProductName = "Product4", Stock = 0, Price = 50 },
            new Product { ProductName = "Product5", Stock = 500, Price = 45 }
        };

        foreach (var p in products)
        {
            context.Products.Add(p);
        }

        // Değişiklikleri kaydet
        context.SaveChanges();
    }
}
