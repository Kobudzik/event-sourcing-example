using System.Threading;
using System.Threading.Tasks;
using EventSourcingExample.Application.Abstraction.Persistence;

namespace EventSourcingExample.Application.Abstraction
{
    public class CompositeUnitOfWork(IEventSourcingUnitOfWork eventSourcingUnitOfWork, ISqlUnitOfWork sqlUnitOfWork) : IUnitOfWork
	{
		public async Task<int> CommitAsync(CancellationToken cancellationToken)
		{
			int result = 0;

			if (eventSourcingUnitOfWork != null)
			{
				result += await eventSourcingUnitOfWork.CommitAsync(cancellationToken);
			}

			if (sqlUnitOfWork != null)
			{
				result += await sqlUnitOfWork.CommitAsync(cancellationToken);
			}

			return result;
		}
	}
}