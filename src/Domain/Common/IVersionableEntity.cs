namespace EventSourcingExample.Domain.Common
{
    public interface IVersionableEntity
    {
        public int Version { get; set; }

        public string Identifier { get; }
    }
}
