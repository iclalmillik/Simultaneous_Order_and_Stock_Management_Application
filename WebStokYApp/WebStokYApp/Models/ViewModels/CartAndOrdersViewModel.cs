using WebStokYApp.Models.Entities;

namespace WebStokYApp.Models.ViewsModel
{
    public class CartAndOrdersViewModel
    {
        public IEnumerable<Cart> CartItems { get; set; }
        public IEnumerable<Order> ApprovedOrders { get; set; }
        public IEnumerable<Order> PendingOrders { get; set; } 
        public IEnumerable<Order> RejectedOrders { get; set; } 


    }
}
