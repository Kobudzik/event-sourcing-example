using System;

namespace EventSourcingExample.Infrastructure.Identity.Users.Exceptions
{
    [Serializable]
    public class UserNotDeletedException : Exception
    {
        public UserNotDeletedException(string error)
            : base($"Unable to remove user: {error}")
        {
        }
    }
}