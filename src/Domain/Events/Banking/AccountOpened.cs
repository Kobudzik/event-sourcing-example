using System;

namespace EventSourcingExample.Domain.Events.Banking
{
    public class AccountOpened(Guid accountId)
    {
		public Guid AccountId { get; } = accountId;
	}
}
