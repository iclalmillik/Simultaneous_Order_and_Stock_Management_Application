using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebStokYApp.Models;
using WebStokYApp.Models.Entities;

namespace WebStokYApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

      
        [AllowAnonymous]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> LoginCustomer()
        {
            
            var products = await _context.Products.ToListAsync();
            ViewData["Products"] = products;


            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                // Giriş yapan kullanıcının adı
                var customerName = User.Identity.Name;
                ViewData["CustomerName"] = customerName;
            }
            else
            {
                ViewData["CustomerName"] = "Guest";
            }


            return View();
        }
		

      
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Customer customer)
        {
            
            var existingCustomer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerName == customer.CustomerName && c.Password == customer.Password);

            if (existingCustomer != null)
            {
             
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, existingCustomer.CustomerName),
                    new Claim(ClaimTypes.Role, "Customer") 
                };

                var claimsIdentity = new ClaimsIdentity(claims, "CustomerCookie");
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true, 
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) 
                };

                await HttpContext.SignInAsync("CustomerCookie", new ClaimsPrincipal(claimsIdentity), authProperties);

               
                return RedirectToAction(nameof(LoginCustomer)); 
            }

           
            TempData["ErrorMessage"] = "Hatalı kullanıcı adı veya şifre.";
            return RedirectToAction("CustomerLogin", "Home");
        }

     


        public IActionResult Index()
        {
			
			return View();
		
        }
    }
}
