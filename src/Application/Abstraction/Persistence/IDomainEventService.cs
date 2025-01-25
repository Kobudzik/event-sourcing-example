using System.Threading.Tasks;
using EventSourcingExample.Domain.Common;

namespace EventSourcingExample.Application.Abstraction.Persistence
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
