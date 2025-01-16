namespace EventSourcingExample.Application.Abstraction.Email.Authentication.ResetPassword
{
    public interface IResetPasswordEmailHandler : ICanSendEmail
    {
        void SetTemplateData(string changePasswordToken, string username, string userId, string frontendUrl);
    }
}