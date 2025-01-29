using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebStokYApp.Models;
using WebStokYApp.Models.Entities;
using WebStokYApp.Models.ViewsModel;
using WebStokYApp.Helpers;


namespace WebStokYApp.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
      
        public CartController(AppDbContext context)
        {
            _context = context;
        
        
    }


        [HttpGet]
        public async Task<IActionResult> AddToCart()
        {
            var customerName = User.Identity.Name;

          
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerName == customerName);
            if (customer == null) return Unauthorized();

        
            var cartItems = await _context.Carts
                .Where(c => c.CustomerId == customer.CustomerID)
                .Include(c => c.Product)
                .ToListAsync();

            // Bekleyen ve onaylı siparişleri al
            var pendingOrders = await _context.Orders
                .Where(o => o.CustomerID == customer.CustomerID && o.OrderStatus == "Pending")
                .Include(o => o.Product)
                .ToListAsync();

            var approvedOrders = await _context.Orders
                .Where(o => o.CustomerID == customer.CustomerID && o.OrderStatus == "Approved")
                .Include(o => o.Product)
                .ToListAsync();

            var model = new CartAndOrdersViewModel
            {
                CartItems = cartItems,
                PendingOrders = pendingOrders,
                ApprovedOrders = approvedOrders
            };

            
            if (!cartItems.Any() && !pendingOrders.Any() && !approvedOrders.Any())
            {
                TempData["InfoMessage"] = "Your cart is empty and you have no orders.";
                return View(model); 
            }

            return View(model); 
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var customerName = User.Identity.Name;
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerName == customerName);
            if (customer == null) return Unauthorized();

            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductID == productId);
            if (product == null)
            {
                await LogHelper.AddLog(_context, LogHelper.CreateLog(customer.CustomerID, "Error", "No product found."));
                return NotFound();
            }

            if (product.Stock <= 0)
            {
                await LogHelper.AddLog(_context, LogHelper.CreateLog(customer.CustomerID, "Error", "Product stock is insufficient."));
                TempData["ErrorMessage"] = "Sorry, this product is out of stock.";
                return RedirectToAction("AddToCart");
            }

            var existingCart = await _context.Carts.FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerID && c.ProductId == productId);
            if (existingCart != null)
            {
                if (existingCart.Quantity < 5)
                {
                    existingCart.Quantity++;
                }
                else
                {
                    await LogHelper.AddLog(_context, LogHelper.CreateLog(customer.CustomerID, "Error", "You cannot add more than 5 of the same product to the cart."));
                    TempData["ErrorMessage"] = "You cannot add more than 5 of the same product to the cart.";
                    return RedirectToAction("AddToCart");
                }
            }
            else
            {
                var cart = new Cart
                {
                    CustomerId = customer.CustomerID,
                    ProductId = productId,
                    Quantity = 1
                };
                _context.Carts.Add(cart);
            }

            await _context.SaveChangesAsync();
            await LogHelper.AddLog(_context, LogHelper.CreateLog(customer.CustomerID, "Information", $"Product added to cart: {product.ProductName}."));

            return RedirectToAction("AddToCart");
        }


        // Sepeti görüntüle
        public async Task<IActionResult> Index()
        {
           

            return View();
        }


        [HttpPost]

        public async Task<IActionResult> RemoveFromCart(int cartId)
        {
            
            var cartItem = await _context.Carts.Include(c => c.Product).FirstOrDefaultAsync(c => c.Id == cartId);
            if (cartItem == null)
            {
                return NotFound();
            }

           
            if (cartItem.Quantity > 1)
            {
               
                cartItem.Quantity--;




                _context.Update(cartItem.Product); 
            }
            else
            {
                _context.Carts.Remove(cartItem);




                _context.Update(cartItem.Product); 
            }

               await _context.SaveChangesAsync();

           
            return RedirectToAction("AddToCart");
        }
  


        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            var customerName = User.Identity.Name;
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerName == customerName);
            if (customer == null) return Unauthorized();

            var cartItems = await _context.Carts.Where(c => c.CustomerId == customer.CustomerID).Include(c => c.Product).ToListAsync();
            if (cartItems == null || !cartItems.Any())
            {
                await LogHelper.AddLog(_context, LogHelper.CreateLog(customer.CustomerID, "Error", "The cart is empty"));
                return RedirectToAction("AddToCart");
            }

            var orders = cartItems.Select(cartItem => new Order
            {
                CustomerID = customer.CustomerID,
                ProductID = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                TotalPrice = cartItem.Quantity * cartItem.Product.Price,
                OrderStatus = "Pending",
                OrderDate = DateTime.Now
            }).ToList();

            _context.Orders.AddRange(orders);
            _context.Carts.RemoveRange(cartItems);

            try
            {
                await _context.SaveChangesAsync();
                foreach (var order in orders)
                {
                    await LogHelper.AddLog(_context, LogHelper.CreateLog(customer.CustomerID, "Information", $"Order created: {order.ProductID}.", order.OrderID));
                }
            }
            catch (Exception ex)
            {
                await LogHelper.AddLog(_context, LogHelper.CreateLog(customer.CustomerID, "Error", $"An error occurred while creating the order.: {ex.Message}."));
                return RedirectToAction("AddToCart");
            }

            return View("OrderSummary", orders);
        }


        public IActionResult OrderSummary()
        {
            return View();
        }
        public IActionResult PendingOrders()
        {
            return View();
        }

    }
}

     

