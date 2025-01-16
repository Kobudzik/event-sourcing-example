using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using EventSourcingExample.WebUI.DTO.Authentication;
using EventSourcingExample.WebUI.DTO.Errors;
using EventSourcingExample.Application.CQRS.Authentication.Commands.SignIn;
using EventSourcingExample.Application.CQRS.Authentication.Commands.ConfirmEmail;
using EventSourcingExample.Application.CQRS.Authentication.Commands.ForgotPassword;
using EventSourcingExample.Application.Abstraction.Configurations;
using EventSourcingExample.Application.CQRS.Authentication.Commands.ChangePassword;

namespace EventSourcingExample.WebUI.Controllers
{
    public class AuthenticationController(IApplicationConfiguration applicationConfiguration) : ApiControllerBase
    {
        private readonly IApplicationConfiguration _applicationConfiguration = applicationConfiguration;

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<SignInResponse>> SignIn([FromBody] SignInRequest request)
        {
            var signInCommandResponse = await Mediator.Send(new SignInCommand(request.Username, request.Password));

            var response = new SignInResponse
            {
                Token = signInCommandResponse.AccessToken,
                RefreshToken = signInCommandResponse.RefreshToken,
            };

            if (string.IsNullOrEmpty(signInCommandResponse.Error))
                return Ok(response);

            return new BadRequestObjectResult(new ExceptionDto(signInCommandResponse.Error));
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConfirmAccountResponse>> ConfirmEmail([FromQuery] ConfirmAccountRequest request)
        {
            await Mediator.Send(new ConfirmEmailCommand()
            {
                Token = request.Token,
                UserId = request.UserId
            });

            return RedirectPermanent("_applicationConfiguration.FrontendUrl" + "/login?justCreated=true");
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            await Mediator.Send(new ForgotPasswordCommand
            {
                Username = request.Username
            });

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("change-password")]
        [ProducesResponseType(typeof(ExceptionDto), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResetPasswordResponse>> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        // [HttpPost]
        // [Authorize(Policy = PolicyNameKeys.TokenValid)]
        // public async Task<ActionResult<SignOutResponse>> SignOut([FromBody] SignOutRequest request)
        // {
        //     await _userModule.ExecuteCommandAsync(new SignOutCommand());
        //
        //     return Ok(new SignOutResponse());
        // }
        //
        // [AllowAnonymous]
        // public async Task<ActionResult<ExchangeRefreshTokenResponse>> ExchangeRefreshToken(
        //     [FromBody] ExchangeRefreshTokenCommand command)
        // {
        //     return await _userModule.ExecuteCommandAsync(command);
        // }
    }
}