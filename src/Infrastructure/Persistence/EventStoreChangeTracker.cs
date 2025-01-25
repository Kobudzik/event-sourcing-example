using EventSourcingExample.Domain.Common;
using System.Collections.Generic;

namespace EventSourcingExample.Infrastructure.Persistence
{
    public class EventStoreChangeTracker<T>() : IEventStoreChangeTracker<T> where T : IEventSourceEntity
    {
		private readonly List<T> _aggregatesToSave = [];

		public void AddAggregateToSave(T eventSourceEntity)
		{
			_aggregatesToSave.Add(eventSourceEntity);
		}

		public List<T> GetAggregatesToSave()
		{
			return _aggregatesToSave;
		}
	}
}