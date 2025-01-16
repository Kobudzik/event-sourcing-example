namespace EventSourcingExample.Application.CQRS.Users.Queries.GetAllUsers
{
    public class GetPaginatedUsersFilterModel
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsActive { get; set; }
    }
}