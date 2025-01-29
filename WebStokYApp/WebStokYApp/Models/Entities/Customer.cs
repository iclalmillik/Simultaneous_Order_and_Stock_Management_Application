using System.ComponentModel.DataAnnotations;

namespace WebStokYApp.Models.Entities
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        [Required]
        [StringLength(100)]
        public string? CustomerName { get; set; }

     
        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }


        [Required]
        [Range(0, double.MaxValue)]
        public decimal? Budget { get; set; }

        [Required]
        [StringLength(50)]
        public string? CustomerType { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? TotalSpent { get; set; }
    }
}
