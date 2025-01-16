using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using EventSourcingExample.Application.Abstraction;
using EventSourcingExample.Application.Common.Models;
using EventSourcingExample.Domain.Common;

namespace EventSourcingExample.Infrastructure.Common
{
    public class DomainEventService : IDomainEventService
    {
        private readonly ILogger<DomainEventService> _logger;
        private readonly IPublisher _mediator;

        public DomainEventService(ILogger<DomainEventService> logger, IPublisher mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Log information and publish event
        /// </summary>
        public async Task Publish(DomainEvent domainEvent)
        {
            _logger.LogInformation("Publishing domain event. Event - {event}", domainEvent.GetType().Name);

            await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(domainEvent));
        }

        private INotification GetNotificationCorrespondingToDomainEvent(DomainEvent domainEvent)
        {
            return (INotification)Activator.CreateInstance(
                typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType()), domainEvent);
        }
    }
}