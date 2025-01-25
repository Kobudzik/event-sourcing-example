using AutoMapper;
using EventSourcingExample.Domain.Entities.Banking;
using EventSourcingExample.Application.CQRS.Authentication.DTOs;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourcingExample.Application.Common.Exceptions;
using EventSourcingExample.Application.Abstraction.Persistence;

namespace EventSourcingExample.Application.CQRS.Banking.Queries.GetBalance
{
    public sealed class GetBalanceQuery(Guid identifier) : IRequest<BankAccountDto>
	{
		public Guid Identifier { get; } = identifier;

		internal sealed class GetBalanceCommandHandler(IRepository<BankAccount> bankRepository, IMapper mapper) : IRequestHandler<GetBalanceQuery, BankAccountDto>
		{
			public async Task<BankAccountDto> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
			{
				var entity = await bankRepository.GetByIdAsync(request.Identifier);
				if (entity == null)
					throw new NotFoundException(nameof(BankAccount), request.Identifier);

				if(!entity.IsOpened)
					throw new DomainLogicException("Account is closed. Can't get balance.");

				return mapper.Map<BankAccountDto>(entity);
			}
		}
	}
}