using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebStokYApp.Models.Entities
{
    public class Log
    {
        [Key]
        public int LogID { get; set; }

        [ForeignKey("Customer")]
        public int? CustomerID { get; set; } // Nullable, admin işlemleri için gerekmez

        [ForeignKey("Order")]
        public int? OrderID { get; set; } // Nullable, bazı loglar için gerekli değil

        [ForeignKey("Product")]
        public int? ProductID { get; set; }

        [Required]
        public DateTime LogDate { get; set; } // İşlem zamanı


        [Required]
        [StringLength(50)]
        public string LogType { get; set; } // Hata, Uyarı, Bilgilendirme

        [Required]
 
        [StringLength(500)]
        public string LogDetails { get; set; } // İşlem sonucu veya hata mesajı

        // Navigation Properties
        public virtual Customer? Customer { get; set; }
        public virtual Product? Product { get; set; }

        public virtual Order? Order { get; set; }
    }
}
