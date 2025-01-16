using System.Threading.Tasks;

namespace EventSourcingExample.Infrastructure.Identity.Jwt.RefreshTokens
{
    public interface ITokenStoreManager
    {
        Task<bool> IsCurrentTokenActiveAsync();
        Task DeactivateCurrentAsync();
    }
}