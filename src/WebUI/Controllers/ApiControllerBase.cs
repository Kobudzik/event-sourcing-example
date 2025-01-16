using Microsoft.AspNetCore.Mvc;

namespace EventSourcingExample.WebUI.Controllers
{
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ApiControllerBaseUnrouted
    {
    }
}
