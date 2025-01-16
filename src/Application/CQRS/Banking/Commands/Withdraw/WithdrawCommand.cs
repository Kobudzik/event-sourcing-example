using CommandsRegistry.Application.Abstraction;
using CommandsRegistry.Domain.Entities.Banking;
using EventSourcingExample.Application.Common.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CommandsRegistry.Application.CQRS.Banking.Commands.Withdraw
{
    public sealed class WithdrawCommand : IRequest
    {
        public WithdrawCommand(Guid identifier, decimal amount)
        {
            Identifier = identifier;
            Amount = amount;
        }

        public Guid Identifier { get; }
        public decimal Amount { get; }

        internal sealed class WithdrawCommandHandler : IRequestHandler<WithdrawCommand>
        {
            private readonly IRepository<BankAccount> _bankRepository;

            public WithdrawCommandHandler(IRepository<BankAccount> bankRepository)
            {
                _bankRepository = bankRepository;
            }

            public async Task<Unit> Handle(WithdrawCommand request, CancellationToken cancellationToken)
            {
                var entity = await _bankRepository.GetByIdAsync(request.Identifier);
                if (entity == null)
                    throw new NotFoundException(nameof(BankAccount), request.Identifier);

                entity.Withdraw(request.Amount);

                await _bankRepository.SaveAsync(entity);
                return Unit.Value;
            }
        }
    }
}