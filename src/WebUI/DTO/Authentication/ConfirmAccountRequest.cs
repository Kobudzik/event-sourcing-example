using System;

namespace EventSourcingExample.WebUI.DTO.Authentication
{
    public class ConfirmAccountRequest
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }
}