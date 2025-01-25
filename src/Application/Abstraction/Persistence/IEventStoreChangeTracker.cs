using EventSourcingExample.Domain.Common;
using System.Collections.Generic;

namespace EventSourcingExample.Infrastructure.Persistence
{
    public interface IEventStoreChangeTracker<T> where T : IEventSourceEntity
    {
        public void AddAggregateToSave(T eventSourceEntity);
        public List<T> GetAggregatesToSave();
    }
}