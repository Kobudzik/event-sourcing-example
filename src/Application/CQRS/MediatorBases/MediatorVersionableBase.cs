namespace EventSourcingExample.Application.CQRS.MediatorBases
{
    public abstract class MediatorVersionableBase
    {
        protected MediatorVersionableBase(int version, string identifier)
        {
            Version = version;
            Identifier = identifier;
        }

        public int Version { get; }
        public string Identifier { get; }
    }
}