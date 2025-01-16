using System;

namespace EventSourcingExample.Application.Abstraction
{
    public interface IDateTime
    {
        DateTime UtcNow { get; }
    }
}
