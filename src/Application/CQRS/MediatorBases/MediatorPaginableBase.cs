using EventSourcingExample.Application.Common.Models;

namespace EventSourcingExample.Application.CQRS.MediatorBases
{
    public abstract class MediatorPaginableBase<T>
    {
        protected MediatorPaginableBase(Pager pager, T filter)
        {
            Pager = pager;
            Filter = filter;
        }

        protected Pager Pager { get; }
        protected T Filter { get; }
    }
}