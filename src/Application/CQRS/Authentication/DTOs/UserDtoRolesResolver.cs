using System.Linq;
using AutoMapper;
using EventSourcingExample.Application.Abstraction.User;
using EventSourcingExample.Domain.Entities.Core;

namespace EventSourcingExample.Application.CQRS.Authentication.DTOs
{
    public class UserDtoRolesResolver : IValueResolver<UserAccount, UserDto, string[]>
    {
        private readonly IUserManagementService _userManagementService;

        public UserDtoRolesResolver(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        public string[] Resolve(UserAccount source, UserDto destination, string[] destMember, ResolutionContext context)
        {
            var roles = _userManagementService.GetUserRolesAsync(source.UserName)
                .GetAwaiter()
                .GetResult()
                .ToArray();

            return roles;
        }
    }
}
