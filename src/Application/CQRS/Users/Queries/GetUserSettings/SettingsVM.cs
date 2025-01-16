using EventSourcingExample.Application.Common.Mappings;
using EventSourcingExample.Domain.Entities.Core;

namespace EventSourcingExample.Application.CQRS.Users.Queries.GetUserSettings
{
    public class SettingsVM : IMapFrom<UserAccount>
    {
    }
}
