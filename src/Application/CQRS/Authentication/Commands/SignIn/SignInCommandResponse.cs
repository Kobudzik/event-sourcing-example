using System;

namespace EventSourcingExample.Application.CQRS.Authentication.Commands.SignIn
{
    public class SignInCommandResponse
    {
        private SignInCommandResponse(string accessToken, string refreshToken, SignInResultStatus signInResultStatus)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Error = MapStatusToMessageError(signInResultStatus);
        }

        private SignInCommandResponse(SignInResultStatus signInResultStatus)
        {
            AccessToken = null;
            RefreshToken = null;
            Error = MapStatusToMessageError(signInResultStatus);
        }

        public string RefreshToken { get; }
        public string AccessToken { get; }
        public string Error { get; }

        internal static SignInCommandResponse CreateFailed(SignInResultStatus signInResultStatus)
        {
            return new SignInCommandResponse(signInResultStatus);
        }

        internal static SignInCommandResponse CreateSuccess(string accessToken, string refreshToken)
        {
            return new SignInCommandResponse(accessToken, refreshToken, SignInResultStatus.Success);
        }

        private static string MapStatusToMessageError(SignInResultStatus error)
        {
            return error switch
            {
                SignInResultStatus.Failure => SignInErrorMessages.PasswordOrLoginInvalid,
                SignInResultStatus.Success => string.Empty,
                SignInResultStatus.ExpiredPassword => SignInErrorMessages.PasswordExpired,
                SignInResultStatus.Locked => SignInErrorMessages.AccountLocked,
                SignInResultStatus.AccountDisactivated => SignInErrorMessages.AccountDisactivated,
                _ => throw new ArgumentOutOfRangeException(nameof(error), error, null)
            };
        }
    }
}