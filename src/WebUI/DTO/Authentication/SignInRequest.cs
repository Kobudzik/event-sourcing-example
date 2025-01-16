namespace EventSourcingExample.WebUI.DTO.Authentication
{
    public class SignInRequest
    {
        public SignInRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public SignInRequest()
        {
        }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}
