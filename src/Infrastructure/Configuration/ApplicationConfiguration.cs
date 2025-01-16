using EventSourcingExample.Application.Abstraction.Configurations;
using Microsoft.Extensions.Configuration;

namespace EventSourcingExample.Infrastructure.Configuration
{
    internal sealed class ApplicationConfiguration : ConfigurationSectionBase, IApplicationConfiguration
    {
        public ApplicationConfiguration(IConfiguration configuration)
            : base(configuration, "Application")
        {
        }

        public string BackendUrl => configurationSection.GetValue<string>(nameof(BackendUrl));
    }
}