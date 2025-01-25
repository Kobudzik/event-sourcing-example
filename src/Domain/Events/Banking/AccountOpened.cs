using System;

namespace EventSourcingExample.Domain.Events.Banking
{
    public class AccountOpened(Guid accountId) : IDomainEvent
	{
		public Guid AccountId { get; } = accountId;
	}
}
