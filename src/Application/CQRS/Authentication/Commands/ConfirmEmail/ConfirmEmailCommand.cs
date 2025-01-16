using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourcingExample.Application.Abstraction.User;
using MediatR;

namespace EventSourcingExample.Application.CQRS.Authentication.Commands.ConfirmEmail
{
    public sealed class ConfirmEmailCommand : IRequest
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }

        internal class Handler : IRequestHandler<ConfirmEmailCommand>
        {
            private readonly IUserManagementService _userManagementService;

            public Handler(IUserManagementService userManagementService)
            {
                _userManagementService = userManagementService;
            }

            public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
            {
                await _userManagementService.ConfirmEmailAsync(
                    request.UserId.ToString(),
                    request.Token);

                return Unit.Value;
            }
        }
    }
}