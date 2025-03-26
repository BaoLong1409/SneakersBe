using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Sneakers.DataSeeder
{
    public class DataSeeder
    {
        public static async Task SeedRoleAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            string[] roleNames = { "Admin" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var role = new Role { Name = roleName };
                    await roleManager.CreateAsync(role);
                }
            }
        }

        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            string adminEmail = "anhruoia1a1@gmail.com";
            string adminPass = "longlatoi";
            string adminRole = "Admin";

            var roleExists = await roleManager.RoleExistsAsync(adminRole);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new Role { Name = adminRole });
            }

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new User { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true, FirstName = "Admin", LastName = "" };
                var createAdmin = await userManager.CreateAsync(newAdmin, adminPass);
                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }


        }
    }
}
