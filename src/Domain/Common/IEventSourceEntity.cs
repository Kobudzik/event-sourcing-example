using System;
using System.Collections.Generic;

namespace EventSourcingExample.Domain.Common
{
    public interface IEventSourceEntity
    {
        public Guid Id { get; }
        public List<object> GetUncommittedChanges();
        public void ApplyEvent(object eventItem);
        public object DeserializeEvent(string eventJson, string eventType);
    }
}
