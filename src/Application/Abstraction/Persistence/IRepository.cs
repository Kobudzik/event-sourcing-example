using EventSourcingExample.Domain.Common;
using System;
using System.Threading.Tasks;

namespace EventSourcingExample.Application.Abstraction.Persistence
{
    public interface IRepository<T> where T : IEventSourceEntity, new()
    {
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
    }
}
