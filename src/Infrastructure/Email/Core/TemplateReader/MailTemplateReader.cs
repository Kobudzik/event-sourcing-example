using EventSourcingExample.Application.Common.Exceptions;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EventSourcingExample.Infrastructure.Email.Core.TemplateReader
{
    internal sealed class MailTemplateReader : IMailTemplateReader
    {
        private readonly IConfiguration _configuration;

        public MailTemplateReader(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Read(string relativePathToTemplate)
        {
            var rootTemplatePath = _configuration.GetSection("Mail").GetValue<string>("TemplatesPath");

            if (string.IsNullOrEmpty(rootTemplatePath))
                throw new DomainLogicException(nameof(rootTemplatePath) + " not set.");

            var pathToTemplate = Path.Combine(rootTemplatePath, relativePathToTemplate);
            return File.ReadAllText(pathToTemplate);
        }
    }
}