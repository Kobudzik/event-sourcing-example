using EventSourcingExample.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventSourcingExample.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
        }
    }
}