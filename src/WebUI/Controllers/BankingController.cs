using Carter;
using EventSourcingExample.Application.CQRS.Authentication.DTOs;
using EventSourcingExample.Application.CQRS.Banking.Commands.CloseAccount;
using EventSourcingExample.Application.CQRS.Banking.Commands.Deposit;
using EventSourcingExample.Application.CQRS.Banking.Commands.OpenAccount;
using EventSourcingExample.Application.CQRS.Banking.Commands.Withdraw;
using EventSourcingExample.Application.CQRS.Banking.Queries.GetBalance;
using EventSourcingExample.Application.CQRS.Users.Queries.GetUserSettings;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace EventSourcingExample.WebUI.Controllers;

public class BankingModule : CarterModule
{
    public BankingModule() : base("/banking")
    {
    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/open", async (IMediator mediator) =>
        {
            var command = new OpenAccountCommand();
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("Open an account")
        .AllowAnonymous()
        .Produces<SettingsVM>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        app.MapGet("/balance/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var query = new GetBalanceQuery(id);
            var result = await mediator.Send(query);
            return Results.Ok(result);
        })
        .WithName("Check account balance")
        .AllowAnonymous()
        .Produces<UserDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        app.MapPatch("/close/{id:guid}", async (Guid id, IMediator mediator) =>
        {
            var command = new CloseAccountCommand(id);
            var result = await mediator.Send(command);
            return Results.Ok(result);
        })
        .WithName("Close an account")
        .AllowAnonymous()
        .Produces<SettingsVM>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        app.MapPost("/deposit", async (DepositCommand command, IMediator mediator) =>
        {
            await mediator.Send(command);
            return Results.Ok();
        })
        .WithName("Deposit money")
        .AllowAnonymous()
        .Produces(StatusCodes.Status200OK);

        app.MapPost("/withdraw", async (WithdrawCommand command, IMediator mediator) =>
        {
            await mediator.Send(command);
            return Results.Ok();
        })
        .WithName("Withdraw money")
        .AllowAnonymous()
        .Produces(StatusCodes.Status200OK);
    }
}