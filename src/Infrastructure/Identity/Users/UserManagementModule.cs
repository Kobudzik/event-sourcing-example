using EventSourcingExample.Application.Abstraction.User;
using EventSourcingExample.Application.CQRS.Authentication;
using EventSourcingExample.Infrastructure.Identity.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcingExample.Infrastructure.Identity.Users
{
    internal static class UserManagementModule
    {
        internal static void AddUserManagementModule(this IServiceCollection services)
        {
            //services.AddSingleton<IUsersConfiguration, UsersConfiguration>();
            //services.AddSingleton<IAdminAccountConfiguration, AdminAccountConfiguration>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<ISignInManagementService, SignInManagementService>();
            services.AddScoped<IRoleManagementService, RoleManagementService>();
            //services.AddScoped<IPasswordComparer, PasswordComparer>();
            services.AddScoped<IPasswordsManagementService, PasswordsManagementService>();
            //services.AddScoped<IUsersSeeder, UsersSeeder>();
        }
    }
}