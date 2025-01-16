using AutoMapper;
using CommandsRegistry.Domain.Entities.Banking;
using EventSourcingExample.Application.Common.Mappings;
using System;

namespace EventSourcingExample.Application.CQRS.Authentication.DTOs
{
    public class BankAccountDto : IMapFrom<BankAccount>
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<BankAccount, BankAccountDto>();
        }
    }
}