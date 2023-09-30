using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentitySeedingData
    {
        public static async Task SeedUserAsync( UserManager<AppUser> userManager) {
            if (!userManager.Users.Any()) {
                var user = new AppUser {
                    DisplayName = "Abdullah Imam Dawoud",
                    Email = "aemam816@gmail.com",
                    UserName = "aemam816@gmail.com",
                    Address = new Address {
                        Street = "karrar",
                        State = "Egypt",
                        City = "Giza",
                        ZibCode = "12919"
                    }
                };

                await userManager.CreateAsync(user, "@Aa20130156");
            }
        }
    }
}