using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourcingExample.Application.Abstraction.User;

namespace EventSourcingExample.Application.CQRS.Users.Commands.ToggleUserState
{
    public class ToggleUserStateCommand : IRequest
    {
        public ToggleUserStateCommand(Guid UserId)
        {
            this.UserId = UserId;
        }

        public Guid UserId { get; }

        internal sealed class Handler : IRequestHandler<ToggleUserStateCommand>
        {
            private readonly IUserManagementService userManagementService;

            public Handler(IUserManagementService userManagementService)
            {
                this.userManagementService = userManagementService;
            }

            public async Task<Unit> Handle(ToggleUserStateCommand request, CancellationToken cancellationToken)
            {
                await userManagementService.ToggleUserState(request.UserId.ToString());
                return Unit.Value;
            }
        }
    }
}