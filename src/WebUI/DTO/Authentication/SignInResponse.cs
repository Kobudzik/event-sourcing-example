namespace EventSourcingExample.WebUI.DTO.Authentication
{
    public class SignInResponse
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}