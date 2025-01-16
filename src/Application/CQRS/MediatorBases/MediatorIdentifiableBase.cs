namespace EventSourcingExample.Application.CQRS.MediatorBases
{
    public abstract class MediatorIdentifiableBase
    {
        protected MediatorIdentifiableBase(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}