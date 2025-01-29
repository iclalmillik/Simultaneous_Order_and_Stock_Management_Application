using WebStokYApp.Models.Entities;
using WebStokYApp.Models;

namespace WebStokYApp.Helpers
{
    public class LogHelper
    {
        public static async Task AddLog(AppDbContext context, Log log)
        {
            context.Logs.Add(log);
            await context.SaveChangesAsync();
        }

        public static Log CreateLog(int? customerId, string logType, string logDetails, int? orderId = null)
        {
            return new Log
            {
                CustomerID = customerId,
                LogType = logType,
                LogDetails = logDetails,
                OrderID = orderId,
                LogDate = DateTime.Now
            };
        }
    }
}
