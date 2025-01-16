using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using EventSourcingExample.Domain.Entities.Core;

namespace EventSourcingExample.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAndRolesAsync(
            UserManager<UserAccount> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            var administratorRole = new IdentityRole("Administrator");

            var roles = roleManager.Roles.Select(s => s.Name).ToList();

            if (roles.All(r => r != administratorRole.Name))
                await roleManager.CreateAsync(administratorRole);

            var administrator = UserAccount.Create("Admin", "Admin", "administrator@localhost", "Admin", true);
            //var kobudzik = UserAccount.Create("Konrad", "Budzik", "konrad@localhost", "kobudzik", true);
            //var kabdzik = UserAccount.Create("Kamil", "Budzik", "kamil@localhost", "kabdzik", true);

            if (userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                administrator.EmailConfirmed = true;
                administrator.IsActive = true;
                await userManager.CreateAsync(administrator, "admin123");
                await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }
    }
}
