using FluentValidation;

namespace EventSourcingExample.Application.CQRS.Authentication.Commands.ForgotPassword
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(f => f.Username)
                .NotEmpty();
        }
    }
}