using FluentValidation;

namespace EventSourcingExample.Application.CQRS.Users.Commands.DeleteUser
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(v => v.PublicId).NotEmpty();
        }
    }
}