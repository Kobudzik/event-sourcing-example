using EventSourcingExample.Application.Abstraction;
using EventSourcingExample.Domain.Entities.Banking;
using EventSourcingExample.Application.Common.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingExample.Application.CQRS.Banking.Commands.Deposit
{
    public sealed class DepositCommand : IRequest
    {
        public DepositCommand(Guid identifier, decimal amount)
        {
            Identifier = identifier;
            Amount = amount;
        }

        public Guid Identifier { get; }
        public decimal Amount { get; }

        internal sealed class DepositCommandHandler : IRequestHandler<DepositCommand>
        {
            private readonly IRepository<BankAccount> _bankRepository;

            public DepositCommandHandler(IRepository<BankAccount> bankRepository)
            {
                _bankRepository = bankRepository;
            }

            public async Task<Unit> Handle(DepositCommand request, CancellationToken cancellationToken)
            {
                var entity = await _bankRepository.GetByIdAsync(request.Identifier);
                if (entity == null)
                    throw new NotFoundException(nameof(BankAccount), request.Identifier);

                entity.Deposit(request.Amount);

                await _bankRepository.SaveAsync(entity);
                return Unit.Value;
            }
        }
    }
}