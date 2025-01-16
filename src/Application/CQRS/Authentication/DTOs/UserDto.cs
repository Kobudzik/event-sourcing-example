using System;
using AutoMapper;
using EventSourcingExample.Application.Common.Mappings;
using EventSourcingExample.Domain.Entities.Core;

namespace EventSourcingExample.Application.CQRS.Authentication.DTOs
{
    public class UserDto : IMapFrom<UserAccount>
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }

        public string[] Roles { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserAccount, UserDto>()
                .ForMember(d => d.Roles, opt => opt.MapFrom<UserDtoRolesResolver>());
        }
    }
}