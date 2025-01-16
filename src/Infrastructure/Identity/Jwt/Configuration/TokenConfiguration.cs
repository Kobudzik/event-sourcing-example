using EventSourcingExample.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;

namespace EventSourcingExample.Infrastructure.Identity.Jwt.Configuration
{
    public class TokenConfiguration : ConfigurationSectionBase, ITokenConfiguration
    {
        public TokenConfiguration(IConfiguration configuration)
            : base(configuration, "TokenConfiguration")
        {
        }

        public int LinksExpirationInMinutes => configurationSection.GetValue<int>("LinksExpirationInMinutes");

        public string Secret => configurationSection.GetValue<string>("Secret");

        public string Issuer => configurationSection.GetValue<string>("Issuer");

        public string Audience => configurationSection.GetValue<string>("Audience");

        public int AccessExpirationInMinutes => configurationSection.GetValue<int>("AccessExpirationInMinutes");

        public int RefreshExpirationInMinutes => configurationSection.GetValue<int>("RefreshExpirationInMinutes");
    }
}