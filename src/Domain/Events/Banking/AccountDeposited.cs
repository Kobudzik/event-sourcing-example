using System;

namespace CommandsRegistry.Domain.Events.Banking
{
    public class AccountDeposited
    {
        public Guid AccountId { get; }
        public decimal Amount { get; }

        public AccountDeposited(Guid accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}
