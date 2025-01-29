using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebStokYApp.Models;
using WebStokYApp.Models.Entities;
using WebStokYApp.Models.ViewsModel;
using WebStokYApp.Helpers2;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace WebStokYApp.Controllers
{
    public class AdminController : Controller
    {

        private readonly AppDbContext _context;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AdminController(AppDbContext context, IServiceScopeFactory serviceScopeFactory)
        {
            _context = context;
            _serviceScopeFactory = serviceScopeFactory; 
        }
        [Authorize]
        public IActionResult Index()
        {
            var customers = _context.Customers.ToList();
            var products = _context.Products.ToList();

            var model = Tuple.Create(customers.AsEnumerable(), products.AsEnumerable());
            return View("CustomerAndProductList", model);
        }
        [Authorize]
        public IActionResult CustomerAndProductList()
        {

            var customers = _context.Customers.ToList(); 
            var products = _context.Products.ToList(); 

           
            var model = Tuple.Create(customers.AsEnumerable(), products.AsEnumerable());
            return View(model);
        }



        [HttpGet]

        public IActionResult Login()
        {
            return View();
        }

    

        [HttpPost]

        public async Task<IActionResult> Login(Admin admin)
        {
            var existingAdmin = await _context.Admins
                .FirstOrDefaultAsync(a => a.UserName == admin.UserName && a.Password == admin.Password);

            if (existingAdmin != null)
            {
                // Admin doğrulandı, kimlik oluştur
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, existingAdmin.UserName),
            new Claim(ClaimTypes.Role, "Admin") // Rol: Admin
        };

                var claimsIdentity = new ClaimsIdentity(claims, "AdminCookie");
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                };

                await HttpContext.SignInAsync("AdminCookie", new ClaimsPrincipal(claimsIdentity), authProperties);

                // Giriş başarılı, admin paneline yönlendir
                return RedirectToAction("CustomerAndProductList", "Admin");
            }

            // Hatalı giriş
            TempData["ErrorMessage"] = "Hatalı kullanıcı adı veya şifre.";
            return RedirectToAction("AdminLogin", "Home");
        }




    
        public IActionResult CustomerList()
        {
            const double WaitingTimeWeight = 0.5; // Bekleme süresi ağırlığı

            // Müşteri listesini al
            var customers = _context.Customers
                .ToList() 
                .Select(customer => new
                {
                    customer.CustomerID,
                    customer.CustomerName,
                    customer.CustomerType,
                    customer.Budget,
                    customer.TotalSpent,
                    PendingOrders = _context.Orders
                        .Where(o => o.CustomerID == customer.CustomerID && o.OrderStatus == "Pending")
                        .ToList(), 
                    WaitingTime = _context.Orders
                        .Where(o => o.CustomerID == customer.CustomerID && o.OrderStatus == "Pending")
                        .ToList() 
                        .Select(o => (DateTime.Now - o.OrderTime).TotalSeconds)
                        .DefaultIfEmpty(0)
                        .Average() // Bekleme süresinin ortalamasını hesapla
                })
                .Select(c => new
                {
                    c.CustomerID,
                    c.CustomerName,
                    c.CustomerType,
                    c.Budget,
                    c.TotalSpent,
                    WaitingTime = Math.Round(c.WaitingTime, 2),
                    PriorityScore = (c.CustomerType == "Premium" ? 15 : 10) + (c.WaitingTime * WaitingTimeWeight)
                })
                .ToList();

            ViewData["Customers"] = customers;
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddProduct()
        {
            // Yeni ürün eklemek için boş bir model gönderiyoruz
            return View(new Product());
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);

                try
                {
                    await _context.SaveChangesAsync();
                    await LogHelper.AddLog(_context, LogHelper.CreateLog("Information", $"Product added(Admin): {product.ProductName}."));

                    TempData["SuccessMessage"] = "Product added successfully!";
                    return RedirectToAction("CustomerAndProductList");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occurred while saving the product.";
                    Console.WriteLine(ex.Message);
                    await LogHelper.AddLog(_context, LogHelper.CreateLog("Error", $"Error occurred while adding product(Admin): {ex.Message}."));

                    return View(product);
                }
            }

            return View(product);
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                _context.Products.Remove(product);

                try
                {
                    await _context.SaveChangesAsync();
                    await LogHelper.AddLog(_context, LogHelper.CreateLog("Information", $"Product deleted(Admin): {product.ProductName}."));
                }
                catch (Exception ex)
                {
                    await LogHelper.AddLog(_context, LogHelper.CreateLog("Error", $"Error occurred while deleting the product(Admin): {ex.Message}."));
                }
            }
            return RedirectToAction("CustomerAndProductList");
        }

        [Authorize]
        [HttpPost]
        public IActionResult UpdateStock(int productId, int stockChange, byte[] rowVersion)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                try
                {
                    if (product.Stock + stockChange >= 0)
                    {
                        product.Stock += stockChange;
                       
                        _context.SaveChanges();
                    }
                    else
                    {
                        TempData["Error"] = "Insufficient stock!";
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["Error"] = "Another process updated the stock. Please try again.";
                }
            }
            return RedirectToAction("CustomerAndProductList");
        }

        [HttpGet]

        public IActionResult ApproveOrder()
        {
            var pendingOrders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .Where(o => o.OrderStatus == "Pending")
                .ToList();

            var approvedOrders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .Where(o => o.OrderStatus == "Approved")
                .ToList();

            var rejectedOrders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                .Where(o => o.OrderStatus == "Rejected")
                .ToList();

            var model = new OrdersViewModel
            {
                PendingOrders = pendingOrders,
                ApprovedOrders = approvedOrders,
                RejectedOrders = rejectedOrders
            };

            return View(model);
        }



        [HttpPost]
        [Authorize]
       
        public async Task<IActionResult> ApproveOrder(List<int> orderIds)
        {
            if (orderIds == null || !orderIds.Any())
            {
                TempData["ErrorMessage"] = "No orders selected to approve.";
                return RedirectToAction("ApproveOrder");
            }

            try
            {
                const double WaitingTimeWeight = 0.5;

                var orders = _context.Orders
                                     .Include(o => o.Customer)
                                     .Where(o => orderIds.Contains(o.OrderID) && o.OrderStatus == "Pending")
                                     .ToList();

                var prioritizedOrders = orders
                    .Select(order =>
                    {
                        var waitingTime = (DateTime.Now - order.OrderTime).TotalSeconds;
                        var basePriorityScore = order.Customer.CustomerType == "Premium" ? 15 : 10;
                        var priorityScore = basePriorityScore + (waitingTime * WaitingTimeWeight);

                        return new
                        {
                            Order = order,
                            PriorityScore = priorityScore
                        };
                    })
                    .OrderByDescending(o => o.PriorityScore)
                    .ToList();

                var threads = new List<Thread>();

                foreach (var prioritizedOrder in prioritizedOrders)
                {
                    var order = prioritizedOrder.Order;
                    var thread = new Thread(() => ApproveOrderInNewThread(order));
                    threads.Add(thread);
                    thread.Start();
                }

                foreach (var thread in threads)
                {
                    thread.Join();
                }

                TempData["SuccessMessage"] = "All selected orders have been approved successfully.";
                await LogHelper.AddLog(_context, LogHelper.CreateLog("Information", "Selected orders have been successfully approved.(Admin)"));
                return RedirectToAction("ApproveOrder");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while approving orders: {ex.Message}";
                await LogHelper.AddLog(_context, LogHelper.CreateLog("Error", $"Error while approving orders(Admin): {ex.Message}"));
                return RedirectToAction("ApproveOrder");
            }
        }


        private static readonly Mutex OrderApprovalMutex = new Mutex();

        private void ApproveOrderInNewThread(Order order)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                OrderApprovalMutex.WaitOne();

                try
                {
                    var dbOrder = context.Orders.Include(o => o.Customer).FirstOrDefault(o => o.OrderID == order.OrderID);
                    if (dbOrder == null || dbOrder.OrderStatus != "Pending") return;

                    var product = context.Products.FirstOrDefault(p => p.ProductID == dbOrder.ProductID);

                    if (product != null && product.Stock >= dbOrder.Quantity)
                    {
                        product.Stock -= dbOrder.Quantity;
                        dbOrder.OrderStatus = "Approved";

                        var customer = dbOrder.Customer;
                        if (customer != null)
                        {
                            // TotalSpent güncelleniyor
                            customer.TotalSpent = (customer.TotalSpent ?? 0) + dbOrder.TotalPrice;

                            // Eğer toplam harcama 2000 TL'yi geçerse müşteri "Premium" yapılır
                            if (customer.TotalSpent >= 2000 && customer.CustomerType != "Premium")
                            {
                                customer.CustomerType = "Premium";
                            }

                            context.Update(customer);
                        }

                        context.Update(product);
                        context.Update(dbOrder);
                        context.SaveChanges();

                        LogHelper.AddLog(context, LogHelper.CreateLog(
                            "Information",
                            $"(Admin)Order {dbOrder.OrderID} approved successfully. Product: {product.ProductName}, Quantity: {dbOrder.Quantity}",
                            dbOrder.CustomerID,
                            dbOrder.OrderID
                        )).Wait();
                    }
                    else
                    {
                        dbOrder.OrderStatus = "Rejected";
                        context.Update(dbOrder);
                        context.SaveChanges();

                        LogHelper.AddLog(context, LogHelper.CreateLog(
                            "Warning",
                            $"(Admin)Order {dbOrder.OrderID} rejected due to insufficient stock. Customer: {dbOrder.Customer?.CustomerName ?? "Unknown"}, Product: {product?.ProductName ?? "Unknown"}",
                            dbOrder.CustomerID,
                            dbOrder.OrderID
                        )).Wait();
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.AddLog(context, LogHelper.CreateLog(
                        "Error",
                        $"Error while processing order {order.OrderID}: {ex.Message}",
                        order.CustomerID,
                        order.OrderID
                    )).Wait();
                }
                finally
                {
                    OrderApprovalMutex.ReleaseMutex();
                }
            }
        }






        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                var order = _context.Orders.FirstOrDefault(o => o.OrderID == orderId);
                if (order == null)
                {
                    TempData["ErrorMessage"] = "Order not found.";
                    return RedirectToAction("ApproveOrder");
                }

                order.OrderStatus = "Rejected";
                _context.Update(order);

                await _context.SaveChangesAsync();
                await LogHelper.AddLog(_context, LogHelper.CreateLog("Information", $"Order cancelled(Admin): {order.OrderID}.", order.CustomerID, order.OrderID));
            }
            catch (Exception ex)
            {
                await LogHelper.AddLog(_context, LogHelper.CreateLog("Error", $"An error occurred while canceling the order(Admin): {ex.Message}."));
                TempData["ErrorMessage"] = "An error occurred while cancelling the order.";
            }

            return RedirectToAction("ApproveOrder");
        }





        public IActionResult StockDashboard()
        {
           
            var products = _context.Products.ToList();

            
            var productNames = products.Select(p => p.ProductName).ToArray();
            var productStocks = products.Select(p => p.Stock).ToArray();

            const int CriticalStockLevel = 10;

            var criticalStocks = products
                .Where(p => p.Stock <= CriticalStockLevel)
                .Select(p => new { p.ProductName, p.Stock })
                .ToList();

            ViewData["ProductNames"] = productNames;
            ViewData["ProductStocks"] = productStocks;
            ViewData["CriticalStocks"] = criticalStocks;

            return View();
        }

       
        public IActionResult OrderStatusDashboard()
        {
            var orders = _context.Orders
                .Include(o => o.Customer)
                .Select(o => new
                {
                    o.OrderID,
                    o.Customer.CustomerName,
                    o.OrderStatus // "Pending", "Processing", "Completed"
                })
                .ToList();

          
            ViewData["OrderStatuses"] = orders;

            return View();
        }

      
    

    }


}
    

