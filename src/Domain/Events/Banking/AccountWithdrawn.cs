using System;

namespace EventSourcingExample.Domain.Events.Banking
{
    public class AccountWithdrawn(Guid accountId, decimal amount)
	{
		public Guid AccountId { get; } = accountId;
		public decimal Amount { get; } = amount;
	}
}
