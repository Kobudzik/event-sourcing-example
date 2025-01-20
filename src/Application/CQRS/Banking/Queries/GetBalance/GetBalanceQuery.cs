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
    public sealed class GetBalanceQuery(Guid identifier) : IRequest<BankAccountDto>
    {
		public Guid Identifier { get; } = identifier;

		internal sealed class GetBalanceCommandHandler(IRepository<BankAccount> bankRepository, IMapper mapper) : IRequestHandler<GetBalanceQuery, BankAccountDto>
        {
            private readonly IRepository<BankAccount> _bankRepository = bankRepository;
            private readonly IMapper _mapper = mapper;

			public async Task<BankAccountDto> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
            {
                var entity = await _bankRepository.GetByIdAsync(request.Identifier);

                entity.GetBalance(); // hack: force ex here if account closed

                return _mapper.Map<BankAccountDto>(entity);
            }
        }
    }
}