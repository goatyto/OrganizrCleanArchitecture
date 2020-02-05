using System.Collections.Generic;
using Organizr.Domain.Interfaces;

namespace Organizr.Domain.AggregateModel.ListAggregate
{
    public abstract class ListBase<TItem> : Auditable, IContributable, IOwned
        where TItem : OrderedListItemBase
    {
        public string OwnerId { get; protected set; }

        protected List<TItem> _items;
        public IReadOnlyCollection<TItem> Items => _items.AsReadOnly();

        protected List<Contributor> _contributors;
        public IReadOnlyCollection<Contributor> Contributors => _contributors.AsReadOnly();

        protected ListBase()
        {
            _items = new List<TItem>();
            _contributors = new List<Contributor>();
        }
    }
}
