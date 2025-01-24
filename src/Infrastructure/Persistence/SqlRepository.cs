using EventSourcingExample.Application.Abstraction;
using EventSourcingExample.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace EventSourcingExample.Infrastructure.Persistence
{
    public class SqlRepository<T>(ApplicationDbContext context) : IRepository<T> where T : class, IEventSourceEntity, new()
    {
		public async Task<T> GetByIdAsync(Guid id)
            => await context.Set<T>().FindAsync(id);

		public async Task SaveAsync(T entity)
        {
            var existingEntity = await GetByIdAsync(entity.Id);

            if (existingEntity == null)
            {
				await context.Set<T>().AddAsync(entity);
			}
			else
			{
				context.Entry(existingEntity).CurrentValues.SetValues(entity);
				context.Entry(existingEntity).State = EntityState.Modified;
			}

			foreach (var resolvedEvent in entity.GetUncommittedChanges())
            {
                entity.ApplyEvent(resolvedEvent);
            }

			await context.SaveChangesAsync();
        }
    }
}
