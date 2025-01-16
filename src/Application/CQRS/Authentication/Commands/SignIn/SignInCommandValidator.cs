using FluentValidation;

namespace EventSourcingExample.Application.CQRS.Authentication.Commands.SignIn
{
    public sealed class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator()
        {
            RuleFor(signInCommand => signInCommand.Password).NotEmpty();
            RuleFor(signInCommand => signInCommand.Username).NotEmpty();
        }
    }
}