using System;

namespace EventSourcingExample.Infrastructure.Email.Exceptions
{
    [Serializable]
    public class MailValidationException : Exception
    {
        public MailValidationException(string error)
            : base($"Mail validation occured: {error}")
        {
        }

        public MailValidationException() : base("Mail validation failed")
        {
        }
    }
}