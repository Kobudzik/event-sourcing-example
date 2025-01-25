﻿using EventSourcingExample.Domain.Entities.Banking;
using EventSourcingExample.Application.Common.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourcingExample.Application.Abstraction.Persistence;

namespace EventSourcingExample.Application.CQRS.Banking.Commands.Withdraw
{
    public sealed class WithdrawCommand(Guid identifier, decimal amount) : IRequest
    {
		public Guid Identifier { get; } = identifier;
		public decimal Amount { get; } = amount;

		internal sealed class WithdrawCommandHandler(IRepository<BankAccount> bankRepository) : IRequestHandler<WithdrawCommand>
        {
			public async Task<Unit> Handle(WithdrawCommand request, CancellationToken cancellationToken)
            {
                var entity = await bankRepository.GetByIdAsync(request.Identifier);
                if (entity == null)
                    throw new NotFoundException(nameof(BankAccount), request.Identifier);

                entity.Withdraw(request.Amount);

				bankRepository.AddAggregateToSave(entity);

				return Unit.Value;
            }
        }
    }
}