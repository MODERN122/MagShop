using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class IdentityContextSeed
    {
        public static async Task SeedAsync(UserManager<UserAuthAccess> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any() && !userManager.Users.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Constants.ConstantsAPI.ADMINISTRATORS));

                var defaultUser = new UserAuthAccess("demouser@microsoft.com", "demouser@microsoft.com");
                await userManager.CreateAsync(defaultUser, Constants.ConstantsAPI.DEFAULT_PASSWORD);

                string adminUserName = "admin@microsoft.com";
                var adminUser = new UserAuthAccess(adminUserName, adminUserName);
                await userManager.CreateAsync(adminUser, Constants.ConstantsAPI.DEFAULT_PASSWORD);
                adminUser = await userManager.FindByNameAsync(adminUserName);
                await userManager.AddToRoleAsync(adminUser, Constants.ConstantsAPI.ADMINISTRATORS);
            }
        }
    }
}
