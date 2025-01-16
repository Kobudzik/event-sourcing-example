using EventSourcingExample.Application.Common.Extensions.Validations;
using FluentValidation;

namespace EventSourcingExample.Application.CQRS.Authentication.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(p => p.NewPassword)
                .NotEmpty()
                .MustBeCorrectPasswordFormat();

            RuleFor(p => p.ChangePasswordToken).NotEmpty();
            RuleFor(p => p.UserId).NotEmpty();
        }
    }
}