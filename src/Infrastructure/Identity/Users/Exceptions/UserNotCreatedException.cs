using System;
using System.Collections.Generic;

namespace EventSourcingExample.Infrastructure.Identity.Users.Exceptions
{
    [Serializable]
    public class UserNotCreatedException : Exception
    {
        public UserNotCreatedException(List<string> errors) 
            : base($"Unable to create user because of errors: {string.Join(',', errors)}")
        {
        }

        public UserNotCreatedException() : base("Unable to create user")
        {
        }
    }
}