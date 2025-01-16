using System;

namespace EventSourcingExample.Domain.Events.Banking
{
    public class AccountWithdrawn
    {
        public Guid AccountId { get; }
        public decimal Amount { get; }

        public AccountWithdrawn(Guid accountId, decimal amount)
        {
            AccountId = accountId;
            Amount = amount;
        }
    }
}
