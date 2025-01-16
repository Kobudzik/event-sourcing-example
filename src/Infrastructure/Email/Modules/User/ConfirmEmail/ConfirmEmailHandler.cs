using EventSourcingExample.Application.Abstraction.Configurations;
using EventSourcingExample.Application.Abstraction.Email.User.ConfirmEmail;
using EventSourcingExample.Infrastructure.Email.Core;
using EventSourcingExample.Infrastructure.Email.Core.TemplateReader;

namespace EventSourcingExample.Infrastructure.Email.Modules.User.ConfirmEmail
{
    internal class ConfirmEmailHandler: EmailHandlerTemplate, IConfirmEmailHandler
    {
        private readonly IApplicationConfiguration aplicationConfiguration;

        public ConfirmEmailHandler(
            IEmailSender emailSender,
            IMailTemplateReader mailTemplateReader,
            IApplicationConfiguration aplicationConfiguration)
            : base(emailSender, mailTemplateReader)
        {
            this.aplicationConfiguration = aplicationConfiguration;
        }

        protected override string Subject => "Już prawie!";
        protected override string TemplateFileName => "email_confirmation.html";

        public string Username { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }

        public void SetTemplateData(string username, string userId, string token)
        {
            Username= username;
            UserId= userId;
            Token= token;
        }

        protected override string InjectData(string stringifiedTemplate)
        {
            var injectedTemplate = stringifiedTemplate
                .Replace("{Username}", Username)
                .Replace("{TokenPlaceholder}", Token.Replace("+", "%2B"))
                .Replace("{UserIdPlaceholder}", UserId)
                .Replace("{BackendUrl}", aplicationConfiguration.BackendUrl);

            return injectedTemplate;
        }
    }
}