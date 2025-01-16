using System;

namespace EventSourcingExample.WebUI.DTO.Errors
{
    [Serializable]
    public class ExceptionDto(string message)
    {
        public string Message { get; set; } = message;
    }
}