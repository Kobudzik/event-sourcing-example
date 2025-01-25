using EventSourcingExample.Application.Abstraction.Persistence;
using EventSourcingExample.Application.Common.Exceptions;
using EventSourcingExample.Domain.Entities.Banking;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingExample.Application.CQRS.Banking.Commands.CloseAccount
{
    public sealed class CloseAccountCommand(Guid identifier) : IRequest
	{
		public Guid Identifier { get; set; } = identifier;

		internal sealed class CloseAccountCommandHandler(IRepository<BankAccount> bankRepository) : IRequestHandler<CloseAccountCommand>
		{
			public async Task<Unit> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
			{
				var account = await bankRepository.GetByIdAsync(request.Identifier);
				if (account == null)
					throw new NotFoundException(nameof(BankAccount), request.Identifier);

				account.Close();
				bankRepository.AddAggregateToSave(account);
				return Unit.Value;
			}
		}
	}
}