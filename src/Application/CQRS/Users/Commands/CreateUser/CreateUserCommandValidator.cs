using FluentValidation;
using EventSourcingExample.Application.Common.Extensions.Validations;

namespace EventSourcingExample.Application.CQRS.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.FirstName).MaximumLength(255).NotEmpty().MustNotFakeAdmin();
            RuleFor(x => x.LastName).MaximumLength(255).NotEmpty().MustNotFakeAdmin();
            RuleFor(x => x.UserName).Username().NotEmpty().MustNotFakeAdmin();
            RuleFor(x => x.Email).MaximumLength(255).EmailAddress().NotEmpty().MustNotFakeAdmin();
            RuleFor(x => x.Password).MaximumLength(255).NotEmpty().MustBeCorrectPasswordFormat();
        }
    }
}