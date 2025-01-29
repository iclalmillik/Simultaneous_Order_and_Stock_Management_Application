using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStokYApp.Models;
using WebStokYApp.Models.Entities;

namespace WebStokYApp.Controllers
{
   
    public class LogController : Controller
    {
        private readonly AppDbContext _context;

        public LogController(AppDbContext context)
        {
            _context = context;
        }

     
        public async Task<IActionResult> LogPanel(string logType, string customerType, string search, DateTime? startDate, DateTime? endDate)
        {
            var logs = _context.Logs.Include(l => l.Customer).AsQueryable();

      
            if (!string.IsNullOrEmpty(logType))
                logs = logs.Where(l => l.LogType == logType);

            if (!string.IsNullOrEmpty(customerType))
                logs = logs.Where(l => l.Customer.CustomerType == customerType);

            if (!string.IsNullOrEmpty(search))
                logs = logs.Where(l => EF.Functions.Like(l.LogDetails, $"%{search}%"));

            if (startDate.HasValue)
                logs = logs.Where(l => l.LogDate >= startDate.Value);

            if (endDate.HasValue)
                logs = logs.Where(l => l.LogDate <= endDate.Value);

            var logList = await logs
                .OrderByDescending(l => l.LogDate)
                .ToListAsync();

            return View(logList);
        }
    }
}
