﻿using EventSourcingExample.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;

namespace EventSourcingExample.Infrastructure.Email.Core.Configuration
{
    internal sealed class MailConfiguration : ConfigurationSectionBase, IMailConfiguration
    {
        public MailConfiguration(IConfiguration configuration) : base(configuration, "Mail")
        {
        }

        public string ServerAddress => configurationSection.GetValue<string>(nameof(ServerAddress));

        public int ServerPort => configurationSection.GetValue<int>(nameof(ServerPort));

        public string ServerLogin => configurationSection.GetValue<string>(nameof(ServerLogin));

        public string ServerPassword => configurationSection.GetValue<string>(nameof(ServerPassword));

        public string MailFrom => configurationSection.GetValue<string>(nameof(MailFrom));
        public bool DisableSsl => configurationSection.GetValue<bool>(nameof(DisableSsl));
        public int ChunkSize => configurationSection.GetValue<int>(nameof(ChunkSize));
    }
}