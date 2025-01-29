using WebStokYApp.Models.Entities;

namespace WebStokYApp.Models.ViewsModel
{
    public class OrdersViewModel
    {
        public List<Order> PendingOrders { get; set; }
        public List<Order> ApprovedOrders { get; set; }
        public List<Order> RejectedOrders { get; set; }
    }
}
