using EventSourcingExample.Domain.Events.Banking;
using EventSourcingExample.Domain.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EventSourcingExample.Domain.Entities.Banking;

public class BankAccount : IEventSourceEntity
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public decimal Balance { get; private set; } = 0;
    public bool IsOpened { get; private set; }

    private List<object> _eventSourceChanges = new List<object>();

    public void Open()
    {
        IsOpened = true;
        AddEvent(new AccountOpened()); // For event sourcing
    }

    public void Close()
    {
        IsOpened = false;
        AddEvent(new AccountClosed()); // For event sourcing
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new InvalidOperationException("Amount must be positive");

        Balance += amount;
        AddEvent(new AccountDeposited(Id, amount)); // For event sourcing
    }

    public void Withdraw(decimal amount)
    {
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
        }
    }

    // Method to get all uncommitted changes (for event sourcing)
    public List<object> GetUncommittedChanges()
    {
        return _eventSourceChanges;
    }

    // Mark changes as committed (for event sourcing)
    public void MarkChangesAsCommitted()
    {
        _eventSourceChanges.Clear();
    }

    // Add an event to the list (for event sourcing)
    private void AddEvent(object eventItem)
    {
        _eventSourceChanges.Add(eventItem);
    }

    // Standard persistence: update the balance directly
    public void UpdateBalance(decimal newBalance)
    {
        Balance = newBalance;
    }

    public object DeserializeEvent(string eventJson, string eventType)
    {
        switch (eventType)
        {
            case nameof(AccountDeposited):
                return JsonConvert.DeserializeObject<AccountDeposited>(eventJson);
            case nameof(AccountWithdrawn):
                return JsonConvert.DeserializeObject<AccountWithdrawn>(eventJson);
            default:
                throw new InvalidOperationException($"Unknown event type: {eventType}");
        }
    }
}