using Microsoft.Extensions.Configuration;

namespace EventSourcingExample.Infrastructure.Configuration
{
    public abstract class ConfigurationSectionBase
    {
        protected IConfigurationSection configurationSection;

        protected ConfigurationSectionBase(IConfiguration configuration, string sectionName)
        {
            configurationSection = configuration.GetSection(sectionName);
        }
    }
}