using EventSourcingExample.Application.Abstraction;
using EventSourcingExample.Application.Common.Exceptions;
using EventSourcingExample.Domain.Entities.Banking;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcingExample.Application.CQRS.Banking.Commands.Withdraw
{
    public sealed class CloseAccountCommand : IRequest
    {
        public CloseAccountCommand(Guid identifier)
        {
            Identifier = identifier;
        }

        public Guid Identifier { get; set; }

        internal sealed class CloseAccountCommandHandler : IRequestHandler<CloseAccountCommand>
        {
            private readonly IRepository<BankAccount> _bankRepository;

            public CloseAccountCommandHandler(IRepository<BankAccount> bankRepository)
            {
                _bankRepository = bankRepository;
            }

            public async Task<Unit> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
            {
                var account = await _bankRepository.GetByIdAsync(request.Identifier);
                if(account == null)
                    throw new NotFoundException(nameof(BankAccount), request.Identifier);

				account.Close();
                await _bankRepository.SaveAsync(account);
                return Unit.Value;
            }
        }
    }
}