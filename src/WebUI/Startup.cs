using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Net.NetworkInformation;
using EventSourcingExample.Infrastructure.Persistence;
using EventSourcingExample.WebUI.Filters;
using EventSourcingExample.WebUI.Services;
using EventSourcingExample.Application;
using EventSourcingExample.Application.Abstraction.User;
using EventSourcingExample.Infrastructure;
using EventSourcingExample.Infrastructure.Email;
using FluentValidation;
using System.Globalization;
using EventSourcingExample.WebUI.Middlewares.Extensions;
using NReco.Logging.File;
using Carter;

namespace EventSourcingExample.WebUI
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEmailsModule();
            services.AddApplication(configuration);
            services.AddInfrastructure(Configuration);
            services.AddCors();

            services.AddControllersWithViews(options =>
                options.Filters.Add<ApiExceptionFilterAttribute>())
                    .AddNewtonsoftJson(options =>
                        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter()
                    )
            ).AddRazorRuntimeCompilation();

            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("pl-PL");
            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("pl");
            ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;
            services.AddFluentValidationAutoValidation();

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            services.AddHttpContextAccessor();

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "EventSourcingExample", Version = "v1" }));

            services.AddLogging(loggingBuilder =>
            {
                //var loggingSection = Configuration.GetSection("Logging");

                loggingBuilder.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);

                loggingBuilder.AddFile(
                    "logs/app_{0:yyyy}-{0:MM}-{0:dd}.log",
                    fileLoggerOpts => fileLoggerOpts.FormatLogFileName = fName => string.Format(fName, DateTime.Now)
                );
            });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot";
            });

            services.AddCarter();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Set-Authorization")
            );

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                //app.UseHttpsRedirection();
            }

            app.UseSlidingExpiration();

            // Re-write path only. / to /index.html
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Start serve files from wwwroot (Must be after UseDefaultFiles obviously.)
            app.UseSpaStaticFiles();

            app.UseHealthChecks("/health");

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapCarter();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("/index.html");
            });
        }

        public static HealthCheckResult GetHealthByPinging(string address)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = ping.Send(address);

                    if (reply.Status != IPStatus.Success)
                        return HealthCheckResult.Unhealthy();

                    if (reply.RoundtripTime > 100)
                        return HealthCheckResult.Degraded();

                    return HealthCheckResult.Healthy();
                }
            }
            catch
            {
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}