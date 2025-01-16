using System;

namespace EventSourcingExample.Application.Common.Exceptions
{
    public class DomainLogicException : Exception
    {
        public DomainLogicException(string messageForUser)
            : base(messageForUser)
        {
        }
    }
}
