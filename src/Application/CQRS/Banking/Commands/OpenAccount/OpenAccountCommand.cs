using EventSourcingExample.Application.Abstraction;
using EventSourcingExample.Domain.Entities.Banking;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingExample.Application.CQRS.Banking.Commands.Withdraw
{
    public sealed class OpenAccountCommand : IRequest<Guid>
    {
        public OpenAccountCommand()
        {
        }

        internal sealed class OpenAccountCommandHandler(IRepository<BankAccount> bankRepository) : IRequestHandler<OpenAccountCommand, Guid>
        {
            private readonly IRepository<BankAccount> _bankRepository = bankRepository;

			public async Task<Guid> Handle(OpenAccountCommand request, CancellationToken cancellationToken)
            {
                var account = new BankAccount();
                account.Open();

                await _bankRepository.SaveAsync(account);
                return account.Id;
            }
        }
    }
}