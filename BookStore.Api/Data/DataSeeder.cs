using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BookStore.Api.Data
{
    public static class DataSeeder
    {
        const string administratorRoleName = "Administrator";
        const string customerRoleName = "Customer";

        public async static Task Seed(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            await SeedRoles(roleManager);
            await SeedUsers(userManager);
        }

        private async static Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync(administratorRoleName))
            {
                IdentityRole role = new IdentityRole(administratorRoleName);
                await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync(customerRoleName))
            {
                IdentityRole role = new IdentityRole(customerRoleName);
                await roleManager.CreateAsync(role);
            }
        }

        private async static Task SeedUsers(UserManager<IdentityUser> userManager)
        {
            string adminEmail = "admin@bookstore.com";
            bool adminExists = await userManager.FindByEmailAsync(adminEmail) != null;
            if (!adminExists)
            {
                var user = new IdentityUser
                {
                    UserName = "admin",
                    Email = adminEmail
                };
                var result = await userManager.CreateAsync(user, "Ses@mo123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, administratorRoleName);
                }
            }
            for (int counter = 1; counter <= 2; counter++)
            {
                string userName = $"customer0{counter}";
                string customerEmail = $"{userName}@mail.com";
                bool customerExists = await userManager.FindByEmailAsync(customerEmail) != null;
                if (!customerExists)
                {
                    var user = new IdentityUser
                    {
                        UserName = userName,
                        Email = customerEmail
                    };
                    var result = await userManager.CreateAsync(user, "Ses@mo123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, customerRoleName);
                    }
                }
            }
        }
    }
}
