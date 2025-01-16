using System.Threading;
using System.Threading.Tasks;
using EventSourcingExample.Domain.Entities.Banking;
using EventSourcingExample.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;

namespace EventSourcingExample.Application.Abstraction
{
    public interface IApplicationDbContext
    {
        DbSet<UserAccount> UserAccounts { get; set; }
        DbSet<BankAccount> BankAccounts { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
