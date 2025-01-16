using EventSourcingExample.Application.Abstraction.User;
using EventSourcingExample.Application.Common.Models;
using EventSourcingExample.Application.CQRS.Authentication.DTOs;
using EventSourcingExample.Application.CQRS.Users.Commands.CreateUser;
using EventSourcingExample.Application.CQRS.Users.Commands.DeleteUser;
using EventSourcingExample.Application.CQRS.Users.Commands.ToggleUserState;
using EventSourcingExample.Application.CQRS.Users.Commands.UpdateUser;
using EventSourcingExample.Application.CQRS.Users.Queries.GetAllUsers;
using EventSourcingExample.Application.CQRS.Users.Queries.GetUser;
using EventSourcingExample.Application.CQRS.Users.Queries.GetUserSettings;
using EventSourcingExample.Domain.Enums.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSourcingExample.WebUI.Controllers
{
    [Authorize]
    public class UsersController : ApiControllerBase
    {
        private ICurrentUserService _currentUser { get; }

        public UsersController(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

        [HttpGet("list")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<PaginatedList<UserListItemModel>>> GetList(
            [FromQuery] Pager pager,
            [FromQuery] GetPaginatedUsersFilterModel filter)
        {
            var query = new GetPaginatedUsersQuery(pager, filter);
            return await Mediator.Send(query);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Get([FromRoute] Guid id)
        {
            if (!_currentUser.IsAdmin && id != _currentUser.UserGuid())
                return Forbid();

            var query = new GetUserDetailQuery { PublicId = id };
            var result = await Mediator.Send(query);

            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("settings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SettingsVM>> GetSettings()
        {
            var query = new GetUserSettingsQuery { PublicId = _currentUser.UserGuid() };
            var result = await Mediator.Send(query);

            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        [AllowAnonymous]
        public async Task<ActionResult> Create([FromBody] CreateUserCommand command)
        {
            if (_currentUser.UserId == null) //registration process!
                command.Roles = new List<string>() { nameof(UserRoles.Seller) };

            await Mediator.Send(command);
            return Ok();
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] UpdateUserCommand command)
        {
            if (!_currentUser.IsAdmin && command.UserId != _currentUser.UserGuid())
                return Forbid();

            await Mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{publicId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid publicId)
        {
            if (!_currentUser.IsAdmin && publicId != _currentUser.UserGuid())
                return Forbid();

            await Mediator.Send(new DeleteUserCommand(publicId));
            return Ok();
        }

        //[HttpPut("ChangeUserPassword")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesDefaultResponseType]
        //public async Task<ActionResult> ChangeUserPassword([FromBody] ChangeUserPasswordCommand command)
        //{
        //    if (!_currentUser.IsAdmin && command.PublicId != _currentUser.UserGuid())
        //        return Forbid();

        //    await Mediator.Send(command);
        //    return Ok();
        //}

        [Authorize(Roles = "Administrator")]
        [HttpPost("toggle-active-state/{publicId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ToggleActiveState(Guid publicId)
        {
            await Mediator.Send(new ToggleUserStateCommand(publicId));
            return Ok();
        }
    }
}
