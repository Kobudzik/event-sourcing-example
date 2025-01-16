using System;
using EventSourcingExample.Application.Abstraction;

namespace EventSourcingExample.Infrastructure.Common
{
    public class DateTimeService : IDateTime
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
