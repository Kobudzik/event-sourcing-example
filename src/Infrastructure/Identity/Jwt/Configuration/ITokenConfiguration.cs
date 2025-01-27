﻿namespace EventSourcingExample.Infrastructure.Identity.Jwt.Configuration
{
    public interface ITokenConfiguration
    {
        string Secret { get; }
        string Issuer { get; }
        string Audience { get; }
        int AccessExpirationInMinutes { get; }
        int RefreshExpirationInMinutes { get; }
    }
}