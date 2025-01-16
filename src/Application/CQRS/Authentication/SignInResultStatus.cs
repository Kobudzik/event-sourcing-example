namespace EventSourcingExample.Application.CQRS.Authentication
{
    public enum SignInResultStatus
    {
        Success = 1,
        Failure = 2,
        ExpiredPassword = 3,
        Locked = 4,
        AccountDisactivated = 5
    }
}