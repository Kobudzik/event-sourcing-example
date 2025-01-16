using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace EventSourcingExample.WebUI.Extensions
{
    public static class AbstractValidatorExtensions
    {
        public static async Task ValidateRequest<V, R>(this IRequest<R> command, AbstractValidator<V> validator)
        {
            var validationResult = await validator.ValidateAsync((V)command);
            if (validationResult.Errors.Count > 0)
                throw new ValidationException(validationResult.Errors);
        }
    }
}
