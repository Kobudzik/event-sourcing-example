using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourcingExample.Application.Abstraction.User;
using MediatR;

namespace EventSourcingExample.Application.CQRS.Users.Commands.ChangeUserPassword
{
    public sealed class ChangeUserPasswordCommand : IRequest
    {
        public ChangeUserPasswordCommand(Guid publicId, string newPassword)
        {
            PublicId = publicId;
            NewPassword = newPassword;
        }

        public Guid PublicId { get; }
        public string NewPassword { get; }

        internal sealed class Handler : IRequestHandler<ChangeUserPasswordCommand>
        {
            private readonly IPasswordsManagementService _passwordsManagementService;

            public Handler(IPasswordsManagementService passwordsManagementService)
            {
                _passwordsManagementService = passwordsManagementService;
            }

            public async Task<Unit> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
            {
                await _passwordsManagementService.ChangeUserPasswordAsync(request.PublicId, request.NewPassword, cancellationToken);
                return Unit.Value;
            }
        }
    }
}