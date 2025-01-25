using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using EventSourcingExample.Application.Abstraction.Persistence;

namespace EventSourcingExample.Application.Abstraction
{
    public class SqlUnitOfWork(DbContext context) : ISqlUnitOfWork
	{
		public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
		{
			return await context.SaveChangesAsync(cancellationToken);
		}
	}
}