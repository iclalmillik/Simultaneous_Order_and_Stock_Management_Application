using System.ComponentModel.DataAnnotations;

namespace WebStokYApp.Models.Entities
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }

        [Required]
        [StringLength(100)]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }


    }
}
