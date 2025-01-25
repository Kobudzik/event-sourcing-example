using EventSourcingExample.Domain.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingExample.Application.Abstraction.Persistence
{
    public interface IRepository<T> where T : IEventSourceEntity, new()
    {
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        void AddAggregateToSave(T eventSourceEntity);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
