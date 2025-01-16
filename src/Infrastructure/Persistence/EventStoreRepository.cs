using EventSourcingExample.Application.Abstraction;
using EventSourcingExample.Domain.Common;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EventStoreRepository<T> : IRepository<T> where T : IEventSourceEntity, new()
{
    private readonly IEventStoreConnection _eventStoreConnection;

    public EventStoreRepository(IEventStoreConnection eventStoreConnection)
    {
        _eventStoreConnection = eventStoreConnection;
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        var events = await _eventStoreConnection.ReadStreamEventsForwardAsync($"{nameof(T)}-{id}", 0, 200, false);
        var entity = new T();

        foreach (var resolvedEvent in events.Events)
        {
            var eventJson = Encoding.UTF8.GetString(resolvedEvent.OriginalEvent.Data);
            var eventType = resolvedEvent.OriginalEvent.EventType;
            var @event = entity.DeserializeEvent(eventJson, eventType);
            entity.ApplyEvent(@event);
        }

        return entity;
    }

    public async Task SaveAsync(T bankAccount)
    {
        var events = bankAccount.GetUncommittedChanges();

        var eventDataList = events.Select(ParseToEventData).ToArray();

        await _eventStoreConnection.AppendToStreamAsync($"{nameof(T)}-{bankAccount.Id}", ExpectedVersion.Any, eventDataList);
    }

    private static EventData ParseToEventData(object e)
    {
        var eventType = e.GetType().Name;
        var eventJson = JsonConvert.SerializeObject(e);

        return new EventData(
            Guid.NewGuid(),
            eventType, true,
            Encoding.UTF8.GetBytes(eventJson),
            null
        );
    }
}
