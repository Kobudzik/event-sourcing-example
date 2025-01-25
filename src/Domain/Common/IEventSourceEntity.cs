using EventSourcingExample.Domain.Events;
using System;
using System.Collections.Generic;

namespace EventSourcingExample.Domain.Common
{
    public interface IEventSourceEntity
    {
        public Guid Id { get; }
        public List<IDomainEvent> GetUncommittedChanges();
        public void ApplyEvent(IDomainEvent eventItem);
        public object DeserializeEvent(string eventJson, string eventType);
    }
}
