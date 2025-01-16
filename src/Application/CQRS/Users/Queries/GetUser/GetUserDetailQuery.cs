using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourcingExample.Application.Abstraction;
using EventSourcingExample.Application.Abstraction.User;
using EventSourcingExample.Application.CQRS.Authentication.DTOs;

namespace EventSourcingExample.Application.CQRS.Users.Queries.GetUser
{
    public class GetUserDetailQuery : IRequest<UserDto>
    {
        public Guid PublicId { get; set; }

        internal sealed class GetUserDetailQueryHandler : IRequestHandler<GetUserDetailQuery, UserDto>
        {
            private readonly IUserManagementService _userManagementService;
            private readonly IApplicationDbContext _context;

            public GetUserDetailQueryHandler(IUserManagementService userManagementService, IApplicationDbContext context)
            {
                _userManagementService = userManagementService;
                _context = context;
            }

            public async Task<UserDto> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
            {
                return await _userManagementService.GetUserDetailsAsync(request.PublicId);
            }
        }
    }
}