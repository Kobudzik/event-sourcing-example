using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourcingExample.Application.Abstraction.User;

namespace EventSourcingExample.Application.CQRS.Users.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest
    {
        public DeleteUserCommand(Guid publicId)
        {
            PublicId = publicId;
        }

        public Guid PublicId { get; }

        internal sealed class Handler : IRequestHandler<DeleteUserCommand>
        {
            private readonly IUserManagementService userManagementService;

            public Handler(IUserManagementService userManagementService)
            {
                this.userManagementService = userManagementService;
            }

            public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                await userManagementService.RemoveUserAsync(request.PublicId.ToString());
                return Unit.Value;
            }
        }
    }
}