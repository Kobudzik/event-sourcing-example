﻿using EventSourcingExample.Domain.Events.Banking;
using EventSourcingExample.Domain.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EventSourcingExample.Domain.Entities.Banking;

public class BankAccount : IEventSourceEntity
{
    public Guid Id { get; private set; }
	private decimal Balance { get; set; } = 0;
    public bool IsOpened { get; private set; }

    private List<object> _eventSourceChanges = new List<object>();

    public decimal GetBalance()
	{
        if(!IsOpened)
			throw new InvalidOperationException("Account is closed");
		return Balance;
	}

	public void Open()
    {
        if(IsOpened)
			throw new InvalidOperationException("Account is already opened");

        Id = Guid.NewGuid();
		IsOpened = true;
        AddEvent(new AccountOpened(Id)); // For event sourcing
    }

    public void Close()
    {
        if(!IsOpened)
			IsOpened = false;
        AddEvent(new AccountClosed()); // For event sourcing
    }

    public void Deposit(decimal amount)
    {
		if (!IsOpened)
			throw new InvalidOperationException("Account is closed");

		if (amount <= 0)
            throw new InvalidOperationException("Amount must be positive");

        Balance += amount;
        AddEvent(new AccountDeposited(Id, amount)); // For event sourcing
    }

    public void Withdraw(decimal amount)
    {
        if(!IsOpened)
			throw new InvalidOperationException("Account is closed");

		if (amount <= 0)
            throw new InvalidOperationException("Amount must be positive");

        if (Balance < amount)
            throw new InvalidOperationException("Insufficient funds");

        Balance -= amount;
        AddEvent(new AccountWithdrawn(Id, amount)); // For event sourcing
    }

	// Method to apply an event (for event sourcing)
	public void ApplyEvent(object eventItem)
    {
        switch (eventItem)
        {
            case AccountDeposited deposited:
                Balance += deposited.Amount;
            break;

            case AccountWithdrawn withdrawn:
                Balance -= withdrawn.Amount;
            break;

			case AccountOpened opened:
			    IsOpened = true;
                Id = opened.AccountId;
			break;

			case AccountClosed:
			    IsOpened = false;
			break;
		}
    }

    // Method to get all uncommitted changes (for event sourcing)
    public List<object> GetUncommittedChanges()
    {
        return _eventSourceChanges;
    }

    // Add an event to the list (for event sourcing)
    private void AddEvent(object eventItem)
    {
        _eventSourceChanges.Add(eventItem);
    }

    public object DeserializeEvent(string eventJson, string eventType)
    {
		return eventType switch
		{
			nameof(AccountDeposited) => JsonConvert.DeserializeObject<AccountDeposited>(eventJson),
			nameof(AccountWithdrawn) => JsonConvert.DeserializeObject<AccountWithdrawn>(eventJson),
			nameof(AccountOpened) => JsonConvert.DeserializeObject<AccountOpened>(eventJson),
			nameof(AccountClosed) => JsonConvert.DeserializeObject<AccountClosed>(eventJson),
			_ => throw new InvalidOperationException($"Unknown event type for {nameof(BankAccount)}: {eventType}"),
		};
	}
}