using EventSourcingExample.Domain.Common;
using System;
using System.Threading.Tasks;

namespace CommandsRegistry.Application.Abstraction
{
    public interface IRepository<T> where T : IEventSourceEntity, new()
    {
        Task<T> GetByIdAsync(Guid id);
        Task SaveAsync(T entity);
    }
}
