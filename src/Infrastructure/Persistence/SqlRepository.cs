﻿using EventSourcingExample.Application.Abstraction.Persistence;
using EventSourcingExample.Domain.Common;
using System;
using System.Threading.Tasks;

namespace EventSourcingExample.Infrastructure.Persistence
{
    public class SqlRepository<T>(ApplicationDbContext context) : IRepository<T>
		where T : class, IEventSourceEntity, new()
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
	}
}
