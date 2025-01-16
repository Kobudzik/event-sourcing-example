using EventSourcingExample.Domain.Common;
using MediatR;

namespace EventSourcingExample.Application.Common.Models
{
    public class DomainEventNotification<TDomainEvent> : INotification
        where TDomainEvent : DomainEvent
    {
        public DomainEventNotification(TDomainEvent domainEvent)
        {
            DomainEvent = domainEvent;
        }

        public TDomainEvent DomainEvent { get; }
    }
}
