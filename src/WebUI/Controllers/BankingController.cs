using EventSourcingExample.Application.CQRS.Authentication.DTOs;
using EventSourcingExample.Application.CQRS.Banking.Commands.Deposit;
using EventSourcingExample.Application.CQRS.Banking.Commands.Withdraw;
using EventSourcingExample.Application.CQRS.Users.Commands.ChangeUserPassword;
using EventSourcingExample.Application.CQRS.Users.Queries.GetUserSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace EventSourcingExample.WebUI.Controllers
{
    [AllowAnonymous]
    public class BankingController : ApiControllerBase
    {
		[HttpPost("open")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<SettingsVM>> Open()
		{
			var command = new OpenAccountCommand();
			var result = await Mediator.Send(command);
            return Ok(result);
		}

		[HttpGet("balance/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetBalance([FromRoute] Guid id)
        {
            var query = new GetBalanceQuery(id);
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPatch("close/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SettingsVM>> Close([FromRoute] Guid id)
        {
            var command = new CloseAccountCommand(id);
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("deposit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Deposit([FromBody] DepositCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }

		[HttpPost("withdraw")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult> Withdraw([FromBody] WithdrawCommand command)
		{
			await Mediator.Send(command);
			return Ok();
		}
    }
}
