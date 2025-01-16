using System.Threading.Tasks;

namespace EventSourcingExample.Application.CQRS.Authentication
{
    public interface IRoleManagementService
    {
        Task AddNewRole(string roleName);
        Task<bool> RoleExists(string roleName);
    }
}