using FluentValidation;

namespace EventSourcingExample.Application.CQRS.Authentication.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(p => p.Token).NotEmpty();
            RuleFor(p => p.UserId).NotEmpty();
        }
    }
}