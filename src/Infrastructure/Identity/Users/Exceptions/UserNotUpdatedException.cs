using System;
using System.Collections.Generic;

namespace EventSourcingExample.Infrastructure.Identity.Users.Exceptions
{
    [Serializable]
    public class UserNotUpdatedException : Exception
    {
        public UserNotUpdatedException(List<string> errors) : base(
            $"Unable to update user because of errors: {string.Join(',', errors)}")
        {
        }

        public UserNotUpdatedException() : base("Unable update user")
        {
        }
    }
}