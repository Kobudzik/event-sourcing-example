namespace EventSourcingExample.Application.CQRS.Users.Commands.CreateUser.Request
{
    public class CreateUserModel
    {
        /// <summary>
        /// Model użyty do wyciągnięcia podstawowych danych użytkownika z CreateUserCommand
        /// </summary>
        public CreateUserModel(string username, string firstName, string lastName, string email, string password, bool isActive)
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsActive = isActive;
            Password = password;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string Username { get; }
        public string Email { get; }
        public string Password { get; }
        public bool IsActive { get; }
    }
}