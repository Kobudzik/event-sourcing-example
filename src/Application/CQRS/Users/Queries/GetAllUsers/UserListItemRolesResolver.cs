using System.Linq;
using AutoMapper;
using EventSourcingExample.Application.Abstraction.User;
using EventSourcingExample.Domain.Entities.Core;

namespace EventSourcingExample.Application.CQRS.Users.Queries.GetAllUsers
{
    public class UserListItemRolesResolver : IValueResolver<UserAccount, UserListItemModel, string[]>
    {
        private readonly IUserManagementService _userManagementService;

        public UserListItemRolesResolver(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        public string[] Resolve(UserAccount source, UserListItemModel destination, string[] destMember, ResolutionContext context)
        {
            var roles = _userManagementService.GetUserRolesAsync(source.UserName)
                .GetAwaiter()
                .GetResult()
                .ToArray();

            return roles;
        }
    }
}
