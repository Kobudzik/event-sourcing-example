using EventSourcingExample.Application.Abstraction.Email.Authentication.ResetPassword;
using EventSourcingExample.Infrastructure.Email.Core;
using EventSourcingExample.Infrastructure.Email.Core.TemplateReader;

namespace EventSourcingExample.Infrastructure.Email.Modules.Authentication.ResetPasswordEmail
{
    internal class ResetPasswordEmailHandler : EmailHandlerTemplate, IResetPasswordEmailHandler
    {
        public ResetPasswordEmailHandler(
            IEmailSender emailSender,
            IMailTemplateReader mailTemplateReader)
            : base(emailSender, mailTemplateReader)
        {
        }

        protected override string Subject => "Reset hasła";

        protected override string TemplateFileName => "email_password_reset.html";

        private string ChangePasswordToken { get; set; }
        private string Username { get; set; }
        private string UserId { get; set; }
        private string FrontendUrl { get; set; }

        public void SetTemplateData(string changePasswordToken, string username, string userId, string frontendUrl)
        {
            ChangePasswordToken = changePasswordToken;
            Username = username;
            UserId = userId;
            FrontendUrl = frontendUrl;
        }

        protected override string InjectData(string stringifiedTemplate)
        {
            var injectedTemplate = stringifiedTemplate
                .Replace("{Username}", Username)
                .Replace("{Token}", ChangePasswordToken.Replace("+", "%2B"))
                .Replace("{UserId}", UserId)
                .Replace("{FrontendUrl}", FrontendUrl);

            return injectedTemplate;
        }
    }
}