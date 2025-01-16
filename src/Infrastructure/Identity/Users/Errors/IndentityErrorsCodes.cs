namespace EventSourcingExample.Infrastructure.Identity.Users.Errors
{
    public static class IndentityErrorsCodes
    {
        public const string EmailAlreadyTaken = nameof(EmailAlreadyTaken);
        public const string UsernameAlreadyTaken = nameof(UsernameAlreadyTaken);
        public const string DuplicateUserName = nameof(DuplicateUserName);
        public const string PasswordRepeated = nameof(PasswordRepeated);
        public const string TokenExpired = nameof(TokenExpired);
        public const string UserNotFound = nameof(UserNotFound);
        public const string TokenInvalid = nameof(TokenInvalid);
    }
}