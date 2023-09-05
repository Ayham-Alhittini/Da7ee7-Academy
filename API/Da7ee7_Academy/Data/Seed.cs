using Da7ee7_Academy.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Da7ee7_Academy.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            /////Seed Roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole{Name = RolesSrc.Admin},
                new IdentityRole{Name = RolesSrc.Moderator},
                new IdentityRole{Name = RolesSrc.Teacher},
                new IdentityRole{Name = RolesSrc.Student}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            /////Seed Admin
            var admin = new AppUser
            {
                Email = "da7ee7.academy.management@gmail.com",
                FirstName = "Admin",
                FullName = "Da7ee7 academy management",
                Role = RolesSrc.Admin,
                EmailConfirmed = true,
            };

            admin.UserName = admin.Email;
            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRoleAsync(admin, RolesSrc.Admin);
        }
    }
}
