﻿using EventSourcingExample.Application.Abstraction.Persistence;
using EventSourcingExample.Domain.Common;
using EventSourcingExample.Domain.Events;
using EventStore.Client;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSourcingExample.Infrastructure.Persistence
{
    public class EventStoreRepository<T>(EventStoreClient eventStoreConnection) : IRepository<T> where T : IEventSourceEntity, new()
	{

		public async Task<T?> GetByIdAsync(Guid id)
		{
			try
			{
				var events = eventStoreConnection.ReadStreamAsync(Direction.Forwards, $"{typeof(T).Name}-{id}", StreamPosition.Start);
				var entity = new T();

				await foreach (var resolvedEvent in events)
				{
					var eventJson = Encoding.UTF8.GetString(resolvedEvent.Event.Data.ToArray());
					var eventType = resolvedEvent.Event.EventType;
					var @event = entity.DeserializeEvent(eventJson, eventType) as IDomainEvent;
					entity.ApplyEvent(@event);
				}

				return entity;
			}
			catch (StreamNotFoundException)
			{
				return default;
			}
		}

		public async Task AddAsync(T entity)
		{
			var events = entity.GetUncommittedChanges();
			var eventDataList = events.Select(ParseToEventData).ToArray();
			await eventStoreConnection.AppendToStreamAsync($"{typeof(T).Name}-{entity.Id}", StreamState.Any, eventDataList);
		}

		private static EventData ParseToEventData(IDomainEvent e)
		{
			var eventType = e.GetType().Name;
			var eventJson = JsonConvert.SerializeObject(e);

			return new EventData(
				Uuid.NewUuid(),
				eventType,
				Encoding.UTF8.GetBytes(eventJson)
			);
		}
	}
}