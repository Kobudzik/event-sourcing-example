using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourcingExample.Application.Abstraction.User;
using EventSourcingExample.Application.CQRS.Users.Commands.UpdateUser.Request;
using MediatR;

namespace EventSourcingExample.Application.CQRS.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SubiektUserName { get; set; }

        internal sealed class Handler : IRequestHandler<UpdateUserCommand>
        {
            private readonly IUserManagementService _userManagementService;

            public Handler(IUserManagementService userManagementService)
            {
                _userManagementService = userManagementService;
            }

            public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManagementService.GetUserDetailsAsync(request.UserId);

                var updateModel = new UpdateUserModel(
                    request.UserId,
                    request.FirstName,
                    request.LastName,
                    request.SubiektUserName
                );

                await _userManagementService.UpdateAsync(updateModel);
                return Unit.Value;
            }
        }
    }
}
