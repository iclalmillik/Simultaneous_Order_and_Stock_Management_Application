using System.ComponentModel.DataAnnotations;

namespace WebStokYApp.Models.Entities
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required]
        [StringLength(100)]
        public string? ProductName { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal? Price { get; set; }

        //[Timestamp]
        //public byte[]? RowVersion { get; set; } // Eşzamanlılık denetimi için
    }
}
