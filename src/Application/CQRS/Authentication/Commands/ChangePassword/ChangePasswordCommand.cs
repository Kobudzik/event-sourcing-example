using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourcingExample.Application.Abstraction.User;
using MediatR;

namespace EventSourcingExample.Application.CQRS.Authentication.Commands.ChangePassword
{
    public sealed class ChangePasswordCommand : IRequest
    {
        public Guid UserId { get; set; }
        public string NewPassword { get; set; }
        public string ChangePasswordToken { get; set; }

        public class Handler : IRequestHandler<ChangePasswordCommand>
        {
            private readonly IPasswordsManagementService _passwordsManagementService;

            public Handler(IPasswordsManagementService passwordsManagementService)
            {
                _passwordsManagementService = passwordsManagementService;
            }

            public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
            {
                await _passwordsManagementService.ChangePasswordAsync(
                    request.UserId,
                    request.NewPassword,
                    request.ChangePasswordToken,
                    cancellationToken);

                return Unit.Value;
            }
        }
    }
}