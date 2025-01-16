using CommandsRegistry.Application.Abstraction;
using EventSourcingExample.Domain.Common;
using EventSourcingExample.Infrastructure.Persistence;
using System;
using System.Threading.Tasks;

namespace CommandsRegistry.Infrastructure.Persistence
{
    public class SqlRepository<T> : IRepository<T> where T : class, IEventSourceEntity, new()
    {
        private readonly ApplicationDbContext _context;

        public SqlRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task SaveAsync(T entity)
        {
            var existingEntity = GetByIdAsync(entity.Id);

            if (existingEntity == null)
            {
                await _context.Set<T>().AddAsync(entity);
            }
            else
            {
                _context.Set<T>().Update(entity);
            }

            await _context.SaveChangesAsync();
        }
    }

}
