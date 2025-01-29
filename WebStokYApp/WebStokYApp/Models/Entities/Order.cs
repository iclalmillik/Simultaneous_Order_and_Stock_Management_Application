using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebStokYApp.Models.Entities
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [ForeignKey("Customer")]
        public int CustomerID { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal? TotalPrice { get; set; } // Nullable kaldırıldı
        [Required]
        public string? OrderStatus { get; set; } = "Pending"; // Sipariş durumu (Pending/Approved)
        [Required]
        public DateTime? OrderDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime OrderTime { get; set; } = DateTime.Now; // Sipariş oluşturulma zamanı

        // Navigation Properties
        public virtual Customer? Customer { get; set; }
        public virtual Product? Product { get; set; }
    }
}
