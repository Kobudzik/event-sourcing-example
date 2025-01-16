using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using EventSourcingExample.Application.Abstraction;
using EventSourcingExample.Application.Abstraction.User;
using EventSourcingExample.Domain.Common;
using EventSourcingExample.Domain.Entities.Core;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace EventSourcingExample.Infrastructure.Persistence
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<UserAccount>, IApplicationDbContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTime _dateTime;
        private readonly IDomainEventService _domainEventService;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            ICurrentUserService currentUserService,
            IDomainEventService domainEventService,
            IDateTime dateTime)
            : base(options, operationalStoreOptions)
        {
            _currentUserService = currentUserService;
            _domainEventService = domainEventService;
            _dateTime = dateTime;
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            AuditAuditableEntries();
            VersionVersionableEntries();
            await DispatchEvents();
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        private void AuditAuditableEntries()
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedById = _currentUserService.UserId;
                        entry.Entity.CreatedDateUtc = _dateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedById = _currentUserService.UserId;
                        entry.Entity.LastModifiedDateUtc = _dateTime.UtcNow;
                        break;
                }
            }
        }

        private void VersionVersionableEntries()
        {
            foreach (var entry in ChangeTracker.Entries<IVersionableEntity>().Where(x => x.State == EntityState.Added))
            {
                var versionable = entry.Entity;
                var type = versionable.GetType();

                var latestVersion = Database
                    .SqlQueryRaw<int>($@"
                        SELECT TOP(1) {nameof(versionable.Version)} 
                        FROM {type.Name}s
                        WHERE {nameof(versionable.Identifier)} = '{versionable.Identifier}'
                        ORDER BY {nameof(versionable.Version)} DESC
                    ")
                    .ToList();

                entry.Entity.Version = latestVersion.SingleOrDefault() + 1;
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            #region .Net Identity renames
            builder.Entity<UserAccount>(entity =>
            {
                entity.ToTable(name: "Users");
            });

            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserToRolesRelations");
                entity.HasKey(key => new { key.UserId, key.RoleId });
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
                entity.HasKey(key => new { key.ProviderKey, key.LoginProvider });
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
                entity.HasKey(key => new { key.UserId, key.LoginProvider, key.Name });
            });
            #endregion
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_myLoggerFactory);
        }

        private async Task DispatchEvents()
        {
            while (true)
            {
                var domainEventEntity = ChangeTracker.Entries<IHasDomainEvent>()
                    .Select(x => x.Entity.DomainEvents)
                    .SelectMany(x => x)
                    .FirstOrDefault(domainEvent => !domainEvent.IsPublished);

                if (domainEventEntity == null)
                    break;

                domainEventEntity.IsPublished = true;

                await _domainEventService.Publish(domainEventEntity);
            }
        }

        public static readonly LoggerFactory _myLoggerFactory = new LoggerFactory(new[] {
                new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()
         });
    }
}
