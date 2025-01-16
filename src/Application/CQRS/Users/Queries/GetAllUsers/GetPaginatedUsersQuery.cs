using System.Threading;
using System.Threading.Tasks;
using EventSourcingExample.Application.Abstraction.User;
using EventSourcingExample.Application.Common.Models;
using EventSourcingExample.Application.CQRS.MediatorBases;
using MediatR;

namespace EventSourcingExample.Application.CQRS.Users.Queries.GetAllUsers
{
    public class GetPaginatedUsersQuery: MediatorPaginableBase<GetPaginatedUsersFilterModel>, IRequest<PaginatedList<UserListItemModel>>
    {
        public GetPaginatedUsersQuery(Pager pager, GetPaginatedUsersFilterModel filter): base(pager, filter)
        {
        }

        internal class Handler : IRequestHandler<GetPaginatedUsersQuery, PaginatedList<UserListItemModel>>
        {
            private readonly IUserManagementService _userManagementService;

            public Handler(IUserManagementService userManagementService)
            {
                _userManagementService = userManagementService;
            }

            public async Task<PaginatedList<UserListItemModel>> Handle(GetPaginatedUsersQuery request, CancellationToken cancellationToken)
            {
                var users = await _userManagementService.GetUsersAsync(request.Pager, request.Filter);
                return new PaginatedList<UserListItemModel>(users, request.Pager.TotalRows, request.Pager.Index, request.Pager.Size);
            }
        }
    }
}