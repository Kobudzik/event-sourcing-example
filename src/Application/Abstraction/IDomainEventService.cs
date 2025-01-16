using System.Threading.Tasks;
using EventSourcingExample.Domain.Common;

namespace EventSourcingExample.Application.Abstraction
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
