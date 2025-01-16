using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourcingExample.Application.Abstraction.Email.Authentication.ResetPassword;
using EventSourcingExample.Application.Abstraction.Email.User.ConfirmEmail;
using MediatR;

namespace EventSourcingExample.Application.CQRS.Users.Commands.CreateUser
{
    public sealed class UserCreatedEvent : INotification
    {
        private UserCreatedEvent(Guid userPublicId, string email, string userName, string confirmAccountToken)
        {
            UserPublicId = userPublicId;
            Email = email;
            UserName = userName;
            ConfirmAccountToken = confirmAccountToken;
        }

        public Guid UserPublicId { get; }
        public string Email { get; }
        public string UserName { get; }
        public string ConfirmAccountToken { get; }

        public static UserCreatedEvent Create(Guid userPublicId, string email, string userName, string confirmAccountToken)
        {
            return new UserCreatedEvent(userPublicId, email, userName, confirmAccountToken);
        }
    }

    internal sealed class Handler : INotificationHandler<UserCreatedEvent>
    {
        private readonly IConfirmEmailHandler _confirmAccountMailSender;

        public Handler(IConfirmEmailHandler confirmAccountMailSender)
        {
            _confirmAccountMailSender = confirmAccountMailSender;
        }

        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            _confirmAccountMailSender.SetTemplateData(
                notification.UserName,
                notification.UserPublicId.ToString(),
                notification.ConfirmAccountToken
            );

            await _confirmAccountMailSender.SendEmail(notification.Email);
        }
    }
}