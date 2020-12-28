using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class MagShopContextSeed
    {
        public static async Task SeedAsync(MagShopContext catalogContext,
            ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                // TODO: Only run this if using a real database
                // context.Database.Migrate();
                if (!await catalogContext.Users.AnyAsync())
                {
                    await catalogContext.Users.AddRangeAsync(
                        GetPreconfiguredUsers());

                    await catalogContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<MagShopContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(catalogContext, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }

        private static IEnumerable<User> GetPreconfiguredUsers()
        {
            return new List<User>()
            {
                new User("1234","Mikhail","Filippov", "filmih24@mail.ru", "+79954116858", DateTimeOffset.Parse("24.09.1998"),
                new Basket(){ BasketId="12345", UserId="1234" }, new UserAuthAccess("admin","password"),
                new List<CreditCard>(){
                    new CreditCard(){ CreditCardId="123", CardNumber="12345678"}
                },
                new List<Address>()
                {
                    new Address("Pkorova", "23")
                })
            };
        }
    }
}
