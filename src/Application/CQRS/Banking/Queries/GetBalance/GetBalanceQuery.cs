using AutoMapper;
using EventSourcingExample.Application.Abstraction;
using EventSourcingExample.Domain.Entities.Banking;
using EventSourcingExample.Application.CQRS.Authentication.DTOs;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingExample.Application.CQRS.Users.Commands.ChangeUserPassword
{
    public sealed class GetBalanceQuery : IRequest<BankAccountDto>
    {
        public GetBalanceQuery(Guid identifier)
        {
            Identifier = identifier;
        }

        public Guid Identifier { get; }

        internal sealed class GetBalanceCommandHandler : IRequestHandler<GetBalanceQuery, BankAccountDto>
        {
            private readonly IRepository<BankAccount> _bankRepository;
            private readonly IMapper _mapper;

            public GetBalanceCommandHandler(IRepository<BankAccount> bankRepository, IMapper mapper)
            {
                _bankRepository = bankRepository;
                _mapper = mapper;
            }

            public async Task<BankAccountDto> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
            {
                var entity = await _bankRepository.GetByIdAsync(request.Identifier);

                entity.GetBalance();

                return _mapper.Map<BankAccountDto>(entity);
            }
        }
    }
}