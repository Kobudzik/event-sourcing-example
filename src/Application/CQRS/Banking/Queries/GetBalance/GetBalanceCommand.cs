using AutoMapper;
using CommandsRegistry.Application.Abstraction;
using CommandsRegistry.Domain.Entities.Banking;
using EventSourcingExample.Application.CQRS.Authentication.DTOs;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingExample.Application.CQRS.Users.Commands.ChangeUserPassword
{
    public sealed class GetBalanceCommand : IRequest<BankAccountDto>
    {
        public GetBalanceCommand(Guid identifier)
        {
            Identifier = identifier;
        }

        public Guid Identifier { get; }

        internal sealed class GetBalanceCommandHandler : IRequestHandler<GetBalanceCommand, BankAccountDto>
        {
            private readonly IRepository<BankAccount> _bankRepository;
            private readonly IMapper _mapper;

            public GetBalanceCommandHandler(IRepository<BankAccount> bankRepository, IMapper mapper)
            {
                _bankRepository = bankRepository;
                _mapper = mapper;
            }

            public async Task<BankAccountDto> Handle(GetBalanceCommand request, CancellationToken cancellationToken)
            {
                var entity = _bankRepository.GetByIdAsync(request.Identifier);
                return _mapper.Map<BankAccountDto>(entity);
            }
        }
    }
}