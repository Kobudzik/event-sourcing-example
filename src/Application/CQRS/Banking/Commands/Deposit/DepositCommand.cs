using EventSourcingExample.Domain.Entities.Banking;
using EventSourcingExample.Application.Common.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using EventSourcingExample.Application.Abstraction.Persistence;
using EventSourcingExample.Infrastructure.Persistence;

namespace EventSourcingExample.Application.CQRS.Banking.Commands.Deposit
{
    public sealed class DepositCommand(Guid identifier, decimal amount) : IRequest
    {
		public Guid Identifier { get; } = identifier;
		public decimal Amount { get; } = amount;

		internal sealed class DepositCommandHandler(
            IRepository<BankAccount> bankRepository,
            IEventStoreChangeTracker<BankAccount> eventStoreChangeTracker) : IRequestHandler<DepositCommand>
        {
			public async Task<Unit> Handle(DepositCommand request, CancellationToken cancellationToken)
            {
                var entity = await bankRepository.GetByIdAsync(request.Identifier);
                if (entity == null)
                    throw new NotFoundException(nameof(BankAccount), request.Identifier);

                entity.Deposit(request.Amount);

                eventStoreChangeTracker.AddAggregateToSave(entity);

				return Unit.Value;
            }
        }
    }
}