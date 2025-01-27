﻿using System;
using System.Text;
using EventSourcingExample.Application.CQRS.Authentication;
using EventSourcingExample.Infrastructure.Identity.Jwt.Configuration;
using EventSourcingExample.Infrastructure.Identity.Jwt.Policies;
using EventSourcingExample.Infrastructure.Identity.Jwt.RefreshTokens;
using EventSourcingExample.Infrastructure.Identity.Jwt.RefreshTokens.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EventSourcingExample.Infrastructure.Identity.Jwt
{
    internal static class JwtTokenAuthorizationModule
    {
        /// <summary>
        /// Adds JWT Authentication along with Authorization and policies
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        internal static void AddJwtTokenAuthorizationModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITokenStoreManager, TokenStoreManager>();
            services.AddScoped<IAuthorizationHandler, TokenValidAuthorizationHandler>();
            services.AddSingleton<ITokenConfiguration, TokenConfiguration>();
            services.AddScoped<IJwtTokenManagementService, JwtTokenManagementService>();
            services.AddScoped<ITokenGenerationService, TokenGenerationService>();
            services.AddScoped<IRefreshTokenGenerationService, RefreshTokenGenerationService>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            var tokenConfiguration = new TokenConfiguration(configuration);

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenConfiguration.Secret)),
                        //ValidIssuer = tokenConfiguration.Issuer,
                        //ValidAudience = tokenConfiguration.Audience,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    };
                });

            services.AddAuthorization(options =>
            {
                var defaultAuthBuilder = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);

                options.DefaultPolicy = defaultAuthBuilder
                    .RequireAuthenticatedUser()
                    .Build();

                //options.AddPolicy(PolicyNameKeys.TokenValid,
                //    policy => policy.Requirements.Add(new TokenValidRequirement()));
            });

            services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromMinutes(tokenConfiguration.LinksExpirationInMinutes));

            services.AddDistributedMemoryCache();
        }
    }
}