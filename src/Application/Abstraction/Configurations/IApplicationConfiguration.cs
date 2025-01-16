namespace EventSourcingExample.Application.Abstraction.Configurations
{
    public interface IApplicationConfiguration
    {
        string BackendUrl { get; }
    }
}