using EventSourcingExample.Application.Abstraction;
using EventSourcingExample.Domain.Common;
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
                await context.Set<T>().AddAsync(entity);
            else
                context.Set<T>().Update(entity);

            await context.SaveChangesAsync();
        }
    }

}
