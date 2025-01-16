using System;
using MediatR;

namespace EventSourcingExample.Application.CQRS.Users.Commands.ChangeUserPassword
{
    public class UserPasswordChangedNotification : INotification
    {
        public Guid PublicId { get; set; }
    }
}