using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcingExample.WebUI.Controllers
{
    [ApiController]
    public abstract class ApiControllerBaseUnrouted : ControllerBase
    {
        private ISender _mediator;

        protected ISender Mediator
            => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
    }
}
