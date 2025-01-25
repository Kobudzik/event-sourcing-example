using EventSourcingExample.Application.Abstraction;
using EventSourcingExample.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingExample.Infrastructure.Persistence
{
    public class SqlRepository<T>(ApplicationDbContext context) : IRepository<T> where T : class, IEventSourceEntity, new()
    {
		public async Task<T> GetByIdAsync(Guid id)
            => await context.Set<T>().FindAsync(id);

		public async Task AddAsync(T entity)
		{
			await context.Set<T>().AddAsync(entity);

			foreach (var resolvedEvent in entity.GetUncommittedChanges())
            {
                entity.ApplyEvent(resolvedEvent);
            }
		}

		public void AddAggregateToSave(T eventSourceEntity)
		{
			//do nothing
		}

		public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
		{
			return await context.SaveChangesAsync(cancellationToken);
		}
	}
}
