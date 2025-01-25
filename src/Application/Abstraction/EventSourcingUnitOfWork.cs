using EventSourcingExample.Domain.Common;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingExample.Application.Abstraction
{
	public class EventSourcingUnitOfWork<T>(IRepository<T> repository) : IEventSourcingUnitOfWork
		where T : class, IEventSourceEntity, new ()
	{
		public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
		{
			return await repository.SaveChangesAsync(cancellationToken);
		}
	}
}