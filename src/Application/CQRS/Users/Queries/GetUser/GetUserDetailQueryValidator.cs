using FluentValidation;

namespace EventSourcingExample.Application.CQRS.Users.Queries.GetUser
{
    public class GetUserDetailQueryValidator : AbstractValidator<GetUserDetailQuery>
    {
        public GetUserDetailQueryValidator()
        {
            RuleFor(v => v.PublicId).NotEmpty();
        }
    }
}