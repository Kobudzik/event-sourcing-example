using EventSourcingExample.Infrastructure.Email.Core;
using EventSourcingExample.Infrastructure.Email.Core.TemplateReader;
using System.Threading.Tasks;

namespace EventSourcingExample.Infrastructure.Email.Modules
{
    internal abstract class EmailHandlerTemplate
    {
        protected readonly IEmailSender EmailSender;
        protected readonly IMailTemplateReader MailTemplateReader;

        protected abstract string Subject { get; }
        protected abstract string TemplateFileName { get; }

        protected EmailHandlerTemplate(IEmailSender emailSender, IMailTemplateReader mailTemplateReader)
        {
            EmailSender = emailSender;
            MailTemplateReader = mailTemplateReader;
        }

        public async Task SendEmail(string receiverEmail)
        {

            string rawTemplate = MailTemplateReader.Read(TemplateFileName);
            var overridenTemplate = InjectData(rawTemplate);
            await EmailSender.SendEmailAsync(receiverEmail, Subject, overridenTemplate);
        }

        protected abstract string InjectData(string stringifiedTemplate);
    }
}