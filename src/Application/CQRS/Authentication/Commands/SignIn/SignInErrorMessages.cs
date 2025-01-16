namespace EventSourcingExample.Application.CQRS.Authentication.Commands.SignIn
{
    internal static class SignInErrorMessages
    {
        internal const string PasswordOrLoginInvalid = "Login or password invalid";
        internal const string AccountLocked = "Your account is temporarily locked";
        internal static string PasswordExpired => "Password expired! Reset password link was sent to email";
        internal static string AccountDisactivated => "Your account is not active";
    }
}