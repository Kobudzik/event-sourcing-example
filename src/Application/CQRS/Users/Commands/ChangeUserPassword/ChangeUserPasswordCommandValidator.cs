using FluentValidation;
using EventSourcingExample.Application.Common.Extensions.Validations;

namespace EventSourcingExample.Application.CQRS.Users.Commands.ChangeUserPassword
{
    public class ChangeUserPasswordCommandValidator : AbstractValidator<ChangeUserPasswordCommand>
    {
        public ChangeUserPasswordCommandValidator()
        {
            RuleFor(command => command.PublicId).NotEmpty();
            RuleFor(command => command.NewPassword).MaximumLength(255).NotEmpty().MustBeCorrectPasswordFormat();
        }
    }
}