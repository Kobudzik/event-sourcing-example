using EventSourcingExample.Application.Abstraction.Persistence;
using EventSourcingExample.Domain.Common;
using EventSourcingExample.Infrastructure.Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingExample.Application.Abstraction
{
    public class EventSourcingUnitOfWork<T>(
		EventStoreChangeTracker<T> eventStoreChangeTracker,
		IRepository<T> repository
	) : IEventSourcingUnitOfWork where T : class, IEventSourceEntity, new ()
	{
		public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
		{
			var entities = eventStoreChangeTracker.GetAggregatesToSave();

			foreach(var entity in entities) {
				await repository.AddAsync(entity);
			}

            return entities.Sum(x => x.GetUncommittedChanges().Count);
        }
    }
}