using System;
using System.Threading.Tasks;
using EventSourcingExample.Application.CQRS.Authentication;
using Microsoft.AspNetCore.Identity;

namespace EventSourcingExample.Infrastructure.Identity.Users.Services
{
    public class RoleManagementService : IRoleManagementService
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public RoleManagementService(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task AddNewRole(string roleName)
        {
            if (await roleManager.RoleExistsAsync(roleName))
                throw new InvalidOperationException($"Role [{roleName}] already exists.");

            var result = await roleManager.CreateAsync(new IdentityRole(roleName));

            if (!result.Succeeded)
                throw new NotImplementedException(); // todo
        }

        public async Task<bool> RoleExists(string roleName) 
            => await roleManager.RoleExistsAsync(roleName);
    }
}