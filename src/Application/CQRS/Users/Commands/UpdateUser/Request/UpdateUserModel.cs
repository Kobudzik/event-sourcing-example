using System;
using AutoMapper;
using EventSourcingExample.Application.Common.Mappings;
using EventSourcingExample.Domain.Entities.Core;

namespace EventSourcingExample.Application.CQRS.Users.Commands.UpdateUser.Request
{
    public class UpdateUserModel : IMapFrom<UserAccount>
    {
        public UpdateUserModel()
        {
        }

        public UpdateUserModel(
            Guid userId,
            string firstName,
            string lastName,
            string subiektUserName)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            SubiektUserName = subiektUserName;
        }

        public Guid UserId { get; set; }
        public string FirstName { get; }
        public string LastName { get; }
        public string SubiektUserName { get; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateUserModel, UserAccount>();
        }
    }
}