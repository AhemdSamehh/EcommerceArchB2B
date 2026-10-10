using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(
    OrderContext context,
    ILogger<OrderContextSeed> logger)
        {
            try
            {
                if (!context.Orders.Any())
                {
                    context.Orders.AddRange(GetOrders());
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding OrderDb");
                throw;
            }
        }


        private static IEnumerable<Order> GetOrders()
        {
            return new List<Order>
            {
                new()
                {
                    UserName = "ahmed",
                    FirstName = "ahmed",
                    LastName = "sameh",
                    EmailAddress = "ahmed@gmail.net",
                    AddressLine = "Ranchi",
                    State = "JH",
                    Country = "India",
                    ZipCode = "834009",
                    CardName = "Visa",
                    CardNumber = "4111111111111111",
                    CreatedBy = "ahmed",
                    Expiration = "12/25",
                    Cvv = "123",
                    PaymentMethod = 1,
                    LastModifiedBy = "ahmed",
                    LastModifiedDate = DateTime.UtcNow
                }
            };
        }
    }
}
