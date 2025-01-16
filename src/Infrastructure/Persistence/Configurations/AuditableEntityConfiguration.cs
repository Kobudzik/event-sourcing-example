using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using EventSourcingExample.Domain.Common;

namespace EventSourcingExample.Infrastructure.Persistence.Configurations
{
    public class AuditableEntityConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
     where TEntity : AuditableEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(c => c.CreatedById).IsRequired();
        }
    }
}
