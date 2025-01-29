using WebStokYApp.Models.Entities;
using WebStokYApp.Models;

namespace WebStokYApp.Helpers2
{
    public class LogHelper
    {
        public static async Task AddLog(AppDbContext context, Log log)
        {
            context.Logs.Add(log);
            await context.SaveChangesAsync();
        }

        public static Log CreateLog(string logType, string logDetails, int? customerId = null, int? orderId = null)
        {
            return new Log
            {
                CustomerID = customerId,
                OrderID = orderId,
                LogType = logType,
                LogDetails = logDetails,
                LogDate = DateTime.Now
            };
        }
    }
}
