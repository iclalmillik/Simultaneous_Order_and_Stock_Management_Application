using WebStokYApp.Models.Entities;

namespace WebStokYApp.Models.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; } 
        public int ProductId { get; set; }  
        public int Quantity { get; set; } = 1; 

        // İlişkiler
        public Customer Customer { get; set; }
        public Product Product { get; set; }

     
    }
}
