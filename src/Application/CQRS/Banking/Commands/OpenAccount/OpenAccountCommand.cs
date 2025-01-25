using EventSourcingExample.Application.Abstraction.Persistence;
using EventSourcingExample.Domain.Entities.Banking;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingExample.Application.CQRS.Banking.Commands.OpenAccount
{
    public sealed class OpenAccountCommand : IRequest<Guid>
	{
		internal sealed class OpenAccountCommandHandler(IRepository<BankAccount> bankRepository) : IRequestHandler<OpenAccountCommand, Guid>
		{
			public async Task<Guid> Handle(OpenAccountCommand request, CancellationToken cancellationToken)
			{
				var account = new BankAccount();
				account.Open();

                await bankRepository.AddAsync(account);
				return account.Id;
			}
		}
	}
}