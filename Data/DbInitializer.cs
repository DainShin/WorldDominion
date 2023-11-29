using Microsoft.AspNetCore.Identity;

namespace WorldDominion.Models
{
    public class DbInitializer
    {
        public static async Task Initiallize(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        
        {
            string[] roleNames = {"Admin", "Customer"};
            foreach (var roleName in roleNames)
            {
                var roleExits = await roleManager.RoleExistsAsync(roleName);

                if(!roleExits)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var user = new IdentityUser
            {
                UserName = "admin@admin.com",
                Email = "admin@admin.com",
            };

            string userPWD = "Password@123";

            var createUser = await userManager.CreateAsync(user, userPWD);

            if(createUser.Succeeded)
            {
                await userManager.AddToRoleAsync(user,"Admin");
            }
        }
    }
}