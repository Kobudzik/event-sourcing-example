using System.Threading.Tasks;
using System.Threading;

namespace EventSourcingExample.Application.Abstraction
{
	public interface IUnitOfWork
	{
		Task<int> CommitAsync(CancellationToken cancellationToken = default);
	}
}