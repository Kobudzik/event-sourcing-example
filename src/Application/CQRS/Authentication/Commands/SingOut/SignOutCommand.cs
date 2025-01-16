using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace EventSourcingExample.Application.CQRS.Authentication.Commands.SingOut
{
    public class SignOutCommand : IRequest
    {
        public class Handler : IRequestHandler<SignOutCommand>
        {
            private readonly ISignInManagementService signInManagementService;

            public Handler(ISignInManagementService signInManagementService)
            {
                this.signInManagementService = signInManagementService;
            }

            public async Task<Unit> Handle(SignOutCommand request, CancellationToken cancellationToken)
            {
                await signInManagementService.SignOutAsync();

                return Unit.Value;
            }
        }
    }
}