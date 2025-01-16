using System.Threading;
using System.Threading.Tasks;
using EventSourcingExample.Application.Abstraction.Configurations;
using EventSourcingExample.Application.Abstraction.Email.Authentication.ResetPassword;
using EventSourcingExample.Application.Abstraction.User;
using EventSourcingExample.Application.Common.Exceptions;
using MediatR;

namespace EventSourcingExample.Application.CQRS.Authentication.Commands.ForgotPassword
{
    public sealed class ForgotPasswordCommand : IRequest
    {
        public string Username { get; set; }

        internal sealed class Handler : IRequestHandler<ForgotPasswordCommand>
        {
            private readonly IPasswordsManagementService _passwordsManagementService;
            private readonly IUserManagementService _userManagementService;
            private readonly IResetPasswordEmailHandler _resetPasswordEmailHandler;
            private readonly IApplicationConfiguration _applicationConfiguration;

            public Handler(IPasswordsManagementService passwordsManagementService,
                IUserManagementService userManagementService,
                IResetPasswordEmailHandler resetPasswordEmailHandler,
                IApplicationConfiguration applicationConfiguration)
            {
                _passwordsManagementService = passwordsManagementService;
                _userManagementService = userManagementService;
                _resetPasswordEmailHandler = resetPasswordEmailHandler;
                _applicationConfiguration = applicationConfiguration;
            }

            public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            {
                var userExists = await _userManagementService.UserExists(request.Username);
                if (!userExists)
                    throw new NotFoundException("User", request.Username);

                var userDetails = await _userManagementService.GetUserDetailsAsync(request.Username);
                var resetToken = await _passwordsManagementService.GetResetPasswordTokenAsync(userDetails.Id);

                _resetPasswordEmailHandler.SetTemplateData(
                    resetToken,
                    userDetails.UserName,
                    userDetails.Id.ToString(),
                    "dummyFrontendUrl"
                );

                await _resetPasswordEmailHandler.SendEmail(userDetails.Email);
                return Unit.Value;
            }
        }
    }
}